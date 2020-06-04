using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.DTO.Base;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Business
{
    public class AlunoBusiness : BaseBusiness
    {
        private IAlunoEntity _alunoRepository;
        private IClienteEntity _clienteRepository;
        private IConfigData _configRepository;
        private IVersaoAppPermissaoEntityData _versaoAppRepository;
        private ILogOperacoesConcursoData _logOperacoes;

        public AlunoBusiness(IAlunoEntity alunoRepository, IClienteEntity clienteRepository, IConfigData configRepository, IVersaoAppPermissaoEntityData versaoAppRepository)
        {
            _alunoRepository = alunoRepository;
            _clienteRepository = clienteRepository;
            _configRepository = configRepository;
            _versaoAppRepository = versaoAppRepository;
        }

        public AlunoBusiness(IClienteEntity clienteRepository, ILogOperacoesConcursoData logOperacoes)
        {
            _logOperacoes = logOperacoes;
            _clienteRepository = clienteRepository;
        }

        public AlunoBusiness(IAlunoEntity alunoRepository, IClienteEntity clienteRepository)
        {
            _alunoRepository = alunoRepository;
            _clienteRepository = clienteRepository;
        }

        public NotaCorte GetNotaCorte(int provaId, int alunoNota, int matricula)
        {
            var anoProva = _alunoRepository.GetAnoProva(provaId);
            var especialidade = _alunoRepository.GetEspecialidadeAluno(matricula);
            var nota = new NotaCorte
            {
                EspecialidadeAlunoID = Convert.ToInt32(especialidade.Valor),
                Especialidade = especialidade.Descricao
            };
            nota.Especialidades = new Especialidades();
            var concursoID = _alunoRepository.GetConcursoIDByProvaId(provaId);
            var totalQuestoes = _alunoRepository.GetTotalQuestoesByProvaId(provaId);
            alunoNota = (int)(((decimal)alunoNota / (totalQuestoes == 0 ? 1 : totalQuestoes)) * 100);
            nota.Nota = alunoNota;
            var epecialidadeNotas = _alunoRepository.GetEspecialiddesByConcursoIdAnoProva(concursoID, anoProva);
                

            List<Especialidade> especialidades = new List<Especialidade>();

            //foreach (var item in epecialidadeNotas)
            Parallel.ForEach(epecialidadeNotas, item =>
            {
                int? status = null;
                int? notaCorte = null;

                var NotaAnoPosterior = _alunoRepository.GetNotadeCorteAnoPosteriorByNmConcursoAnoProvaEspecialideID(item.txtDescription, anoProva, item.intEspecialidadeID);
                var dblNotaCorte = NotaAnoPosterior;
                try
                {

                    if (!string.IsNullOrEmpty(dblNotaCorte.ToString()))
                    {

                        notaCorte = Convert.ToInt32(dblNotaCorte);
                        status = (int)((decimal)alunoNota / 100 * (notaCorte > 100 && notaCorte <= 1000 ? 1000 : (notaCorte > 10 && notaCorte <= 100 ? 100 : 10))) >= notaCorte ? 1 : 0;
                    }
                }
                catch
                {
                }

                especialidades.Add(new Especialidade
                {
                    Descricao = item.DE_ESPECIALIDADE,
                    NotaCorte = notaCorte,
                    Status = status,
                    Id = Convert.ToInt32(item.intEspecialidadeID)
                });

            });

            nota.Especialidades.AddRange(especialidades.OrderByDescending(n => n.NotaCorte));
            var multiplicador = especialidades.Where(a => a.NotaCorte != null && (a.NotaCorte > 10 && a.NotaCorte <= 100)).Count();
            var multiplicador1000 = especialidades.Where(a => a.NotaCorte != null && (a.NotaCorte > 100 && a.NotaCorte <= 1000)).Count();
            nota.Nota = (int)((decimal)alunoNota / 100 * (multiplicador1000 > 0 ? 1000 : (multiplicador > 0 ? 100 : 10)));
           

            return nota;
        }

        public AlunoMedsoft GetAcessoAluno(string register, string senha, int idAplicacao, string appVersion, string tokenDevice, int idDevice)
        {
            var response = new AlunoMedsoft();

            try
            {
                var funcionarioEntity = new FuncionarioEntity();
                var pessoaEntity = new PessoaEntity(); 
                
                var cliente = new Cliente();
                using(MiniProfiler.Current.Step("Obtendo dados do usuario"))
                {
                    cliente = CacheClienteGetByFilters(register, idAplicacao);
                }

                if (cliente == null || cliente.ID == 0)
                {
                    SetResponse(false,
                       TipoErroAcesso.CadastroInexistente.GetDescription(),
                       _alunoRepository.GetMensagensLogin(idAplicacao, (int)TipoMensagemMEDSOFT.USUARIO_NAO_CADASTRADO),
                       TipoErroAcesso.CadastroInexistente);
                    return response;
                }
                

                var aluno = new AlunoMedsoft();
                using(MiniProfiler.Current.Step("Obtendo informações do tipo de usuário"))
                {
                    var golden = _clienteRepository.UserGolden(cliente.Register, Aplicacoes.MsProMobile);
                    var tipoPessoa = pessoaEntity.GetPersonType(cliente.Register);
                    var tipoPerfil = funcionarioEntity.GetTipoPerfilUsuario(cliente.ID);
                    var isExAluno = false;
                    if (!_alunoRepository.IsAlunoPendentePagamento(cliente.ID))
                    {
                        isExAluno = _alunoRepository.IsExAlunoTodosProdutosCache(cliente.ID);
                    }
                    var tempoInadimplenciaTimeout = new ConfigBusiness(new ConfigEntity()).GetTempoInadimplenciaTimeoutParametro();

                    aluno = new AlunoMedsoft
                    {
                        ID = cliente.ID,
                        Nome = cliente.Nome.TrimEnd(),
                        NickName = cliente.NickName,
                        Register = cliente.Register.TrimEnd(),
                        Senha = cliente.Senha,
                        Foto = cliente.Foto,
                        FotoPerfil = cliente.FotoPerfil,
                        IsGolden = golden > 0,
                        TipoPessoa = tipoPessoa,
                        TipoPerfil = tipoPerfil,
                        ExAluno = isExAluno,
                        tokenLogin = Util.AuthJWT.GeraJwt(cliente.ID, Constants.doisDiasEmMinutos),
                        TempoInadimplenciaTimeout = tempoInadimplenciaTimeout,
                        LstOrdemVendaMsg = cliente.LstOrdemVendaMsg
                    };

                    if (aluno.Senha == string.Empty)
                    {
                        SetResponse(false,
                            TipoErroAcesso.SemSenhaCadastrada.GetDescription(),
                            _alunoRepository.GetMensagensLogin(idAplicacao, (int)TipoMensagemMEDSOFT.CADASTRAR_SENHA_MEDSOFTPRO),
                            TipoErroAcesso.SemSenhaCadastrada);
                        return aluno;
                    }

                    if (!aluno.IsGolden && aluno.Senha != senha)
                    {
                        SetResponse(false,
                        TipoErroAcesso.SenhaIncorreta.GetDescription(),
                        _alunoRepository.GetMensagensLogin(idAplicacao, (int)TipoMensagemMEDSOFT.ACESSO_NEGADO_SENHA_INCORRETA),
                        TipoErroAcesso.SenhaIncorreta);
                        return aluno;
                    }

                    if (cliente.RetornoStatus == Cliente.StatusRetorno.SemAcesso || cliente.RetornoStatus == Cliente.StatusRetorno.Cancelado)
                    {
                        if (string.IsNullOrEmpty(cliente.MensagemRetorno))
                        {
                            SetResponse(false,
                            TipoErroAcesso.SemProdutosContratados.GetDescription(),
                            _alunoRepository.GetMensagensLogin(idAplicacao, (int)TipoMensagemMEDSOFT.SEM_PRODUTOS),
                            TipoErroAcesso.SemProdutosContratados);
                        }
                        else
                        {
                            SetResponse(false, cliente.TituloMensagemRetorno, cliente.MensagemRetorno, cliente.ETipoErro);
                        }
                        return response;
                    }
                    else if (!string.IsNullOrEmpty(cliente.MensagemRetorno))
                    {
                        SetResponse(true, cliente.TituloMensagemRetorno, cliente.MensagemRetorno, cliente.ETipoErro);
                    }
                }

                var produtosPermitidos = new List<Produto.Produtos>();
                var produtosContradados = new List<Produto>();
                using(MiniProfiler.Current.Step("Obtendo informações de produtos contratados"))
                {
                    produtosPermitidos = _alunoRepository.GetProdutosPermitidosLogin(idAplicacao);
                    produtosContradados = ProdutoEntity.GetProdutosContratadosPorAnoCache(cliente.ID, false, 0, true, idAplicacao);
                }
                var anoLetivoAtual = Utilidades.GetYear();
                var anoSeguinte = anoLetivoAtual + 1;
                var anoAnterior = anoLetivoAtual - 1;
                var anoAnteriorAntesDataLimite = Utilidades.IsAntesDatalimiteCache(anoAnterior, idAplicacao);


                var anoDireitoVitalicio = Convert.ToInt32(ConfigurationProvider.Get("Settings:anoComDireitoVitalicio"));
                var anosPermitidos = new List<int>();

                for (var ano = anoDireitoVitalicio; ano <= anoSeguinte; ano++)
                {
                    anosPermitidos.Add(ano);
                }

                if (anoAnteriorAntesDataLimite) anosPermitidos.Add(anoAnterior);

                var hasPermitidos = produtosContradados
                    .Any(p => produtosPermitidos.Contains((Produto.Produtos)p.IDProduto) && anosPermitidos.Contains(p.Ano.Value));

                if (!hasPermitidos) {
                    SetResponse(false,
                        TipoErroAcesso.SemProdutosContratados.GetDescription(),
                        _alunoRepository.GetMensagensLogin(idAplicacao, (int)TipoMensagemMEDSOFT.SEM_PRODUTOS),
                         TipoErroAcesso.SemProdutosContratados);
                    return response;
                }

                bool isVersaoBloqueada = string.IsNullOrEmpty(appVersion) ? _configRepository.GetDeveBloquearAppVersaoNulaCache() : new VersaoAppPermissaoBusiness(_versaoAppRepository).IsVersaoBloqueada(appVersion, idAplicacao);

                if (isVersaoBloqueada)
                {
                    SetResponse(false,
                        TipoErroAcesso.VersaoAppDesatualizada.GetDescription(),
                        _alunoRepository.GetMensagensLogin(idAplicacao, (int)TipoMensagemMEDSOFT.APLICATIVO_DESATUALIZADO),
                        TipoErroAcesso.VersaoAppDesatualizada);
                    return response;
                }

                var permissaoDevice = new PermissaoDevice();
                using(MiniProfiler.Current.Step("Verificando permissões de acesso de usuário"))
                {
                    permissaoDevice = new AlunoEntity().GetPermissaoAcesso(idAplicacao, aluno.ID, tokenDevice, (Utilidades.TipoDevice)idDevice);
                    aluno.PermiteAcesso = (aluno.IsGolden || permissaoDevice.PermiteAcesso == 1);
                    aluno.PermiteTroca = (!aluno.IsGolden && permissaoDevice.PermiteTroca == 1);
                }

                if (!aluno.PermiteAcesso && !aluno.PermiteTroca)
                {
                    SetResponse(false,
                       TipoErroAcesso.DeviceBloqueado.GetDescription(),
                       _alunoRepository.GetMensagensLogin(idAplicacao, (int)TipoMensagemMEDSOFT.DISPOSITIVO_BLOQUEADO),
                       TipoErroAcesso.DeviceBloqueado);
                    return response; 
                }

                if (!aluno.LstOrdemVendaMsg.Any(x => x.PermiteAcesso == 1))
                {
                    SetResponse(false,
                      TipoErroAcesso.BloqueadoInadimplencia.GetDescription(),
                      cliente.MensagemRetorno,
                      TipoErroAcesso.BloqueadoInadimplencia);

                    return response;
                }

                LogLogin log = new LogLogin
                {
                    Matricula = aluno.ID,
                    AplicacaoId = idAplicacao,
                    AcessoId = 0
                };

                new LogEntity().InsertAcessoLogin(log);

                SetResponse(true);
                return aluno;
            }
            catch
            {
                throw;
            }
        }

        public ResponseDTO<AlunoMedsoft> GetAcessoAluno(string register, int idAplicacao, string appVersion)
        {
            var response = new ResponseDTO<AlunoMedsoft>
            {
                Retorno = new AlunoMedsoft()
            };

            try
            {
                var funcionarioEntity = new FuncionarioEntity();
                var pessoaEntity = new PessoaEntity();
                var cliente = new Cliente();
                using(MiniProfiler.Current.Step("Obtendo dados do usuario"))
                {
                    cliente = CacheClienteGetByFilters(register, idAplicacao);
                }

                if (cliente == null || cliente.ID == 0)
                {
                    SetResponse(false,
                       TipoErroAcesso.CadastroInexistente.GetDescription(),
                       _alunoRepository.GetMensagensLogin(idAplicacao, (int)TipoMensagemMEDSOFT.USUARIO_NAO_CADASTRADO),
                       TipoErroAcesso.CadastroInexistente);
                    return response;
                }

                var golden = 0;
                var tipoPessoa = new Pessoa.EnumTipoPessoa();
                var tipoPerfil = new EnumTipoPerfil();
                var isExAluno = false;
                float tempoInadimplenciaTimeout = 0;

                using(MiniProfiler.Current.Step("Obtendo dados do usuario"))
                {
                    golden = _clienteRepository.UserGolden(cliente.Register, Aplicacoes.MsProMobile);
                    tipoPessoa = pessoaEntity.GetPersonType(cliente.Register);
                    tipoPerfil = funcionarioEntity.GetTipoPerfilUsuario(cliente.ID);
                    isExAluno = _alunoRepository.IsExAlunoTodosProdutos(cliente.ID);
                    tempoInadimplenciaTimeout = new ConfigBusiness(new ConfigEntity()).GetTempoInadimplenciaTimeoutParametro();                    
                }


                var aluno = new AlunoMedsoft
                {
                    ID = cliente.ID,
                    Nome = cliente.Nome,
                    NickName = cliente.NickName,
                    Register = cliente.Register,
                    Senha = cliente.Senha,
                    Foto = cliente.Foto,
                    FotoPerfil = cliente.FotoPerfil,
                    IsGolden = golden > 0,
                    TipoPessoa = tipoPessoa,
                    TipoPerfil = tipoPerfil,
                    ExAluno = isExAluno,
                    tokenLogin = Util.AuthJWT.GeraJwt(cliente.ID, Constants.doisDiasEmMinutos),
                    TempoInadimplenciaTimeout = tempoInadimplenciaTimeout,
                    LstOrdemVendaMsg = cliente.LstOrdemVendaMsg
                };

                var anoLetivoAtual = Utilidades.GetYear();
                var anoSeguinte = anoLetivoAtual + 1;
                var anoAnterior = anoLetivoAtual - 1;
                var anoAnteriorAntesDataLimite = Utilidades.IsAntesDatalimiteCache(anoAnterior, idAplicacao);

                var anoDireitoVitalicio = Convert.ToInt32(ConfigurationProvider.Get("Settings:anoComDireitoVitalicio"));
                var anosPermitidos = new List<int>();

                for (var ano = anoDireitoVitalicio; ano <= anoSeguinte; ano++)
                {
                    anosPermitidos.Add(ano);
                }

                if (anoAnteriorAntesDataLimite) anosPermitidos.Add(anoAnterior);

                response.LstOrdemVendaMsg = cliente.LstOrdemVendaMsg;

                response.Retorno = aluno;
                response.Sucesso = true;
                return response;

            }
            catch
            {
                throw;
            }
        }

        public AlunoMedsoft GetPreAcessoAluno(string register, int idAplicacao)
        {
            var response = new AlunoMedsoft();

            try
            {
                var cliente = new Cliente();
                using(MiniProfiler.Current.Step("Obtendo dados do usuario"))
                {
                    cliente = _clienteRepository.GetPreByFilters(new Cliente { Register = register }, aplicacao: (Aplicacoes)idAplicacao).FirstOrDefault();
                }

                if (cliente == null || cliente.ID == 0)
                {
                    SetResponse(false,
                       TipoErroAcesso.CadastroInexistente.GetDescription(),
                       _alunoRepository.GetMensagensLogin(idAplicacao, (int)TipoMensagemMEDSOFT.USUARIO_NAO_CADASTRADO),
                       TipoErroAcesso.CadastroInexistente);
                    return response;
                }

                var golden = _clienteRepository.UserGolden(cliente.Register, Aplicacoes.MsProMobile);

                var aluno = new AlunoMedsoft
                {
                    ID = cliente.ID,
                    Nome = cliente.Nome.TrimEnd(),
                    NickName = cliente.NickName,
                    Register = cliente.Register.TrimEnd(),
                    Senha = cliente.Senha,
                    IsGolden = golden > 0,
                    FotoPerfil = cliente.FotoPerfil,
                    tokenLogin = null,
                    LstOrdemVendaMsg = null
                };

                if (aluno.Senha == string.Empty)
                {
                    SetResponse(false,
                        TipoErroAcesso.SemSenhaCadastrada.GetDescription(),
                        _alunoRepository.GetMensagensLogin(idAplicacao, (int)TipoMensagemMEDSOFT.CADASTRAR_SENHA_MEDSOFTPRO),
                        TipoErroAcesso.SemSenhaCadastrada);
                    return aluno;
                }

                SetResponse(true);
                return aluno;
            }
            catch
            {
                throw;
            }
            
        }

        public PermissaoInadimplencia GetPermissaoInadimplencia(string registro, int aplicativoId, string versaoAplicacao)
        {
            const string MSPRO_VERSAO_BUG_INADIMPLENCIA = "4.4.0";

            var isVersaoMsProMobileComBugInadimplencia = aplicativoId == (int)Aplicacoes.MsProMobile && versaoAplicacao == MSPRO_VERSAO_BUG_INADIMPLENCIA;
           
            if (Utilidades.IsValidEmail(registro))
            {
               var aluno = _clienteRepository.GetAlunoPorFiltros(new AlunoDTO { Email = registro });
                if (aluno != null)
                    registro = aluno.Register;
            }

            var permissao = new PermissaoInadimplencia();
            using(MiniProfiler.Current.Step("Obtendo permissões do usuario"))
            {
                permissao = _alunoRepository.GetPermissao(registro, Convert.ToInt32(aplicativoId));
            }

            if (isVersaoMsProMobileComBugInadimplencia)
            {
                using(MiniProfiler.Current.Step("Removendo OV de inadimplente bloqueado"))
                {
                    permissao = _alunoRepository.RemoveOvInadimplenteBloqueado(permissao);  
                }
            }

            return permissao;
        }

        private Cliente CacheClienteGetByFilters(String register, int idAplicacao)
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.Login.KeyLoginClienteGetByFilters))
            {
                var Key = string.Format("{0}:{1}:{2}", RedisCacheConstants.Login.KeyLoginClienteGetByFilters, register, idAplicacao);
                var ret = RedisCacheManager.GetItemObject<Cliente>(Key);
                if (ret != null)
                {
                    return ret;
                }
                else
                {
                    var cliente = _clienteRepository.GetByFilters(new Cliente { Register = register },
                        aplicacao: (Aplicacoes)Convert.ToInt32(idAplicacao))
                    .FirstOrDefault();

                    RedisCacheManager.SetItemObject(Key, cliente, TimeSpan.FromMinutes(5));
                    return cliente;
                }

            }
            else
            {
                return _clienteRepository.GetByFilters(new Cliente { Register = register },
                        aplicacao: (Aplicacoes)Convert.ToInt32(idAplicacao))
                    .FirstOrDefault();
            }

        }
    }
}