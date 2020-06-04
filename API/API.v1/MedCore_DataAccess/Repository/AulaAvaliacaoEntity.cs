using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Util;
using Microsoft.EntityFrameworkCore;
using MedCore_DataAccess.Contracts.Data;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Repository
{
    public class AulaAvaliacaoEntity : IAulaAvaliacaoData
    {
        public List<AulaAvaliacao> GetAulaAvaliacaoPorAluno(int alunoID)
        {
            using(MiniProfiler.Current.Step("Obtendo avaliação de aula do aluno"))
            {
                List<int> apostilaIDs = new List<int>();
                List<int> apostilaLessonIDs = new List<int>();
                List<int> chamadoTrocaTemporaria = new List<int>() { 1845, 1844 };
                ConcurrentBag<AulaAvaliacao> retorno = null;
                //var ctx = new materiaisDireitoEntities(true);
                retorno = new ConcurrentBag<AulaAvaliacao>();
                string urlProfessorFoto = "http://arearestrita.medgrupo.com.br/_static/images/professores/";
                string imagemTipoJpg = ".jpg";

                int[] statusPermitidoArray = new int[] { 0, 2, 5 };
                var isAcademico = new int[] { 96409, 173010, 207126 }.Contains(alunoID);
                var produtosEad = new int[] { 8, 9 };
                bool isAntecedencia = false;
                int? turmaConvidadaMED = null;
                DateTime dataAtual = new DateTime();
                bool alunoComRegraExcecao = AlunoComRegraExcecaoSlideAulas(alunoID);

                using (var ctx = new DesenvContext())
                {
                    turmaConvidadaMED = ctx.Set<msp_LoadCP_MED_Choice_Result>().FromSqlRaw("msp_LoadCP_MED_Choice @matricula = {0}", alunoID).ToList().FirstOrDefault().TurmaConvidadaMed;
                    isAntecedencia = ctx.tblAPI_VisualizarAntecedencia.Where(x => x.intContactID == alunoID).Any();
                    dataAtual = !isAntecedencia ? DateTime.Now : DateTime.Now.AddMonths(2);

                    var ids = (from sellOrders in ctx.tblSellOrders
                                join sellOrderDetails in ctx.tblSellOrderDetails on sellOrders.intOrderID equals sellOrderDetails.intOrderID
                                join products in ctx.tblProducts on sellOrderDetails.intProductID equals products.intProductID
                                join productGroups1 in ctx.tblProductGroups1 on products.intProductGroup1 equals productGroups1.intProductGroup1ID
                                join courses in ctx.tblCourses on sellOrderDetails.intProductID equals courses.intCourseID
                                join mviewCronograma in ctx.mview_Cronograma on courses.intCourseID equals mviewCronograma.intCourseID
                                join lessonTitles in ctx.tblLessonTitles on mviewCronograma.intLessonTitleID equals lessonTitles.intLessonTitleID
                                join lessonMaterial in ctx.tblLesson_Material on mviewCronograma.intLessonID equals lessonMaterial.intLessonID
                                where statusPermitidoArray.Contains(sellOrders.intStatus ?? 0) &&
                                sellOrders.intClientID == alunoID
                                && mviewCronograma.dteDateTime < dataAtual
                                && (
                                !alunoComRegraExcecao ? true
                                : productGroups1.intProductGroup1ID == (int)Produto.Produtos.MEDELETRO ? mviewCronograma.intYear >= Utilidades.AnoLancamentoMaterialMedEletro
                                : productGroups1.intProductGroup1ID == (int)Produto.Produtos.INTENSIVAO ? mviewCronograma.intYear >= Utilidades.AnoLancamentoMaterialIntensivao
                                : productGroups1.intProductGroup1ID == (int)Produto.Produtos.CPMED ? mviewCronograma.intYear >= Utilidades.AnoLancamentoMaterialCPMed
                                : mviewCronograma.intYear > Utilidades.AnoLancamentoMedsoftPro
                                )
                                select new
                                {
                                    ID = lessonMaterial.intMaterialID,
                                    LessonID = lessonMaterial.intLessonID
                                }).ToList();

                    var ids2 = (from ccc in ctx.tblCallCenterCalls
                                join mvc in ctx.mview_Cronograma on ccc.intCourseID equals mvc.intCourseID
                                join lm in ctx.tblLesson_Material on mvc.intLessonID equals lm.intLessonID
                                join lessonTitles in ctx.tblLessonTitles on mvc.intLessonTitleID equals lessonTitles.intLessonTitleID
                                join courses in ctx.tblProducts on mvc.intCourseID equals courses.intProductID
                                join material in ctx.tblProducts on lm.intMaterialID equals material.intProductID
                                join productGroups1 in ctx.tblProductGroups1 on courses.intProductGroup1 equals productGroups1.intProductGroup1ID
                                where ccc.intClientID == alunoID && chamadoTrocaTemporaria.Contains(ccc.intCallCategoryID) && ccc.intStatusID != 7
                                && mvc.dteDateTime < dataAtual
                                && (
                                !alunoComRegraExcecao ? true
                                : productGroups1.intProductGroup1ID == (int)Produto.Produtos.MEDELETRO ? mvc.intYear >= Utilidades.AnoLancamentoMaterialMedEletro
                                : productGroups1.intProductGroup1ID == (int)Produto.Produtos.INTENSIVAO ? mvc.intYear >= Utilidades.AnoLancamentoMaterialIntensivao
                                : productGroups1.intProductGroup1ID == (int)Produto.Produtos.CPMED ? mvc.intYear >= Utilidades.AnoLancamentoMaterialCPMed
                                : mvc.intYear > Utilidades.AnoLancamentoMedsoftPro
                                )
                                select new
                                {
                                    ID = lm.intMaterialID,
                                    LessonID = lm.intLessonID
                                }).ToList();

                    var ids3 = (from mvc in ctx.mview_Cronograma
                                join lm in ctx.tblLesson_Material on mvc.intLessonID equals lm.intLessonID
                                join books in ctx.tblBooks on lm.intMaterialID equals books.intBookID
                                join lessonTitles in ctx.tblLessonTitles on mvc.intLessonTitleID equals lessonTitles.intLessonTitleID
                                join courses in ctx.tblProducts on mvc.intCourseID equals courses.intProductID
                                join productGroups1 in ctx.tblProductGroups1 on courses.intProductGroup1 equals productGroups1.intProductGroup1ID
                                where mvc.intCourseID == turmaConvidadaMED && books.intBookEntityID == Constants.APOSTILATREINAMENTOTEORICOPRATICO
                                && mvc.dteDateTime < dataAtual
                                && (
                                !alunoComRegraExcecao ? true
                                : productGroups1.intProductGroup1ID == (int)Produto.Produtos.MEDELETRO ? mvc.intYear >= Utilidades.AnoLancamentoMaterialMedEletro
                                : productGroups1.intProductGroup1ID == (int)Produto.Produtos.INTENSIVAO ? mvc.intYear >= Utilidades.AnoLancamentoMaterialIntensivao
                                : productGroups1.intProductGroup1ID == (int)Produto.Produtos.CPMED ? mvc.intYear >= Utilidades.AnoLancamentoMaterialCPMed
                                : mvc.intYear > Utilidades.AnoLancamentoMedsoftPro
                                )
                                select new
                                {
                                    ID = lm.intMaterialID,
                                    LessonID = lm.intLessonID
                                }).ToList(); 

                    ids.AddRange(ids2);
                    ids.AddRange(ids3);
                    ids = ids.Distinct().ToList();

                    apostilaIDs = ids.Select(r => r.ID).Distinct().ToList();
                    apostilaLessonIDs = ids.Select(r => r.LessonID).Distinct().ToList();
                }

                List<int> MarcasCompartilhadas = new List<int>();
                var MarcasPorApostila = new List<KeyValuePair<int, List<int>>>();

                MarcasPorApostila = GetMarcasCompartilhadasList(apostilaIDs);

                if (MarcasPorApostila.Count > 0)
                    MarcasPorApostila.ForEach(m => MarcasCompartilhadas.AddRange(m.Value));

                var aulaAvaliacaoConsulta = new List<AulaAvaliacaoConsultaDTO>();

                using (var ctx = new DesenvContext())
                {

                    aulaAvaliacaoConsulta = (from sellOrders in ctx.tblSellOrders
                                                join sellOrderDetails in ctx.tblSellOrderDetails on sellOrders.intOrderID equals sellOrderDetails.intOrderID
                                                join products in ctx.tblProducts on sellOrderDetails.intProductID equals products.intProductID
                                                join productGroups1 in ctx.tblProductGroups1 on products.intProductGroup1 equals productGroups1.intProductGroup1ID
                                                join courses in ctx.tblCourses on sellOrderDetails.intProductID equals courses.intCourseID
                                                join mviewCronograma in ctx.mview_Cronograma on courses.intCourseID equals mviewCronograma.intCourseID
                                                join lessonTitles in ctx.tblLessonTitles on mviewCronograma.intLessonTitleID equals lessonTitles.intLessonTitleID
                                                join lessonMaterial in ctx.tblLesson_Material on mviewCronograma.intLessonID equals lessonMaterial.intLessonID
                                                join lessonTeachersByGroupAndTitle in ctx.tblLessonTeachersByGroupAndTitle on mviewCronograma.intLessonTitleID equals lessonTeachersByGroupAndTitle.intLessonTitleID
                                                join professores in ctx.tblPersons on lessonTeachersByGroupAndTitle.intEmployeeID equals professores.intContactID
                                                where statusPermitidoArray.Contains(sellOrders.intStatus ?? 0) &&
                                                    sellOrders.intClientID == alunoID
                                                    && mviewCronograma.dteDateTime < dataAtual
                                                    && (
                                                        !alunoComRegraExcecao ? true
                                                        : productGroups1.intProductGroup1ID == (int)Produto.Produtos.MEDELETRO ? mviewCronograma.intYear >= Utilidades.AnoLancamentoMaterialMedEletro
                                                        : productGroups1.intProductGroup1ID == (int)Produto.Produtos.INTENSIVAO ? mviewCronograma.intYear >= Utilidades.AnoLancamentoMaterialIntensivao
                                                        : productGroups1.intProductGroup1ID == (int)Produto.Produtos.CPMED ? mviewCronograma.intYear >= Utilidades.AnoLancamentoMaterialCPMed
                                                        : mviewCronograma.intYear > Utilidades.AnoLancamentoMedsoftPro
                                                        )
                                                select new AulaAvaliacaoConsultaDTO()
                                                {
                                                    ID = lessonMaterial.intMaterialID,
                                                    ProfessorNome = professores.txtName.Trim(),
                                                    ProfessorID = professores.intContactID,
                                                    ProdutoNome = productGroups1.txtDescription.Trim(),
                                                    ProdutoID = productGroups1.intProductGroup1ID,
                                                    TemaNome = lessonTitles.txtLessonTitleName.Trim(),
                                                    AulaID = mviewCronograma.intLessonID,
                                                    TemaID = mviewCronograma.intLessonTitleID,
                                                    TemaData = mviewCronograma.dteDateTime,
                                                    AlunoID = sellOrders.intClientID,
                                                    AlunoStatus = sellOrders.intStatus,
                                                    SalaAula = mviewCronograma.intClassRoomID,
                                                    DataPrevisao1 = new DateTime?(),
                                                    DataPrevisao2 = new DateTime?()
                                                }).ToList();

                    aulaAvaliacaoConsulta = aulaAvaliacaoConsulta.Where(x => (apostilaIDs.Contains(x.ID) || (MarcasCompartilhadas.Contains(x.ID))) && apostilaLessonIDs.Contains(x.AulaID)).ToList();

                    var aulaAvaliacaoConsulta2 = (from ccc in ctx.tblCallCenterCalls
                                                join mvc in ctx.mview_Cronograma on ccc.intCourseID equals mvc.intCourseID
                                                join lm in ctx.tblLesson_Material on mvc.intLessonID equals lm.intLessonID
                                                join lessonTitles in ctx.tblLessonTitles on mvc.intLessonTitleID equals lessonTitles.intLessonTitleID
                                                join courses in ctx.tblProducts on mvc.intCourseID equals courses.intProductID
                                                join material in ctx.tblProducts on lm.intMaterialID equals material.intProductID
                                                join productGroups1 in ctx.tblProductGroups1 on courses.intProductGroup1 equals productGroups1.intProductGroup1ID
                                                join lessonTeachersByGroupAndTitle in ctx.tblLessonTeachersByGroupAndTitle on mvc.intLessonTitleID equals lessonTeachersByGroupAndTitle.intLessonTitleID
                                                join professores in ctx.tblPersons on lessonTeachersByGroupAndTitle.intEmployeeID equals professores.intContactID
                                                where ccc.intClientID == alunoID && chamadoTrocaTemporaria.Contains(ccc.intCallCategoryID) && ccc.intStatusID != 7
                                                && mvc.dteDateTime < dataAtual
                                                && (
                                                        !alunoComRegraExcecao ? true
                                                        : productGroups1.intProductGroup1ID == (int)Produto.Produtos.MEDELETRO ? mvc.intYear >= Utilidades.AnoLancamentoMaterialMedEletro
                                                        : productGroups1.intProductGroup1ID == (int)Produto.Produtos.INTENSIVAO ? mvc.intYear >= Utilidades.AnoLancamentoMaterialIntensivao
                                                        : productGroups1.intProductGroup1ID == (int)Produto.Produtos.CPMED ? mvc.intYear >= Utilidades.AnoLancamentoMaterialCPMed
                                                        : mvc.intYear > Utilidades.AnoLancamentoMedsoftPro
                                                    )
                                                select new AulaAvaliacaoConsultaDTO()
                                                {
                                                    ID = lm.intMaterialID,
                                                    ProfessorNome = professores.txtName.Trim(),
                                                    ProfessorID = professores.intContactID,
                                                    ProdutoNome = productGroups1.txtDescription.Trim(),
                                                    ProdutoID = productGroups1.intProductGroup1ID,
                                                    TemaNome = lessonTitles.txtLessonTitleName.Trim(),
                                                    AulaID = mvc.intLessonID,
                                                    TemaID = mvc.intLessonTitleID,
                                                    TemaData = mvc.dteDateTime,
                                                    AlunoID = ccc.intClientID,
                                                    AlunoStatus = new int?(2),
                                                    SalaAula = mvc.intClassRoomID,
                                                    DataPrevisao1 = ccc.dteDataPrevisao1,
                                                    DataPrevisao2 = ccc.dteDataPrevisao2,
                                                }).ToList();

                    aulaAvaliacaoConsulta2 = aulaAvaliacaoConsulta2.Where(x => apostilaIDs.Contains(x.ID) && apostilaLessonIDs.Contains(x.AulaID)).ToList();

                    var aulaAvaliacaoConsulta3 = (from mvc in ctx.mview_Cronograma
                                                join lm in ctx.tblLesson_Material on mvc.intLessonID equals lm.intLessonID
                                                join books in ctx.tblBooks on lm.intMaterialID equals books.intBookID
                                                join lessonTitles in ctx.tblLessonTitles on mvc.intLessonTitleID equals lessonTitles.intLessonTitleID
                                                join courses in ctx.tblProducts on mvc.intCourseID equals courses.intProductID
                                                join productGroups1 in ctx.tblProductGroups1 on courses.intProductGroup1 equals productGroups1.intProductGroup1ID
                                                join lessonTeachersByGroupAndTitle in ctx.tblLessonTeachersByGroupAndTitle on mvc.intLessonTitleID equals lessonTeachersByGroupAndTitle.intLessonTitleID
                                                join professores in ctx.tblPersons on lessonTeachersByGroupAndTitle.intEmployeeID equals professores.intContactID
                                                where mvc.intCourseID == turmaConvidadaMED && books.intBookEntityID == Constants.APOSTILATREINAMENTOTEORICOPRATICO
                                                && mvc.dteDateTime < dataAtual
                                                && (
                                                        !alunoComRegraExcecao ? true
                                                        : productGroups1.intProductGroup1ID == (int)Produto.Produtos.MEDELETRO ? mvc.intYear >= Utilidades.AnoLancamentoMaterialMedEletro
                                                        : productGroups1.intProductGroup1ID == (int)Produto.Produtos.INTENSIVAO ? mvc.intYear >= Utilidades.AnoLancamentoMaterialIntensivao
                                                        : productGroups1.intProductGroup1ID == (int)Produto.Produtos.CPMED ? mvc.intYear >= Utilidades.AnoLancamentoMaterialCPMed
                                                        : mvc.intYear > Utilidades.AnoLancamentoMedsoftPro
                                                )
                                                select new AulaAvaliacaoConsultaDTO()
                                                {
                                                    ID = lm.intMaterialID,
                                                    ProfessorNome = professores.txtName.Trim(),
                                                    ProfessorID = professores.intContactID,
                                                    ProdutoNome = productGroups1.txtDescription.Trim(),
                                                    ProdutoID = productGroups1.intProductGroup1ID,
                                                    TemaNome = lessonTitles.txtLessonTitleName.Trim(),
                                                    AulaID = mvc.intLessonID,
                                                    TemaID = mvc.intLessonTitleID,
                                                    TemaData = mvc.dteDateTime,
                                                    AlunoID = alunoID,
                                                    AlunoStatus = new int?(2),
                                                    SalaAula = mvc.intClassRoomID,
                                                    DataPrevisao1 = new DateTime?(),
                                                    DataPrevisao2 = new DateTime?()
                                                }).ToList();

                    aulaAvaliacaoConsulta3 = aulaAvaliacaoConsulta3.Where(x => apostilaIDs.Contains(x.ID) && apostilaLessonIDs.Contains(x.AulaID)).ToList();

                    aulaAvaliacaoConsulta.AddRange(aulaAvaliacaoConsulta2);
                    aulaAvaliacaoConsulta.AddRange(aulaAvaliacaoConsulta3);
                    aulaAvaliacaoConsulta = aulaAvaliacaoConsulta.Distinct().ToList();
                }

                if (aulaAvaliacaoConsulta.Any())
                {
                    var temasAvaliados = new List<TemasAvaliadosDTO>();

                    using (var ctx = new DesenvContext())
                    {
                        temasAvaliados = (from lessonsEvaluation in ctx.tblLessonsEvaluation
                                        join mviewCronograma in ctx.mview_Cronograma on lessonsEvaluation.intLessonID equals mviewCronograma.intLessonID
                                        join lessonTitles in ctx.tblLessonTitles on mviewCronograma.intLessonTitleID equals lessonTitles.intLessonTitleID
                                        where lessonsEvaluation.intClientID == alunoID
                                        && lessonsEvaluation.intNota != 0
                                        select new TemasAvaliadosDTO()
                                        {
                                            AulaID = lessonsEvaluation.intLessonID,
                                            Tema = lessonTitles.txtLessonTitleName.Trim(),
                                            TemaData = lessonsEvaluation.dteEvaluationDate,
                                            AvaliacaoID = lessonsEvaluation.intEvaluationID
                                        }
                        ).ToList();
                    }



                    var aulasID = aulaAvaliacaoConsulta.Select(aac => aac.AulaID).Distinct().ToList();
                    var temasID = aulaAvaliacaoConsulta.Select(aac => aac.TemaID).Distinct().ToList();
                    var professoresID = aulaAvaliacaoConsulta.Select(aac => aac.ProfessorID).Distinct().ToList();

                    var professoresSubID = new List<int?>();

                    var professoresSubstitutos = new List<ProfessoresSubstitutosDTO>();
                    var professoresSub = new Dictionary<int, string>();
                    using (var ctx = new DesenvContext())
                    {
                        professoresSubstitutos = (from professor in ctx.tblLessonTeacherSubstituto
                                                    join employee in ctx.tblEmployees on professor.intEmployeeID equals employee.intEmployeeID
                                                    join person in ctx.tblPersons on employee.intEmployeeID equals person.intContactID
                                                select new ProfessoresSubstitutosDTO { professorSubstituto = professor, Nome = person.txtName}).ToList();

                        professoresSubstitutos = professoresSubstitutos.Where(professor => aulasID.Contains(professor.professorSubstituto.intLessonID.Value)).OrderByDescending(x => x.professorSubstituto.intID).ToList();

                        var professoresSubNome = new List<string>();
                        professoresSubID = professoresSubstitutos.Select(ps => ps.professorSubstituto.intEmployeeID).ToList();

                        professoresSub = professoresSubstitutos.Select(ps => new { Id = ps.professorSubstituto.intEmployeeID.Value, Nome = ps.Nome.Trim() }).Distinct().ToDictionary(x => x.Id, x => x.Nome);


                    }

                    var slides = new List<SlideAulaDTO>();
                    using (var ctx = new DesenvContext())
                    {
                        slides = (from s in ctx.tblRevisaoAula_Slides
                                where temasID.Contains(s.intLessonTitleID)
                                    && (professoresID.Contains(s.intProfessorID)
                                    || professoresSubID.Contains(s.intProfessorID)
                                    )
                                orderby s.intOrder
                                select new SlideAulaDTO()
                                {
                                    TemaID = s.intLessonTitleID,
                                    ProfessorID = s.intProfessorID,
                                    SlideID = s.intSlideAulaID
                                }
                        ).ToList();
                    }

                    List<KeyValuePair<string, KeyValuePair<int, int>>> slidesAula = new List<KeyValuePair<string, KeyValuePair<int, int>>>();

                    foreach (var slide in slides)
                    {
                        KeyValuePair<int, int> temaProfessor = new KeyValuePair<int, int>(slide.TemaID, slide.ProfessorID);
                        string url = Constants.URLSLIDES_MSCROSS.Replace("IDSLIDE", slide.SlideID.ToString()).Replace("FORMATO", "cellimage");
                        slidesAula.Add(new KeyValuePair<string, KeyValuePair<int, int>>(url, temaProfessor));
                    }

                    var apostilas = new List<ApostilaDTO>();
                    var tblLogAccess = new List<tblAccessLog>();
                    var aulaAvaliacaoConsultaApostilaIDs = new List<int>();

                    using (var ctx = new DesenvContext())
                    {
                        
                        apostilas = (from apostila in ctx.Set<msp_API_ListaApostilas_Result>().FromSqlRaw("msp_API_ListaApostilas @intYear = {0}", 0).ToList()
                                    select new ApostilaDTO()
                                    {
                                        ID = apostila.intBookID,
                                        Capa = !String.IsNullOrEmpty(apostila.Capa) ? Convert.ToString(apostila.Capa).Trim() : "",
                                        Codigo = !String.IsNullOrEmpty(apostila.txtCode) ? Convert.ToString(apostila.txtCode).Trim() : "",
                                        Titulo = !String.IsNullOrEmpty(apostila.txtTitle) ? Convert.ToString(apostila.txtTitle).Trim() : "",
                                        Ano = Convert.ToInt32(apostila.intYear),
                                        IdProduto = (apostila.intProductGroup2.Equals((int)Produto.Cursos.MEDMEDCURSO)) ?
                                        (int)Produto.Cursos.MEDCURSO : (apostila.intProductGroup2.Equals((int)Produto.Cursos.MEDCPMED)) ?
                                        (int)Produto.Cursos.MED : Convert.ToInt32(apostila.intProductGroup2),
                                        IdProdutoGrupo = apostila.intProductGroup2.Value,
                                        IdGrandeArea = Convert.ToInt32(apostila.intClassificacaoID),
                                        IdSubEspecialidade = Convert.ToInt32(apostila.intProductGroup3),
                                        IdEntidade = Convert.ToInt32(apostila.intBookEntityID),
                                        ProdutosAdicionais = (apostila.intProductGroup2.Equals((int)Produto.Cursos.MEDMEDCURSO)) ?
                                        Convert.ToInt32(Produto.Cursos.MED).ToString() : (apostila.intProductGroup2.Equals((int)Produto.Cursos.MEDCPMED)) ?
                                        Convert.ToInt32(Produto.Cursos.CPMED).ToString() : (apostila.intProductGroup2.Equals((int)Produto.Cursos.RAC) || apostila.intProductGroup2.Equals((int)Produto.Cursos.RACIPE)) ? Convert.ToInt32((int)Produto.Cursos.RA).ToString() : "",
                                        NomeCompleto = string.Format("{0} -{1}", apostila.txtCode, (apostila.txtName ?? "").Trim().Replace(apostila.intYear.ToString(), "")),
                                    }).ToList();

                        apostilas = apostilas.Where(x => apostilaIDs.Contains(x.ID)).ToList();

                        tblLogAccess = (from a in ctx.tblAccessLog
                                        where alunoID == a.intPeopleID
                                        select a).Distinct().ToList();

                        var temasAluno = aulaAvaliacaoConsulta.Select(x => x.TemaID).Distinct().ToList();
                        var temasEad = aulaAvaliacaoConsulta.Where(y => produtosEad.Contains(y.ProdutoID)).Select(x => x.TemaID).Distinct().ToList();

                        var aulasEmTodasAsFiliais = ctx.mview_Cronograma.Where(x => temasAluno.Contains(x.intLessonTitleID))
                            .Select(x => new AulasEmFiliaisDTO()
                            {
                                idTema = x.intLessonTitleID,
                                TemaData = x.dteDateTime,
                                sala = x.intClassRoomID
                            }).ToList();

                        var aulasAssistidasTodasAsFiliais = new List<int>();

                        if (!isAcademico)
                        {
                            var tblLogAccessDistinct = tblLogAccess.Select(x => new { sala = (int)x.intClassroomID, TemaData = x.dteDateTime.ToString("dd/MM/yyyy") }).Distinct().ToList();
                            var aulasEmTodasAsFiliaisDistinct = aulasEmTodasAsFiliais.Select(x => new { idTema = x.idTema, sala = x.sala, TemaData = x.TemaData.ToString("dd/MM/yyyy") }).Distinct().ToList();

                            var aulasAssistidasTodasAsFiliaisUnion = (from t1 in tblLogAccessDistinct
                                                join t2 in aulasEmTodasAsFiliaisDistinct on new { t1.sala, t1.TemaData } equals new { t2.sala, t2.TemaData }
                                        select t2).ToList();

                            aulasAssistidasTodasAsFiliais = aulasAssistidasTodasAsFiliaisUnion.Select(x => x.idTema).Distinct().ToList();
                        }
                        else
                        {
                            aulasAssistidasTodasAsFiliais = aulasEmTodasAsFiliais.Select(x => x.idTema).Distinct().ToList();
                        }

                        aulaAvaliacaoConsultaApostilaIDs = aulaAvaliacaoConsulta.Where(y => aulasAssistidasTodasAsFiliais.Any(z => z == y.TemaID)).Select(x => x.ID).Distinct().ToList();     
                    }

                    aulaAvaliacaoConsultaApostilaIDs = aulaAvaliacaoConsultaApostilaIDs.Where(x => apostilas.Select(y => y.ID).ToList().Contains(x)).ToList();

                    Parallel.For<ConcurrentBag<AulaAvaliacao>>(0, aulaAvaliacaoConsultaApostilaIDs.Count(), () => new ConcurrentBag<AulaAvaliacao>(), (index, loop, retornoParalelo) =>
                    {
                        {
                            AulaAvaliacao aulaAvaliacao = new AulaAvaliacao();

                            var a = aulaAvaliacaoConsultaApostilaIDs[(int)index];

                            var idApostila = a;

                            for (var x = 0; x < MarcasPorApostila.Count(); x++)
                                if (MarcasPorApostila[x].Value.Contains(a))
                                {
                                    idApostila = MarcasPorApostila[x].Key;
                                    break;
                                }
                            
                            var apostila = apostilas.Where(y => y.ID == a).FirstOrDefault();

                            if (apostila != null)
                            {
                                aulaAvaliacao.ID = a;
                                aulaAvaliacao.IdProduto = apostila.IdProduto;
                                aulaAvaliacao.Titulo = apostila.Titulo;
                                aulaAvaliacao.IdGrandeArea = apostila.IdGrandeArea;
                                aulaAvaliacao.IdSubEspecialidade = apostila.IdSubEspecialidade;
                                aulaAvaliacao.ProdutosAdicionais = apostila.ProdutosAdicionais;
                                aulaAvaliacao.NomeCompleto = apostila.NomeCompleto;
                                aulaAvaliacao.Ano = apostila.Ano;

                                var assistidas = aulaAvaliacaoConsulta.Where(i => i.ID == a);
                                var aulaAvaliacaoOrdenado = assistidas.OrderBy(y => y.TemaData);
                                var temaAtualList = aulaAvaliacaoOrdenado.ToList();
                                var lTemas = new List<AulaTema>();
                                
                                if (assistidas.Any())
                                { 
                                    foreach (var aulaItem in temaAtualList)
                                    {

                                        var professorSubstituto = professoresSubstitutos == null || professoresSubstitutos.Count == 0 ? null : professoresSubstitutos.First();
                                        if (professoresSubstitutos != null) professoresSubstitutos.ForEach(ps => { if (ps.professorSubstituto.intLessonID.Value == aulaItem.AulaID) professorSubstituto = ps; });
                                        if (professorSubstituto != null && professorSubstituto.professorSubstituto.intLessonID.Value != aulaItem.AulaID) professorSubstituto = null;

                                        if (professorSubstituto != null)
                                        {
                                            aulaItem.ProfessorID = (int)professorSubstituto.professorSubstituto.intEmployeeID;
                                            aulaItem.ProfessorNome = professoresSub.Where(x => x.Key == aulaItem.ProfessorID).First().Value;
                                        }
                                        else
                                        {
                                            aulaItem.ProfessorID = aulaItem.ProfessorID;
                                            aulaItem.ProfessorNome = aulaItem.ProfessorNome.Trim();
                                        }

                                    }
                                    var agrupamento_professores_temas = temaAtualList.Select(x => new { ProfessorID = x.ProfessorID, TemaID = x.TemaID, TemaData = temaAtualList.Where(t => t.ProfessorID == x.ProfessorID && t.TemaID == x.TemaID).ToList().Max(y => y.TemaData) }).Distinct().ToList();

                                    var temaAtualListDistinct = temaAtualList.Where(x => agrupamento_professores_temas.Where(y => y.ProfessorID == x.ProfessorID && y.TemaID == x.TemaID && y.TemaData.ToString("dd/MM/yyyy") == x.TemaData.ToString("dd/MM/yyyy")).Any()).Distinct().ToList(); 

                                    foreach (var aulaItem in temaAtualListDistinct)
                                    {
                                        AulaTema aulaTema = new AulaTema();
                                        var temaAvaliado = temasAvaliados == null || temasAvaliados.Count == 0 ? null : temasAvaliados.First();
                                        if (temasAvaliados != null) temasAvaliados.ForEach(ta => { if (ta.AulaID == aulaItem.AulaID) temaAvaliado = ta; });
                                        if (temaAvaliado != null && temaAvaliado.AulaID != aulaItem.AulaID) temaAvaliado = null;

                                        bool podeAvaliar = (temaAvaliado == null) && (aulaItem.TemaData < Utilidades.GetServerDate(-1)) && statusPermitidoArray.Contains((int)aulaItem.AlunoStatus) && (aulaItem.TemaData > DateTime.Now.AddMonths(-11));


                                        var professorSubstituto = professoresSubstitutos == null || professoresSubstitutos.Count == 0 ? null : professoresSubstitutos.First();
                                        if (professoresSubstitutos != null) professoresSubstitutos.ForEach(ps => { if (ps.professorSubstituto.intLessonID.Value == aulaItem.AulaID) professorSubstituto = ps; });
                                        if (professorSubstituto != null && professorSubstituto.professorSubstituto.intLessonID.Value != aulaItem.AulaID) professorSubstituto = null;

                                        if (professorSubstituto != null)
                                        {
                                            aulaTema.ProfessorID = (int)professorSubstituto.professorSubstituto.intEmployeeID;
                                            aulaTema.ProfessorNome = professoresSub.Where(x => x.Key == aulaTema.ProfessorID).First().Value;
                                            aulaTema.ProfessorFoto = urlProfessorFoto + professorSubstituto.professorSubstituto.intEmployeeID.Value.ToString() + imagemTipoJpg;
                                        }
                                        else
                                        {
                                            aulaTema.ProfessorID = aulaItem.ProfessorID;
                                            aulaTema.ProfessorNome = aulaItem.ProfessorNome.Trim();
                                            aulaTema.ProfessorFoto = urlProfessorFoto + aulaItem.ProfessorID + imagemTipoJpg;
                                        }

                                        aulaTema.Nome = aulaItem.TemaNome.Trim();
                                        aulaTema.Data = aulaItem.TemaData.ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                        aulaTema.AulaID = aulaItem.AulaID;
                                        aulaTema.TemaID = aulaItem.TemaID;
                                        aulaTema.PodeAvaliar = podeAvaliar;
                                        aulaTema.IsAvaliado = (temaAvaliado == null) ? false : true;
                                        aulaTema.AvaliacaoID = (temaAvaliado == null) ? 0 : temaAvaliado.AvaliacaoID;
                                        aulaTema.Rotulo = "Primeira Aula";

                                        List<string> slidesDoTemaProfessor = new List<string>();

                                        if (slidesAula != null && slidesAula.Count > 0)
                                        {
                                            slidesAula.ForEach(s =>
                                            {
                                                if (s.Value.Key == aulaTema.TemaID && s.Value.Value == aulaTema.ProfessorID)
                                                    slidesDoTemaProfessor.Add(s.Key);
                                            });
                                        }

                                        if (aulaTema.ProfessorID != 0)
                                            aulaTema.ProfessorNome = PessoaEntity.GetNomeResumido(aulaTema.ProfessorID);

                                        if (slidesDoTemaProfessor.Count > 0)
                                        {
                                            aulaTema.Slides = slidesDoTemaProfessor;
                                            lTemas.Add(aulaTema);
                                        }
                                    }

                                    if (lTemas.Count() > 0)
                                    {
                                        var temas = lTemas.GroupBy(x => new { x.ProfessorNome, x.TemaID });

                                        aulaAvaliacao.Temas = new List<AulaTema>();
                                        aulaAvaliacao.ID = idApostila;

                                        if (retornoParalelo.Count() == 0)
                                            retornoParalelo.Add(aulaAvaliacao);
                                        else if (!retornoParalelo.Select(r => r.ID).Contains(aulaAvaliacao.ID))
                                            retornoParalelo.Add(aulaAvaliacao);

                                        var temasBag = new List<AulaTema>();
                                        foreach (var t in temas)
                                        {
                                            var dataAv = t.Max(x => x.Data);
                                            var pos = t.Where(y => y.Data == dataAv).First();
                                            var tema = new AulaTema
                                            {
                                                Data = dataAv,
                                                ProfessorNome = t.Key.ProfessorNome,
                                                TemaID = t.Key.TemaID,

                                                AulaID = pos.AulaID,
                                                AvaliacaoID = pos.AvaliacaoID,
                                                IsAvaliado = pos.IsAvaliado,
                                                Nome = pos.Nome,
                                                PodeAvaliar = pos.PodeAvaliar,
                                                ProfessorFoto = pos.ProfessorFoto,
                                                ProfessorID = pos.ProfessorID,
                                                Rotulo = pos.Rotulo,
                                                Slides = pos.Slides,

                                            };
                                            temasBag.Add(tema);
                                        }
                                        retornoParalelo.Where(r => r.ID == aulaAvaliacao.ID).FirstOrDefault().Temas.AddRange(temasBag.ToList());
                                    }
                                }
                            }
                        }
                            return retornoParalelo;
                        },
                        (retornoParalelo) => retornoParalelo.ToList().ForEach(x => retorno.Add(x))
                        );
                    }
                return retorno.ToList();
            }
        }

        private bool AlunoComRegraExcecaoSlideAulas(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblAlunoExcecaoSlideAulas.Any(x => x.intClientID == matricula);
            }
        }

        public List<AulaAvaliacao> GetAulaAvaliacaoPorAluno(int alunoID, int produto, int grandeArea)
        {
            List<int> apostilaIDs = new List<int>();
            List<int> apostilaLessonIDs = new List<int>();
            List<int> chamadoTrocaTemporaria = new List<int>() { 1845, 1844 };
            List<AulaAvaliacao> retorno = null;
            var lTemas = new List<AulaTema>();
            var ctx = new DesenvContext();
            retorno = new List<AulaAvaliacao>();
            string urlProfessorFoto = "http://arearestrita.medgrupo.com.br/_static/images/professores/";
            string imagemTipoJpg = ".jpg";

            int[] statusPermitidoArray = new int[] { 0, 2, 5 };
            var isAcademico = new int[] { 96409, 173010, 207126 }.Contains(alunoID);
            var isAntecedencia = ctx.tblAPI_VisualizarAntecedencia.Where(X => X.intContactID == alunoID).Any();
            var produtosEad = new int[] { 8, 9 };
            DateTime dataAtual = !isAntecedencia ? DateTime.Now : DateTime.Now.AddMonths(2);

            var turmaConvidadaMED = ctx.Set<msp_LoadCP_MED_Choice_Result>().FromSqlRaw("msp_LoadCP_MED_Choice @matricula = {0}", alunoID).ToList().FirstOrDefault().TurmaConvidadaMed;

            ctx.SetCommandTimeOut(90);

            var t1 = (from sellOrders in ctx.tblSellOrders
                       join sellOrderDetails in ctx.tblSellOrderDetails on sellOrders.intOrderID equals sellOrderDetails.intOrderID
                       join products in ctx.tblProducts on sellOrderDetails.intProductID equals products.intProductID
                       join productGroups1 in ctx.tblProductGroups1 on products.intProductGroup1 equals productGroups1.intProductGroup1ID
                       join courses in ctx.tblCourses on sellOrderDetails.intProductID equals courses.intCourseID
                       join alunos in ctx.tblPersons on sellOrders.intClientID equals alunos.intContactID
                       join mviewCronograma in ctx.mview_Cronograma on courses.intCourseID equals mviewCronograma.intCourseID
                       join lessonTitles in ctx.tblLessonTitles on mviewCronograma.intLessonTitleID equals lessonTitles.intLessonTitleID
                       join lessonMaterial in ctx.tblLesson_Material on mviewCronograma.intLessonID equals lessonMaterial.intLessonID
                       join lessonTeachersByGroupAndTitle in ctx.tblLessonTeachersByGroupAndTitle on mviewCronograma.intLessonTitleID equals lessonTeachersByGroupAndTitle.intLessonTitleID
                       join professores in ctx.tblPersons on lessonTeachersByGroupAndTitle.intEmployeeID equals professores.intContactID
                       where statusPermitidoArray.Contains(sellOrders.intStatus ?? 0) &&
                       alunos.intContactID == alunoID
                       && mviewCronograma.dteDateTime < dataAtual
                       select new
                       {
                           ID = lessonMaterial.intMaterialID,
                           LessonID = lessonMaterial.intLessonID
                       }).ToList();

            var t2 = (from ccc in ctx.tblCallCenterCalls
                                join mvc in ctx.mview_Cronograma on ccc.intCourseID equals mvc.intCourseID
                                join alunos in ctx.tblPersons on ccc.intClientID equals alunos.intContactID
                                join lm in ctx.tblLesson_Material on mvc.intLessonID equals lm.intLessonID
                                join lessonTitles in ctx.tblLessonTitles on mvc.intLessonTitleID equals lessonTitles.intLessonTitleID
                                join courses in ctx.tblProducts on mvc.intCourseID equals courses.intProductID
                                join material in ctx.tblProducts on lm.intMaterialID equals material.intProductID
                                join productGroups1 in ctx.tblProductGroups1 on courses.intProductGroup1 equals productGroups1.intProductGroup1ID
                                join lessonTeachersByGroupAndTitle in ctx.tblLessonTeachersByGroupAndTitle on mvc.intLessonTitleID equals lessonTeachersByGroupAndTitle.intLessonTitleID
                                join professores in ctx.tblPersons on lessonTeachersByGroupAndTitle.intEmployeeID equals professores.intContactID
                                where ccc.intClientID == alunoID && chamadoTrocaTemporaria.Contains(ccc.intCallCategoryID) && ccc.intStatusID != 7
                                && mvc.dteDateTime < dataAtual
                                select new
                                {
                                    ID = lm.intMaterialID,
                                    LessonID = lm.intLessonID
                                }).ToList();
            var t3 = (from mvc in ctx.mview_Cronograma
                                         join lm in ctx.tblLesson_Material on mvc.intLessonID equals lm.intLessonID
                                         join books in ctx.tblBooks on lm.intMaterialID equals books.intBookID
                                         join lessonTitles in ctx.tblLessonTitles on mvc.intLessonTitleID equals lessonTitles.intLessonTitleID
                                         join courses in ctx.tblProducts on mvc.intCourseID equals courses.intProductID
                                         join material in ctx.tblProducts on lm.intMaterialID equals material.intProductID
                                         join productGroups1 in ctx.tblProductGroups1 on courses.intProductGroup1 equals productGroups1.intProductGroup1ID
                                         join lessonTeachersByGroupAndTitle in ctx.tblLessonTeachersByGroupAndTitle on mvc.intLessonTitleID equals lessonTeachersByGroupAndTitle.intLessonTitleID
                                         join professores in ctx.tblPersons on lessonTeachersByGroupAndTitle.intEmployeeID equals professores.intContactID
                                         where mvc.intCourseID == turmaConvidadaMED && books.intBookEntityID == Constants.APOSTILATREINAMENTOTEORICOPRATICO
                                         && mvc.dteDateTime < dataAtual
                                         select new
                                         {
                                             ID = lm.intMaterialID,
                                             LessonID = lm.intLessonID
                                         }).ToList();

            var ids = t1.Union(t2).Union(t3).Distinct().ToList().OrderBy(x => x.ID);

            apostilaIDs = ids.Select(r => r.ID).Distinct().ToList();
            apostilaLessonIDs = ids.Select(r => r.LessonID).Distinct().ToList();

            List<int> MarcasCompartilhadas = new List<int>();
            var MarcasPorApostila = new List<KeyValuePair<int, List<int>>>();

            MarcasPorApostila = GetMarcasCompartilhadasList(apostilaIDs);

            if (MarcasPorApostila.Count > 0)
                MarcasPorApostila.ForEach(m => MarcasCompartilhadas.AddRange(m.Value));

            var lessonMaterialList = (from lm in ctx.tblLesson_Material
                                      where (apostilaIDs.Contains(lm.intMaterialID) || MarcasCompartilhadas.Contains(lm.intMaterialID))
                                        && apostilaLessonIDs.Contains(lm.intLessonID)
                                      select new
                                      {
                                          intMaterialID = lm.intMaterialID,
                                          intLessonID = lm.intLessonID,
                                      }).Distinct().AsEnumerable();

            var c1 = (from sellOrders in ctx.tblSellOrders
                                         join sellOrderDetails in ctx.tblSellOrderDetails on sellOrders.intOrderID equals sellOrderDetails.intOrderID
                                         join products in ctx.tblProducts on sellOrderDetails.intProductID equals products.intProductID
                                         join productGroups1 in ctx.tblProductGroups1 on products.intProductGroup1 equals productGroups1.intProductGroup1ID
                                         join courses in ctx.tblCourses on sellOrderDetails.intProductID equals courses.intCourseID
                                         join alunos in ctx.tblPersons on sellOrders.intClientID equals alunos.intContactID
                                         join mviewCronograma in ctx.mview_Cronograma on courses.intCourseID equals mviewCronograma.intCourseID
                                         join lessonTitles in ctx.tblLessonTitles on mviewCronograma.intLessonTitleID equals lessonTitles.intLessonTitleID
                                         join lessonMaterial in lessonMaterialList on mviewCronograma.intLessonID equals lessonMaterial.intLessonID
                                         join lessonTeachersByGroupAndTitle in ctx.tblLessonTeachersByGroupAndTitle on mviewCronograma.intLessonTitleID equals lessonTeachersByGroupAndTitle.intLessonTitleID
                                         join professores in ctx.tblPersons on lessonTeachersByGroupAndTitle.intEmployeeID equals professores.intContactID
                                         where statusPermitidoArray.Contains(sellOrders.intStatus ?? 0) &&
                                             alunos.intContactID == alunoID
                                             && (apostilaIDs.Contains(lessonMaterial.intMaterialID) || (MarcasCompartilhadas.Contains(lessonMaterial.intMaterialID)))
                                             && mviewCronograma.dteDateTime < dataAtual
                                         select new
                                         {
                                             ID = lessonMaterial.intMaterialID,
                                             ProfessorNome = professores.txtName.Trim(),
                                             ProfessorID = professores.intContactID,
                                             ProdutoNome = productGroups1.txtDescription.Trim(),
                                             ProdutoID = productGroups1.intProductGroup1ID,
                                             TemaNome = lessonTitles.txtLessonTitleName.Trim(),
                                             AulaID = mviewCronograma.intLessonID,
                                             TemaID = mviewCronograma.intLessonTitleID,
                                             TemaData = mviewCronograma.dteDateTime,
                                             AlunoID = alunos.intContactID,
                                             AlunoStatus = sellOrders.intStatus,
                                             SalaAula = mviewCronograma.intClassRoomID,
                                             DataPrevisao1 = new DateTime?(),
                                             DataPrevisao2 = new DateTime?()
                                         }).ToList();
            
            var c2 = (from ccc in ctx.tblCallCenterCalls
                                                  join mvc in ctx.mview_Cronograma on ccc.intCourseID equals mvc.intCourseID
                                                  join alunos in ctx.tblPersons on ccc.intClientID equals alunos.intContactID
                                                  join lm in lessonMaterialList on mvc.intLessonID equals lm.intLessonID
                                                  join lessonTitles in ctx.tblLessonTitles on mvc.intLessonTitleID equals lessonTitles.intLessonTitleID
                                                  join courses in ctx.tblProducts on mvc.intCourseID equals courses.intProductID
                                                  join material in ctx.tblProducts on lm.intMaterialID equals material.intProductID
                                                  join productGroups1 in ctx.tblProductGroups1 on courses.intProductGroup1 equals productGroups1.intProductGroup1ID
                                                  join lessonTeachersByGroupAndTitle in ctx.tblLessonTeachersByGroupAndTitle on mvc.intLessonTitleID equals lessonTeachersByGroupAndTitle.intLessonTitleID
                                                  join professores in ctx.tblPersons on lessonTeachersByGroupAndTitle.intEmployeeID equals professores.intContactID
                                                  where ccc.intClientID == alunoID && chamadoTrocaTemporaria.Contains(ccc.intCallCategoryID) && ccc.intStatusID != 7 && apostilaIDs.Contains(lm.intMaterialID)
                                                   && mvc.dteDateTime < dataAtual
                                                  select new
                                                  {
                                                      ID = lm.intMaterialID,
                                                      ProfessorNome = professores.txtName.Trim(),
                                                      ProfessorID = professores.intContactID,
                                                      ProdutoNome = productGroups1.txtDescription.Trim(),
                                                      ProdutoID = productGroups1.intProductGroup1ID,
                                                      TemaNome = lessonTitles.txtLessonTitleName.Trim(),
                                                      AulaID = mvc.intLessonID,
                                                      TemaID = mvc.intLessonTitleID,
                                                      TemaData = mvc.dteDateTime,
                                                      AlunoID = alunos.intContactID,
                                                      AlunoStatus = new int?(2),
                                                      SalaAula = mvc.intClassRoomID,
                                                      DataPrevisao1 = ccc.dteDataPrevisao1,
                                                      DataPrevisao2 = ccc.dteDataPrevisao2,
                                                  }).ToList();

            var c3 = (from mvc in ctx.mview_Cronograma
                                                           join lm in lessonMaterialList on mvc.intLessonID equals lm.intLessonID
                                                           join books in ctx.tblBooks on lm.intMaterialID equals books.intBookID
                                                           join lessonTitles in ctx.tblLessonTitles on mvc.intLessonTitleID equals lessonTitles.intLessonTitleID
                                                           join courses in ctx.tblProducts on mvc.intCourseID equals courses.intProductID
                                                           join material in ctx.tblProducts on lm.intMaterialID equals material.intProductID
                                                           join productGroups1 in ctx.tblProductGroups1 on courses.intProductGroup1 equals productGroups1.intProductGroup1ID
                                                           join lessonTeachersByGroupAndTitle in ctx.tblLessonTeachersByGroupAndTitle on mvc.intLessonTitleID equals lessonTeachersByGroupAndTitle.intLessonTitleID
                                                           join professores in ctx.tblPersons on lessonTeachersByGroupAndTitle.intEmployeeID equals professores.intContactID
                                                           where apostilaIDs.Contains(lm.intMaterialID) && mvc.intCourseID == turmaConvidadaMED && books.intBookEntityID == Constants.APOSTILATREINAMENTOTEORICOPRATICO
                                                            && mvc.dteDateTime < dataAtual
                                                           select new
                                                           {
                                                               ID = lm.intMaterialID,
                                                               ProfessorNome = professores.txtName.Trim(),
                                                               ProfessorID = professores.intContactID,
                                                               ProdutoNome = productGroups1.txtDescription.Trim(),
                                                               ProdutoID = productGroups1.intProductGroup1ID,
                                                               TemaNome = lessonTitles.txtLessonTitleName.Trim(),
                                                               AulaID = mvc.intLessonID,
                                                               TemaID = mvc.intLessonTitleID,
                                                               TemaData = mvc.dteDateTime,
                                                               AlunoID = alunoID,
                                                               AlunoStatus = new int?(2),
                                                               SalaAula = mvc.intClassRoomID,
                                                               DataPrevisao1 = new DateTime?(),
                                                               DataPrevisao2 = new DateTime?()
                                                           }).ToList();


            var aulaAvaliacaoConsulta = c1.Union(c2).Union(c3).Distinct().ToList();

            if (aulaAvaliacaoConsulta.Any())
            {

                var temasAvaliados = (from lessonsEvaluation in ctx.tblLessonsEvaluation
                                      join mviewCronograma in ctx.mview_Cronograma on lessonsEvaluation.intLessonID equals mviewCronograma.intLessonID
                                      join lessonTitles in ctx.tblLessonTitles on mviewCronograma.intLessonTitleID equals lessonTitles.intLessonTitleID
                                      where lessonsEvaluation.intClientID == alunoID
                                      && lessonsEvaluation.intNota != 0
                                      select new
                                      {
                                          AulaID = lessonsEvaluation.intLessonID,
                                          Tema = lessonTitles.txtLessonTitleName.Trim(),
                                          TemaData = lessonsEvaluation.dteEvaluationDate,
                                          AvaliacaoID = lessonsEvaluation.intEvaluationID
                                      }
                ).ToList();

                var aulasID = aulaAvaliacaoConsulta.Select(aac => aac.AulaID).ToList();
                var temasID = aulaAvaliacaoConsulta.Select(aac => aac.TemaID).ToList();
                var professoresID = aulaAvaliacaoConsulta.Select(aac => aac.ProfessorID).ToList();
                var professoresSubstitutos = (from professor in ctx.tblLessonTeacherSubstituto
                                                join employee in ctx.tblEmployees on professor.intEmployeeID equals employee.intEmployeeID
                                                join person in ctx.tblPersons on employee.intEmployeeID equals person.intContactID
                                              select new ProfessoresSubstitutosDTO { professorSubstituto = professor, Nome = person.txtName}).OrderByDescending(x => x.professorSubstituto.intID).ToList();

                var professoresSubID = professoresSubstitutos.Select(ps => ps.professorSubstituto.intEmployeeID).ToList();

                var slides = (from s in ctx.tblRevisaoAula_Slides
                              where temasID.Contains(s.intLessonTitleID)
                                   && (professoresID.Contains(s.intProfessorID)
                                   || professoresSubID.Contains(s.intProfessorID))
                              orderby s.intOrder
                              select new
                              {
                                  TemaID = s.intLessonTitleID,
                                  ProfessorID = s.intProfessorID,
                                  SlideID = s.intSlideAulaID
                              }
                ).ToList();

                List<KeyValuePair<string, KeyValuePair<int, int>>> slidesAula = new List<KeyValuePair<string, KeyValuePair<int, int>>>();
                const string URLBASESLIDES = "https://api.medgrupo.com.br/Media.svc/json/Aula/";
                const string URLSLIDES = "IDSLIDE/?formato=FORMATO";

                foreach (var slide in slides)
                {
                    KeyValuePair<int, int> temaProfessor = new KeyValuePair<int, int>(slide.TemaID, slide.ProfessorID);
                    string url = URLSLIDES.Replace("IDSLIDE", slide.SlideID.ToString()).Replace("FORMATO", "cellimage");
                    slidesAula.Add(new KeyValuePair<string, KeyValuePair<int, int>>(url, temaProfessor));
                }

                var aulaAvaliacaoConsultaApostilaIDs = aulaAvaliacaoConsulta.Select(x => x.ID).Distinct().ToList();

                var apostilas = (from apostila in ctx.Set<msp_API_ListaApostilas_Result>().FromSqlRaw("msp_API_ListaApostilas @intYear = {0}", 0).ToList()
                                 where apostilaIDs.Contains(apostila.intBookID)
                                  && (apostila.intClassificacaoID == grandeArea)
                                  && (apostila.intProductGroup2 == produto)
                                 select new ApostilaDTO()
                                 {
                                     ID = apostila.intBookID,
                                     Capa = !String.IsNullOrEmpty(apostila.Capa) ? Convert.ToString(apostila.Capa).Trim() : "",
                                     Codigo = !String.IsNullOrEmpty(apostila.txtCode) ? Convert.ToString(apostila.txtCode).Trim() : "",
                                     Titulo = !String.IsNullOrEmpty(apostila.txtTitle) ? Convert.ToString(apostila.txtTitle).Trim() : "",
                                     Ano = Convert.ToInt32(apostila.intYear),
                                     IdProduto = (apostila.intProductGroup2.Equals((int)Produto.Cursos.MEDMEDCURSO)) ?
                                      (int)Produto.Cursos.MEDCURSO : (apostila.intProductGroup2.Equals((int)Produto.Cursos.MEDCPMED)) ?
                                      (int)Produto.Cursos.MED : Convert.ToInt32(apostila.intProductGroup2),
                                     IdProdutoGrupo = apostila.intProductGroup2.Value,
                                     IdGrandeArea = Convert.ToInt32(apostila.intClassificacaoID),
                                     IdSubEspecialidade = Convert.ToInt32(apostila.intProductGroup3),
                                     IdEntidade = Convert.ToInt32(apostila.intBookEntityID),
                                     ProdutosAdicionais = (apostila.intProductGroup2.Equals((int)Produto.Cursos.MEDMEDCURSO)) ?
                                      Convert.ToInt32(Produto.Cursos.MED).ToString() : (apostila.intProductGroup2.Equals((int)Produto.Cursos.MEDCPMED)) ?
                                      Convert.ToInt32(Produto.Cursos.CPMED).ToString() : (apostila.intProductGroup2.Equals((int)Produto.Cursos.RAC) || apostila.intProductGroup2.Equals((int)Produto.Cursos.RACIPE)) ? Convert.ToInt32((int)Produto.Cursos.RA).ToString() : "",
                                     NomeCompleto = string.Format("{0} -{1}", apostila.txtCode, (apostila.txtName ?? "").Trim().Replace(apostila.intYear.ToString(), "")),
                                 }).ToList();

                var tblLogAccess = (from a in ctx.tblAccessLog
                                    where alunoID == a.intPeopleID
                                    select a).Distinct().ToList();


                foreach (var a in aulaAvaliacaoConsultaApostilaIDs)
                {

                    AulaAvaliacao aulaAvaliacao = new AulaAvaliacao();

                    var idApostila = a;

                    for (var x = 0; x < MarcasPorApostila.Count(); x++)
                        if (MarcasPorApostila[x].Value.Contains(a))
                        {
                            idApostila = MarcasPorApostila[x].Key;
                            break;
                        }

                    var apostila = apostilas.Where(y => y.ID == a).FirstOrDefault();

                    if (apostila != null)
                    {
                        aulaAvaliacao.ID = a;
                        aulaAvaliacao.IdProduto = apostila.IdProduto;
                        aulaAvaliacao.Titulo = apostila.Titulo;
                        aulaAvaliacao.IdGrandeArea = apostila.IdGrandeArea;
                        aulaAvaliacao.IdSubEspecialidade = apostila.IdSubEspecialidade;
                        aulaAvaliacao.ProdutosAdicionais = apostila.ProdutosAdicionais;
                        aulaAvaliacao.NomeCompleto = apostila.NomeCompleto;
                        aulaAvaliacao.Ano = apostila.Ano;
                        aulaAvaliacao.UrlFile = URLBASESLIDES;

                        var temasAluno = aulaAvaliacaoConsulta.Where(y => y.ID == a).Select(x => x.TemaID).Distinct().ToList();
                        var temasEad = aulaAvaliacaoConsulta.Where(y => produtosEad.Contains(y.ProdutoID) && y.ID == a).Select(x => x.TemaID).Distinct().ToList();
                        var aulasEmTodasAsFiliais = ctx.mview_Cronograma.Where(x => temasAluno.Contains(x.intLessonTitleID)).Select(x => new { idTema = x.intLessonTitleID, TemaData = x.dteDateTime, sala = x.intClassRoomID }).ToList();
                        var aulasAssistidasTodasAsFiliais = aulasEmTodasAsFiliais.Where(i => ((tblLogAccess.Where(z => z.dteDateTime.ToString("dd/MM/yyyy") == i.TemaData.ToString("dd/MM/yyyy") && i.sala == z.intClassroomID)).Any() || temasEad.Contains(i.idTema) || isAcademico)).ToList();
                        var assistidas = aulaAvaliacaoConsulta.Where(i => i.ID == a && (aulasAssistidasTodasAsFiliais.Any(x => x.idTema == i.TemaID)));
                        var aulaAvaliacaoOrdenado = assistidas.OrderBy(y => y.TemaData);
                        var temaAtualList = aulaAvaliacaoOrdenado.ToList();

                        lTemas = new List<AulaTema>();

                        if (assistidas.Any())
                        {

                            foreach (var aulaItem in temaAtualList)
                            {
                                AulaTema aulaTema = new AulaTema();
                                var temaAvaliado = temasAvaliados == null || temasAvaliados.Count == 0 ? null : temasAvaliados.First();
                                if (temasAvaliados != null) temasAvaliados.ForEach(ta => { if (ta.AulaID == aulaItem.AulaID) temaAvaliado = ta; });
                                if (temaAvaliado != null && temaAvaliado.AulaID != aulaItem.AulaID) temaAvaliado = null;

                                bool podeAvaliar = (temaAvaliado == null) && (aulaItem.TemaData < Utilidades.GetServerDate(-1)) && statusPermitidoArray.Contains((int)aulaItem.AlunoStatus) && (aulaItem.TemaData > DateTime.Now.AddMonths(-11));


                                var professorSubstituto = professoresSubstitutos == null || professoresSubstitutos.Count == 0 ? null : professoresSubstitutos.First();
                                if (professoresSubstitutos != null) professoresSubstitutos.ForEach(ps => { if (ps.professorSubstituto.intLessonID.Value == aulaItem.AulaID) professorSubstituto = ps; });
                                if (professorSubstituto != null && professorSubstituto.professorSubstituto.intLessonID.Value != aulaItem.AulaID) professorSubstituto = null;

                                if (professorSubstituto != null)
                                {
                                    aulaTema.ProfessorID = (int)professorSubstituto.professorSubstituto.intEmployeeID;
                                    aulaTema.ProfessorNome = professorSubstituto.Nome.Trim();
                                    aulaTema.ProfessorFoto = urlProfessorFoto + professorSubstituto.professorSubstituto.intEmployeeID.Value.ToString() + imagemTipoJpg;
                                }
                                else
                                {
                                    aulaTema.ProfessorID = aulaItem.ProfessorID;
                                    aulaTema.ProfessorNome = aulaItem.ProfessorNome.Trim();
                                    aulaTema.ProfessorFoto = urlProfessorFoto + aulaItem.ProfessorID + imagemTipoJpg;
                                }

                                aulaTema.Nome = aulaItem.TemaNome.Trim();
                                aulaTema.Data = aulaItem.TemaData.ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                aulaTema.AulaID = aulaItem.AulaID;
                                aulaTema.TemaID = aulaItem.TemaID;
                                aulaTema.PodeAvaliar = podeAvaliar;
                                aulaTema.IsAvaliado = (temaAvaliado == null) ? false : true;
                                aulaTema.AvaliacaoID = (temaAvaliado == null) ? 0 : temaAvaliado.AvaliacaoID;
                                aulaTema.Rotulo = "Primeira Aula";

                                List<string> slidesDoTemaProfessor = new List<string>();

                                if (slidesAula != null && slidesAula.Count > 0)
                                {
                                    slidesAula.ForEach(s =>
                                    {
                                        if (s.Value.Key == aulaTema.TemaID && s.Value.Value == aulaTema.ProfessorID)
                                            slidesDoTemaProfessor.Add(s.Key);
                                    });
                                }

                                if (aulaTema.ProfessorID != 0)
                                    aulaTema.ProfessorNome = PessoaEntity.GetNomeResumido(aulaTema.ProfessorID);

                                if (slidesDoTemaProfessor.Count > 0)
                                {
                                    aulaTema.Slides = slidesDoTemaProfessor;
                                    lTemas.Add(aulaTema);
                                }
                            }

                            if (lTemas.Count() > 0)
                            {
                                var temas = lTemas.GroupBy(x => new { x.ProfessorNome, x.TemaID });

                                aulaAvaliacao.Temas = new List<AulaTema>();
                                aulaAvaliacao.ID = idApostila;

                                if (retorno.Count() == 0)
                                    retorno.Add(aulaAvaliacao);
                                else if (!retorno.Select(r => r.ID).Contains(aulaAvaliacao.ID))
                                    retorno.Add(aulaAvaliacao);

                                int? aulaSel = null;

                                foreach (var t in temas)
                                {
                                    var dataAv = t.Max(x => x.Data);
                                    var pos = t.Where(y => y.Data == dataAv).First();
                                    var tema = new AulaTema
                                    {
                                        Data = dataAv,
                                        ProfessorNome = t.Key.ProfessorNome,
                                        TemaID = t.Key.TemaID,

                                        AulaID = pos.AulaID,
                                        AvaliacaoID = pos.AvaliacaoID,
                                        IsAvaliado = pos.IsAvaliado,
                                        Nome = pos.Nome,
                                        PodeAvaliar = pos.PodeAvaliar,
                                        ProfessorFoto = pos.ProfessorFoto,
                                        ProfessorID = pos.ProfessorID,
                                        Rotulo = pos.Rotulo,
                                        Slides = pos.Slides,


                                    };
                                    if (aulaSel != null) retorno[aulaSel ?? 0].Temas.Add(tema);
                                    else
                                    {
                                        for (var x = 0; x < retorno.Count(); x++)
                                            if (retorno[x].ID == aulaAvaliacao.ID)
                                            {
                                                retorno[x].Temas.Add(tema);
                                                aulaSel = x;
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return retorno;
        }


        private List<KeyValuePair<int, List<int>>> GetMarcasCompartilhadasList(List<int> ApostilaIds)
        {
            using (var ctx = new DesenvContext())
            {

                var marcas = ctx.tblMedCode_MarcasCompartilhadas.Where(m => ApostilaIds.Contains(m.intBookOriginal)).Select(m => m).ToList();
                var MarcasPorApostila = new List<KeyValuePair<int, List<int>>>();

                if (marcas.Any())
                {
                    marcas.ForEach(marca =>
                    {
                        List<int> containsKey = new List<int>();
                        MarcasPorApostila.ForEach(mpa => { if (mpa.Key == marca.intBookOriginal) containsKey.Add(mpa.Key); });

                        if (containsKey.Count > 0)
                            MarcasPorApostila.ForEach(mpa => { if (containsKey.Contains(mpa.Key)) mpa.Value.Add(marca.intBookCompartilhadaId); });
                        else
                            MarcasPorApostila.Add(new KeyValuePair<int, List<int>>(marca.intBookOriginal, new List<int>() { marca.intBookCompartilhadaId }));
                    });
                }

                return MarcasPorApostila;

            }
         }

        public AulaAvaliacao GetAulaAvaliacao(int alunoID, int apostilaID)
        {
            List<int> chamadoTrocaTemporaria = new List<int>() { 1845, 1844 };
            AulaAvaliacao retorno = null;
            var lTemas = new List<AulaTema>();
            var ctx = new DesenvContext();
            retorno = new AulaAvaliacao();
            string urlProfessorFoto = "http://arearestrita.medgrupo.com.br/_static/images/professores/";
            string imagemTipoJpg = ".jpg";
            int[] statusPermitidoArray = new int[] { 0, 2, 5 };
            var isAcademico = new int[] { 96409, 173010, 207126 }.Contains(alunoID);
            var isAntecedencia = ctx.tblAPI_VisualizarAntecedencia.Where(X => X.intContactID == alunoID).Any();
            var produtosEad = new int[] { 8, 9 };
            DateTime dataAtual = !isAntecedencia ? DateTime.Now : DateTime.Now.AddMonths(2);
            List<int> MarcasCompartilhadas = GetMarcasCompartilhadas(apostilaID);

            var turmaConvidadaMED = ctx.Set<msp_LoadCP_MED_Choice_Result>().FromSqlRaw("msp_LoadCP_MED_Choice @matricula = {0}", alunoID).ToList().FirstOrDefault().TurmaConvidadaMed;
            using(MiniProfiler.Current.Step("Consulta de aula avaliação do usuário"))
            {
                var t1 = (from sellOrders in ctx.tblSellOrders
                                            join sellOrderDetails in ctx.tblSellOrderDetails on sellOrders.intOrderID equals sellOrderDetails.intOrderID
                                            join products in ctx.tblProducts on sellOrderDetails.intProductID equals products.intProductID
                                            join productGroups1 in ctx.tblProductGroups1 on products.intProductGroup1 equals productGroups1.intProductGroup1ID
                                            join courses in ctx.tblCourses on sellOrderDetails.intProductID equals courses.intCourseID
                                            join alunos in ctx.tblPersons on sellOrders.intClientID equals alunos.intContactID
                                            join mviewCronograma in ctx.mview_Cronograma on courses.intCourseID equals mviewCronograma.intCourseID
                                            join lessonTitles in ctx.tblLessonTitles on mviewCronograma.intLessonTitleID equals lessonTitles.intLessonTitleID
                                            join lessonMaterial in ctx.tblLesson_Material on mviewCronograma.intLessonID equals lessonMaterial.intLessonID
                                            join lessonTeachersByGroupAndTitle in ctx.tblLessonTeachersByGroupAndTitle on mviewCronograma.intLessonTitleID equals lessonTeachersByGroupAndTitle.intLessonTitleID
                                            join professores in ctx.tblPersons on lessonTeachersByGroupAndTitle.intEmployeeID equals professores.intContactID
                                            where statusPermitidoArray.Contains(sellOrders.intStatus ?? 0) &&
                                            alunos.intContactID == alunoID
                                            && (lessonMaterial.intMaterialID == apostilaID || (MarcasCompartilhadas.Contains(lessonMaterial.intMaterialID)))
                                            && mviewCronograma.dteDateTime < dataAtual
                                            select new
                                            {
                                                ID = lessonMaterial.intMaterialID,
                                                ProfessorNome = professores.txtName.Trim(),
                                                ProfessorID = professores.intContactID,
                                                ProdutoNome = productGroups1.txtDescription.Trim(),
                                                ProdutoID = productGroups1.intProductGroup1ID,
                                                TemaNome = lessonTitles.txtLessonTitleName.Trim(),
                                                AulaID = mviewCronograma.intLessonID,
                                                TemaID = mviewCronograma.intLessonTitleID,
                                                TemaData = mviewCronograma.dteDateTime,
                                                AlunoID = alunos.intContactID,
                                                AlunoStatus = sellOrders.intStatus,
                                                SalaAula = mviewCronograma.intClassRoomID,
                                                DataPrevisao1 = new DateTime?(),
                                                DataPrevisao2 = new DateTime?()
                                            }).ToList();

                var t2 = (from ccc in ctx.tblCallCenterCalls
                                                    join mvc in ctx.mview_Cronograma on ccc.intCourseID equals mvc.intCourseID
                                                    join alunos in ctx.tblPersons on ccc.intClientID equals alunos.intContactID
                                                    join lm in ctx.tblLesson_Material on mvc.intLessonID equals lm.intLessonID
                                                    join lessonTitles in ctx.tblLessonTitles on mvc.intLessonTitleID equals lessonTitles.intLessonTitleID
                                                    join courses in ctx.tblProducts on mvc.intCourseID equals courses.intProductID
                                                    join material in ctx.tblProducts on lm.intMaterialID equals material.intProductID
                                                    join productGroups1 in ctx.tblProductGroups1 on courses.intProductGroup1 equals productGroups1.intProductGroup1ID
                                                    join lessonTeachersByGroupAndTitle in ctx.tblLessonTeachersByGroupAndTitle on mvc.intLessonTitleID equals lessonTeachersByGroupAndTitle.intLessonTitleID
                                                    join professores in ctx.tblPersons on lessonTeachersByGroupAndTitle.intEmployeeID equals professores.intContactID
                                                    where ccc.intClientID == alunoID && chamadoTrocaTemporaria.Contains(ccc.intCallCategoryID) && ccc.intStatusID != 7 && lm.intMaterialID == apostilaID
                                                    && mvc.dteDateTime < dataAtual
                                                    select new
                                                    {
                                                        ID = lm.intMaterialID,
                                                        ProfessorNome = professores.txtName.Trim(),
                                                        ProfessorID = professores.intContactID,
                                                        ProdutoNome = productGroups1.txtDescription.Trim(),
                                                        ProdutoID = productGroups1.intProductGroup1ID,
                                                        TemaNome = lessonTitles.txtLessonTitleName.Trim(),
                                                        AulaID = mvc.intLessonID,
                                                        TemaID = mvc.intLessonTitleID,
                                                        TemaData = mvc.dteDateTime,
                                                        AlunoID = alunos.intContactID,
                                                        AlunoStatus = new int?(2),
                                                        SalaAula = mvc.intClassRoomID,
                                                        DataPrevisao1 = ccc.dteDataPrevisao1,
                                                        DataPrevisao2 = ccc.dteDataPrevisao2,
                                                    }).ToList();

                var t3 = (from mvc in ctx.mview_Cronograma
                                                            join lm in ctx.tblLesson_Material on mvc.intLessonID equals lm.intLessonID
                                                            join books in ctx.tblBooks on lm.intMaterialID equals books.intBookID
                                                            join lessonTitles in ctx.tblLessonTitles on mvc.intLessonTitleID equals lessonTitles.intLessonTitleID
                                                            join courses in ctx.tblProducts on mvc.intCourseID equals courses.intProductID
                                                            join material in ctx.tblProducts on lm.intMaterialID equals material.intProductID
                                                            join productGroups1 in ctx.tblProductGroups1 on courses.intProductGroup1 equals productGroups1.intProductGroup1ID
                                                            join lessonTeachersByGroupAndTitle in ctx.tblLessonTeachersByGroupAndTitle on mvc.intLessonTitleID equals lessonTeachersByGroupAndTitle.intLessonTitleID
                                                            join professores in ctx.tblPersons on lessonTeachersByGroupAndTitle.intEmployeeID equals professores.intContactID
                                                            where lm.intMaterialID == apostilaID && mvc.intCourseID == turmaConvidadaMED && books.intBookEntityID == Constants.APOSTILATREINAMENTOTEORICOPRATICO
                                                                && mvc.dteDateTime < dataAtual
                                                            select new
                                                            {
                                                                ID = lm.intMaterialID,
                                                                ProfessorNome = professores.txtName.Trim(),
                                                                ProfessorID = professores.intContactID,
                                                                ProdutoNome = productGroups1.txtDescription.Trim(),
                                                                ProdutoID = productGroups1.intProductGroup1ID,
                                                                TemaNome = lessonTitles.txtLessonTitleName.Trim(),
                                                                AulaID = mvc.intLessonID,
                                                                TemaID = mvc.intLessonTitleID,
                                                                TemaData = mvc.dteDateTime,
                                                                AlunoID = alunoID,
                                                                AlunoStatus = new int?(2),
                                                                SalaAula = mvc.intClassRoomID,
                                                                DataPrevisao1 = new DateTime?(),
                                                                DataPrevisao2 = new DateTime?()
                                                            }).ToList();

                var aulaAvaliacaoConsulta = t1.Union(t2.Union(t3)).Distinct().ToList();
            

                if (aulaAvaliacaoConsulta.Any())
                {
                    retorno.ID = apostilaID;
                    retorno.IdProduto = aulaAvaliacaoConsulta.First().ProdutoID;
                    retorno.Titulo = ctx.tblProducts.Where(i => i.intProductID == apostilaID).FirstOrDefault().txtCode;

                    var temasAluno = aulaAvaliacaoConsulta.Select(x => x.TemaID).Distinct().ToList();
                    var temasEad = aulaAvaliacaoConsulta.Where(y => produtosEad.Contains(y.ProdutoID)).Select(x => x.TemaID).Distinct().ToList();

                    var aulasEmTodasAsFiliais = ctx.mview_Cronograma.Where(x => temasAluno.Contains(x.intLessonTitleID)).Select(x => new { idTema = x.intLessonTitleID, TemaData = x.dteDateTime, sala = x.intClassRoomID }).ToList();


                    var tblLogAccess = ctx.tblAccessLog.Where(x => x.intPeopleID == alunoID).ToList();

                    var aulasAssistidasTodasAsFiliais = aulasEmTodasAsFiliais.Where(i => ((tblLogAccess.Where(z => z.dteDateTime.ToString("dd/MM/yyyy") == i.TemaData.ToString("dd/MM/yyyy") && i.sala == z.intClassroomID)).Any() || temasEad.Contains(i.idTema) || isAcademico)).ToList();

                    var assistidas = aulaAvaliacaoConsulta.Where(i => (aulasAssistidasTodasAsFiliais.Any(x => x.idTema == i.TemaID)));
                    //var assistidas = aulaAvaliacaoConsulta.Where(i => ((tblLogAccess.Where(z => z.dteDateTime.ToString("dd/MM/yyyy") == i.TemaData.ToString("dd/MM/yyyy"))).Any() || produtosEad.Contains(i.ProdutoID) || isAcademico));

                    //(assistidas.Any()) ?
                    var aulaAvaliacaoOrdenado = assistidas.OrderBy(y => y.TemaData);
                    //:                aulaAvaliacaoConsulta.OrderBy(y => y.TemaData);


                    var temaAtualList = aulaAvaliacaoOrdenado.ToList();
                    //var temaAtualList = from a in aulaAvaliacaoOrdenadoToList();



                    foreach (var aulaItem in temaAtualList)
                    {
                        AulaTema aulaTema = new AulaTema();

                        var temaAvaliado = (from lessonsEvaluation in ctx.tblLessonsEvaluation
                                            join mviewCronograma in ctx.mview_Cronograma on lessonsEvaluation.intLessonID equals mviewCronograma.intLessonID
                                            join lessonTitles in ctx.tblLessonTitles on mviewCronograma.intLessonTitleID equals lessonTitles.intLessonTitleID
                                            where lessonsEvaluation.intClientID == alunoID
                                            && lessonsEvaluation.intLessonID == aulaItem.AulaID
                                            && lessonsEvaluation.intNota != 0
                                            select new
                                            {
                                                Tema = lessonTitles.txtLessonTitleName.Trim(),
                                                TemaData = lessonsEvaluation.dteEvaluationDate,
                                                AvaliacaoID = lessonsEvaluation.intEvaluationID
                                            }
                                            ).FirstOrDefault();
                        var aulaMesmoAno =aulaItem.TemaData.Year.Equals(Utilidades.GetYear()); 
                        var aulaHaMenosDe12Meses = (aulaItem.TemaData > DateTime.Now.AddMonths(-11));

                        bool podeAvaliar = (temaAvaliado == null) && (aulaItem.TemaData < Utilidades.GetServerDate(-1)) && statusPermitidoArray.Contains((int)aulaItem.AlunoStatus) && (aulaMesmoAno||aulaHaMenosDe12Meses);

                        var professorSubstituto = (from professor in ctx.tblLessonTeacherSubstituto
                                                    join employee in ctx.tblEmployees on professor.intEmployeeID equals employee.intEmployeeID
                                                    join person in ctx.tblPersons on employee.intEmployeeID equals person.intContactID
                                                select new ProfessoresSubstitutosDTO { professorSubstituto = professor, Nome = person.txtName}).OrderByDescending(x => x.professorSubstituto.intID).FirstOrDefault();

                        if (professorSubstituto != null)
                        {
                            aulaTema.ProfessorID = (int)professorSubstituto.professorSubstituto.intEmployeeID;
                            aulaTema.ProfessorNome = professorSubstituto.Nome.Trim();
                            aulaTema.ProfessorFoto = urlProfessorFoto + professorSubstituto.professorSubstituto.intEmployeeID.Value.ToString() + imagemTipoJpg;
                        }
                        else
                        {
                            aulaTema.ProfessorID = aulaItem.ProfessorID;
                            aulaTema.ProfessorNome = aulaItem.ProfessorNome.Trim();
                            aulaTema.ProfessorFoto = urlProfessorFoto + aulaItem.ProfessorID + imagemTipoJpg;
                        }



                        aulaTema.Nome = aulaItem.TemaNome.Trim();
                        aulaTema.Data = aulaItem.TemaData.ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                        aulaTema.AulaID = aulaItem.AulaID;
                        aulaTema.TemaID = aulaItem.TemaID;
                        aulaTema.PodeAvaliar = podeAvaliar;
                        aulaTema.IsAvaliado = (temaAvaliado == null) ? false : true;
                        aulaTema.AvaliacaoID = (temaAvaliado == null) ? 0 : temaAvaliado.AvaliacaoID;
                        aulaTema.Rotulo = "Primeira Aula";
                        aulaTema.Slides = GetSlidesAula(aulaItem.TemaID, aulaTema.ProfessorID);

                        if (aulaTema.ProfessorID != 0)
                            aulaTema.ProfessorNome = PessoaEntity.GetNomeResumido(aulaTema.ProfessorID);
                        lTemas.Add(aulaTema);
                    }
                }

                var temas = lTemas.GroupBy(x => new { x.ProfessorNome, x.TemaID });

                retorno.Temas = new List<AulaTema>();
                foreach (var t in temas)
                {
                    var dataAv = t.Max(x => x.Data);
                    var tema = new AulaTema
                    {
                        Data = dataAv,
                        ProfessorNome = t.Key.ProfessorNome,
                        TemaID = t.Key.TemaID,

                        AulaID = t.Where(y => y.Data == dataAv).First().AulaID,
                        AvaliacaoID = t.Where(y => y.Data == dataAv).First().AvaliacaoID,
                        IsAvaliado = t.Where(y => y.Data == dataAv).First().IsAvaliado,
                        Nome = t.Where(y => y.Data == dataAv).First().Nome,
                        PodeAvaliar = t.Where(y => y.Data == dataAv).First().PodeAvaliar,
                        ProfessorFoto = t.Where(y => y.Data == dataAv).First().ProfessorFoto,
                        ProfessorID = t.Where(y => y.Data == dataAv).First().ProfessorID,
                        Rotulo = t.Where(y => y.Data == dataAv).First().Rotulo,
                        Slides = t.Where(y => y.Data == dataAv).First().Slides,


                    };
                    retorno.Temas.Add(tema);
                }
                return retorno;
            }
        }

        public AulaTema SetAulaAvaliacao(AulaAvaliacaoPost aulaAvaliacaoPost)
        {

            using(MiniProfiler.Current.Step("Consulta de aula avaliação do usuário"))
            {
                AulaTema retorno = new AulaTema();
                var ctx = new DesenvContext();

                var parametrosList = aulaAvaliacaoPost.GetType().GetProperties().ToDictionary(prop => prop.Name, prop => prop.GetValue(aulaAvaliacaoPost, null)).ToList();

                bool parametrosValidos = parametrosList.All(x => x.Value != null && (x.Key == "Observacao" || Convert.ToInt32(x.Value) >= 0));

                if (parametrosValidos)
                {
                    var isMarcasCompartilhada = (from mc in ctx.tblMedCode_MarcasCompartilhadas
                                                where mc.intBookOriginal == aulaAvaliacaoPost.ApostilaID
                                                select mc.intMarcasCompartilhadasId).Any();

                    var apostilaAula = (from lm in ctx.tblLesson_Material
                                        where lm.intLessonID == aulaAvaliacaoPost.AulaID
                                        select lm.intMaterialID).FirstOrDefault();

                    var temaAvaliado = (from lessonsEvaluation in ctx.tblLessonsEvaluation
                                        join lesson in ctx.tblLessons on lessonsEvaluation.intLessonID equals lesson.intLessonID
                                        join lessontitle in ctx.tblLessonTitles on lesson.intLessonTitleID equals lessontitle.intLessonTitleID
                                        where lessonsEvaluation.intClientID == aulaAvaliacaoPost.AlunoID
                                        && lessonsEvaluation.intLessonID == aulaAvaliacaoPost.AulaID
                                        select new { LessonEvaluation = lessonsEvaluation, LessonTitle = lessontitle }
                                        ).FirstOrDefault();

                    var SomenteObs = (from lessonsEvaluation in ctx.tblLessonsEvaluation
                                    join lesson in ctx.tblLessons on lessonsEvaluation.intLessonID equals lesson.intLessonID
                                    join lessontitle in ctx.tblLessonTitles on lesson.intLessonTitleID equals lessontitle.intLessonTitleID
                                    where lessonsEvaluation.intClientID == aulaAvaliacaoPost.AlunoID
                                    && lessonsEvaluation.intLessonID == aulaAvaliacaoPost.AulaID
                                    && lessonsEvaluation.intNota == 0
                                    select lessonsEvaluation
                                        ).FirstOrDefault();

                    if (SomenteObs != null)
                    {
                        var av = ctx.tblLessonsEvaluation.First(x => x.intLessonID == aulaAvaliacaoPost.AulaID && x.intClientID == aulaAvaliacaoPost.AlunoID);
                        av.intNota = aulaAvaliacaoPost.Nota;
                        ctx.SaveChanges();
                    }

                    else if (temaAvaliado == null)
                    {
                        tblLessonsEvaluation avaliacao = new tblLessonsEvaluation();

                        var produtoIdCorreto = 0;

                        switch(aulaAvaliacaoPost.ProdutoID)
                        {
                            case (int)Produto.Cursos.MEDCURSO:
                                produtoIdCorreto = (int)Produto.Produtos.MEDCURSO;
                                break;
                            case (int)Produto.Cursos.MED:
                                produtoIdCorreto = (int)Produto.Produtos.MED;
                                break;
                            default:
                                produtoIdCorreto = aulaAvaliacaoPost.ProdutoID;
                                break;
                        }

                        if (aulaAvaliacaoPost.ProfessorID == 0)
                        {
                            aulaAvaliacaoPost.ProfessorID = GetProfessorAula(aulaAvaliacaoPost.AulaID);
                        }

                        avaliacao.intApplicationID = aulaAvaliacaoPost.ApplicationID;
                        avaliacao.intBookID = (isMarcasCompartilhada && apostilaAula > 0) ? apostilaAula : aulaAvaliacaoPost.ApostilaID; //ajuste para marca trocada
                        avaliacao.intClassroomID = null;
                        avaliacao.intClientID = aulaAvaliacaoPost.AlunoID;
                        avaliacao.intEmployeedID = aulaAvaliacaoPost.ProfessorID;
                        avaliacao.intLessonID = aulaAvaliacaoPost.AulaID;
                        avaliacao.intNota = aulaAvaliacaoPost.Nota;
                        avaliacao.intProductGroup1ID = produtoIdCorreto;
                        avaliacao.dteEvaluationDate = DateTime.Now;
                        avaliacao.txtObservacao = aulaAvaliacaoPost.Observacao;
                    
                        ctx.tblLessonsEvaluation.Add(avaliacao);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        //retorno = new AulaTema();
                        retorno.AvaliacaoID = temaAvaliado.LessonEvaluation.intEvaluationID;
                        retorno.AulaID = temaAvaliado.LessonEvaluation.intLessonID ?? 0;
                        retorno.Data = ((DateTime)temaAvaliado.LessonEvaluation.dteEvaluationDate).ToString();
                        retorno.IsAvaliado = true;
                        retorno.Nome = temaAvaliado.LessonTitle.txtLessonTitleName.Trim();
                    }
                }

                return retorno;
            }
        }

        public int GetProfessorAula(int intLessonID)
        {
            using(MiniProfiler.Current.Step("Consulta de professor por aula"))
            {
                var idProfessor = 0;
                using (var ctx = new DesenvContext()) 
                {
                    var _professorAulaSubistituto = ctx.tblLessonTeacherSubstituto.Where(x => x.intLessonID == intLessonID).FirstOrDefault();

                    if (_professorAulaSubistituto == null)
                    {
                        var _professorAula = (from aula in ctx.tblLessons
                                            join professor in ctx.tblLessonTeachersByGroupAndTitle on aula.intLessonTitleID equals professor.intLessonTitleID
                                            where aula.intLessonID == intLessonID
                                            select professor).FirstOrDefault();
                        if (_professorAula != null) idProfessor = _professorAula.intEmployeeID;
                    }
                    else
                    {
                        idProfessor = (int)_professorAulaSubistituto.intEmployeeID;
                    }
                }

                return idProfessor;
            }
        }
        
        public List<AulaAvaliacao> GetSlidesPermitidos(int idAluno)
        {
            string urlProfessorFoto = "http://arearestrita.medgrupo.com.br/_static/images/professores/";
            string imagemTipoJpg = ".jpg";
            var ctx = new DesenvContext();
            using(MiniProfiler.Current.Step("Obtendo informaçoes de aula"))
            {
                var consulta = (from lessonsEvaluation in ctx.tblLessonsEvaluation
                            join mviewCronograma in ctx.mview_Cronograma on lessonsEvaluation.intLessonID equals mviewCronograma.intLessonID
                            join lessonTitles in ctx.tblLessonTitles on mviewCronograma.intLessonTitleID equals lessonTitles.intLessonTitleID
                            where lessonsEvaluation.intClientID == idAluno
                            select new
                            {
                                mviewCronograma.intYear,
                                lessonsEvaluation.intBookID,
                                lessonTitles.intLessonTitleID,
                                lessonsEvaluation.intLessonID,
                                lessonsEvaluation.dteEvaluationDate,
                                lessonsEvaluation.intEmployeedID,
                                lessonsEvaluation.intNota,
                                lessonTitles.txtLessonTitleName,
                                mviewCronograma.dteDateTime,
                                lessonsEvaluation.intEvaluationID
                            }).ToList();
            

                var lav = new List<AulaAvaliacao>();
                var apostilas = consulta.GroupBy(x => x.intBookID);

                foreach (var a in apostilas)
                {
                    var lt = new AulaAvaliacao
                    {
                        ID = a.Key.Value
                    };
                    lt.Temas = new List<AulaTema>();
                    foreach (var it in a)
                    {
                        var t = new AulaTema
                        {
                            TemaID = it.intLessonTitleID,
                            AulaID = it.intLessonID ?? 0,
                            Slides = GetSlidesAula(it.intLessonTitleID, it.intEmployeedID ?? 0),
                            ProfessorID = it.intEmployeedID ?? 0,
                            Nome = it.txtLessonTitleName,
                            AvaliacaoID = it.intEvaluationID,
                            Data = Convert.ToString(it.dteDateTime),
                            ProfessorNome = PessoaEntity.GetNomeResumido(it.intEmployeedID ?? 0),
                            ProfessorFoto = String.Concat(urlProfessorFoto, it.intEmployeedID.Value.ToString(), imagemTipoJpg)


                        };
                        if (t.Slides.Count() > 0)
                            lt.Temas.Add(t);
                    }
                    if (lt.Temas.Count() > 0)
                        lav.Add(lt);

                }
                return lav;                
            }
            


        }

        private List<int> GetMarcasCompartilhadas(int ApostilaId)
        {
            using (var ctx = new DesenvContext())
            {

                return ctx.tblMedCode_MarcasCompartilhadas.Where(m => m.intBookOriginal == ApostilaId).Select(m => m.intBookCompartilhadaId).ToList();

            }
        }


        public List<string> GetSlidesAula(int idTema, int idProfessor)
        {
            var ctx = new DesenvContext();
            var consulta = from s in ctx.tblRevisaoAula_Slides
                           where s.intLessonTitleID == idTema && s.intProfessorID == idProfessor
                           orderby s.intOrder
                           select s.intSlideAulaID;
            var retorno = new List<string>();
            foreach (var slideId in consulta)
            {
                /*var g = guid.ToString().ToUpper();
                var r = Criptografia.GetS3SignedPlayer(String.Concat(@"https://s3-sa-east-1.amazonaws.com/iosstream/slidesaula/", g, @".png"));*/
                var r = Constants.URLSLIDES.Replace("IDSLIDE", slideId.ToString()).Replace("FORMATO", "cellimage");
                retorno.Add(r);
            }
            return retorno;
        }        
    }
}