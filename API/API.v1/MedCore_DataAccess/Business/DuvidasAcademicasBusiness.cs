using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Repository;
using MedCore_DataAccess.DTO.DuvidaAcademica;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Contracts.Data;
using System.Linq;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.DTO;
using Medgrupo.DataAccessEntity;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Business
{
    public class DuvidasAcademicasBusiness : IDuvidasAcademicasBusiness
    {
        private const string OrigemOutras = "O";

        private readonly IDuvidasAcademicasData _repository;
        private readonly IFuncionarioData _funcionarioData;
        private readonly IMaterialApostilaData _materialApostila;
        private readonly IConcursoData _concursoRepository;
        private readonly INotificacaoDuvidasAcademicasBusiness _notificacaoBusiness;

        public DuvidasAcademicasBusiness(IDuvidasAcademicasData repository,
            IFuncionarioData funcionarioData,
            IMaterialApostilaData materialApostila,
            IConcursoData concursoRepository,
            INotificacaoDuvidasAcademicasBusiness notificacaoBusiness)
        {
            _repository = repository;
            _funcionarioData = funcionarioData;
            _materialApostila = materialApostila;
            _concursoRepository = concursoRepository;
            _notificacaoBusiness = notificacaoBusiness;
        }

        #region Delete

        public int DeleteDuvida(DuvidaAcademicaInteracao interacao)
        {
            var filtro = new DuvidaAcademicaFiltro()
            {
                ClientId = interacao.ClientId,
                DuvidaId = interacao.DuvidaId
            };

            using(MiniProfiler.Current.Step("Deletando duvidas academicas e suas dependencias."))
            {
                var duvida = _repository.GetDuvidas(filtro).FirstOrDefault();

                if(duvida != null)
                {
                    if(duvida.ApostilaId != null && duvida.CodigoMarcacao != null)
                    {
                        var materialApostilaAlunoManager = new MaterialApostilaAlunoManager();
                        var materialApostilaAluno = _materialApostila.GetMaterialApostilaAluno(duvida.ClientId, duvida.ApostilaId.Value);
                        var apostilaVersao = Utilidades.GetDetalhesApostila(materialApostilaAluno);
                        string chave = Utilidades.CriarNomeApostila(duvida.ClientId, duvida.ApostilaId.Value, apostilaVersao.Versao);
                        var conteudo = materialApostilaAlunoManager.ObterArquivo(chave);

                        conteudo = Utilidades.RemoveMarcacaoApostila(Constants.COMP_DUVIDA_APOSTILA, conteudo, duvida.CodigoMarcacao);

                        _materialApostila.PostModificacaoApostila(duvida.ClientId, duvida.ApostilaId.Value, conteudo);
                    }
                    return _repository.DeleteDuvida(interacao);
                }
            }
            return 0;
        }

        public int DeleteDuvidaApostilaPorMarcacao(DuvidaAcademicaInteracao interacao)
        {
            using(MiniProfiler.Current.Step("Deletando apostila dúvidas academicas por marcação."))
            {
                return _repository.DeleteDuvidaApostilaPorMarcacao(interacao);
            }
        }

        public int DeleteRespostaReplica(DuvidaAcademicaInteracao interacao)
        {
            using(MiniProfiler.Current.Step("Deletando respostas e replicas, dúvidas acadêmicas."))
            {
                return _repository.DeleteRespostaReplica(interacao);
            }
        }

        public IList<DuvidaAcademicaContract> GetDuvidas(DuvidaAcademicaFiltro filtroPost)
        {
            using(MiniProfiler.Current.Step("Carregando lista de dúvidas baseado em filtro."))
            {
                filtroPost.DuvidaId = string.IsNullOrEmpty(filtroPost.DuvidaId) ? "0" : filtroPost.DuvidaId;
                filtroPost.QuestaoId = string.IsNullOrEmpty(filtroPost.QuestaoId) ? "0" : filtroPost.QuestaoId;
                var tipoUsuario = _funcionarioData.GetTipoPerfilUsuario(filtroPost.ClientId);
                filtroPost.IsAcademico = (tipoUsuario == EnumTipoPerfil.Professor || tipoUsuario == EnumTipoPerfil.Coordenador);
                filtroPost.IsCoordenador = tipoUsuario == EnumTipoPerfil.Coordenador;
                
                IList<DuvidaAcademicaContract> result = new List<DuvidaAcademicaContract>();
                if (tipoUsuario != EnumTipoPerfil.Professor && tipoUsuario != EnumTipoPerfil.Coordenador && tipoUsuario != EnumTipoPerfil.Master)
                {
                    if (filtroPost.IdsMateriais != null && filtroPost.IdsMateriais.Any())
                        result = _repository.GetDuvidasAlunoApostila(filtroPost);
                    else if ((filtroPost.QuestaoId != null && Convert.ToInt32(filtroPost.QuestaoId) > 0) ||
                        (filtroPost.IdsSimulados != null && filtroPost.IdsSimulados.Any()) ||
                        (filtroPost.SiglasConcurso != null && filtroPost.SiglasConcurso.Any()) ||
                        (filtroPost.IdsApostilas != null && filtroPost.IdsApostilas.Any()) || filtroPost.ConcursoId != null)
                        result = _repository.GetDuvidasAlunoQuestoes(filtroPost);
                    else
                        result = _repository.GetDuvidasAluno(filtroPost);
                }
                else
                {
                    result = _repository.GetDuvidas(filtroPost);
                }

                var dataAtual = DateTime.Now;
                foreach (var item in result)
                {
                    if (tipoUsuario == EnumTipoPerfil.Professor || tipoUsuario == EnumTipoPerfil.Coordenador || tipoUsuario == EnumTipoPerfil.Master)
                        item.OrigemCompleta = item.OrigemProduto != null ? string.Format("{0} - {1} | {2}", item.OrigemSubnivel, item.Origem, item.OrigemProduto) : null;

                    if (dataAtual > item.DataCriacao.AddDays(Constants.MINIMODIASOCIOSA))
                    {
                        item.Data = "";
                    }
                    else
                    {
                        item.Data = Utilidades.GetTempoDecorrido(item.DataCriacao);
                    }
                    item.ProfessoresEncaminhados = item.ProfessoresEncaminhados.Select(x => new tblDuvidasAcademicas_DuvidasEncaminhadas()
                    {
                        intEmployeeID = x.intEmployeeID
                    });
                }

                return result;
            }
        }

        public IList<DuvidasAcademicasProfessorDTO> GetDuvidasProfessor(int idProfessor)
        {
            using(MiniProfiler.Current.Step("Carregando dúvidas acadêmicas utilizando o filtro por professor."))
            {
                DuvidaAcademicaFiltro filtro = new DuvidaAcademicaFiltro();
                if (idProfessor > 0)
                {
                    filtro.ClientId = idProfessor;
                    var tipoUsuario = _funcionarioData.GetTipoPerfilUsuario(filtro.ClientId);
                    filtro.IsAcademico = (tipoUsuario == EnumTipoPerfil.Professor || tipoUsuario == EnumTipoPerfil.Coordenador);
                    filtro.IsCoordenador = tipoUsuario == EnumTipoPerfil.Coordenador;
                    filtro.MinhasApostilas = true;
                    var duvidasProfessor = _repository.GetDuvidasProfessor(filtro);
                    foreach (var duvida in duvidasProfessor)
                    {
                        duvida.EntidadeEspecialidade = (duvida.EntidadeConcurso ?? duvida.EntidadeSimulado ?? duvida.EspecialidadeConcurso ?? duvida.EspecialidadeSimulado ?? duvida.EntidadeApostilaDescricao ?? null);
                    }
                    return duvidasProfessor;
                }
                else
                {
                    var duvidas = new List<DuvidasAcademicasProfessorDTO>();
                    var professores = _repository.GetProfessores();
                    foreach(var professor in professores)
                    {
                        filtro.ClientId = professor.Id.Value;
                        var tipoUsuario = _funcionarioData.GetTipoPerfilUsuario(professor.Id.Value);
                        filtro.IsAcademico = (tipoUsuario == EnumTipoPerfil.Professor || tipoUsuario == EnumTipoPerfil.Coordenador);
                        filtro.IsCoordenador = tipoUsuario == EnumTipoPerfil.Coordenador;
                        filtro.MinhasApostilas = true;
                        var duvidasProfessor = _repository.GetDuvidasProfessor(filtro);
                        foreach(var duvida in duvidasProfessor)
                        {
                            duvida.EntidadeEspecialidade = (duvida.EntidadeConcurso ?? duvida.EntidadeSimulado ?? duvida.EspecialidadeConcurso ?? duvida.EspecialidadeSimulado ?? duvida.EntidadeApostilaDescricao ?? null);
                        }

                        duvidas.AddRange(duvidasProfessor);
                    } 
                    return duvidas;
                }
            }
        }

        public tblDuvidasAcademicas_Resposta GetResposta(int idResposta)
        {
            using(MiniProfiler.Current.Step("Obtendo resposta da duvida academica pelo identificador da resposta."))
            {
                return _repository.GetResposta(idResposta);
            }
        }

        public IList<DuvidaAcademicaContract> GetRespostasPorDuvida(DuvidaAcademicaFiltro filtro)
        {
            using(MiniProfiler.Current.Step("Obtendo respostas filtradas por dúvidas acadêmicas."))
            {
                var result = _repository.GetRespostasPorDuvida(filtro);
                filtro.Page = Constants.PAGINA_INICIAL_CONSULTA;
                filtro.QuantidadeReplicas = Constants.QUANTIDADE_MINIMA_DUVIDASREPLICA;

                foreach(var item in result)
                {
                    filtro.RespostaId = item.RespostaId;
                    var obj = GetReplicasResposta(filtro);
                    item.Replicas = obj.Replicas;
                    item.Data = Utilidades.GetTempoDecorrido(item.DataCriacao);
                    item.QuantidadeReplicas = obj.QuantidadeReplicas;
                    item.NomeAlunoCompleto = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.NomeAlunoCompleto.Trim().ToLower());
                }
                return result;
            }
        }

        public DuvidaAcademicaReplicaResponse GetReplicasResposta(DuvidaAcademicaFiltro filtroPost)
        {
            using(MiniProfiler.Current.Step("Obtendo réplicas de respostas filtrado por dúvidas acadêmicas."))
            {
                if(filtroPost.QuantidadeReplicas == 0)
                {
                    filtroPost.QuantidadeReplicas = Constants.DEZ_PRIMEIRAS_REPLICAS;
                }

                var result = _repository.GetReplicasResposta(filtroPost);

                if(filtroPost.Page == Constants.PAGINA_INICIAL_CONSULTA && filtroPost.QuantidadeReplicas > 1 && result.Replicas.Count > 1)
                {
                    result.Replicas.RemoveAt(0);
                }

                foreach(var item in result.Replicas)
                {
                    item.Data = Utilidades.GetTempoDecorrido(item.DataCriacao);
                }
                return result;
            }
        }

        public string GetTrechoApostilaSelecionado(int duvidaId)
        {
            using(MiniProfiler.Current.Step("Obtendo trecho selecionado de apostila pelo identificador da dúvida."))
            {
                return _repository.GetTrechoApostilaSelecionado(duvidaId);
            }
        }

        public List<CronogramaSimplificadoDTO> GetCronogramaSimplificado(int idProduto, int matricula, bool isQuestao)
        {
            using(MiniProfiler.Current.Step("Obter cronograma simplificado."))
            {
                var cronograma = new List<CronogramaSimplificadoDTO>();
                if (isQuestao)
                {
                    cronograma = _repository.GetExerciciosDuvidasQuestao();

                    var listaQuestoes = cronograma.GroupBy(x => x.IdEntidade)
                        .Select(y => y.First())
                        .OrderBy(y => y.Nome)
                        .ToList();

                    return listaQuestoes;
                }
                else
                {
                    cronograma = _repository.GetProdutoIdDuvidasApostila();

                    var listaMateriais = cronograma.GroupBy(x => x.MaterialId)
                        .Select(y => y.First())
                        .OrderBy(y => y.Nome)
                        .ToList();

                    return listaMateriais;
                }
            }
        }

        public DuvidaAcademicaDTO GetDuvida(int DuvidaId)
        {
            using(MiniProfiler.Current.Step("Obtendo dúvida acadêmica pelo identificador."))
            {
                var duvida = _repository.GetDuvida(DuvidaId);
                if (duvida.DuvidaId > 0)
                    return duvida;
                return null;
            }
        }

        public List<ConcursoDTO> GetConcursoProva(int matricula, int idaplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo prova do concurso."))
            {
                var duvidasConcurso = _repository.GetDuvidasConcurso();
                var idsProvas = duvidasConcurso.Select(x => x.ExercicioId.Value).ToList();
                var concursos = _concursoRepository.GetConcursosPorProvas(matricula, idaplicacao, idsProvas);
                foreach (var concurso in concursos)
                {
                    var provas = _concursoRepository.GetProvas(concurso.Sigla, matricula);
                    concurso.IdsProva = provas.Select(x => x.ID);
                }
                return concursos;
            }
        }

        public List<ExercicioDTO> GetAllSimulados(int matricula, int idAplicacao = 1)
        {
            using(MiniProfiler.Current.Step("Obtendo todos o simulados"))
            {
                var exercicioRepository = new ExercicioEntity();
                        
                var list = new List<Exercicio>();
                var exercicios = exercicioRepository.GetSimulados(matricula, idAplicacao);
                var simulados =  GetSimuladosDistintosAgrupados(exercicios);
                var duvidasSimulado = _repository.GetExerciciosDuvidasQuestao();
                var idsExercicios = duvidasSimulado.Select(x => x.IdEntidade).ToList();

                return simulados.Where(w => idsExercicios.Contains(w.ID))
                                    .OrderByDescending(x => x.Ano).ToList();
            }
        }

        private IEnumerable<ExercicioDTO> GetSimuladosDistintosAgrupados(IEnumerable<Exercicio> simulados)
        {
            using(MiniProfiler.Current.Step("Obtendo simulados distintos agrupados."))
            {
                var simuladosAgrupados = simulados.Select(s => new ExercicioDTO
                {
                    Ano = s.Ano,
                    Descricao = s.Descricao,
                    ID = s.ID,
                    ExercicioName = s.ExercicioName,
                    DataInicio = s.DataInicio,
                    DataFim = s.DataFim,
                    IdTipoRealizacao = 1,
                    Online = s.Online,
                    Ativo = s.Ativo,
                    Duracao = s.Duracao,
                    DtLiberacaoRanking = s.DtLiberacaoRanking
                }).Distinct(new ExercicioDTO()).ToList();

                return simuladosAgrupados;
            }
        }

        #endregion

        #region Inserts

        public int InsertDuvida(DuvidaAcademicaInteracao interacao)
        {
            using(MiniProfiler.Current.Step("Inserindo interação dúvida acadêmica."))
            {
                interacao.EstadoAluno = Utilidades.GetEstadoCursoAluno(interacao.ClientId);
                var novaDuvida = interacao.DuvidaId != null;

                if (interacao.ApostilaId > 0)
                {
                    interacao.DuvidaId = InsertDuvidaApostila(interacao);
                }
                else if (Convert.ToInt32(interacao.QuestaoId) > 0)
                {
                    interacao.DuvidaId = InsertDuvidaQuestao(interacao);
                }
                else
                {
                    interacao.DuvidaId = InsertDuvidaGeral(interacao);
                }

                var denunciada = ValidarDenunciaAutomatica(interacao);
                if(denunciada)
                {
                    interacao.BitAtivaDesenv = false;
                    _repository.UpdateDuvida(interacao);
                }
                else if (!denunciada && Convert.ToInt32(interacao.DuvidaId) > 0 && novaDuvida)
                {
                    interacao.BitAtivaDesenv = true;
                    _repository.UpdateDuvida(interacao);
                }

                return Convert.ToInt32(interacao.DuvidaId);
            }
        }

        private string InsertDuvidaGeral(DuvidaAcademicaInteracao interacao)
        {
            using(MiniProfiler.Current.Step("Inserindo dúvida geral."))
            {
                if (interacao.DuvidaId == null)
                {
                    return _repository.InsertDuvida(interacao).ToString();
                }
                else
                {
                    return _repository.UpdateDuvida(interacao).ToString();
                }
            }
        }

        private string InsertDuvidaQuestao(DuvidaAcademicaInteracao interacao)
        {
            using(MiniProfiler.Current.Step("Inserindo questão."))
            {
                if (interacao.TipoExercicioId == (int)Exercicio.tipoExercicio.SIMULADO)
                {
                    var questaoAcademico = _repository.GetQuestaoConcurso(Convert.ToInt32(interacao.QuestaoId));
                    if(questaoAcademico != null)
                    {
                        interacao.ConcursoQuestaoId = questaoAcademico.QuestaoConcursoId;
                        interacao.EspecialidadeId = questaoAcademico.EspecialidadeId;
                    }
                }

                if (interacao.DuvidaId == null)
                {
                    return _repository.InsertDuvidaQuestao(interacao).ToString();
                }
                else
                {
                    return _repository.UpdateDuvida(interacao).ToString();
                }
            }
        }

        private string InsertDuvidaApostila(DuvidaAcademicaInteracao interacao)
        {
            using(MiniProfiler.Current.Step("Inserindo apostila."))
            {
                if(interacao.DuvidaId == null)
                {
                    return _repository.InsertDuvidaApostila(interacao).ToString();
                }
                else
                {
                    return _repository.UpdateDuvida(interacao).ToString();
                }
            }
        }

        public DuvidaAcademicaContract InsertReplica(DuvidaAcademicaInteracao interacao)
        {
            using(MiniProfiler.Current.Step("Inserir réplica."))
            {
                interacao.EstadoAluno = Utilidades.GetEstadoCursoAluno(interacao.ClientId);
                var denunciada = ValidarDenunciaAutomatica(interacao);
                if (interacao.RespostaId == null)
                {
                    interacao.RespostaId = _repository.InsertRespostaReplica(interacao);
                    if (denunciada)
                    {
                        interacao.BitAtivaDesenv = false;
                        _repository.UpdateRespostaReplica(interacao);
                    }
                    else
                    {
                        var duvida = _repository.GetDuvida(Convert.ToInt32(interacao.DuvidaId));
                        var alunosSeguem = _repository.ListarUsuariosFavoritaramDuvida(Convert.ToInt32(interacao.DuvidaId), duvida.ClientId, interacao.ClientId);
                        var respostaClientId = GetResposta(interacao.RespostaParentId.Value).intClientID;

                        var duvidaContract = new DuvidaAcademicaContract()
                        {
                            DuvidaId = Convert.ToInt32(interacao.DuvidaId),
                            ClientId = duvida.ClientId,
                            Descricao = duvida.Descricao,
                            Origem = duvida.Origem,
                            OrigemSubnivel = duvida.OrigemSubnivel
                        };

                        var AlunoReplicouPropriaResposta = respostaClientId == interacao.ClientId;

                        if (AlunoReplicouPropriaResposta)
                        {
                            respostaClientId = 0;
                        }

                        //_notificacaoBusiness.SetNotificacao(duvidaContract, alunosSeguem, new List<int>(), EnumTipoNotificacaoDuvidasAcademicas.Replica, respostaClientId);
                    }
                }
                else
                {
                    if (denunciada)
                    {
                        interacao.BitAtivaDesenv = false;
                    }
                    else if (!denunciada && interacao.RespostaId > 0)
                    {
                        interacao.BitAtivaDesenv = true;
                        _repository.UpdateRespostaReplica(interacao);
                    }
                    var result = _repository.UpdateRespostaReplica(interacao);
                }

                var filtro = new DuvidaAcademicaFiltro()
                {
                    ClientId = interacao.ClientId,
                    DuvidaId = interacao.DuvidaId,
                    RespostaId = interacao.RespostaParentId,
                    Page = Constants.PAGINA_INICIAL_CONSULTA,
                    QuantidadeDuvidas = Constants.QUANTIDADE_MINIMA_DUVIDASREPLICA
                };

                var replica = _repository.GetReplica(filtro);
                if (replica != null)
                {
                    replica.Data = Utilidades.GetTempoDecorrido(replica.DataCriacao);
                    return replica;
                }

                return null;
            }
        }

        public DuvidaAcademicaContract InsertResposta(DuvidaAcademicaInteracao interacao)
        {
            using(MiniProfiler.Current.Step("Inserindo Resposta."))
            {
                interacao.EstadoAluno = Utilidades.GetEstadoCursoAluno(interacao.ClientId);
                var tipoUsuario = _funcionarioData.GetTipoPerfilUsuario(interacao.ClientId);
                interacao.RespostaMedGrupo = (tipoUsuario == EnumTipoPerfil.Professor || tipoUsuario == EnumTipoPerfil.Coordenador) && interacao.NomeFake == null && interacao.EstadoFake == null;
                var denunciada = ValidarDenunciaAutomatica(interacao);
                if (interacao.RespostaId == null)
                {
                    interacao.RespostaId = _repository.InsertRespostaReplica(interacao);
                    if (denunciada)
                    {
                        interacao.BitAtivaDesenv = false;
                        _repository.UpdateRespostaReplica(interacao);
                    }
                    else
                    {
                        var tipo = interacao.RespostaMedGrupo ? EnumTipoNotificacaoDuvidasAcademicas.RespostaMedGrupo : EnumTipoNotificacaoDuvidasAcademicas.Resposta;
                        var duvida = _repository.GetDuvida(Convert.ToInt32(interacao.DuvidaId));
                        var alunosSeguem = _repository.ListarUsuariosFavoritaramDuvida(Convert.ToInt32(interacao.DuvidaId), duvida.ClientId, interacao.ClientId);
                        var alunosInteracao = _repository.ListarUsuariosResponderamDuvida(Convert.ToInt32(interacao.DuvidaId), interacao.ClientId);

                        var duvidaContract = new DuvidaAcademicaContract()
                        {
                            DuvidaId = Convert.ToInt32(interacao.DuvidaId),
                            ClientId = duvida.ClientId,
                            Descricao = duvida.Descricao,
                            Origem = duvida.Origem,
                            OrigemSubnivel = duvida.OrigemSubnivel
                        };
                        //_notificacaoBusiness.SetNotificacao(duvidaContract, alunosSeguem, alunosInteracao, tipo, null);
                    }
                }
                else
                {
                    if(denunciada)
                    {
                        interacao.BitAtivaDesenv = false;
                    }
                    else if(!denunciada && interacao.RespostaId > 0)
                    {
                        interacao.BitAtivaDesenv = true;
                    }
                    _repository.UpdateRespostaReplica(interacao);
                }

                var filtro = new DuvidaAcademicaFiltro()
                {
                    ClientId = interacao.ClientId,
                    DuvidaId = interacao.DuvidaId,
                    Page = Constants.PAGINA_INICIAL_CONSULTA,
                    QuantidadeDuvidas = Constants.QUANTIDADE_MINIMA_DUVIDASREPLICA
                };

                var idResposta = interacao.RespostaParentId > 0 ? interacao.RespostaParentId : interacao.RespostaId;
                var objResposta = GetRespostasPorDuvida(filtro).FirstOrDefault(x => x.RespostaId == idResposta);
                return objResposta;
            }
        }

        public DuvidaAcademicaContract InsertObservacaoMedGrupo(DuvidaAcademicaInteracao interacao)
        {
            using(MiniProfiler.Current.Step("Inserindo obserevação MedGrupo."))
            {
                _repository.UpdateObservacaoMedGrupo(interacao);

                var filtro = new DuvidaAcademicaFiltro()
                {
                    ClientId = interacao.ClientId,
                    DuvidaId = interacao.DuvidaId,
                    Page = Constants.PAGINA_INICIAL_CONSULTA,
                    QuantidadeDuvidas = Constants.QUANTIDADE_MINIMA_DUVIDASREPLICA
                };
                var objResposta = GetRespostasPorDuvida(filtro);
                var respostaObservacao = objResposta.FirstOrDefault(x => x.RespostaId == interacao.RespostaId);
                return respostaObservacao;
            }
        }

        public int InsertInteracao(DuvidaAcademicaInteracao interacao)
        {
            using(MiniProfiler.Current.Step("Inserindo interação de dúvida acadêmica."))
            {
                var hasInteracao = _repository.GetInteracao(interacao);
                if (hasInteracao != null)
                {
                    if(interacao.RespostaId != null && (interacao.TipoInteracao == (int)TipoInteracaoDuvida.Upvote || interacao.TipoInteracao == (int)TipoInteracaoDuvida.Downvote) && interacao.TipoInteracao != hasInteracao.intTipoInteracaoId)
                    {
                        _repository.DeleteInteracao(hasInteracao);   
                    }
                    else
                    {
                        return _repository.DeleteInteracao(hasInteracao);
                    }
                }

                return _repository.InsertInteracao(interacao);
            }
        }

        public bool SetDenuncia(DenunciaDuvidasAcademicasDTO obj)
        {
            using(MiniProfiler.Current.Step("Cria denúncia deletando qualquer duplicata."))
            {
                var temDenuncia = _repository.DeleteDenuncia(obj);
                return temDenuncia ? temDenuncia : _repository.InsertDenuncia(obj);
            }
        }

        public int InsertDuvidaLida(DuvidaAcademicaInteracao interacao)
        {
            using(MiniProfiler.Current.Step("Marcando dúvida como lida."))
            {
                return _repository.InsertDuvidaLida(new tblDuvidasAcademicas_Lidas
                {
                    intClientID = interacao.ClientId,
                    intDuvidaID = Convert.ToInt32(interacao.DuvidaId),
                    dteCriacao = DateTime.Now
                });
            }
        }

        public int InsertDuvidasEncaminhadas(DuvidaAcademicaInteracao duvidaInteracao)
        {
            using(MiniProfiler.Current.Step("Inserindo dúvidas encaminhadas"))
            {
                return _repository.InsertDuvidasEncaminhadas(duvidaInteracao);
            }
        }

        public bool SetRespostaHomologada(DuvidaAcademicaInteracao interacao)
        {
            using(MiniProfiler.Current.Step("Atribui respostas homologada"))
            {
                interacao.AprovacaoMedGrupo = true;
                var result = _repository.SetRespostaHomologada(interacao);
                if (result > 0)
                {
                    var duvida = _repository.GetDuvida(Convert.ToInt32(interacao.DuvidaId));
                    var alunosSeguem = _repository.ListarUsuariosFavoritaramDuvida(Convert.ToInt32(interacao.DuvidaId), duvida.ClientId, interacao.ClientId);
                    var alunosInteragiram = _repository.ListarUsuariosResponderamDuvida(Convert.ToInt32(interacao.DuvidaId), interacao.ClientId);

                    var duvidaContract = new DuvidaAcademicaContract()
                    {
                        DuvidaId = Convert.ToInt32(interacao.DuvidaId),
                        ClientId = duvida.ClientId,
                        Descricao = duvida.Descricao,
                        Origem = duvida.Origem,
                        OrigemSubnivel = duvida.OrigemSubnivel
                    };
                    //_notificacaoBusiness.SetNotificacao(duvidaContract, alunosSeguem, alunosInteragiram, EnumTipoNotificacaoDuvidasAcademicas.Homologada, null);
                    var hasHomologada = _repository.HasRespostaHomologada(Convert.ToInt32(interacao.DuvidaId));
                    return hasHomologada;
                }
                return false;
            }
        }

        public int SetDuvidaArquivada(DuvidaAcademicaInteracao duvidaInteracao)
        {
            using(MiniProfiler.Current.Step("Atribui dúvida arquivada"))
            {
                var entity = _repository.GetDuvidaArquivada(duvidaInteracao);
                if(entity == null)
                {
                    return _repository.SetDuvidaArquivada(duvidaInteracao);
                }
                else
                {
                    return _repository.DeleteDuvidaArquivada(entity);
                }
            }
        }

        public bool SetDuvidaAcademicaPrivada(DuvidasRespostaPrivadaDTO obj)
        {
            using(MiniProfiler.Current.Step("Atribuir dúvida acadêmica privada."))
            {
                return _repository.SetDuvidaAcademicaPrivada(obj);
            }
        }

        public bool SetRespostaReplicaPrivada(DuvidasRespostaPrivadaDTO obj)
        {
            using(MiniProfiler.Current.Step("Atribuir réplicas privadas."))
            {
                return _repository.SetRespostaReplicaPrivada(obj);
            }
        }


        #endregion

        #region Helpers       

        public void EnviarEmails()
        {
            EnviarEmailsCoordenadores();
            EnviarEmailsProfessores();
        }

        public bool EnviarEmailsCoordenadores()
        {
            var demaisDuvidasItems = new List<DAEmailItemDTO>();

            var coordenadores = _repository.GetCoordenadores();
            var professores = _repository.GetProfessores();
            var duvidasGeral = GetDuvidasProfessor(0).ToList();
            
            foreach(var professor in professores)
            {
                var demaisDuvidaItem = new DAEmailItemDTO();
                demaisDuvidaItem.DuvidasResolvidas = new Dictionary<int, int>();
                demaisDuvidaItem.Professor = professor.Nome;
                demaisDuvidaItem.Menos2Dias = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.DataOriginal.Date >= DateTime.Now.Date.AddDays(-2)); 
                demaisDuvidaItem.Entre2e7Dias = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.DataOriginal.Date >= DateTime.Now.Date.AddDays(-7) && DateTime.Now.Date.AddDays(-2) > x.DataOriginal.Date);
                demaisDuvidaItem.Mais7Dias = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && DateTime.Now.Date.AddDays(-7) > x.DataOriginal.Date);
                demaisDuvidaItem.Total = demaisDuvidaItem.Menos2Dias + demaisDuvidaItem.Entre2e7Dias + demaisDuvidaItem.Mais7Dias;
                demaisDuvidaItem.Encaminhadas = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.BitEncaminhada);
                demaisDuvidaItem.Questoes = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade != null && !x.MinhaDuvidaApostila);
                demaisDuvidaItem.Apostilas = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.MinhaDuvidaApostila);
                demaisDuvidaItem.SemVinculo = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == null && !x.MinhaDuvidaApostila);
                demaisDuvidaItem.TotalOrigem = demaisDuvidaItem.Questoes + demaisDuvidaItem.Apostilas + demaisDuvidaItem.SemVinculo;

                demaisDuvidaItem.PrimeirasMenos2Dias = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.DataOriginal.Date >= DateTime.Now.Date.AddDays(-2));
                demaisDuvidaItem.PrimeirasEntre2e7Dias = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.DataOriginal.Date >= DateTime.Now.Date.AddDays(-7) && DateTime.Now.Date.AddDays(-2) > x.DataOriginal.Date);
                demaisDuvidaItem.PrimeirasMais7Dias = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && DateTime.Now.Date.AddDays(-7) > x.DataOriginal.Date);
                demaisDuvidaItem.PrimeirasTotal = demaisDuvidaItem.PrimeirasMenos2Dias + demaisDuvidaItem.PrimeirasEntre2e7Dias + demaisDuvidaItem.PrimeirasMais7Dias;
                demaisDuvidaItem.PrimeirasEncaminhadas = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.BitEncaminhada);
                demaisDuvidaItem.PrimeirasQuestoes = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade != null && !x.MinhaDuvidaApostila);
                demaisDuvidaItem.PrimeirasApostilas = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.MinhaDuvidaApostila);
                demaisDuvidaItem.PrimeirasSemVinculo = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == null && !x.MinhaDuvidaApostila);
                demaisDuvidaItem.PrimeirasTotalOrigem = demaisDuvidaItem.PrimeirasQuestoes + demaisDuvidaItem.PrimeirasApostilas + demaisDuvidaItem.PrimeirasSemVinculo;

                demaisDuvidaItem.TotalGeralDias = demaisDuvidaItem.Total + demaisDuvidaItem.PrimeirasTotal;
                demaisDuvidaItem.TotalGeralOrigem = demaisDuvidaItem.TotalOrigem + demaisDuvidaItem.PrimeirasTotalOrigem;

                var duvidasResolvidas = _repository.GetResolvidosProfessor(professor.Id.Value);
                for (int i = 1; i <= 12; i++)
                {
                    var quantidade = duvidasResolvidas.Count(x => x.DataCriacao.Month == i && x.DataCriacao.Year == DateTime.Now.Year);
                    demaisDuvidaItem.DuvidasResolvidas.Add(i, quantidade);
                    demaisDuvidaItem.TotaisRespondidas = demaisDuvidaItem.DuvidasResolvidas.Sum(x => x.Value);
                }
                demaisDuvidasItems.Add(demaisDuvidaItem);
            }

            var duvidasUnicas = new DAEmailItemDTO();

            demaisDuvidasItems = demaisDuvidasItems.OrderByDescending(x => x.TotalGeralDias).ToList();

            var duvidasUnificadas = duvidasGeral.GroupBy(x => new {
                x.DuvidaId,
                x.PrimeirasDuvidas,
                x.DataOriginal,
                x.BitEncaminhada,
                x.MinhaDuvidaApostila,
                x.EntidadeEspecialidade
            }).Select(y => new { y.Key.DuvidaId, y.Key.DataOriginal, y.Key.PrimeirasDuvidas, y.Key.MinhaDuvidaApostila, y.Key.EntidadeEspecialidade, y.Key.BitEncaminhada }).OrderBy(z => z.DuvidaId).ToList();

            duvidasUnicas.Menos2Dias = duvidasUnificadas.Count(x => !x.PrimeirasDuvidas && x.DataOriginal.Date >= DateTime.Now.Date.AddDays(-2));
            duvidasUnicas.Entre2e7Dias = duvidasUnificadas.Count(x => !x.PrimeirasDuvidas && x.DataOriginal.Date >= DateTime.Now.Date.AddDays(-7) && DateTime.Now.Date.AddDays(-2) > x.DataOriginal.Date);
            duvidasUnicas.Mais7Dias = duvidasUnificadas.Count(x => !x.PrimeirasDuvidas && DateTime.Now.Date.AddDays(-7) > x.DataOriginal.Date);
            duvidasUnicas.Total = duvidasUnicas.Menos2Dias + duvidasUnicas.Entre2e7Dias + duvidasUnicas.Mais7Dias;
            duvidasUnicas.Encaminhadas = duvidasUnificadas.Count(x => !x.PrimeirasDuvidas && x.BitEncaminhada);
            duvidasUnicas.Questoes = duvidasUnificadas.Count(x => !x.PrimeirasDuvidas && x.EntidadeEspecialidade != null && !x.MinhaDuvidaApostila);
            duvidasUnicas.Apostilas = duvidasUnificadas.Count(x => !x.PrimeirasDuvidas && x.MinhaDuvidaApostila);
            duvidasUnicas.SemVinculo = duvidasUnificadas.Count(x => !x.PrimeirasDuvidas && x.EntidadeEspecialidade == null && !x.MinhaDuvidaApostila);
            duvidasUnicas.TotalOrigem = duvidasUnicas.Questoes + duvidasUnicas.Apostilas + duvidasUnicas.SemVinculo;

            duvidasUnicas.PrimeirasMenos2Dias = duvidasUnificadas.Count(x => x.PrimeirasDuvidas && x.DataOriginal.Date >= DateTime.Now.Date.AddDays(-2));
            duvidasUnicas.PrimeirasEntre2e7Dias = duvidasUnificadas.Count(x => x.PrimeirasDuvidas && x.DataOriginal.Date >= DateTime.Now.Date.AddDays(-7) && DateTime.Now.Date.AddDays(-2) > x.DataOriginal.Date);
            duvidasUnicas.PrimeirasMais7Dias = duvidasUnificadas.Count(x => x.PrimeirasDuvidas && DateTime.Now.Date.AddDays(-7) > x.DataOriginal.Date);
            duvidasUnicas.PrimeirasTotal = duvidasUnicas.PrimeirasMenos2Dias + duvidasUnicas.PrimeirasEntre2e7Dias + duvidasUnicas.PrimeirasMais7Dias;
            duvidasUnicas.PrimeirasEncaminhadas = duvidasUnificadas.Count(x => x.PrimeirasDuvidas && x.BitEncaminhada);
            duvidasUnicas.PrimeirasQuestoes = duvidasUnificadas.Count(x => x.PrimeirasDuvidas && x.EntidadeEspecialidade != null && !x.MinhaDuvidaApostila);
            duvidasUnicas.PrimeirasApostilas = duvidasUnificadas.Count(x => x.PrimeirasDuvidas && x.MinhaDuvidaApostila);
            duvidasUnicas.PrimeirasSemVinculo = duvidasUnificadas.Count(x => x.PrimeirasDuvidas && x.EntidadeEspecialidade == null && !x.MinhaDuvidaApostila);
            duvidasUnicas.PrimeirasTotalOrigem = duvidasUnicas.PrimeirasQuestoes + duvidasUnicas.PrimeirasApostilas + duvidasUnicas.PrimeirasSemVinculo;

            duvidasUnicas.TotalGeralDias = duvidasUnicas.Total + duvidasUnicas.PrimeirasTotal;
            duvidasUnicas.TotalGeralOrigem = duvidasUnicas.TotalOrigem + duvidasUnicas.PrimeirasTotalOrigem;

            foreach (var c in coordenadores)
            {
                var nomeFormatado = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(c.Nome);
                var body = BuildEmail(nomeFormatado, demaisDuvidasItems, true, null, duvidasUnicas);
                if (body != null && c.Email != null)
                {
                    var result = _repository.EnviarEmailDuvidaAcademica(c.Email + Constants.EMAIL_DUVIDASACADEMICAS, body, "Interações em Dúvidas Acadêmicas", "ses");
                    if (result != 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool EnviarEmailsProfessores()
        {
            var professores = _repository.GetProfessores().ToList();

            foreach (var professor in professores)
            {
                var demaisDuvidasItems = new List<DAEmailItemDTO>();
                var duvidasGeral = GetDuvidasProfessor(professor.Id.Value).ToList();

                var nomeFormatado = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(professor.Nome);
                var cabecalho = BuildCabecalhoEmailDuvidasAcademicas(nomeFormatado);

                var body = "";

                //retornar DA do ano atual
                body += BuildInfoBodyEmail(duvidasGeral, professor, true);
                //retornar DA dos outros anos
                body += BuildInfoBodyEmail(duvidasGeral, professor, false);

                if (body != null && body != "" && professor.Email != null)
                {
                    var result = _repository.EnviarEmailDuvidaAcademica(professor.Email + Constants.EMAIL_DUVIDASACADEMICAS, cabecalho + body, "Interações em Dúvidas Acadêmicas", "ses");
                    if (result != 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private string BuildInfoBodyEmail(IList<DuvidasAcademicasProfessorDTO> duvidasGeral, AcademicoDADTO professor, bool isAnoAtual)
        {
            var demaisDuvidasItems = new List<DAEmailItemDTO>();

            var entidades = duvidasGeral.Where(x => x.DataOriginal.Year == DateTime.Now.Year).GroupBy(x => new { x.EntidadeEspecialidade }).Select(y => new
            {
                y.Key.EntidadeEspecialidade,
            }).ToList();
        
            if (!isAnoAtual)            
            {
                entidades = duvidasGeral.Where(x => x.DataOriginal.Year != DateTime.Now.Year).GroupBy(x => new { x.EntidadeEspecialidade }).Select(y => new
                {
                    y.Key.EntidadeEspecialidade,
                }).ToList();
            }

            if (entidades.Count == 0)
                return "";
            
            foreach (var entidade in entidades)
            {
                var demaisDuvidaItem = new DAEmailItemDTO();
                demaisDuvidaItem.Professor = entidade.EntidadeEspecialidade;
                demaisDuvidaItem.Menos2Dias = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == entidade.EntidadeEspecialidade && x.DataOriginal.Date >= DateTime.Now.Date.AddDays(-2));
                demaisDuvidaItem.Entre2e7Dias = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == entidade.EntidadeEspecialidade && x.DataOriginal.Date >= DateTime.Now.Date.AddDays(-7) && DateTime.Now.Date.AddDays(-2) > x.DataOriginal.Date);
                demaisDuvidaItem.Mais7Dias = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == entidade.EntidadeEspecialidade && DateTime.Now.Date.AddDays(-7) > x.DataOriginal.Date);
                demaisDuvidaItem.Total = demaisDuvidaItem.Menos2Dias + demaisDuvidaItem.Entre2e7Dias + demaisDuvidaItem.Mais7Dias;
                demaisDuvidaItem.Encaminhadas = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.BitEncaminhada);
                demaisDuvidaItem.Questoes = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == entidade.EntidadeEspecialidade && x.EntidadeEspecialidade != null && !x.MinhaDuvidaApostila);
                demaisDuvidaItem.Apostilas = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == entidade.EntidadeEspecialidade && x.MinhaDuvidaApostila);
                demaisDuvidaItem.SemVinculo = duvidasGeral.Count(x => !x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == null && !x.MinhaDuvidaApostila);
                demaisDuvidaItem.TotalOrigem = demaisDuvidaItem.Questoes + demaisDuvidaItem.Apostilas;
                demaisDuvidaItem.TemEntidade = entidade.EntidadeEspecialidade == null;

                demaisDuvidaItem.PrimeirasMenos2Dias = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == entidade.EntidadeEspecialidade && x.DataOriginal.Date >= DateTime.Now.Date.AddDays(-2));
                demaisDuvidaItem.PrimeirasEntre2e7Dias = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == entidade.EntidadeEspecialidade && x.DataOriginal.Date >= DateTime.Now.Date.AddDays(-7) && DateTime.Now.Date.AddDays(-2) > x.DataOriginal.Date);
                demaisDuvidaItem.PrimeirasMais7Dias = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == entidade.EntidadeEspecialidade && DateTime.Now.Date.AddDays(-7) > x.DataOriginal.Date);
                demaisDuvidaItem.PrimeirasTotal = demaisDuvidaItem.PrimeirasMenos2Dias + demaisDuvidaItem.PrimeirasEntre2e7Dias + demaisDuvidaItem.PrimeirasMais7Dias;
                demaisDuvidaItem.PrimeirasEncaminhadas = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.BitEncaminhada);
                demaisDuvidaItem.PrimeirasQuestoes = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == entidade.EntidadeEspecialidade && x.EntidadeEspecialidade != null && !x.MinhaDuvidaApostila);
                demaisDuvidaItem.PrimeirasApostilas = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == entidade.EntidadeEspecialidade && x.MinhaDuvidaApostila);
                demaisDuvidaItem.PrimeirasSemVinculo = duvidasGeral.Count(x => x.PrimeirasDuvidas && x.IdProfessor == professor.Id && x.EntidadeEspecialidade == null && !x.MinhaDuvidaApostila);
                demaisDuvidaItem.PrimeirasTotalOrigem = demaisDuvidaItem.PrimeirasQuestoes + demaisDuvidaItem.PrimeirasApostilas;

                if (demaisDuvidaItem.TemEntidade)
                {
                    demaisDuvidaItem.TotalOrigem += demaisDuvidaItem.SemVinculo;
                    demaisDuvidaItem.PrimeirasTotalOrigem += demaisDuvidaItem.PrimeirasSemVinculo;
                }


                demaisDuvidaItem.TotalGeralDias = demaisDuvidaItem.Total + demaisDuvidaItem.PrimeirasTotal;
                demaisDuvidaItem.TotalGeralOrigem = demaisDuvidaItem.TotalOrigem + demaisDuvidaItem.PrimeirasTotalOrigem;

                demaisDuvidasItems.Add(demaisDuvidaItem);
            }

            demaisDuvidasItems = demaisDuvidasItems.OrderByDescending(x => x.TotalGeralDias).ToList();

            var duvidasResolvidas = _repository.GetResolvidosProfessor(professor.Id.Value);
            var listDuvidas = new Dictionary<int, int>();
            for (int i = 1; i <= 12; i++)
            {
                var quantidade = duvidasResolvidas.Count(x => x.DataCriacao.Month == i && x.DataCriacao.Year == DateTime.Now.Year);
                listDuvidas.Add(i, quantidade);
            }

            return BuildBodyEmail(demaisDuvidasItems, false, listDuvidas, null, isAnoAtual);
        }

        public int EnviarEmail(string email, string body)
        {
            return _repository.EnviarEmailDuvidaAcademica(email, body, "Interações em Dúvidas Acadêmicas", Constants.DEFAULT_EMAIL_PROFILE);
        }

        public bool ValidarDenunciaAutomatica(DuvidaAcademicaInteracao interacao)
        {
            var blackWords = _repository.GetBlackWords();
            var frasesBlackWord = blackWords.Where(x => !x.IsAWord()).ToList();
            var texto = Regex.Replace(interacao.Descricao.ToLower(), @"[^\w\s]", string.Empty).RemoverAcentuacao();
            var palavras = texto.Split(' ');

            var blackWordsEncontradas = blackWords.Where(x => palavras.Contains(x)).ToList();
            blackWordsEncontradas.AddRange(frasesBlackWord.Where(texto.Contains).ToList());

            if (blackWordsEncontradas.Count > 0)
            {
                var denuncia = new DenunciaDuvidasAcademicasDTO();
                denuncia.DuvidaId = Convert.ToInt32(interacao.DuvidaId);
                denuncia.RespostaId = interacao.RespostaId;
                denuncia.TipoDenuncia = TipoDenuncia.Blackword;
                denuncia.Comentario = "Denunciada automaticamente pela(s) black-word(s): " + string.Join("; ", blackWordsEncontradas);
                var result = _repository.InsertDenuncia(denuncia);
                return true;
            }
            return false;
        }

        #endregion
         #region Montar Email

        public static string BuildEmail(string nome, List<DAEmailItemDTO> objEmail, bool paraCoordenador, Dictionary<int, int> mensal, DAEmailItemDTO unicos)
        {
            var body = "<span style=\"font-family: arial, sans-serif; \">Olá " + nome + "</span>";
            body += "<br/><span style=\"font-family: arial, sans-serif;\"> Existem novas interações em Dúvidas Acadêmicas pendentes</span><br/>";
            body += BuildBodyEmail(objEmail, paraCoordenador, mensal, unicos, null);
            return body;
        }

        public static string BuildCabecalhoEmailDuvidasAcademicas(string nome)
        {
            var body = "<span style=\"font-family: arial, sans-serif; \">Olá " + nome + "</span>";
            body += "<br/><span style=\"font-family: arial, sans-serif;\"> Existem novas interações em Dúvidas Acadêmicas pendentes </span><br/>";
            return body;
        }

        public static string BuildBodyEmail(List<DAEmailItemDTO> objEmail, bool paraCoordenador, Dictionary<int, int> mensal, DAEmailItemDTO unicos, bool? isAnoAtual)
        {
            var menos2Dias = 0;
            var Entre2e7Dias = 0;
            var Mais7Dias = 0;
            var Totais = 0;
            var TotaisOrigem = 0;
            var Encaminhadas = 0;
            var Questoes = 0;
            var Apostilas = 0;
            var SemVinculo = 0;
            var primeirasmenos2Dias = 0;
            var primeirasEntre2e7Dias = 0;
            var primeirasMais7Dias = 0;
            var primeirasTotais = 0;
            var primeirasTotaisOrigem = 0;
            var primeirasEncaminhadas = 0;
            var primeirasQuestoes = 0;
            var primeirasApostilas = 0;
            var primeirasSemVinculo = 0;
            var Janeiro = 0;
            var Fevereiro = 0;
            var Marco = 0;
            var Abril = 0;
            var Maio = 0;
            var Junho = 0;
            var Julho = 0;
            var Agosto = 0;
            var Setembro = 0;
            var Outubro = 0;
            var Novembro = 0;
            var Dezembro = 0;
            var TotalMensal = 0;

            var TotalGeral = 0;
            var TotalOrigem = 0;

            foreach (var item in objEmail)
            {
                menos2Dias += item.Menos2Dias;
                Entre2e7Dias += item.Entre2e7Dias;
                Mais7Dias += item.Mais7Dias;
                Totais += item.Total;
                Encaminhadas += item.Encaminhadas;
                Questoes += item.Questoes;
                Apostilas += item.Apostilas;
                if (paraCoordenador)
                    SemVinculo += item.SemVinculo;
                else
                    SemVinculo = item.SemVinculo;

                TotaisOrigem += item.TotalOrigem;

                TotalGeral += item.TotalGeralDias;
                TotalOrigem += item.TotalGeralOrigem;

                primeirasmenos2Dias += item.PrimeirasMenos2Dias;
                primeirasEntre2e7Dias += item.PrimeirasEntre2e7Dias;
                primeirasMais7Dias += item.PrimeirasMais7Dias;
                primeirasTotais += item.PrimeirasTotal;
                primeirasEncaminhadas += item.PrimeirasEncaminhadas;
                primeirasQuestoes += item.PrimeirasQuestoes;
                primeirasApostilas += item.PrimeirasApostilas;
                if (paraCoordenador)
                    primeirasSemVinculo += item.PrimeirasSemVinculo;
                else
                    primeirasSemVinculo = item.PrimeirasSemVinculo;
                primeirasTotaisOrigem += item.PrimeirasTotalOrigem;

                Janeiro += paraCoordenador ? item.DuvidasResolvidas[1] : 0;
                Fevereiro += paraCoordenador ? item.DuvidasResolvidas[2] : 0;
                Marco += paraCoordenador ? item.DuvidasResolvidas[3] : 0;
                Abril += paraCoordenador ? item.DuvidasResolvidas[4] : 0;
                Maio += paraCoordenador ? item.DuvidasResolvidas[5] : 0;
                Junho += paraCoordenador ? item.DuvidasResolvidas[6] : 0;
                Julho += paraCoordenador ? item.DuvidasResolvidas[7] : 0;
                Agosto += paraCoordenador ? item.DuvidasResolvidas[8] : 0;
                Setembro += paraCoordenador ? item.DuvidasResolvidas[9] : 0;
                Outubro += paraCoordenador ? item.DuvidasResolvidas[10] : 0;
                Novembro += paraCoordenador ? item.DuvidasResolvidas[11] : 0;
                Dezembro += paraCoordenador ? item.DuvidasResolvidas[12] : 0;
                TotalMensal += paraCoordenador ? item.DuvidasResolvidas.Sum(x => x.Value) : 0;
            }

            var body = string.Empty;
            body += "<table>";

            if (isAnoAtual != null)
            {
                body += "<tr>";
                body += "<th colspan='2'></th>";
                if ((bool)isAnoAtual)
                    body += "<th colspan='8' style='border: 1px solid #dddddd;text-align:left;padding:8px;text-align: center;'> Dúvidas de " + DateTime.Now.Year + "</th>";
                else
                    body += "<th colspan='8' style='border: 1px solid #dddddd;text-align:left;padding:8px;text-align: center;'> Dúvidas dos Anos Anteriores </th>";
                body += "</tr>";
            }

            body += "<tr style='border: 1px solid #dddddd;text-align:left;padding:8px'>";
            body += "<th colspan='2'></th>";
            body += "<th colspan='4' style='border: 1px solid #dddddd;text-align:left;padding:8px;text-align: center;'>1ª a 5ª DAs / Aluno</th>";
            body += "<th colspan='4' style='border: 1px solid #dddddd;text-align:left;padding:8px;text-align: center;'>6ª ou + DA / Aluno</th>";
            body += "<tr style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">" + (paraCoordenador ? "Professor" : "Entidade") + "</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Total Geral</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Menos 2 dias</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Entre 2 e 7 dias</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Mais de 7 dias</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Total</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Menos 2 dias</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Entre 2 e 7 dias</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Mais de 7 dias</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Total</th>";
            body += "</tr>";
            body += "<tr>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? "Totais Únicos" : "Totais") + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.TotalGeralDias : TotalGeral) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.PrimeirasMenos2Dias : primeirasmenos2Dias) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.PrimeirasEntre2e7Dias : primeirasEntre2e7Dias) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.PrimeirasMais7Dias : primeirasMais7Dias) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.PrimeirasTotal : primeirasTotais) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.Menos2Dias : menos2Dias) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.Entre2e7Dias : Entre2e7Dias) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.Mais7Dias : Mais7Dias) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.Total : Totais) + "</td>";
            body += "</tr>";
            foreach (var e in objEmail)
            {
                body += "<tr>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + (e.Professor ?? "SEM VÍNCULO") + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + e.TotalGeralDias + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.PrimeirasMenos2Dias + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.PrimeirasEntre2e7Dias + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.PrimeirasMais7Dias + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + e.PrimeirasTotal + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.Menos2Dias + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.Entre2e7Dias + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.Mais7Dias + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + e.Total + "</td>";
                body += "</tr>";
            }
            body += "</table><br>";
            body += "<table>";
            body += "<tr style='border: 1px solid #dddddd;text-align:left;padding:8px'>";
            body += "<th colspan='2'></th>";
            body += "<th colspan='5' style='border: 1px solid #dddddd;text-align:left;padding:8px;text-align: center;'>1ª a 5ª DAs / Aluno</th>";
            body += "<th colspan='5' style='border: 1px solid #dddddd;text-align:left;padding:8px;text-align: center;'>6ª ou + DA / Aluno</th>";
            body += "<tr style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">" + (paraCoordenador ? "Professor" : "Entidade") + "</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Total Geral</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Questões</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Apostilas</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Sem vínculo</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Total</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Encaminhadas</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Questões</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Apostilas</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Sem vínculo</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Total</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Encaminhadas</th>";
            body += "</tr>";

            body += "<tr>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? "Totais Únicos" : "Totais") + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.TotalGeralOrigem : TotalOrigem) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.PrimeirasQuestoes : primeirasQuestoes) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.PrimeirasApostilas : primeirasApostilas) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.PrimeirasSemVinculo : primeirasSemVinculo) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.PrimeirasTotalOrigem : primeirasTotaisOrigem) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.PrimeirasEncaminhadas : primeirasEncaminhadas) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.Questoes : Questoes) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.Apostilas : Apostilas) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.SemVinculo : SemVinculo) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.TotalOrigem : TotaisOrigem) + "</td>";
            body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + (paraCoordenador ? unicos.Encaminhadas : Encaminhadas) + "</td>";
            body += "</tr>";
            foreach (var e in objEmail)
            {
                body += "<tr>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + (e.Professor ?? "SEM VINCULO") + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + e.TotalGeralOrigem + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.PrimeirasQuestoes + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.PrimeirasApostilas + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + (e.TemEntidade || paraCoordenador ? e.PrimeirasSemVinculo : 0) + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + e.PrimeirasTotalOrigem + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.PrimeirasEncaminhadas + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.Questoes + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.Apostilas + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + (e.TemEntidade || paraCoordenador ? e.SemVinculo : 0) + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + e.TotalOrigem + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.Encaminhadas + "</td>";
                body += "</tr>";
            }
            body += "</table><br>";
            body += "<table>";
            body += "<tr style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Respondidas</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Janeiro</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Fevereiro</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Março</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Abril</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Maio</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Junho</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Julho</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Agosto</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Setembro</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Outubro</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Novembro</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Dezembro</th>";
            body += "<th style=\"border: 1px solid #dddddd;text - align: left;padding: 8px; \">Total</th>";
            body += "</tr>";


            if (paraCoordenador)
            {
                body += "<tr>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">Totais</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + Janeiro + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + Fevereiro + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + Marco + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + Abril + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + Maio + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + Junho + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + Julho + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + Agosto + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + Setembro + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + Outubro + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + Novembro + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + Dezembro + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + TotalMensal + "</td>";
                body += "</tr>";

                var ordenadasRespondidas = objEmail.OrderByDescending(x => x.TotaisRespondidas).ToList();
                foreach (var e in ordenadasRespondidas)
                {
                    body += "<tr>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.Professor + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.DuvidasResolvidas[1] + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.DuvidasResolvidas[2] + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.DuvidasResolvidas[3] + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.DuvidasResolvidas[4] + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.DuvidasResolvidas[5] + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.DuvidasResolvidas[6] + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.DuvidasResolvidas[7] + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.DuvidasResolvidas[8] + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.DuvidasResolvidas[9] + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.DuvidasResolvidas[10] + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.DuvidasResolvidas[11] + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.DuvidasResolvidas[12] + "</td>";
                    body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; \">" + e.TotaisRespondidas + "</td>";
                    body += "</tr>";
                }
            }
            else
            {
                body += "<tr>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">Totais</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal[1] + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal[2] + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal[3] + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal[4] + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal[5] + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal[6] + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal[7] + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal[8] + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal[9] + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal[10] + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal[11] + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal[12] + "</td>";
                body += "<td style=\"border: 1px solid #dddddd;text-align: left;padding: 8px; background-color: lightgray; \">" + mensal.Sum(x => x.Value) + "</td>";
                body += "</tr>";
            }

            body += "</table>";
            return body;
        }

        

        #endregion

    }
}