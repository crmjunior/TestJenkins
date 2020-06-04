using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using MedCore_API.Util;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCoreAPI.ViewModel.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using StackExchange.Profiling;

namespace MedCore_API.Controllers
{
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [ApiController]
    public class MedCodeController : BaseService
    {

        public MedCodeController(IMapper mapper) 
            : base(mapper) {

        }

        [HttpGet("Slides/Baixados/{idaluno}/")]
        public List<AulaAvaliacao> GetSlidesBaixadosJson(string idaluno)
        {
            return new AulaAvaliacaoEntity().GetSlidesPermitidos(Convert.ToInt32(idaluno));
        }

        
        [HttpGet("Video/Intro/{apostilaID}/")]
        public Video GetVideoIntroJson(string apostilaID, string qualidade)
        {
            return GetVideoIntro(apostilaID, qualidade);
        }

        [HttpGet]
        [MapToXml]
        [Route( "Video/Intro/{apostilaID}/" )]
        public Video GetVideoIntro(string apostilaID, string param = "")
        {
            var retorno = new Video();
            int intApostilaID = String.IsNullOrEmpty(apostilaID) ? 0 : Convert.ToInt32(apostilaID);

            retorno = (new VideoEntity()).GetVideoIntro(intApostilaID, "", param);
            return retorno;
        }

        [HttpGet("MediaComentario/{idapostila}/{idMedia}/")]
        public GetMediaComentarioJsonDTO GetMediaComentarioJson(string idapostila, string idMedia)
        {
            return GetMediaComentario(idapostila, idMedia);
        }

        [HttpGet]
        [MapToXml]
        [Route("MediaComentario/{idapostila}/{idMedia}/")]
        public GetMediaComentarioJsonDTO GetMediaComentario(string idapostila, string idMedia)
        {
            var result = new GetMediaComentarioJsonDTO();
            
            Request.Headers.TryGetValue("idproduto", out StringValues codigo);
            var idA = Convert.ToInt32(idapostila);
            var idProdutoBanco = 0;
            bool isQuestaoInApostila = false;
            bool isMediaDataMatrix = false;
            var idProduto = Convert.ToInt32(codigo);
            using(MiniProfiler.Current.Step("Obtendo id do produto pela apostila"))
            {
                idProdutoBanco = (new ProdutoEntity()).GetByIdApostila(idA);
            }
            var idM = Convert.ToInt32(idMedia);
            var medeletro = new[] { (int)Produto.Cursos.MEDELETRO, (int)Produto.Produtos.MEDELETRO, (int)Produto.Produtos.ECGANTIGO }.ToList();

            using(MiniProfiler.Current.Step("Verifica se questão de apostila"))
            {
                isQuestaoInApostila = (new ApostilaEntity()).IsQuestaoInApostila(idA, idM);
            }
            
            using(MiniProfiler.Current.Step("Verifica media matrix"))
            {
                isMediaDataMatrix = (new MediaEntity()).IsMediaDataMatrix(idA, idM);
            }

            var medias = new Medias();
            if ((!medeletro.Contains(idProdutoBanco) || isQuestaoInApostila) && !isMediaDataMatrix)
            {
                medias = (new MediaEntity()).GetMediaComentario(idM);
            }
            else
            {
                medias = (new MediaEntity()).GetMediaComentario(idA, idM);
            }

            result.GetMediaComentarioJsonResult = medias;
            return result;
        }

        [HttpGet("Apostilas/{idapostila}/Apostila")]
        public List<Apostila> GetApostilaByIdJson(string idapostila)
        {
            return new ApostilaEntity().GetApostila(Convert.ToInt32(idapostila));
        }

        [HttpGet("Aula/Avaliacao/")]
        public AulaAvaliacao GetAulaAvaliacaoJson(string alunoID, string apostilaID)
        {
            return GetAulaAvaliacao(alunoID, apostilaID);
        }

        [HttpGet]
        [MapToXml]
        [Route("Aula/Avaliacao/")]
        public AulaAvaliacao GetAulaAvaliacao(string alunoID, string apostilaID)
        {
            AulaAvaliacao retorno = new AulaAvaliacao();

            int intAlunoID = String.IsNullOrEmpty(alunoID) ? 0 : Convert.ToInt32(alunoID);
            int intApostilaID = String.IsNullOrEmpty(apostilaID) ? 0 : Convert.ToInt32(apostilaID);

            retorno = (new AulaAvaliacaoEntity()).GetAulaAvaliacao(intAlunoID, intApostilaID);

            return retorno;
        }

        
        [HttpGet("AtualizacaoErrata/{idMarca}/")]
        public string GetAtualizacaoErrataJson(string idApostila)
        {
            return AtualizacaoErrataEntity.GetAtualizacaoErrata(Convert.ToInt32(idApostila));
        }


        
        [HttpGet("Resumo/Temas/IdProduto/{idProduto}/Matricula/{matricula}/")]
        public TemasApostila GetTemasVideosResumo(string idProduto, string matricula, string intProfessor = "0", string intAula = "0", string isSiteAdmin = "0")
        {
            return new MednetEntity().GetTemasVideoResumo(Convert.ToInt32(idProduto), Convert.ToInt32(matricula), Convert.ToInt32(intProfessor), Convert.ToInt32(intAula), Convert.ToBoolean(Convert.ToInt32(isSiteAdmin)));
        }

        
        [HttpPost("Aula/Avaliacao/Inserir/")]
        public AulaTema SetAulaAvaliacaoJson(AulaAvaliacaoPost aulaAvaliacaoPost)
        {
            return SetAulaAvaliacao(aulaAvaliacaoPost);
        }

        
        [HttpPost()]
        [MapToXml]
        [Route("Aula/Avaliacao/Inserir/")]
        public AulaTema SetAulaAvaliacao(AulaAvaliacaoPost aulaAvaliacaoPost)
        {
            AulaTema retorno = new AulaTema();

            retorno = (new AulaAvaliacaoEntity()).SetAulaAvaliacao(aulaAvaliacaoPost);

            return retorno;
        }

        
        [HttpGet("Questao/Avaliacao/Permissao/QuestaoId/{questaoId}/ExercicioTipoId/{exercicioTipoId}/TipoComentario/{tipoComentario}/Matricula/{matricula}/")]
        public int GetPermissaoAvaliacao(string idQuestao, string idTipoExercicio, string tipoComentario, string intClientId)
        {

            return new QuestaoEntity().GetPermissaoAvaliacao
                (
                Convert.ToInt32(idQuestao),
                Convert.ToInt32(idTipoExercicio),
                Convert.ToInt32(tipoComentario),
                Convert.ToInt32(intClientId)
                );
        }

        
        [HttpPost("Questao/Duvida/Inserir/")]
        public int SetDuvidaQuestao(QuestaoDuvida questaoDuvida)
        {
            return new QuestaoEntity().SetDuvida(questaoDuvida);
        }

        
        [HttpGet("Aula/Avaliacao/Aluno/{alunoid}/")]
        public List<AulaAvaliacao> GetAulaAvaliacaoPorAluno(string alunoid)
        {
            List<AulaAvaliacao> retorno = new List<AulaAvaliacao>();

            int intAlunoID = String.IsNullOrEmpty(alunoid) ? 0 : Convert.ToInt32(alunoid);

            retorno = (new AulaAvaliacaoEntity()).GetAulaAvaliacaoPorAluno(intAlunoID);

            return retorno;
        }

        
        [HttpGet("Clientes/{register}/")]
        public Clientes GetClienteJson(string register, string idaplicacao)
        {
            return GetClienteXml(register, idaplicacao);
        }

        
        [HttpPost()]
        [MapToXml]
        [Route("Clientes/{register}/")]
        public Clientes GetClienteXml(string register, string idaplicacao = "5")
        {
            var c = new Cliente
            {

                Register = register

            };
            return (Clientes)(new ClienteEntity()).GetByFilters(c, aplicacao: (Aplicacoes)Convert.ToInt32(idaplicacao));

        }

        [HttpGet("Aula/Slide/{idslideaula}")]
        public Stream GetImagemSlideAula(string idslideaula, string formato)
        {
            return (new ImagemEntity()).GetSlideAulaImageMsPro(Convert.ToInt32(idslideaula), formato);
        }        
    }
}