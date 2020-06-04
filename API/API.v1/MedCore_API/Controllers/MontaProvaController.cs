using System;
using System.Collections.Generic;
using AutoMapper;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCoreAPI.ViewModel.Base;
using Microsoft.AspNetCore.Mvc;

namespace MedCoreAPI.Controllers
{
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [ApiController]
    public class MontaProvaController : BaseService
    {
        public MontaProvaController(IMapper mapper): base(mapper) 
        {}
        

        [HttpGet]
        [Route("MontaProva/SyncRepositorioQuestoes/{anoString}")]
        public bool SincronizarRepositorioQuestoes(string anoString)
        {
            var ano = Convert.ToInt32(anoString);
            return new MontaProvaBusiness(new MontaProvaEntity()).SincronizarRepositorioQuestoes(ano);
        }

        [HttpGet]
        [Route("MontaProva/ListaAnosElegiveisSincronizar")]
        public List<int?> ListaAnosElegiveisSincronizar()
        {
            return new MontaProvaManager().GetListaAnosElegiveisSincronizar();
        }

        [HttpPost]
        [Route("MontaProva")]
        public int MontarProva(MontaProvaFiltroPost filtro)
        {
            return new MontaProvaManager().MontarProva(filtro);
        }

        [HttpPost]
        [Route("MontaProvaNovo")]
        public int MontarProvaNovo(MontaProvaFiltroPost filtro)
        {
            return new MontaProvaManager().MontarProva(filtro, Convert.ToInt32(ConfigurationProvider.Get("Settings:QuantidadeQuestaoProva")));
        }

        [HttpPost]
        [Route("MontaProva/Filtro/Filtrar/")]
        public MontaProvaFiltro Filtrar(MontaProvaFiltroPost filtro)
        {
            return new MontaProvaManager().Filtrar(filtro);
        }

        [HttpPost]
        [Route("MontaProva/Filtro/Modulo/")]
        public MontaProvaModuloFiltro FiltrarModulo(MontaProvaFiltroPost filtro)
        {
            return new MontaProvaManager().GetModuloFiltrado(filtro);
        }

        //Realiza Prova

        [HttpPost]
        [Route("RealizaProva/MontaProva/Questao/Objetiva/Inserir/")]
        public int SetMontaProvaRespostaObjetiva(RespostaObjetivaPost resp)
        {
            return new QuestaoBusiness(new QuestaoEntity(), new ImagemEntity(), new VideoEntity(), new EspecialidadeEntity(), 
				new FuncionarioEntity(), new SimuladoEntity(), new MontaProvaEntity()).SetRespostaObjetiva(resp);
        }

        [HttpPost]
        [Route("RealizaProva/MontaProva/Questao/Discursiva/Inserir/")]
        public int SetMontaProvaRespostaDiscursiva(RespostaDiscursivaPost resp)
        {
            return new QuestaoEntity().SetRespostaDiscursiva(resp);
        }

        [HttpGet]
        [Route("RealizaProva/MontaProva/Configuracao/IdProva/{idProva}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public Concurso GetMontaProvaConfiguracao(string idProva, string matricula, string idAplicacao)
        {
            return new ExercicioEntity().GetProvaPersonalizadaConfiguracao(Convert.ToInt32(idProva), Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao));

        }

        [HttpGet]
        [Route("RealizaProva/MontaProva/CartaoResposta/IdProva/{idProva}/Questoes/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public CartoesResposta GetMontaProvaCartaoResposta(string idProva, string matricula, string idAplicacao)
        {
            return new CartaoRespostaBusiness(new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity()).GetCartaoResposta(Convert.ToInt32(idProva), Convert.ToInt32(matricula), Exercicio.tipoExercicio.MONTAPROVA);
        }

        [HttpPost]
        [Route("RealizaProva/MontaProva/Finaliza/")]
        public int SetFinalizaProva(Exercicio exerc)
        {
            return new ExercicioBusiness(new ExercicioEntity(), new RankingSimuladoEntity(), new SimuladoEntity(), new CartaoRespostaEntity(), new QuestaoEntity(), new AulaEntity())
                .SetFinalizaExercicio(exerc);
        }

        [HttpPost]
        [Route("MontaProva/Filtro/{idFiltro}/Prova")]
        public ProvaAluno CriarProva(string idFiltro)
        {
            return new MontaProvaManager().CriarProva(Convert.ToInt32(idFiltro));
        }

        //Minhas Provas

        [HttpGet]
        [Route("MontaProva/Provas/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public FiltrosAluno GetProvasAluno(string matricula, string idAplicacao)
        {
            return new MontaProvaBusiness(new MontaProvaEntity()).GetProvasAluno(Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao));
        }

        [HttpGet]
        [Route("MontaProvaNovo/Provas/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public FiltrosAluno GetProvasAlunoNovo(string matricula, string idAplicacao, string page = "" , string limit = "")
        {
            var npage = String.IsNullOrEmpty(page) ? "0" : page;
            var nlimit = String.IsNullOrEmpty(limit) ? "0" : limit;
            return new MontaProvaBusiness(new MontaProvaEntity()).GetProvasAlunoNovo(Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao), Convert.ToInt32(npage), Convert.ToInt32(nlimit));
        }

        [HttpPost]
        [Route("MontaProva/Provas/Edit")]
        public int EditarProva(ProvaAluno provaAluno)
        {
            return new MontaProvaEntity().Edit(provaAluno);
        }

        [HttpPost]
        [Route("MontaProva/Provas/Delete")]
        public int ExcluirProva(ProvaAluno provaAluno)
        {
            return new MontaProvaEntity().Delete(provaAluno);
        }

        [HttpPost]
        [Route("MontaProvaNovo/Provas/Delete")]
        public int ExcluirProvaNovo(ProvaAluno provaAluno)
        {
            return new MontaProvaEntity().DeleteNovo(provaAluno);
        }

        [HttpPost]
        [Route("MontaProva/Filtro/{idFiltro}/Delete/")]
        public int ExcluirFiltro(string filtroAluno)
        {
            return new MontaProvaEntity().DeleteFiltro(Convert.ToInt32(filtroAluno));
        }

        [HttpPost]
        [Route("MontaProva/Filtro/{idFiltro}/Prova/{idProva}/Questoes/Alterar")]
        public int AlterarQuestoesProva(string idFiltro, string idProva, QuestoesMontaProvaPost questoes)
        {
            return new MontaProvaManager().AlterarQuestoesProva(Convert.ToInt32(idFiltro), Convert.ToInt32(idProva), questoes);
        }

        [HttpPost]
        [Route("MontaProvaNovo/Filtro/{idFiltro}/Prova/{idProva}/Questoes/Alterar")]
        public int AlterarQuestoesProvaNovo(string idFiltro, string idProva, QuestoesMontaProvaPost questoes)
        {
            return new MontaProvaManager().AlterarQuestoesProvaNovo(Convert.ToInt32(idFiltro), Convert.ToInt32(idProva), questoes);
        }

    }
}