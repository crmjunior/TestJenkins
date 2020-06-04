using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCoreAPI.ViewModel.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MedCore_API.Controllers
{
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [ApiController]
    public class QuestoesApostilaController : BaseService
    {
        public QuestoesApostilaController(IMapper mapper) : base(mapper)
        { }

        [HttpGet("RealizaProva/Apostila/Questao/Imagem/IdImagem/{idImagem}/")]
        public string GetApostilaImagem(string idImagem)
        {
            return new ImagemEntity().GetConcursoBase64(Convert.ToInt32(idImagem));
        }

        [HttpGet("RealizaProva/Apostila/Questao/Avaliacao/Permissao/QuestaoId/{idQuestao}/TipoComentario/{tipoComentario}/Matricula/{intClientId}/")]
        public int GetApostilaPermissaoAvaliacao(string idQuestao, string tipoComentario, string intClientId)
        {
            return new QuestaoEntity().GetPermissaoAvaliacao
            (
                Convert.ToInt32(idQuestao),
                Convert.ToInt32(Exercicio.tipoExercicio.APOSTILA),
                Convert.ToInt32(tipoComentario),
                Convert.ToInt32(intClientId)
            );
        }

        [HttpGet("RealizaProva/Apostila/Questao/Estatistica/IdQuestao/{idQuestao}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Dictionary<string, string> GetApostilaEstatisticaQuestao(string idQuestao, string matricula, string idAplicacao)
        {
            return new QuestaoEntity().GetEstatistica(Convert.ToInt32(idQuestao), Convert.ToInt32(Exercicio.tipoExercicio.APOSTILA));
        }

        [HttpGet("RealizaProva/Apostila/Questao/Video/IdQuestao/{idQuestao}/Matricula/{matricula}/IdAplicacao/{idAplication}/")]
        public Video GetApostilaVideoQuestao(string idQuestao, string matricula, string idAplication, string appVersion = null)
        {
            int idAplicacao = Convert.ToInt32(idAplication);         

            if(this.Request != null && this.Request.Headers != null && this.Request.Headers.TryGetValue("appVersion", out StringValues headerValues))
            {
                appVersion = headerValues.ToArray()[0];
            }
            
            if(String.IsNullOrEmpty(appVersion))
            {
                appVersion = idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON ?
                    ConfigurationProvider.Get("ChaveamentoVersaoMinimaMsProDesktop") :
                    ConfigurationProvider.Get("ChaveamentoVersaoMinimaMsPro");
            }
            return new VideoEntity().GetVideo(Convert.ToInt32(idQuestao), Convert.ToInt32(Exercicio.tipoExercicio.APOSTILA), Convert.ToInt32(idAplication), appVersion);
        }

        [HttpGet("RealizaProva/Apostila/Questao/IdQuestao/{idQuestao}/Matricula/{matricula}/IdAplicacao/{idaplicacao}/")]
        public Questao GetApostilaQuestao(string idQuestao, string matricula, string idaplicacao)
        {
            var business = new QuestaoBusiness(new QuestaoEntity(), new FuncionarioEntity());
            return business.GetQuestaoTipoApostila(Convert.ToInt32(idQuestao), Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpGet("RealizaProva/Apostila/Configuracao/IdEntidade/{idEntidade}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]//não funcionando
        public Apostila GetApostilaConfiguracao(string idEntidade, string matricula, string idAplicacao)
        {
            return new ExercicioEntity().GetApostilaConfiguracaoCache(Convert.ToInt32(idEntidade), Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao));
        }

        [HttpGet("RealizaProva/Apostila/CartaoResposta/IdEntidade/{idEntidade}/Questoes/Matricula/{matricula}/IdAplicacao/{idaplicacao}/")] //não funcionando
        public CartoesResposta GetApostilaCartaoResposta(string idEntidade, string matricula, string idaplicacao, string ano)
        {
            return new CartaoRespostaBusiness(new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity()).GetCartaoResposta(Convert.ToInt32(idEntidade), Convert.ToInt32(matricula), Exercicio.tipoExercicio.APOSTILA, Convert.ToInt32(ano));
        }

        [HttpGet("RealizaProva/Filtro/IdEntidade/{idEntidade}/TipoExercicio/{tipoExercicio}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")] //não funcionando
        public CartaoRespostaFiltro GetCartaoRespostaFiltro(string idEntidade, string matricula, string idAplicacao, string tipoExercicio, string ano, string idApostila)
        {
            return new CartaoRespostaBusiness(new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity()).GetCartaoRespostaFiltro(Convert.ToInt32(idEntidade), Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao), Convert.ToInt32(tipoExercicio), Convert.ToInt32(ano), Convert.ToInt32(idApostila));
        }

        [HttpGet("RealizaProva/Apostila/Questao/Avaliacao/Comentario/Opcoes/")]
        public List<QuestaoAvaliacaoComentarioOpcoes> GetApostilaOpcoesAvaliacaoNegativaComentarioQuestao()
        {
            return new QuestaoEntity().GetOpcoesAvaliacaoNegativaComentarioQuestao();
        }

        [HttpGet("RealizaProva/Apostila/CartaoResposta/IdEntidade/{idEntidade}/Questoes/Matricula/{matricula}/IdAplicacao/{idaplicacao}/idApostila/{idApostila}/")]        
        public CartoesResposta GetApostilaCartaoRespostaPorIdApostila(string idEntidade, string matricula, string idaplicacao, string idApostila)
        {
            return new CartaoRespostaEntity().GetCartaoRespostaApostila(Convert.ToInt32(idEntidade), Convert.ToInt32(matricula), Convert.ToInt32(idApostila));
        }

        [HttpGet("RealizaProva/Apostila/Questao/AvaliacaoRealizada/QuestaoId/{questaoId}/TipoComentario/{tipoComentario}/Matricula/{matricula}/")]
        public int GetApostilaAvaliacaoRealizada(string questaoId, string tipoComentario, string matricula)
        {

            return new QuestaoEntity().GetAvaliacaoRealizada
            (
                Convert.ToInt32(questaoId),
                Convert.ToInt32(Exercicio.tipoExercicio.APOSTILA),
                Convert.ToInt32(tipoComentario),
                Convert.ToInt32(matricula)
            );
        }


        [HttpPost("RealizaProva/Apostila/Questao/Objetiva/Inserir/")]
        public int SetApostilaRespostaObjetiva(RespostaObjetivaPost resp)
        {
            return new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), 
				new FuncionarioEntity(), new SimuladoEntity(), new MontaProvaEntity()).SetRespostaObjetiva(resp);
        }

        [HttpPost("RealizaProva/Apostila/Questao/Avaliacao/Comentario/Inserir/")]
        public int SetApostilaQuestaoAvaliacao(QuestaoAvaliacaoComentario questaoAvaliacaoComentario)
        {
            return new QuestaoEntity().SetAvaliacao(questaoAvaliacaoComentario);
        }

        [HttpPost("RealizaProva/Apostila/Questao/Discursiva/Inserir/")]
        public int SetApostilaRespostaDiscursiva(RespostaDiscursivaPost resp)
        {
            return new QuestaoEntity().SetRespostaDiscursiva(resp);
        }

        [HttpPost("RealizaProva/Apostila/Questao/Favorita/")]
        public int SetApostilaQuestaoFavorita(CartoesResposta cartaoresposta)
        {
            var questaoID = cartaoresposta.Questoes.First().Id;
            var clientID = cartaoresposta.ClientID;
            var favorita = cartaoresposta.Questoes.First().Anotacoes.First().Favorita;

            return new QuestaoEntity().SetFavoritaQuestaoApostila(questaoID, clientID, favorita);
        }

        [HttpPost("RealizaProva/Apostila/Questao/Duvida/")]
        public int SetApostilaQuestaoDuvida(CartoesResposta cartaoresposta)
        {
            var questaoID = cartaoresposta.Questoes.First().Id;
            var clientID = cartaoresposta.ClientID;
            var duvida = cartaoresposta.Questoes.First().Anotacoes.First().Duvida;

            return new QuestaoEntity().SetDuvidaQuestaoApostila(questaoID, clientID, duvida);
        }

        [HttpPost("RealizaProva/Apostila/Finaliza/")]
        public int SetFinalizaApostila(Exercicio exerc)
        {
            return new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity())
                .SetFinalizaExercicio(exerc);
        }

        [HttpPost("RealizaProva/Apostila/Questao/Anotacao/")]
        public int SetApostilaQuestaoAnotacao(CartoesResposta cartaoresposta)
        {
            var questaoID = cartaoresposta.Questoes.First().Id;
            var clientID = cartaoresposta.ClientID;
            var anotacao = cartaoresposta.Questoes.First().Anotacoes.First().Anotacao;

            return new QuestaoEntity().SetAnotacaoAlunoQuestao(questaoID, clientID, anotacao, Convert.ToInt32(Exercicio.tipoExercicio.APOSTILA));
        }

        [HttpPost("RealizaProva/Filtro/CartaoReposta/")]
        public CartoesResposta FiltrarCartaoResposta(CartaoRespostaFiltro filtro)
        {
            return new CartaoRespostaBusiness(new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity()).GetCartoesRespostaFiltrado(filtro);
        }
    }
}