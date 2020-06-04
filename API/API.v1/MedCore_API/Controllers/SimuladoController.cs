using System.Net;
using System;
using System.Collections.Generic;
using AutoMapper;
using MedCore_DataAccess;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCoreAPI.ViewModel.Base;
using Microsoft.AspNetCore.Mvc;
using MedCore_DataAccess.Contracts.Enums;
using System.Linq;

namespace MedCoreAPI.Controllers
{
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [ApiController]
    public class SimuladoController : BaseService
    {
        public SimuladoController(IMapper mapper) 
            : base(mapper) {
        }

        [HttpGet("Simulados/Anos/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public List<int> GetAnos(string matricula, string idaplicacao)
        {
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).GetAnosSimuladoExtensivo(Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }   

        [HttpGet("Simulados/Ano/{ano}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public List<Exercicio> GetSimulados(string ano, string matricula, string idaplicacao)
        {
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladosExtensivo(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Simulados/Anos/Matricula/{matricula}/IdAplicacao/{idAplicacao}/SimuladoR3CIR")]
        public List<int> GetAnosSimuladoR3Cir(string matricula, string idaplicacao)
        {
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).GetAnosSimuladoR3Cir(Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Simulados/Anos/Matricula/{matricula}/IdAplicacao/{idAplicacao}/SimuladoR3CLI")]
        public List<int> GetAnosSimuladoR3Cli(string matricula, string idaplicacao)
        {
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).GetAnosSimuladoR3Cli(Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Simulados/Anos/Matricula/{matricula}/IdAplicacao/{idAplicacao}/SimuladoR3PED")]
        public List<int> GetAnosSimuladoR3Ped(string matricula, string idaplicacao)
        {
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).GetAnosSimuladoR3Ped(Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Simulados/Anos/Matricula/{matricula}/IdAplicacao/{idAplicacao}/SimuladoR4GO")]
        public List<int> GetAnosSimuladoR4GO(string matricula, string idaplicacao)
        {
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).GetAnosSimuladoR4GO(Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Simulados/Ano/{ano}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/SimuladoCPMEDEXT")]
        public List<Exercicio> GetSimuladosCPMedExtensivo(string ano, string matricula, string idaplicacao)
        {
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladosCPMedExtensivo(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Simulados/Anos/Matricula/{matricula}/IdAplicacao/{idAplicacao}/SimuladoCPMEDEXT")]
        public List<int> GetAnosSimuladoCPMedExtensivo(string matricula, string idaplicacao)
        {
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).GetAnosSimuladoCPMedExtensivo(Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Simulados/Anos/Matricula/{matricula}/IdAplicacao/{idAplicacao}/SimuladoCPMED")]
        public List<int> GetAnosSimuladoCPMED(string matricula, string idaplicacao)
		{
			return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity(), new ProdutoEntity()).GetAnosSimuladoCPMED(Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
		}

        [HttpGet("Simulados/Ano/{ano}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/SimuladoR3CIR")]
        public List<Exercicio> GetSimuladosR3Cir(string ano, string matricula, string idaplicacao)
        {
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladosR3Cir(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Simulados/Ano/{ano}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/SimuladoR3CLI")]
        public List<Exercicio> GetSimuladosR3Cli(string ano, string matricula, string idaplicacao)
        {
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladosR3Cli(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Simulados/Ano/{ano}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/SimuladoR3PED")]
        public List<Exercicio> GetSimuladosR3Ped(string ano, string matricula, string idaplicacao)
        {
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladosR3Ped(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Simulados/Ano/{ano}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/SimuladoR4GO")]
        public List<Exercicio> GetSimuladosR4GO(string ano, string matricula, string idaplicacao)
        {
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladosR4GO(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Simulados/Ano/{ano}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/SimuladoCPMED")]
        public List<Exercicio> GetSimuladosCPMED(string ano, string matricula, string idaplicacao)
		{
			return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetSimuladosCPMED(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
		}

        [HttpGet("Simulados/GetAllSimulados/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public List<ExercicioDTO> GetAllSimulados(string matricula, string idaplicacao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).GetAllSimulados(Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Simulados/Agendado/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Exercicio GetSimuladoAgendado(string matricula, string idaplicacao)
        {
            return new SimuladoEntity().GetSimuladoAgendado(Convert.ToInt32(matricula));
        }

        [HttpGet("Simulados/Agendado/Info/IdSimulado/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Exercicio GetModalSimuladoOnline(string idSimulado, string matricula, string idAplicacao)
        {
            return new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity())
                .GetModalSimuladoOnline(Convert.ToInt32(idSimulado), Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao));
        }
        
        [HttpGet("RealizaProva/Simulado/Andamento/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public SimuladosEmAndamentoDTO GetSimuladosEmAndamento(string matricula, string idAplicacao)
        {
            return new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity())
                .GetSimuladosIniciados(Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao));
        }

        [HttpGet("RealizaProva/Simulado/CartaoResposta/IdSimulado/{idSimulado}/Questoes/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public CartoesResposta GetSimuladoCartaoResposta(string idSimulado, string matricula, string idaplicacao)
        {
            return new CartaoRespostaBusiness(new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity()).GetCartaoResposta(Convert.ToInt32(idSimulado), Convert.ToInt32(matricula), Exercicio.tipoExercicio.SIMULADO);
        }

        [MapToApiVersion("2")]
        [HttpGet("RealizaProva/SimuladoAgendado/CartaoResposta/IdSimulado/{idSimulado}/ExercicioHistorico/{idExercicioHistorico}/")]
        public ResultViewModel<CartaoRespostaSimuladoAgendadoViewModel> GetSimuladoAgendadoCartaoResposta(string idSimulado, string idExercicioHistorico)
        {
            var result = Execute(() =>
            {
                var business = new CartaoRespostaBusiness(new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity());
                var resposta = business.GetCartaoRespostaSimuladoAgendado(Matricula, Convert.ToInt32(idSimulado), Convert.ToInt32(idExercicioHistorico));
                return resposta;
            });

            return GetResultViewModel<CartaoRespostaSimuladoAgendadoViewModel, CartaoRespostaSimuladoAgendadoDTO>(result);
        }

        [HttpGet("RealizaProva/Simulado/ModoProva/CartaoResposta/IdSimulado/{idSimulado}/Questoes/Matricula/{matricula}/IdHistorico/{idHistorico}/IdAplicacao/{idAplicacao}/")]
        public CartoesResposta GetSimuladoCartaoRespostaModoProva(string idSimulado, string matricula, string idaplicacao, string idHistorico)
        {
            return new CartaoRespostaEntity().GetCartaoRespostaModoProva(Convert.ToInt32(idSimulado), Convert.ToInt32(matricula), Exercicio.tipoExercicio.SIMULADO, Convert.ToInt32(idHistorico));
        }

        
        [MapToApiVersion("2")]
        [HttpGet("RealizaProva/SimuladoAgendado/Questao/IdQuestao/{idQuestao}/ExercicioHistorico/{idExercicioHistorico}/Exercicio/{idExercicio}/")]
        public ResultViewModel<QuestaoSimuladoAgendadoViewModel> GetQuestaoSimuladoAgendado(string idQuestao, string idExercicioHistorico, string idExercicio)
        {
            base.SetStateHeadersFromRequest();
            var result = Execute(() =>
            {
                var business = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
                var resposta = business.GetQuestaoSimuladoAgendado(Convert.ToInt32(idQuestao), base.Matricula, Convert.ToInt32(idExercicio), Convert.ToInt32(idExercicioHistorico));
                return resposta;
            });

            return GetResultViewModel<QuestaoSimuladoAgendadoViewModel, QuestaoSimuladoAgendadoDTO>(result);
        }
        [MapToApiVersion("2")]
        [HttpGet("RealizaProva/SimuladoAgendado/Questao/IdQuestao/{idQuestao}/Exercicio/{idExercicio}/Download/")]
        public ResultViewModel<QuestaoSimuladoAgendadoViewModel> GetQuestaoSimuladoAgendadoDownload(string idQuestao, string idExercicio)
        {
            base.SetStateHeadersFromRequest();
            var result = Execute(() =>
            {
                var business = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
                var resposta = business.GetQuestaoSimuladoAgendado(Convert.ToInt32(idQuestao), Matricula, Convert.ToInt32(idExercicio));
                return resposta;
            });

            return GetResultViewModel<QuestaoSimuladoAgendadoViewModel, QuestaoSimuladoAgendadoDTO>(result);
        }

        [HttpGet("RealizaProva/Simulado/Questao/IdQuestao/{idQuestao}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Questao GetSimuladoQuestao(string idQuestao, string matricula, string idaplicacao)
        {
            //return new RDSQuestaoEntity().GetTipoSimulado(Convert.ToInt32(idQuestao), Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
            return new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity())
                        .GetTipoSimulado(Convert.ToInt32(idQuestao), Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("RealizaProva/Simulado/Questao/Imagem/IdImagem/{idImagem}/")]
        public string GetSimuladoImagem(string idImagem)
        {
            return new ImagemEntity().GetSimuladoBase64(Convert.ToInt32(idImagem));
        }

        [HttpGet("RealizaProva/Simulado/Questao/Video/IdQuestao/{idQuestao}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Video GetSimuladoVideoQuestao(string idQuestao, string matricula, string idAplication)
        {
            string appVersion = null;
            if (Request.Headers["appVersion"] != "")
            {
                appVersion = Request.Headers["appVersion"];
            }
            else if (Request.Query["appversion"] != "")
            {
                appVersion = Request.Query["appversion"];
            }
            else
            {
                appVersion = Convert.ToInt32(idAplication) == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON ?                    
                    ConfigurationProvider.Get("Settings:ChaveamentoVersaoMinimaMsProDesktop") :
                    ConfigurationProvider.Get("Settings:ChaveamentoVersaoMinimaMsPro");
            }
            
            return new VideoEntity().GetVideo(Convert.ToInt32(idQuestao), Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO), Convert.ToInt32(idAplication), appVersion);
        }

        [HttpGet("RealizaProva/Simulado/Configuracao/IdSimulado/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Simulado GetSimuladoConfiguracao(string idSimulado, string matricula, string idAplicacao)
        {
            int intIdAplicacao = Convert.ToInt32(idAplicacao);
            var appVersion = "";

            if(Request.Headers["appVersion"] != "")
            {
                appVersion = Request.Headers["appVersion"];
            }
            else
            {
                appVersion = Utilidades.VersaoMinimaImpressaoSimulado(intIdAplicacao);
            }

            return new ExercicioEntity().GetSimuladoConfiguracao(Convert.ToInt32(idSimulado), Convert.ToInt32(matricula), intIdAplicacao, appVersion);
        }

        [HttpGet("RealizaProva/Simulado/ModoProva/Configuracao/IdSimulado/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Simulado GetSimuladoConfiguracaoModoProva(string idSimulado, string matricula, string idAplicacao)
        {
            int intIdAplicacao = Convert.ToInt32(idAplicacao);
            var appVersion = "";
            if (Request.Headers["appVersion"] != "")
            {
                appVersion = Request.Headers["appVersion"];
            }
            else
            {
                appVersion = Utilidades.VersaoMinimaImpressaoSimulado(intIdAplicacao);
            }
            return new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity())
                .GetSimuladoModoProvaConfiguracao(Convert.ToInt32(idSimulado), Convert.ToInt32(matricula), intIdAplicacao, appVersion);
        }

        [HttpGet("RealizaProva/Simulado/ComboRealizacoes/IdSimulado/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public List<ComboSimuladoDTO> GetComboSimuladosRealizados(string matricula, string idSimulado, string idAplicacao)
        {
            return new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity())
                .GetComboSimuladosRealizados(Convert.ToInt32(matricula), Convert.ToInt32(idSimulado), Convert.ToInt32(idAplicacao));
        }

        [HttpPost("RealizaProva/Simulado/Questao/Objetiva/Inserir/")]
        public int SetSimuladoRespostaObjetiva(RespostaObjetivaPost resp)
        {
            return new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), 
				new FuncionarioEntity(), new SimuladoEntity(), new MontaProvaEntity()).SetRespostaObjetiva(resp);
        }

        [HttpPost("SimuladoAgendado/Resposta/Inserir/")]
        [MapToApiVersion("2")]
        public ResultViewModel<int> SetRespostaSimuladoAgendado(RespostaObjetivaPost resp)
        {
            var result = Execute(() =>
            {
                var business = new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity(), new SimuladoEntity());
                var resposta = business.SetRespostaObjetivaSimuladoAgendado(resp);
                return resposta;
            });

            return GetResultViewModel(result);
        }

        [HttpPost("RealizaProva/Simulado/Questao/Discursiva/Inserir/")]
        public int SetSimuladoRespostaDiscursiva(RespostaDiscursivaPost resp)
        {
            return new QuestaoEntity().SetRespostaDiscursiva(resp);
        }

        [HttpPost("RealizaProva/Simulado/Questao/Favorita/")]
        public int SetSimuladoQuestaoFavorita(CartoesResposta cartaoresposta)
        {
            var questaoID = cartaoresposta.Questoes.First().Id;
            var clientID = cartaoresposta.ClientID;
            var favorita = cartaoresposta.Questoes.First().Anotacoes.First().Favorita;

            return new QuestaoEntity().SetFavoritaQuestaoSimulado(questaoID, clientID, favorita);
        }

        [HttpPost("RealizaProva/Simulado/Questao/Duvida/")]
        public int SetSimuladoQuestaoDuvida(CartoesResposta cartaoresposta)
        {
            var questaoID = cartaoresposta.Questoes.First().Id;
            var clientID = cartaoresposta.ClientID;
            var duvida = cartaoresposta.Questoes.First().Anotacoes.First().Duvida;

            return new QuestaoEntity().SetDuvidaQuestaoSimulado(questaoID, clientID, duvida);
        }

        [HttpPost("RealizaProva/Simulado/Questao/Anotacao/")]
        public int SetSimuladoQuestaoAnotacao(CartoesResposta cartaoresposta)
        {
            var questaoID = cartaoresposta.Questoes.First().Id;
            var clientID = cartaoresposta.ClientID;
            var anotacao = cartaoresposta.Questoes.First().Anotacoes.First().Anotacao;

            return new QuestaoEntity().SetAnotacaoAlunoQuestao(questaoID, clientID, anotacao, Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO));
        }

        [HttpGet("RealizaProva/Simulado/Questao/Estatistica/IdQuestao/{idQuestao}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Dictionary<string, string> GetSimuladoEstatisticaQuestao(string idQuestao, string matricula, string idAplicacao)
        {
            return new QuestaoEntity().GetEstatistica(Convert.ToInt32(idQuestao), Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO));
        }

        [HttpGet("RealizaProva/Simulado/Agendado/Configuracao/IdSimulado/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Simulado GetSimuladoConfiguracaoAgendado(string idSimulado, string matricula, string idAplicacao)
        {
            base.SetStateHeadersFromRequest();
            
            int intIdAplicacao = Convert.ToInt32(idAplicacao);
            var appVersion = "";
            if (Request.Headers["appVersion"] != "")
            {
                appVersion = Request.Headers["appVersion"];
            }
            else
            {
                appVersion = Utilidades.VersaoMinimaImpressaoSimulado(intIdAplicacao);
            }
            return new ExercicioEntity().GetSimuladoOnlineConfiguracao(Convert.ToInt32(idSimulado), Convert.ToInt32(matricula), intIdAplicacao, appVersion); 
        }

        [HttpPost("RealizaProva/Simulado/Finaliza/")]
        public int SetFinalizaSimulado(Exercicio exerc)
        {
            bool finalizarOnlinePorTempo = true;
            return new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity()).SetFinalizaExercicio(exerc, finalizarOnlinePorTempo);
        }

        [HttpPost("RealizaProva/Simulado/Questao/Avaliacao/Comentario/Inserir/")]
        public int SetSimuladoQuestaoAvaliacao(QuestaoAvaliacaoComentario questaoAvaliacaoComentario)
        {
            return new QuestaoEntity().SetAvaliacao(questaoAvaliacaoComentario);
        }

        [HttpGet("RealizaProva/Simulado/Questao/Avaliacao/Permissao/QuestaoId/{questaoId}/TipoComentario/{tipoComentario}/Matricula/{matricula}/")]
        public int GetSimuladoPermissaoAvaliacao(string idQuestao, string tipoComentario, string intClientId)
        {

            return new QuestaoEntity().GetPermissaoAvaliacao
            (
                Convert.ToInt32(idQuestao),
                Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO),
                Convert.ToInt32(tipoComentario),
                Convert.ToInt32(intClientId)
            );
        }

        [HttpGet("RealizaProva/Simulado/Questao/AvaliacaoRealizada/QuestaoId/{questaoId}/TipoComentario/{tipoComentario}/Matricula/{matricula}/")]
        public int GetSimuladoAvaliacaoRealizada(string idQuestao, string tipoComentario, string intClientId)
        {

            return new QuestaoEntity().GetAvaliacaoRealizada
            (
                Convert.ToInt32(idQuestao),
                Convert.ToInt32(Exercicio.tipoExercicio.SIMULADO),
                Convert.ToInt32(tipoComentario),
                Convert.ToInt32(intClientId)
            );
        }

        [HttpGet("RealizaProva/Simulado/Questao/IdSimulado/{idSimulado}/Matricula/{matricula}/IdAplicacao/{ApplicationID}/")]
        public List<Questao> GetSimuladoQuestaoAll(string idSimulado, string matricula, string ApplicationID)
        {
            return new QuestaoEntity().GetTipoSimuladoAll(Convert.ToInt32(idSimulado), Convert.ToInt32(matricula), Convert.ToInt32(ApplicationID));
        }

        [HttpGet("RealizaProva/Simulado/Impresso/Link/{idSimulado}/Matricula/{matricula}/IdAplicacao/{ApplicationID}/")]
        public String GetLinkSimuladoImpresso(string idSimulado = "0", string matricula = "0", string ApplicationID = "16")
        {
            string environmentRootPath = "";
            return new SimuladoBusiness(new SimuladoEntity(), new ExercicioEntity(), new BannerEntity()).GetLinkSimuladoImpresso(Convert.ToInt32(idSimulado), Convert.ToInt32(matricula), Convert.ToInt32(ApplicationID), environmentRootPath);
        }

        [HttpGet("RealizaProva/Simulado/Questao/Avaliacao/Comentario/Opcoes/")]
        public List<QuestaoAvaliacaoComentarioOpcoes> GetSimuladoOpcoesAvaliacaoNegativaComentarioQuestao()
        {
            return new QuestaoEntity().GetOpcoesAvaliacaoNegativaComentarioQuestao();
        }

        //[HttpGet("Simulados/Ranking/Parcial/IdSimulado/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/?especialidade={especialidade}&ufs={unidades}")]
        [HttpGet("Simulados/Ranking/Parcial/IdSimulado/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public RankingSimuladoAluno GetRanking(string idSimulado, string matricula, string especialidade, string unidades, string idAplicacao)
        {
            return new RankingSimuladoEntity().GetRanking(Convert.ToInt32(matricula), Convert.ToInt32(idSimulado), especialidade, unidades, Convert.ToInt32(idAplicacao));
        }

        [HttpGet("Simulados/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/Ranking/EstatisticaAluno/{isOnline}")]
        public AlunoConcursoEstatistica GetEstatisticaAlunoSimulado(string idSimulado, string matricula, string idAplicacao, string isOnline)
        {
            return new RankingSimuladoEntity().GetEstatisticaAlunoSimulado(Convert.ToInt32(matricula), Convert.ToInt32(idSimulado), Convert.ToBoolean(Convert.ToInt32(isOnline)));
        }

        [HttpGet("Simulados/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/Ranking/RankingFinal/Validacao/")]
        public int GetExisteRankingFinal(string idSimulado, string matricula, string idAplicacao)
        {

            var isFaseFinalLiberado = new RankingSimuladoEntity().IsFaseFinalLiberado(Convert.ToInt32(idSimulado));

            return isFaseFinalLiberado ? 1 : 0;

        }

        //[HttpGet("Simulados/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/Ranking/RankingObjetiva/?especialidade={especialidade}&ufs={unidades}")]
        [HttpGet("Simulados/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/Ranking/RankingObjetiva/")]
        public RankingSimuladoAluno GetRankingObjetiva(string idSimulado, string matricula, string idAplicacao, string especialidade, string unidades)
        {
            return new RankingSimuladoEntity().GetRankingObjetiva(Convert.ToInt32(matricula), Convert.ToInt32(idSimulado), especialidade, unidades);
        }

        //[HttpGet("Simulados/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/Ranking/RankingFinal/?especialidade={especialidade}&ufs={unidades}")]
        [HttpGet("Simulados/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/Ranking/RankingFinal/")]
        public RankingSimuladoAluno GetRankingFinal(string idSimulado, string matricula, string idAplicacao, string especialidade, string unidades)
        {
            return new RankingSimuladoEntity().GetRankingFinal(Convert.ToInt32(matricula), Convert.ToInt32(idSimulado), especialidade, unidades);
        }

        //[HttpGet("Simulados/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/Ranking/RankingEstatistica/?especialidade={especialidade}&ufs={unidades}&localidade={localidade}")]
        [HttpGet("Simulados/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/Ranking/RankingEstatistica/")]
        public RankingSimuladoAluno GetRankingResultado(string idSimulado, string matricula, string idAplicacao, string especialidade, string unidades, string localidade)
        {
            return new RankingSimuladoBusiness(new RankingSimuladoEntity(), new EspecialidadeEntity(), new FilialEntity()).GetResultadoRankingAluno(Convert.ToInt32(matricula), Convert.ToInt32(idSimulado), Convert.ToInt32(idAplicacao), especialidade, unidades, localidade); 
        }

        [HttpGet("Simulados/{idSimulado}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/Ranking/FiltroParametros/")]
        public FiltroRanking GetRankingParametrosFiltro(string idSimulado, string matricula, string idAplicacao)
        {
            return new RankingSimuladoBusiness(new RankingSimuladoEntity(), new EspecialidadeEntity(), new FilialEntity()).GetFiltroRankingSimulado(Convert.ToInt32(idSimulado));
        }

        //[HttpGet("RealizaProva/Simulado/RankingModoProva/IdSimulado/{idSimulado}/Matricula/{matricula}/IdHistorico/{idHistorico}/IdAplicacao/{idAplicacao}/?especialidade={especialidade}&ufs={unidades}&localidade={localidade}")]
        [HttpGet("RealizaProva/Simulado/RankingModoProva/IdSimulado/{idSimulado}/Matricula/{matricula}/IdHistorico/{idHistorico}/IdAplicacao/{idAplicacao}/")]
        public RankingSimuladoAluno GetResultadoRankingModoProva(string matricula, string idSimulado, string idHistorico, string idAplicacao, string especialidade, string unidades, string localidade)
        {
            return new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity())
                .GetResultadoRankingModoProva(Convert.ToInt32(matricula), Convert.ToInt32(idSimulado), Convert.ToInt32(idHistorico), Convert.ToInt32(idAplicacao), especialidade, unidades, localidade);
        }
    }
}