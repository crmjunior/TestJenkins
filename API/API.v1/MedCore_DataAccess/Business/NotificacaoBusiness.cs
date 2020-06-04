using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using Newtonsoft.Json;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Business {
    public class NotificacaoBusiness : INotificacaoBusiness {
        private readonly INotificacaoData _notificacaoRepository;
        private readonly IAccessData _accessRepository;
        private readonly IAlunoEntity _alunoRepository;
        private readonly INotificacaoDuvidasAcademicasData _notificacaoDuvidaData;
        private readonly IMenuBusiness _menuBusiness;

        private const int TempoAposAula = 30;

        public NotificacaoBusiness (INotificacaoData notificacaoRepository,
            IAccessData accessRepository,
            IAlunoEntity alunoRepository,
            INotificacaoDuvidasAcademicasData notificacaoDuvidaData,
            IMenuBusiness menuBusiness) {
            _notificacaoRepository = notificacaoRepository;
            _accessRepository = accessRepository;
            _alunoRepository = alunoRepository;
            _notificacaoDuvidaData = notificacaoDuvidaData;
            _menuBusiness = menuBusiness;
        }

        public Notificacao GetNotificacao (int idNotificacao) {
            using (MiniProfiler.Current.Step ("Obtendo notificação")) {
                return _notificacaoRepository.Get (idNotificacao);
            }
        }

        public List<Notificacao> GetAll (int matricula, int idAplicacao) {
            using (MiniProfiler.Current.Step ("Obtendo todas notificações")) {
                return _notificacaoRepository.GetAll (matricula, idAplicacao);
            }
        }

        public int SetNotificacaoLida (Notificacao notificacao) {
            using (MiniProfiler.Current.Step ("Salvando notificação")) {
                return _notificacaoRepository.SetNotificacaoLida (notificacao);
            }
        }

        public OneSignalNotificationResponse SendSeletiva (NotificacaoSeletiva notificacao, string chaveAppId, string chaveGcm) {

            using (MiniProfiler.Current.Step ("enviando seletiva")) {
                IDictionary<string, string> additionalInfo = new Dictionary<string, string> ();
                additionalInfo.Add ("notificacaoTipoId", (notificacao.NotificacaoTipoId.Id).ToString ());
                additionalInfo.Add ("infoAdicional", notificacao.InfoAdicional);

                var oneSignalObject = new OneSignalNotification () {
                    AppId = ConfigurationProvider.Get ("Settings:" + chaveAppId),
                    GcmKey = ConfigurationProvider.Get ("Settings:" + chaveGcm),
                    Message = new OneSignalMessage (notificacao.Mensagem),
                    Title = new OneSignalMessage (notificacao.Titulo),
                    Devices = notificacao.DevicesUsuarios,
                    Data = additionalInfo,
                    DelayedOption = UseInteligentDelivery (notificacao.NotificacaoTipoId.Id) ? "last-active" : "null"
                };

                OneSignalNotificationResponse data = new OneSignalNotificationResponse ();

                using (var client = HttpCalls.CreateClient (ConfigurationProvider.Get ("Settings:oneSignalUrl"))) {
                    try {
                        var response = HttpCalls.PostData (client, ConfigurationProvider.Get ("Settings:oneSignalNotificationResource"), oneSignalObject);
                        data.Sucesso = true;
                        data.Message = response;
                    } catch (Exception ex) {
                        data.Sucesso = false;
                        data.Message = ex.Message;
                    }
                }

                return data;
            }
        }

        private bool UseInteligentDelivery (int tipoNotificacao) {
            using (MiniProfiler.Current.Step ("Usando entrega inteligente")) {
                if (tipoNotificacao == (int) ETipoNotificacao.DuvidasAcademicas)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// DEPRECATED
        /// </summary>
        /// <returns></returns>
        public List<Notificacao> GetNotificacoesPorPerfil (int idClient, int idAplicacao, int conteudoCompleto = 0, int idProduto = 0, string versao = "") {
            using (MiniProfiler.Current.Step ("Obtendo notificações por perfil")) {
                if (!RedisCacheManager.CannotCache (RedisCacheConstants.DadosFakes.KeyGetNotificacoesPorPerfil))
                    return RedisCacheManager.GetItemObject<List<Notificacao>> (RedisCacheConstants.DadosFakes.KeyGetNotificacoesPorPerfil);

                var lstNotificacao = _notificacaoRepository.GetNotificacoesAplicacao (idAplicacao, idClient);

                var menus = _menuBusiness.GetPermitidos ((int) Aplicacoes.MsProMobile, idClient, conteudoCompleto, idProduto, versao);
                var exibeDuvidas = Utilidades.MenuPermitido (menus, (int) Utilidades.EMenuAccessObject.DuvidasAcademicas);

                var notificacoesAdm = lstNotificacao.Where (x => x.Matricula.Equals (-1));

                var lstObjetoNotificacao = notificacoesAdm.Select (x => new AccessObject {
                    Id = x.IdNotificacao,
                        Nome = x.Texto
                }).ToList ();

                var lstPermissoes = GetAlunoPermissoes (lstObjetoNotificacao, idClient, idAplicacao);

                var lstPermissoesVisualizacao = lstPermissoes.Where (x => x.AcessoId != (int) (Utilidades.PermissaoStatus.SemAcesso)).ToList ();

                var notificacaoesPermitidas = new List<Notificacao> ();

                foreach (var objeto in lstObjetoNotificacao) {
                    var notificaocaoPermissao = lstPermissoesVisualizacao.Find (p => p.ObjetoId == objeto.Id);

                    if (notificaocaoPermissao != null) {
                        notificacaoesPermitidas.Add (lstNotificacao.FirstOrDefault (x => x.IdNotificacao == objeto.Id));
                    }
                }

                if (exibeDuvidas) {
                    var notificacoesAutomaticas = _notificacaoDuvidaData.GetNotificacoesDuvidasAcademicasAluno (idClient);

                    foreach (var item in notificacoesAutomaticas) {
                        item.Texto = item.Texto.Replace ("#QTD", GetMensagemNotificacaoDuvidasAcademicas (item.TipoNotificacao.Id, item.Quantidade));
                    }

                    notificacaoesPermitidas.AddRange (notificacoesAutomaticas);
                }

                notificacaoesPermitidas = notificacaoesPermitidas.OrderByDescending (x => x.DataOriginal).ToList ();

                return notificacaoesPermitidas;
            }
        }

        public List<Notificacao> GetNotificacoesPerfilAluno (int idClient, int idAplicacao, string versao = "") {
            using (MiniProfiler.Current.Step ("Obtendo notificações por perfil de aluno")) {
                var lstNotificacao = _notificacaoRepository.GetNotificacoesAplicacao (idAplicacao, idClient);

                var lstObjetoNotificacao = lstNotificacao.Select (x => new AccessObject { Id = x.IdNotificacao }).ToList ();

                var lstPermissoes = GetAlunoPermissoes (lstObjetoNotificacao, idClient, idAplicacao);

                var lstPermissoesVisualizacao = lstPermissoes.Where (x => x.AcessoId != (int) (Utilidades.PermissaoStatus.SemAcesso)).ToList ();

                var notificacaoesPermitidas = new List<Notificacao> ();

                notificacaoesPermitidas = lstNotificacao.Join (lstPermissoesVisualizacao,
                    not => not.IdNotificacao,
                    per => per.ObjetoId,
                    (not, per) => not).ToList ();

                if (PossuiPermissaoDuvidasAcademicas (idClient, idAplicacao)) {
                    notificacaoesPermitidas.AddRange (GetNotificacoesDuvidasAcademicas (idClient));
                }

                notificacaoesPermitidas = notificacaoesPermitidas.OrderByDescending (x => x.DataOriginal).ToList ();

                return notificacaoesPermitidas;
            }
        }

        public List<Notificacao> GetNotificacoesDuvidasAcademicas (int idClient) {
            using (MiniProfiler.Current.Step ("Obtendo notificações dúvidas academicas")) {
                var notificacoesDuvidasAcademicas = _notificacaoDuvidaData.GetNotificacoesDuvidasAcademicasAluno (idClient);

                foreach (var item in notificacoesDuvidasAcademicas) {
                    item.Texto = item.Texto.Replace ("#QTD", GetMensagemNotificacaoDuvidasAcademicas (item.TipoNotificacao.Id, item.Quantidade));
                }

                return notificacoesDuvidasAcademicas;
            }
        }

        public List<PermissaoRegra> GetAlunoPermissoes (List<AccessObject> lstObj, int idClient, int applicationId) {
            using (MiniProfiler.Current.Step ("Obtendo permissões do aluno")) {
                var condicoesPreenchidasPeloAluno = _accessRepository.GetCondicoesPreenchidasPeloAluno (idClient, applicationId);

                var regrasNotificacaoes = _accessRepository.GetRegrasNotificacoes (lstObj);

                var notificacao = from r in regrasNotificacaoes
                group r by r.ObjetoId into g
                select new { IdMenu = g.Key, PermissoesNotificacao = g.ToList ().OrderBy (x => x.Ordem).ToList () };

                var lstPermissoesNotificacao = new List<PermissaoRegra> ();

                var condicoesRegras = _accessRepository.GetRegraCondicoes (applicationId);

                foreach (var item in notificacao) {
                    var permissao = _accessRepository.GetPermissoes (condicoesPreenchidasPeloAluno, item.PermissoesNotificacao, condicoesRegras);
                    lstPermissoesNotificacao.Add (permissao);
                }

                return lstPermissoesNotificacao;
            }
        }

        public bool PossuiPermissaoDuvidasAcademicas (int idClient, int idAplicacao) {
            using (MiniProfiler.Current.Step ("Possui permissao duvidas academicas")) {
                var lstObj = new List<AccessObject> { new AccessObject { Id = (int) Utilidades.EMenuAccessObject.DuvidasAcademicas } };

                var lstPermissoes = _accessRepository.GetAlunoPermissoes (lstObj, idClient, idAplicacao);

                return lstPermissoes.Any (x => x.AcessoId != (int) (Utilidades.PermissaoStatus.SemAcesso));
            }
        }

        public List<NotificacaoPushResponse> ProcessaNotificacoesPush (NotificacaoPushRequest parametros) {
            using (MiniProfiler.Current.Step ("Processando notificações push")) {
                parametros = ValidaDataString (parametros);

                ProcessaAgendamentoNotificacao (parametros);
                var resultEnvio = ProcessaEnvioNotificacoes (parametros);

                return resultEnvio;
            }
        }

        public List<NotificacaoPushResponse> ProcessarNotificacoesPosEvento () {
            using (MiniProfiler.Current.Step ("Processando notificações pós evento")) {
                ProcessarAgendamentoNotificacoesPosEvento ();
                return ProcessarFilaNotificacoesPosEvento ();
            }
        }

        public void ProcessarAgendamentoNotificacoesPosEvento () {
            using (MiniProfiler.Current.Step ("Processando agendamento notificações pós evento")) {
                var notificacoesAgendar = _notificacaoRepository.GetNotificacoesPosEvento (EStatusEnvioNotificacao.NaoEnviado);

                if (notificacoesAgendar != null && notificacoesAgendar.Any ()) {
                    foreach (var notificacao in notificacoesAgendar) {
                        var devices = _notificacaoRepository.DefinirDevicesNotificacaoPosEvento (notificacao, EStatusEnvioNotificacao.NaoEnviado);
                        if (devices != null && devices.Any ()) {
                            var devicesNotificar = new List<DeviceNotificacao> ();
                            devices.ForEach (x => {
                                if (!devicesNotificar.Any (d => d.DeviceToken == x.DeviceToken)) {
                                    devicesNotificar.Add (x);
                                }
                            });
                            _notificacaoRepository.InserirDevicesNotificacao (devicesNotificar);

                            notificacao.StatusEnvio = EStatusEnvioNotificacao.Processando;
                            _notificacaoRepository.UpdateNotificacao (notificacao);
                        }
                    }
                }
            }
        }

        public List<NotificacaoPushResponse> ProcessarFilaNotificacoesPosEvento () {
            using (MiniProfiler.Current.Step ("processando fila notificações pós evento")) {
                var notificacoesProcessar = _notificacaoRepository.GetNotificacoesPosEvento (EStatusEnvioNotificacao.Processando);
                var retorno = new List<NotificacaoPushResponse> ();

                foreach (var notificacao in notificacoesProcessar) {
                    var result = new NotificacaoPushResponse ();
                    try {
                        var fila = _notificacaoRepository.BuscarFilaNotificacaoPosEvento (notificacao.IdNotificacao);
                        var seletivas = AgruparNotificacoesSeletivas (fila, notificacao);
                        result.OneSignalResponse = SendSeletivasList (seletivas, (Aplicacoes) notificacao.AplicacaoId);
                        result.Sucesso = true;
                    } catch (Exception ex) {
                        result.Sucesso = false;
                        result.Erro = ex.Message;
                    }
                    result.NotificacaoId = notificacao.IdNotificacao;
                    retorno.Add (result);

                    notificacao.StatusEnvio = EStatusEnvioNotificacao.NaoEnviado;
                    _notificacaoRepository.UpdateNotificacao (notificacao);
                }
                return retorno;
            }
        }

        public int RegistrarLeituraNotificacao (int idNotificacao) {
            using (MiniProfiler.Current.Step ("Registrando leitura notificação")) {
                var qtd = default (int);
                var notificacao = _notificacaoRepository.GetNotificacaoAlunoPosEvento (idNotificacao);

                if (notificacao != null) {
                    notificacao.intStatusLeitura = (int) ELeituraNotificacaoEvento.Lida;
                    qtd = _notificacaoRepository.AtualizarNotificacoesPosEvento (new List<tblNotificacaoEvento> { notificacao });
                }
                return qtd;
            }
        }

        private List<NotificacaoSeletiva> AgruparNotificacoesSeletivas (List<DeviceNotificacao> fila, Notificacao notificacao) {
            using (MiniProfiler.Current.Step ("Agrupando notificações seletivas")) {
                var maximo = Constants.QTDMAX_NOTIFICACOES;
                var seletivas = new List<NotificacaoSeletiva> ();
                var grupo = fila.GroupBy (x => new { x.Titulo, x.Mensagem, x.InfoAdicional });

                var seletivasTotal = grupo.Select (x => new NotificacaoSeletiva {
                    Titulo = x.Key.Titulo,
                        Mensagem = x.Key.Mensagem,
                        InfoAdicional = x.Key.InfoAdicional,
                        NotificacaoTipoId = notificacao.TipoNotificacao,
                        DevicesUsuarios = x.Select (y => y.DeviceToken).ToList ()
                }).ToList ();

                foreach (var s in seletivasTotal) {
                    if (s.DevicesUsuarios != null && s.DevicesUsuarios.Count > maximo) {
                        for (var i = 0; i < s.DevicesUsuarios.Count; i += maximo) {
                            seletivas.Add (new NotificacaoSeletiva {
                                Titulo = s.Titulo,
                                    Mensagem = s.Mensagem,
                                    InfoAdicional = s.InfoAdicional,
                                    DevicesUsuarios = s.DevicesUsuarios.Skip (i).Take (maximo).ToList ()
                            });
                        }
                    } else {
                        seletivas.Add (s);
                    }
                }
                return seletivas;
            }
        }

        public NotificacaoPushRequest ValidaDataString (NotificacaoPushRequest parametros) {
            using (MiniProfiler.Current.Step ("Validando data string")) {
                if (parametros.ParametrosAvaliacaoAula == null) {
                    parametros.ParametrosAvaliacaoAula = new ParametrosAvaliacaoAula ();
                }
                if (parametros.ParametrosPrimeiraAula == null) {
                    parametros.ParametrosPrimeiraAula = new ParametrosPrimeiraAula ();
                }
                if (!string.IsNullOrEmpty (parametros.ParametrosAvaliacaoAula.DataString)) {
                    DateTime data;
                    DateTime.TryParse (parametros.ParametrosAvaliacaoAula.DataString, out data);

                    parametros.ParametrosAvaliacaoAula.Data = data;
                }
                if (!string.IsNullOrEmpty (parametros.ParametrosPrimeiraAula.DataString)) {
                    DateTime data;
                    DateTime.TryParse (parametros.ParametrosPrimeiraAula.DataString, out data);

                    parametros.ParametrosPrimeiraAula.Data = data;
                }

                return parametros;
            }
        }

        private bool ProcessaAgendamentoNotificacao (NotificacaoPushRequest parametros) {
            using (MiniProfiler.Current.Step ("processando agendamento notificação")) {
                var listNotificacoesPendentes = _notificacaoRepository.GetNotificacoesAProcessar (EStatusEnvioNotificacao.NaoEnviado);
                bool bResult = true;

                foreach (var notificacaoPendente in listNotificacoesPendentes) {
                    try {
                        var devicesInscritos = new List<DeviceNotificacao> ();

                        switch ((ETipoNotificacao) notificacaoPendente.TipoNotificacao.Id) {
                            case ETipoNotificacao.DuvidasAcademicas:
                                devicesInscritos = GetDevicesNotificacaoDuvidasAcademicas ();
                                break;
                            case ETipoNotificacao.AvaliacaoAula:
                                devicesInscritos = GetDevicesNotificacaoAvaliacaoAula (notificacaoPendente, parametros.ParametrosAvaliacaoAula);
                                break;
                            case ETipoNotificacao.PrimeiraAula:
                                devicesInscritos = GetDevicesNotificacaoPrimeiraAula (notificacaoPendente, parametros.ParametrosPrimeiraAula);
                                break;
                            case ETipoNotificacao.SimuladoOnline:

                            default:
                                var alunos = GetAlunosNotificacao (notificacaoPendente);

                                devicesInscritos = alunos.Select (x => x.Devices.LastOrDefault ())
                                    .Select (x => new DeviceNotificacao { DeviceToken = x.Token, NotificacaoId = notificacaoPendente.IdNotificacao })
                                    .ToList ();
                                break;
                        }
                        if (devicesInscritos.Any ()) {
                            CriaFilaDevicesNotificacao (devicesInscritos);
                        }

                        notificacaoPendente.StatusEnvio = EStatusEnvioNotificacao.Processando;

                        _notificacaoRepository.UpdateNotificacao (notificacaoPendente);

                    } catch {
                        throw;
                    }

                }
                return bResult;
            }
        }

        public List<DeviceNotificacao> GetDevicesNotificacaoDuvidasAcademicas () {
            using (MiniProfiler.Current.Step ("Obtendo devices notificação dúvidas academicas")) {
                var alunosDuvidasAcademicas = _notificacaoDuvidaData.GetAlunosNotificacaoDuvida ();
                var devicesInscritos = alunosDuvidasAcademicas;

                return devicesInscritos;
            }
        }

        public List<DeviceNotificacao> GetDevicesNotificacaoAvaliacaoAula (Notificacao notificacao, ParametrosAvaliacaoAula parametros) {
            using (MiniProfiler.Current.Step ("Obtendo devices notificação aula")) {
                var alunosPresentes = _notificacaoRepository.GetAlunoTemaAvaliacao (parametros);
                var cronogramaUltimasAulasDoDia = _notificacaoRepository.GetCursosComUltimaAulaDoDia (parametros);

                var cursoHorarioNotificacao = cronogramaUltimasAulasDoDia.Select (x => new { CourseId = x.intCourseID, DateTime = x.dteDateTime.AddMinutes (x.intDuration).AddMinutes (TempoAposAula) }).ToList ();

                var alunosNotificar = new List<AlunoTemaAvaliacao> ();

                foreach (var curso in cursoHorarioNotificacao.Where (x => x.DateTime <= DateTime.Now)) {
                    var alunos = alunosPresentes.Where (x => x.CourseId == curso.CourseId);
                    alunosNotificar.AddRange (alunos);
                }

                var devicesInscritos = AgrupaDevicesAvaliacao (alunosNotificar, notificacao);

                return devicesInscritos;
            }
        }

        public List<DeviceNotificacao> GetDevicesNotificacaoPrimeiraAula (Notificacao notificacao, ParametrosPrimeiraAula parametros) {
            using (MiniProfiler.Current.Step ("Obtendo devices notificação primeira aula")) {
                var devices = new List<DeviceNotificacao> ();
                var aulaRepository = new AulaEntity ();

                int diasAntecedencia = parametros.DiasAntecedencia == default (int) ? Utilidades.NotificacaoPrimeiraAulaDiasAntecedenciaPadrao : parametros.DiasAntecedencia;

                var dataAula = parametros.Data == default (DateTime) ? DateTime.Now.Date.AddDays (diasAntecedencia) : parametros.Data;

                var turmasPrimeiraAula = aulaRepository.GetPrimeiraAulaTurma (dataAula, parametros.TurmaId);
                var alunos = aulaRepository.GetAlunosAulaTurma (turmasPrimeiraAula);

                var alunosCourse = alunos.GroupBy (x => x.CourseId).ToList ();

                alunosCourse.ForEach (x => {
                    var devicesCurso = x.Where (y => y.CourseId == x.Key).Select (z => new DeviceNotificacao {
                        ClientId = z.ClientId,
                            DeviceToken = z.ClientDeviceToken,
                            NotificacaoId = notificacao.IdNotificacao,
                            Status = EStatusEnvioNotificacao.NaoEnviado,
                            Data = z.LessonDatetime,
                            Titulo = notificacao.Titulo.Replace ("#PRODUTO", z.ProductName),
                            Mensagem = notificacao.Texto.Replace ("#DATA", z.LessonDatetime.ToString ("dd/MM/yyyy")).Replace ("#HORA", z.LessonDatetime.ToString ("HH:mm"))
                    }).ToList ();

                    devices.AddRange (devicesCurso);
                });

                return devices;
            }
        }

        private List<DeviceNotificacao> AgrupaDevicesAvaliacao (List<AlunoTemaAvaliacao> alunosAvaliacao, Notificacao notificacaoPendente) {
            using (MiniProfiler.Current.Step ("agrupando devices avaliação")) {
                var devicesAgrupados = alunosAvaliacao.GroupBy (x => x.ClientID).Select (y => new DeviceNotificacao {
                    ClientId = y.Key,
                        NotificacaoId = notificacaoPendente.IdNotificacao,
                        DeviceToken = y.FirstOrDefault ().DeviceToken,
                        InfoAdicional = MontaArrayInfoAdicional (y.ToList ()),
                        Data = y.FirstOrDefault ().Entrada
                }).ToList ();

                return devicesAgrupados;
            }
        }

        public string MontaArrayInfoAdicional (List<AlunoTemaAvaliacao> temasAluno) {
            using (MiniProfiler.Current.Step ("montando array informação adicional")) {
                List<string> infos = new List<string> ();

                var temasAlunosGroup = temasAluno.GroupBy (x => new { x.LessonTitleID, x.MaterialId }).ToList ();

                foreach (var tema in temasAlunosGroup) {
                    infos.Add (string.Format ("{{\"idApostila\": \"{0}\", \"idTema\": \"{1}\"}}", tema.Key.MaterialId, tema.Key.LessonTitleID));
                }
                var arrayInfo = "[" + string.Join (",", infos) + "]";
                return arrayInfo;
            }
        }

        private void CriaFilaDevicesNotificacao (List<DeviceNotificacao> devicesInscritos) {
            using (MiniProfiler.Current.Step ("Criando fila devices notificação")) {
                try {
                    var data = Utilidades.GetServerDate ();
                    var hoje = data.Date;
                    var notificacaoId = devicesInscritos.FirstOrDefault ().NotificacaoId;

                    var notificacao = GetNotificacao (notificacaoId);

                    var devicesNotificados = GetDevicesNotificados (notificacao);
                    var devicesAnotificar = new List<DeviceNotificacao> ();

                    foreach (var device in devicesInscritos) {
                        if (!devicesNotificados.Any (x => x.DeviceToken == device.DeviceToken) && !devicesAnotificar.Any (x => x.DeviceToken == device.DeviceToken)) {
                            devicesAnotificar.Add (device);
                        }
                    }

                    _notificacaoRepository.InserirDevicesNotificacao (devicesAnotificar);

                    if (notificacao.TipoNotificacao.Id == (int) ETipoNotificacao.DuvidasAcademicas) {
                        foreach (var item in devicesAnotificar) {
                            _notificacaoDuvidaData.SetNotificacaoDuvidasAcademicaAlunoEnviada (item.ClientId, data);
                        }
                    }

                } catch {

                    throw;
                }
            }
        }

        private bool IsNotificacaoRecorrente (int tipoNotificacao) {
            using (MiniProfiler.Current.Step ("Verificando notificação recorrente")) {
                return (tipoNotificacao == (int) ETipoNotificacao.DuvidasAcademicas ||
                    tipoNotificacao == (int) ETipoNotificacao.AvaliacaoAula ||
                    tipoNotificacao == (int) ETipoNotificacao.PrimeiraAula);
            }
        }

        private List<NotificacaoPushResponse> ProcessaEnvioNotificacoes (NotificacaoPushRequest parametros) {
            using (MiniProfiler.Current.Step ("Processando envio de notificações")) {
                var notificacoesProcessar = _notificacaoRepository.GetNotificacoesAProcessar (EStatusEnvioNotificacao.Processando);
                var retorno = new List<NotificacaoPushResponse> ();

                foreach (var notificacao in notificacoesProcessar) {
                    var notificacaoResult = new NotificacaoPushResponse {
                        NotificacaoId = notificacao.IdNotificacao
                    };
                    try {
                        var seletivas = ConfiguraNotificacao (notificacao, parametros);
                        notificacaoResult.OneSignalResponse = SendSeletivasList (seletivas, (Aplicacoes) notificacao.AplicacaoId);
                        notificacaoResult.Sucesso = true;
                    } catch (Exception ex) {
                        notificacaoResult.Sucesso = false;
                        notificacaoResult.Erro = ex.Message;
                    }
                    retorno.Add (notificacaoResult);
                    notificacao.StatusEnvio = IsNotificacaoRecorrente (notificacao.TipoNotificacao.Id) ? EStatusEnvioNotificacao.NaoEnviado : EStatusEnvioNotificacao.Enviado;
                    _notificacaoRepository.UpdateNotificacao (notificacao);
                }
                return retorno;
            }
        }

        public List<NotificacaoSeletiva> ConfiguraNotificacao (Notificacao notificacao, NotificacaoPushRequest parametros) {
            using (MiniProfiler.Current.Step ("Configurando notificação")) {
                var seletivas = new List<NotificacaoSeletiva> ();
                var devices = _notificacaoRepository.GetDevicesNotificacaoFila (notificacao.IdNotificacao);
                if (devices.Any ()) {
                    switch ((ETipoNotificacao) notificacao.TipoNotificacao.Id) {
                        case ETipoNotificacao.AvaliacaoAula:
                            seletivas = SetNotificacoesAvaliacao (devices, notificacao, parametros.ParametrosAvaliacaoAula);
                            break;
                        case ETipoNotificacao.PrimeiraAula:
                            seletivas = SetNotificacoesPrimeiraAula (devices, notificacao);
                            break;
                        case ETipoNotificacao.SimuladoOnline:
                            seletivas = SetNotificacoesSimulado (devices, notificacao);
                            break;
                        default:
                            seletivas = SetNotificacoesSeletivas (devices, notificacao);
                            break;
                    }
                }
                return seletivas;
            }
        }

        public List<NotificacaoSeletiva> SetNotificacoesSimulado (List<DeviceNotificacao> devices, Notificacao notificacao) {
            using (MiniProfiler.Current.Step ("Alterando notificações de simulado")) {
                notificacao.InfoAdicional = GetInfoAdicionalSimuladoSerializado (notificacao);
                return SetNotificacoesSeletivas (devices, notificacao);
            }
        }

        public List<NotificacaoSeletiva> SetNotificacoesSeletivas (List<DeviceNotificacao> devices, Notificacao notificacao) {
            using (MiniProfiler.Current.Step ("Alterando notificações de seletivas")) {
                const int QUANTIDADE_LIMITE_DEVICES = 2000;
                List<NotificacaoSeletiva> lstNotificacaoSeletivas = new List<NotificacaoSeletiva> ();
                for (int i = 0; i < devices.Count; i += QUANTIDADE_LIMITE_DEVICES) {
                    var notificacaoSeletiva = new NotificacaoSeletiva () {
                    NotificacaoTipoId = notificacao.TipoNotificacao,
                    Titulo = notificacao.Titulo,
                    Mensagem = notificacao.Texto,
                    InfoAdicional = notificacao.InfoAdicional,
                    DevicesUsuarios = devices.Select (x => x.DeviceToken).Skip (i).Take (QUANTIDADE_LIMITE_DEVICES).ToList ()
                    };
                    lstNotificacaoSeletivas.Add (notificacaoSeletiva);
                }
                return lstNotificacaoSeletivas;
            }
        }

        public List<NotificacaoSeletiva> SetNotificacoesAvaliacao (List<DeviceNotificacao> devices, Notificacao notificacao, ParametrosAvaliacaoAula parametros) {
            using (MiniProfiler.Current.Step ("Alterando notificações de avaliação")) {
                List<NotificacaoSeletiva> notificacaoSeletivas = new List<NotificacaoSeletiva> ();

                DateTime dataMensagem = parametros.Data != default (DateTime) ? parametros.Data : DateTime.Today;

                var devicesInfoAgrupadas = devices.GroupBy (x => x.InfoAdicional).ToList ();
                var mensagem = notificacao.Texto.Replace ("#DATA", dataMensagem.ToString ("dd/MM/yyyy"));

                foreach (var item in devicesInfoAgrupadas) {
                    var notificacaoSeletivaAvaliacao = new NotificacaoSeletiva () {
                        NotificacaoTipoId = notificacao.TipoNotificacao,
                        Titulo = notificacao.Titulo,
                        Mensagem = mensagem,
                        InfoAdicional = item.Key,
                        DevicesUsuarios = item.Select (x => x.DeviceToken).ToList ()
                    };

                    notificacaoSeletivas.Add (notificacaoSeletivaAvaliacao);
                }

                return notificacaoSeletivas;
            }
        }

        public List<NotificacaoSeletiva> SetNotificacoesPrimeiraAula (List<DeviceNotificacao> devices, Notificacao notificacao) {
            using (MiniProfiler.Current.Step ("Alterando notificações de primeira aula")) {
                List<NotificacaoSeletiva> notificacaoSeletivas = new List<NotificacaoSeletiva> ();

                var devicesInfoAgrupadas = devices.GroupBy (x => new { x.Titulo, x.Mensagem }).ToList ();

                foreach (var item in devicesInfoAgrupadas) {
                    var notificacaoSeletivaAvaliacao = new NotificacaoSeletiva () {
                        NotificacaoTipoId = notificacao.TipoNotificacao,
                        Titulo = item.Key.Titulo,
                        Mensagem = item.Key.Mensagem,
                        DevicesUsuarios = item.Select (x => x.DeviceToken).ToList ()
                    };

                    notificacaoSeletivas.Add (notificacaoSeletivaAvaliacao);
                }
                return notificacaoSeletivas;
            }
        }

        public List<OneSignalNotificationResponse> SendSeletivasList (List<NotificacaoSeletiva> notificacaoSeletivas, Aplicacoes aplicacao) {
            using (MiniProfiler.Current.Step ("Enviando lista de seletivas")) {
                var lstRetorno = new List<OneSignalNotificationResponse> ();
                var chaveAppId = Utilidades.AppIdNotificacoes (aplicacao);
                var chaveGcmKey = Utilidades.GcmKeyNotificacoes (aplicacao);
                foreach (var item in notificacaoSeletivas) {
                    var retorno = SendSeletiva (item, chaveAppId, chaveGcmKey);
                    lstRetorno.Add (retorno);
                    System.Threading.Thread.Sleep (3000);
                }
                return lstRetorno;
            }
        }

        public List<DeviceNotificacao> GetDevicesNotificados (Notificacao notificacao) {
            using (MiniProfiler.Current.Step ("Obtendo dispositivos notificados")) {
                var hoje = Utilidades.GetServerDate ().Date;
                var devices = new List<DeviceNotificacao> ();

                if (IsNotificacaoRecorrente (notificacao.TipoNotificacao.Id)) {
                    devices = _notificacaoRepository.GetDevicesNotificados (notificacao.IdNotificacao, hoje);
                } else {
                    devices = _notificacaoRepository.GetDevicesNotificados (notificacao.IdNotificacao, default (DateTime));
                }

                return devices;
            }
        }

        public List<Aluno> GetAlunosNotificacao (Notificacao notificacao) {
            using (MiniProfiler.Current.Step ("Obtendo notificação de alunos")) {
                var matriculas = new List<Aluno> ();
                Aplicacoes aplicacao = (Aplicacoes) notificacao.AplicacaoId;

                if (notificacao.Matricula.Equals (-1)) {

                    var listAccessObjectNotificacao = new List<AccessObject> {
                        new AccessObject {
                        Id = notificacao.IdNotificacao,
                        Nome = notificacao.Texto
                        }
                    };

                    var listPermissoes = _accessRepository.GetRegrasNotificacoes (listAccessObjectNotificacao);
                    var listCondicoesRegra = new List<RegraCondicao> ();

                    foreach (var regraPermissao in listPermissoes) {
                        listCondicoesRegra.AddRange (_accessRepository.GetRegraCondicoes ((int) aplicacao, regraPermissao.Regra.Id));
                    }

                    matriculas = _accessRepository.GetAlunosPorRegra (listCondicoesRegra, aplicacao);
                } else {
                    matriculas.Add (_alunoRepository.GetAlunosDevice (notificacao.Matricula, aplicacao));
                }

                return matriculas;
            }
        }

        private string GetMensagemNotificacaoDuvidasAcademicas (int Tipo, int Quantidade) {
            using (MiniProfiler.Current.Step ("Obtendo mensagem de notificação do dúvidas acadêmicas")) {
                string strRetorno = "";

                switch ((EnumTipoMensagemNotificacaoDuvidasAcademicas) Tipo) {
                    case EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespondida:
                    case EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespondida:
                        strRetorno = Quantidade > 1 ? Quantidade + " novas respostas." : Quantidade + " nova resposta.";
                        break;
                    case EnumTipoMensagemNotificacaoDuvidasAcademicas.ReplicaDuvida:
                    case EnumTipoMensagemNotificacaoDuvidasAcademicas.ReplicaResposta:
                        strRetorno = Quantidade > 1 ? Quantidade + " novas réplicas." : Quantidade + " nova replica.";
                        break;
                    default:
                        break;
                }

                return strRetorno;
            }
        }

        public NotificacoesClassificadasDTO GetNotificacoesClassificadas (int idClient, int idAplicacao, string versaoAplicacao) {
            using (MiniProfiler.Current.Step ("Obtendo notificações classificadas")) {
                if (!RedisCacheManager.CannotCache (RedisCacheConstants.DadosFakes.KeyGetNotificacoesClassificadas))
                    return RedisCacheManager.GetItemObject<NotificacoesClassificadasDTO> (RedisCacheConstants.DadosFakes.KeyGetNotificacoesClassificadas);

                NotificacoesClassificadasDTO notificacoesClassificadas = new NotificacoesClassificadasDTO ();
                List<NotificacaoDTO> lstNotificacaoDTO = new List<NotificacaoDTO> ();

                var notificacoes = GetNotificacoesPerfilAluno (idClient, idAplicacao, versao : versaoAplicacao);
                var cultureInfo = new CultureInfo ("pt-BR");

                foreach (var not in notificacoes) {
                    var notificacaoDTO = new NotificacaoDTO {
                        IdNotificacao = not.IdNotificacao,
                        Texto = not.Texto,
                        Titulo = not.Titulo,
                        TipoNotificacaoId = not.TipoNotificacao.Id,
                        Matricula = not.Matricula,
                        Data = not.DataOriginal.ToString ("MMM/yy", cultureInfo),
                        Dia = not.DataOriginal.ToString ("dd"),
                        DataUnix = Utilidades.ToUnixTimespan (not.DataOriginal),
                        Quantidade = not.Quantidade,
                        DataOriginal = not.DataOriginal,
                        Lida = not.Lida,
                        TipoNotificacao = not.TipoNotificacao,
                        InfoAdicional = GetInfoAdicional (not),
                        Destaque = not.Destaque
                    };

                    lstNotificacaoDTO.Add (notificacaoDTO);
                }
                notificacoesClassificadas.NotificacaoDestaque = lstNotificacaoDTO.OrderByDescending (x => x.DataOriginal).FirstOrDefault (x => x.Destaque && !x.Lida);
                notificacoesClassificadas.NotificacoesClassificacao = lstNotificacaoDTO.GroupBy (x => new { x.TipoNotificacao.Ordem, x.TipoNotificacao.Alias })
                    .Select (y => new NotificacaoClassificacaoDTO {
                        Notificacoes = y.OrderByDescending (o => o.DataOriginal).ToList (),
                            TipoNotificacao = y.Key.Ordem,
                            Alias = y.Key.Alias,
                            Ordem = y.Key.Ordem,
                            Quantidade = y.Count ()
                    }).OrderBy (x => x.Ordem).ToList ();

                return notificacoesClassificadas;
            }
        }

        public NotificacaoInfoAdicional GetInfoAdicional (Notificacao notificacao) {
            using (MiniProfiler.Current.Step ("Obtendo informação adicional")) {
                var notificacaoInfoAdicional = new NotificacaoInfoAdicional ();

                switch (notificacao.TipoNotificacao.Id) {
                    case (int) ETipoNotificacao.InformativoLinkExterno:
                        notificacaoInfoAdicional.InfoAdicionalLinkExterno = notificacao.InfoAdicional;
                        break;
                    case (int) ETipoNotificacao.SimuladoOnline:
                        notificacaoInfoAdicional.InfoAdicionalSimulado = GetInfoAdicionalSimulado (notificacao.InfoAdicional);
                        break;
                    case (int) ETipoNotificacao.DuvidasAcademicas:
                        notificacaoInfoAdicional.InfoAdicionalDuvidasAcademicas = new InfoAdicionalDuvidasAcademicas {
                            DuvidaId = notificacao.DuvidaId,
                            TipoRespostaId = notificacao.TipoRespostaId
                        };
                        break;
                    default:
                        break;
                }
                return notificacaoInfoAdicional;
            }
        }

        public InfoAdicionalSimulado GetInfoAdicionalSimulado (string infoAdicional) {
            using (MiniProfiler.Current.Step ("Obtendo informação adicional do simulado")) {
                var infoAdicionalArray = infoAdicional.Split (';');

                int simuladoId;
                int tipoSimuladoId;

                int.TryParse (infoAdicionalArray[0], out simuladoId);
                int.TryParse (infoAdicionalArray[1], out tipoSimuladoId);

                var infoSimulado = new InfoAdicionalSimulado {
                    TipoSimuladoId = tipoSimuladoId,
                    SimuladoId = simuladoId
                };
                return infoSimulado;
            }
        }

        public string GetInfoAdicionalSimuladoSerializado (Notificacao notificacao) {
            using (MiniProfiler.Current.Step ("Obtendo informação adicional do simulado serializado")) {
                NotificacaoInfoAdicional infoAdicional = new NotificacaoInfoAdicional ();
                infoAdicional.InfoAdicionalSimulado = GetInfoAdicionalSimulado (notificacao.InfoAdicional);
                return JsonConvert.SerializeObject (infoAdicional);
            }
        }

        #region ADMIN

        public List<Notificacao> GetNotificacoesAdmin (int idAplicacao) {
            using (MiniProfiler.Current.Step ("Obtendo notificação de administrador")) {
                var notificacoes = _notificacaoRepository.GetNotificacoesAdmin (idAplicacao);

                var notficacoesAgrupadas = notificacoes.GroupBy (n => new {
                    n.AplicacaoId,
                        n.Data,
                        n.DataOriginal,
                        n.EmployeeId,
                        n.IdNotificacao,
                        n.StatusEnvio,
                        n.Texto,
                        n.Titulo
                }).Select (g => new Notificacao {
                    AplicacaoId = g.Key.AplicacaoId,
                        Data = g.Key.Data,
                        DataOriginal = g.Key.DataOriginal,
                        EmployeeId = g.Key.EmployeeId,
                        IdNotificacao = g.Key.IdNotificacao,
                        StatusEnvio = g.Key.StatusEnvio,
                        Texto = g.Key.Texto,
                        Titulo = g.Key.Titulo,
                        RegrasVisualizacao = g.SelectMany (x => x.RegrasVisualizacao).ToList (),
                        TipoNotificacao = g.Select (x => x.TipoNotificacao).FirstOrDefault ()

                }).ToList ();

                return notficacoesAgrupadas;
            }
        }

        public List<PermissaoRegra> GetRegrasAdmin () {
            using (MiniProfiler.Current.Step ("Obtendo regras de administrador")) {
                return _notificacaoRepository.GetRegrasAdmin ();
            }
        }

        public int SetNotificacaoAgendada (Notificacao notificacao) {
            using (MiniProfiler.Current.Step ("Alterando notificação agendada")) {
                return _notificacaoRepository.SetNotificacaoAgendada (notificacao);
            }
        }

        public int UpdateNotificacaoAgendada (Notificacao notificacao) {
            using (MiniProfiler.Current.Step ("Atualizando notificação agendada")) {
                return _notificacaoRepository.UpdateNotificacaoAgendada (notificacao);
            }
        }

        public int DeleteNotificacaoAgendada (int notificacaoId) {
            using (MiniProfiler.Current.Step ("Deletando notificação agendada")) {
                return _notificacaoRepository.DeleteNotificacaoAgendada (notificacaoId);
            }
        }

        #endregion
    }
}