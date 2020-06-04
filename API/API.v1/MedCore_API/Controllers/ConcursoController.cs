using System;
using System.Collections.Generic;
using AutoMapper;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Business;
using MedCoreAPI.ViewModel.Base;
using Microsoft.AspNetCore.Mvc;
using MedCore_DataAccess.DTO;
using Microsoft.Extensions.Primitives;
using MedCore_DataAccess.Util;
using System.Linq;
using MedCore_DataAccess;
using MedCore_DataAccess.Contracts.Enums;

namespace MedCoreAPI.Controllers
{
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [ApiController]
    public class ConcursoController : BaseService
    {
        public ConcursoController(IMapper mapper) 
            : base(mapper) {

        }

        [HttpGet("Concursos/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public List<Exercicio> GetConcursos(string matricula, string idaplicacao)
        {
            return new ConcursoEntity().GetAll(Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }
        
        [HttpGet("Concursos/Provas/SiglaConcurso/{siglaConcurso}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public List<Exercicio> GetConcursoProvas(string siglaConcurso, string matricula, string idAplicacao)
        {
            return new ConcursoEntity().GetProvas(siglaConcurso, Convert.ToInt32(matricula));
        }

        [HttpGet("RealizaProva/Concurso/Questao/Video/IdQuestao/{idQuestao}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Video GetConcursoVideoQuestao(string idQuestao, string matricula, string idAplication)
        {
            Request.Headers.TryGetValue("appVersion", out StringValues appVersion);
            Request.Headers.TryGetValue("idAplicacao", out StringValues idAplicacao);

            appVersion = Convert.ToInt32(idAplication) == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON ?
                ConfigurationProvider.Get("Settings:ChaveamentoVersaoMinimaMsProDesktop") :
                ConfigurationProvider.Get("Settings:ChaveamentoVersaoMinimaMsPro");

            return new VideoEntity().GetVideo(Convert.ToInt32(idQuestao), Convert.ToInt32(Exercicio.tipoExercicio.CONCURSO), Convert.ToInt32(idAplication), appVersion);
        }

        [HttpGet("RealizaProva/Concurso/Questao/Estatistica/IdQuestao/{idQuestao}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Dictionary<string, string> GetConcursoEstatisticaQuestao(string idQuestao, string matricula, string idAplicacao)
        {
            return new QuestaoEntity().GetEstatistica(Convert.ToInt32(idQuestao), Convert.ToInt32(Exercicio.tipoExercicio.CONCURSO));
        }

        [HttpGet("RealizaProva/Concurso/Forum/Questao/IdQuestao/{idQuestao}/Matricula/{matricula}/")]
        public ForumQuestaoRecurso GetConcursoForumQuestao(string idQuestao, string matricula)
        {
            return new QuestaoEntity().GetForumQuestaoRecurso(Convert.ToInt32(idQuestao), Convert.ToInt32(matricula), false);
        }

        [HttpPost("RealizaProva/Concurso/Forum/Questao/IdQuestao/{idQuestao}/Matricula/{matricula}/")]
        public int SetRecursoQuestaoAluno(string idQuestao, string matricula, RecursoAlunoLog recursoAlunoLog)
        {
            return new QuestaoEntity().SetRecursoQuestaoAluno(Convert.ToInt32(idQuestao), Convert.ToInt32(matricula), recursoAlunoLog);
        }

        [HttpPost("RealizaProva/Concurso/Questao/Objetiva/Inserir/")]
        public int SetConcursoRespostaObjetiva(RespostaObjetivaPost resp)
        {
            return new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(),
				new FuncionarioEntity(), new SimuladoEntity(), new MontaProvaEntity()).SetRespostaObjetiva(resp);
        }

        [HttpPost("RealizaProva/Concurso/Questao/Discursiva/Inserir/")]
        public int SetConcursoRespostaDiscursiva(RespostaDiscursivaPost resp)
        {
            return new QuestaoEntity().SetRespostaDiscursiva(resp);
        }

        [HttpGet("RealizaProva/Concurso/Configuracao/Idconcurso/{idConcurso}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Concurso GetConcursoConfiguracao(string idConcurso, string matricula, string idAplicacao)
        {
            return new ExercicioEntity().GetConcursoConfiguracao(Convert.ToInt32(idConcurso), Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao));
        }

        //TODO: Não é mspro
        [HttpGet("RealizaProva/Apostila/Questao/Download/IdQuestao/{idQuestao}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public QuestaoApostilaDownload GetQuestaoApostilaDownload(string idQuestao, string matricula, string idAplicacao)
        {
            return new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).GetQuestaoApostilaDownload(Convert.ToInt32(idQuestao), Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao));
        }
 
        [HttpPost("RealizaProva/Apostila/Questao/Download/")]
        public List<QuestaoApostilaDownload> GetListaQuestaoApostilaDownload(QuestoesDownloadRequestDTO questoesDownload)
        {
            Request.Headers.TryGetValue("appVersion", out StringValues appVersion);
            Request.Headers.TryGetValue("idAplicacao", out StringValues idAplicacao);
            
                appVersion = Convert.ToInt32(idAplicacao) == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON ?
                    ConfigurationProvider.Get("Settings:ChaveamentoVersaoMinimaMsProDesktop") :
                    ConfigurationProvider.Get("Settings:ChaveamentoVersaoMinimaMsPro");
            return new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).GetListaQuestaoApostilaDownload(questoesDownload, appVersion);
        }

        [HttpGet("RealizaProva/Concurso/Questao/IdQuestao/{idQuestao}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Questao GetConcursoQuestao(string idQuestao, string matricula, string idaplicacao)
        {
            Request.Headers.TryGetValue("appVersion", out StringValues appVersion);
            Request.Headers.TryGetValue("idAplicacao", out StringValues idAplicacao);

            appVersion = Convert.ToInt32(idAplicacao) == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON ?
                ConfigurationProvider.Get("Settings:ChaveamentoVersaoMinimaMsProDesktop") :
                ConfigurationProvider.Get("Settings:ChaveamentoVersaoMinimaMsPro");

            return new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), new FuncionarioEntity()).ObterDetalhesQuestaoConcurso(Convert.ToInt32(idQuestao), Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao), appVersion);
        }

        [HttpPost("RealizaProva/Concurso/Questao/Favorita/")]
        public int SetConcursoQuestaoFavorita(CartoesResposta cartaoresposta)
        {
            var questaoID = cartaoresposta.Questoes.First().Id;
            var clientID = cartaoresposta.ClientID;
            var favorita = cartaoresposta.Questoes.First().Anotacoes.First().Favorita;

            return new QuestaoEntity().SetFavoritaQuestaoConcurso(questaoID, clientID, favorita);
        }

        [HttpPost("RealizaProva/Concurso/Questao/Anotacao/")]
        public int SetConcursoQuestaoAnotacao(CartoesResposta cartaoresposta)
        {
            var questaoID = cartaoresposta.Questoes.First().Id;
            var clientID = cartaoresposta.ClientID;
            var anotacao = cartaoresposta.Questoes.First().Anotacoes.First().Anotacao;

            return new QuestaoEntity().SetAnotacaoAlunoQuestao(questaoID, clientID, anotacao, Convert.ToInt32(Exercicio.tipoExercicio.CONCURSO));
        }

        [HttpPost("RealizaProva/Concurso/Questao/Duvida/")]
        public int SetConcursoQuestaoDuvida(CartoesResposta cartaoresposta)
        {
            var questaoID = cartaoresposta.Questoes.First().Id;
            var clientID = cartaoresposta.ClientID;
            var duvida = cartaoresposta.Questoes.First().Anotacoes.First().Duvida;

            return new QuestaoEntity().SetDuvidaQuestaoConcurso(questaoID, clientID, duvida);
        }

        [HttpGet("RealizaProva/Concurso/CartaoResposta/IdConcurso/{idConcurso}/Questoes/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
            public CartoesResposta GetConcursoCartaoResposta(string idConcurso, string matricula, string idaplicacao)
        {
            return new CartaoRespostaBusiness(new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity()).GetCartaoResposta(Convert.ToInt32(idConcurso), Convert.ToInt32(matricula), Exercicio.tipoExercicio.CONCURSO);
        }

        [HttpPost("RealizaProva/Concurso/Finaliza/")]
        public int SetFinalizaConcurso(Exercicio exerc)
        {
            return new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity())
                .SetFinalizaExercicio(exerc);
        }

        [HttpGet("RealizaProva/Concurso/Questao/Imagem/IdImagem/{idImagem}/")]
        public string GetConcursoImagem(string idImagem)
        {
            return new ImagemEntity().GetConcursoBase64(Convert.ToInt32(idImagem));
        }

        [HttpPost("RealizaProva/Concurso/Questao/Avaliacao/Comentario/Inserir/")]
        public int SetConcursoQuestaoAvaliacao(QuestaoAvaliacaoComentario questaoAvaliacaoComentario)
        {
            return new QuestaoEntity().SetAvaliacao(questaoAvaliacaoComentario);
        }

        [HttpGet("RealizaProva/Concurso/Questao/Avaliacao/Permissao/QuestaoId/{questaoId}/TipoComentario/{tipoComentario}/Matricula/{matricula}/")]
        public int GetConcursoPermissaoAvaliacao(string idQuestao, string tipoComentario, string intClientId)
        {

            return new QuestaoEntity().GetPermissaoAvaliacao
            (
                Convert.ToInt32(idQuestao),
                Convert.ToInt32(Exercicio.tipoExercicio.CONCURSO),
                Convert.ToInt32(tipoComentario),
                Convert.ToInt32(intClientId)
            );
        }

        [HttpGet("RealizaProva/Concurso/Questao/AvaliacaoRealizada/QuestaoId/{questaoId}/TipoComentario/{tipoComentario}/Matricula/{matricula}/")]
        public int GetConcursoAvaliacaoRealizada(string idQuestao, string tipoComentario, string intClientId)
        {

            return new QuestaoEntity().GetAvaliacaoRealizada
            (
                Convert.ToInt32(idQuestao),
                Convert.ToInt32(Exercicio.tipoExercicio.CONCURSO),
                Convert.ToInt32(tipoComentario),
                Convert.ToInt32(intClientId)
            );
        }

        [MapToApiVersion("2")]
        [HttpGet("Concursos/NotaCorte/ProvaID/{provaID}/AlunoNota/{alunoNota}/Matricula/{matricula}/")]
        public NotaCorte GetNotaCorte(string provaID, string alunoNota, string matricula)
        {
            return new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), new ConfigEntity(), new VersaoAppPermissaoEntity()).GetNotaCorte(Convert.ToInt32(provaID), Convert.ToInt32(alunoNota), Convert.ToInt32(matricula));
        }
        
        [HttpGet("Concursos/NotaCorte/ProvaID/{provaID}/AlunoNota/{alunoNota}/Matricula/{matricula}/")]
        public NotaCorte GetNotaCorteJson(string provaID, string alunoNota, string matricula)
        {
            return GetNotaCorte(provaID, alunoNota, matricula);
        }
        [HttpGet("Concursos/NotaCorte/Permissao/ProvaID/{provaID}/")]
        public int GetIsProvaDiscursiva(string provaID)
        {
            //return new AlunoEntity().GetPermiteNotaCorte(Convert.ToInt32(provaID));
            return new ConcursoEntity().PermiteNotadeCorte(Convert.ToInt32(provaID));
        }
        
        [HttpGet("Concursos/NotaCorte/ProvaID/{provaID}/EstatisticaAluno/Matricula/{matricula}/")]
        public AlunoConcursoEstatistica GetAlunoConcursoEstatistica(string provaID, string matricula, bool anulada = false)
        {
            return new ConcursoBusiness(new ConcursoEntity()).GetConcursoStatsAluno(Convert.ToInt32(provaID), Convert.ToInt32(matricula), anulada);
        }

        [HttpGet("RealizaProva/Concurso/Questao/Avaliacao/Comentario/Opcoes/")]
        public List<QuestaoAvaliacaoComentarioOpcoes> GetConcursoOpcoesAvaliacaoNegativaComentarioQuestao()
        {
            return new QuestaoEntity().GetOpcoesAvaliacaoNegativaComentarioQuestao();
        }

        [HttpGet("Concurso/Impresso/{idexercicio}/")]
        public string GetLinkProvaImpressa(string idexercicio)
        {
            return ExercicioEntity.GetUrlProvaImpressa(Convert.ToInt32(idexercicio), "Concurso");
        }    

        //TODO Está faltando a versão V2 dos Endpoint "v2/Concursos/NotaCorte/ProvaID/{provaID}/EstatisticaAluno/Matricula/{matricula}/?anulada={anulada}"
    }
}