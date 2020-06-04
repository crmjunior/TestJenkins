using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MedCore_API.Academico;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using Microsoft.EntityFrameworkCore;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using System.Data;
using System.Data.SqlClient;
using MedCore_DataAccess.Contracts.Enums;

namespace MedCore_DataAccess.Repository
{
    public class AulaEntity : IAulaEntityData
    {
        public List<MaterialDireitoDTO> AcertarMaterialDireitoByCronograma(List<MaterialDireitoDTO> materialDireito, List<IGrouping<int?, msp_API_ListaEntidades_Result>> semanasCronograma)
        {
            var listaAuxSemanasMaterialDireito = materialDireito.Select(x => x.IntSemana).Distinct().ToList();

            for (int i = 0; i < semanasCronograma.Count(); i++)
            {
                foreach (var direito in materialDireito.Where(x => x.IntSemana == listaAuxSemanasMaterialDireito[i]).ToList())
                {
                    direito.IntSemana = semanasCronograma[i].Key;
                    direito.DataInicio = semanasCronograma[i].FirstOrDefault().dataInicio;
                    direito.DataFim = semanasCronograma[i].LastOrDefault().datafim;
                }
            }

            return materialDireito;
        }

        public bool AlunoTemAcessoAntecipado(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                int IdChamadoAcessoAntecipado = 2353;
                var acessoAntecipado = ctx.tblCallCenterCalls.Any(y => y.intClientID == matricula
                                                                    && y.intCallCategoryID == IdChamadoAcessoAntecipado
                                                                    && y.dteOpen.Year == DateTime.Now.Year);
                return acessoAntecipado;
            }
        }

        public bool AlunoPossuiMedMaster(int matricula, int ano)
        {
            using (var ctx = new DesenvContext())
            {
                var statusPermitidos = new List<int> { (int)OrdemVenda.StatusOv.Ativa, (int)OrdemVenda.StatusOv.Pendente };

                var possuiMedmaster = (from so in ctx.tblSellOrders
                                 join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                 join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                 join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                                 where p.intProductGroup1 == (int)Produto.Produtos.MED_MASTER
                                 && so.intClientID == matricula
                                 && c.intYear == ano
                                 && statusPermitidos.Contains(so.intStatus ?? 0)
                                 select so).Any();
                return possuiMedmaster;
            }
        }

        public bool AlunoPossuiMedOuMedcursoAnoAtualAtivo(int matricula)
        {
            var produtosExtensivo = new int[]
            {
                (int)Produto.Produtos.MED,
                (int)Produto.Produtos.MEDCURSO,
                (int)Produto.Produtos.MEDEAD,
                (int)Produto.Produtos.MEDCURSOEAD
            };

            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                return (from so in ctx.tblSellOrders
                        join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                        join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                        join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                        join p in ctx.tblPersons on so.intClientID equals p.intContactID
                        where so.intClientID == matricula && c.intYear == ano
                            && produtosExtensivo.Contains(pr.intProductGroup1 ?? 0)
                            && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                            
                        select new Aluno
                        {
                            ID = so.intClientID,
                        }).Any();
            }
        }

        public int BuscarSemanaPagaAlunoCancelado(int ano, int matricula, int anoAtual, int cursoID)
        {
            var semanaPaga = -1;
            using (var ctx = new DesenvContext())
            {
                var produtos = new List<int?>();
                if (cursoID == (int)Produto.Cursos.MED)
                {
                    produtos.Add((int)Utilidades.ProductGroups.MED);
                    produtos.Add((int)Utilidades.ProductGroups.CPMED);
                    produtos.Add((int)Utilidades.ProductGroups.MEDEAD);
                }
                else if (cursoID == (int)Produto.Cursos.MEDCURSO)
                {
                    produtos.Add((int)Utilidades.ProductGroups.MEDCURSO);
                    produtos.Add((int)Utilidades.ProductGroups.MEDCURSOEAD);
                }
                else
                {
                    produtos.Add(ProdutoEntity.GetProductByCourse(cursoID));
                }

                var listPagamentos = new List<csp_CustomClient_PagamentosProdutosGeral_Result>();
                var listOrders = (from so in ctx.tblSellOrders
                                  join od in ctx.tblSellOrderDetails on so.intOrderID equals od.intOrderID
                                  join pr in ctx.tblProducts on od.intProductID equals pr.intProductID
                                  join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                  where so.intClientID == matricula
                                  && produtos.Contains(pr.intProductGroup1)
                                  && c.intYear == ano
                                  select new { so.intOrderID, so.intStatus, so.intStatus2 }).ToList();

                if (!listOrders.Where(x => x.intStatus == (int)Utilidades.ESellOrderStatus.Ativa).Any() && listOrders.Any(x => x.intStatus2 == (int)Utilidades.ESellOrderStatus.Cancelada))
                {
                    listPagamentos = ctx.Set<csp_CustomClient_PagamentosProdutosGeral_Result>().FromSqlRaw("csp_CustomClient_PagamentosProdutosGeral @p1 = {0}, @p2 = {1}", matricula, ano, 0)
                        .Where(x => x.intYear == ano).ToList();

                    if (listPagamentos.Count > 0)
                    {
                        var maxMonth = (int)listPagamentos.Where(y => y.txtStatus == "OK" ).Max(x => x.intMonth);
                        if (maxMonth > 0 && maxMonth <=12)
                        {
                            var ultimoDiaMes = DateTime.DaysInMonth(anoAtual, maxMonth);
                            var d = new DateTime(anoAtual, maxMonth, ultimoDiaMes);
                            semanaPaga = Utilidades.GetNumeroSemanaAtual(d);
                        }
                    }
                }
            }

            return semanaPaga;
        }

        public List<int> GetAnosOVAluno(int matricula, int cursoId)
        {
            var produtos = Utilidades.GrupoProduto(cursoId);

            List<int> anosOV = new List<int>();
            var anoAtual = Utilidades.GetYear();
            using (var ctx = new DesenvContext())
            {
                anosOV = (from so in ctx.tblSellOrders
                              join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                              join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                              join c in ctx.tblCourses on p.intProductID equals c.intCourseID
                              where
                                so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                && ((so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente)
                                || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Carencia))
                                && so.intClientID == matricula
                                && produtos.Contains(p.intProductGroup1)
                              select
                                    c.intYear.Value
                                 ).ToList();
            }
            return anosOV;
        }

        public List<tblLiberacaoApostila> RetornaApostilasDeAcordoComMatricula(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                var idsLiberados = ctx.tblLiberacaoApostilaAntecipada.Select(x => x.intContactID).ToList();
                var apostilas = idsLiberados.Contains(matricula) ?
                                (from tbl in ctx.tblLiberacaoApostila
                                 select tbl).ToList() :
                                (from tbl in ctx.tblLiberacaoApostila
                                 where tbl.bitLiberado
                                 select tbl).ToList();
                return apostilas;
            }
        }

        public int GetMesFimCronograma()
        {
            using (var ctx = new DesenvContext())
            {
                var dataLimite = ctx.tblParametrosGenericos.Where(x => x.txtName == "DataFimCronograma").FirstOrDefault().txtValue;
                var mesLimite = Convert.ToDateTime(dataLimite).Month;

                return mesLimite;
            }
        }

        public DataTable GetCronograma(int matricula, int ano)
        {
            // ctx.Set<msp_CronogramaAula_Result>().FromSqlRaw("msp_CronogramaAula {0}, {1}", matricula, ano)
            // var parametros = new SqlParameter[] { new SqlParameter("intContactID", matricula), new SqlParameter("intYear", ano) };
            // var dtCronograma = new DBQuery().ExecuteStoredProcedure("msp_CronogramaAula", parametros).Tables[0];

            // return dtCronograma;
            return new DataTable();
        }

        public List<long?> GetListaMateriaisPermitidos_PorMatriculaAnoProduto(int matricula, int ano, int produto)
        {
            using(var ctx = new DesenvContext())
            {
                
                var ResultProgressoPermitido = ctx.csp_ListaMaterialDireitoAluno(matricula, ano, produto)
                                        .Where(w => w.blnPermitido == 1)
                                        .Select(x => x.intBookEntityID)
                                        .Distinct().ToList();
                if (ResultProgressoPermitido.Count() == 0)
                {
                    var lisAulaPermitido = GetApostilasLiberadasAulaCronograma(ano);
                    ResultProgressoPermitido = ctx.csp_ListaMaterialDireitoAluno(matricula, ano, null)
                        .Where(a => lisAulaPermitido.Contains((int)a.intBookEntityID))
                        .Select(x => x.intBookEntityID)
                        .ToList();
                }
                return ResultProgressoPermitido;
            }
        }

        public List<int?> GetApostilasLiberadasAulaCronograma(int anoMaterial) 
        {
            using (var ctx = new DesenvContext())
            {

              
                var apostilas = (from b in ctx.tblBooks
                                 join l in ctx.tblLiberacaoApostila on b.intBookID equals l.intBookId
                                 join lm in ctx.tblLesson_Material on b.intBookID equals lm.intMaterialID
                                 join mc in ctx.mview_Cronograma on lm.intLessonID equals mc.intLessonID
                                 join lt in ctx.tblLessonTypes on mc.intLessonType equals lt.intLessonType
                                 where
                                 lt.bitAulaPresencialMedicine
                                 && (mc.intStoreID == (int)Utilidades.Filiais.SãoPaulo || mc.intStoreID == (int)Utilidades.Filiais.Salvador)
                                 && b.intYear == anoMaterial
                                 && l.bitLiberado
                                 && mc.dteDateTime < DateTime.Now
                                 select new
                                 {
                                     b.intBookEntityID
                                 }).ToList().Select(x => (int?)x.intBookEntityID).Distinct().ToList();

                return apostilas;
            }  
        }        

        public List<QuestaoExercicioDTO> GetQuestoesApostila_PorAnoProduto(int ano, int produto)
        {
            using (var ctxMat = new DesenvContext())
            {

                var produtosNaoEntram = new int[] { (int)Utilidades.ProductGroups.INTENSIVO, (int)Utilidades.ProductGroups.APOSTILA_MEDELETRON };

                var listaQuestoesApostila = (from cq in ctxMat.tblConcursoQuestoes
                                             join cca in ctxMat.tblConcursoQuestao_Classificacao_Autorizacao on cq.intQuestaoID equals cca.intQuestaoID
                                             join ccaa in ctxMat.tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada on cca.intMaterialID equals ccaa.intProductID
                                             join b in ctxMat.tblBooks on cca.intMaterialID equals b.intBookID
                                             join cc in ctxMat.tblCodigoComentario on new { materialID = cca.intMaterialID, questaoID = cca.intQuestaoID }
                                                                                   equals new { materialID = cc.intApostilaID, questaoID = cc.intQuestaoID }
                                             join be in ctxMat.tblBooks_Entities on b.intBookEntityID equals be.intID
                                             join p in ctxMat.tblProducts on b.intBookID equals p.intProductID

                                             where ccaa.bitActive == true
                                                && cq.bitAnulada != true
                                                && cq.bitAnuladaPosRecurso != true
                                                && cca.bitAutorizacao == true
                                                && b.intYear == ano
                                                && (p.intProductGroup2 == produto && (!produtosNaoEntram.Contains((int)p.intProductGroup2)))
                                             select new QuestaoExercicioDTO
                                             {
                                                 QuestaoID = cq.intQuestaoID,
                                                 ExercicioID = be.intID
                                             }
                                    ).ToList();
                return listaQuestoesApostila;
            }
        }   

        public List<int> GetRespostas_PorMatricula(int matricula)
        {
            using (var ctx = new AcademicoContext())
            {
                var respostas_discursivas = (from cd in ctx.tblCartaoResposta_Discursiva
                                             join eh in ctx.tblExercicio_Historico on cd.intHistoricoExercicioID equals eh.intHistoricoExercicioID
                                             where eh.intClientID == matricula
                                             select cd.intQuestaoDiscursivaID);

                var respostas_objetivas = (from co in ctx.tblCartaoResposta_objetiva
                                           where co.intClientID == matricula
                                           select co.intQuestaoID);

                return respostas_objetivas.Union(respostas_discursivas).ToList();
            }
        }

        public List<Semana> GetSemanas(int ano, int idProduto, int matricula, Semana.TipoAba aba)
        {
            try
            {
                const int ativo = 1;
                const int inativo = 0;
                const int produtoAdaptamed = 73;


                new Util.Log().SetLog(new LogMsPro
                {
                    Matricula = matricula,
                    IdApp = Aplicacoes.MsProMobile,
                    Tela = aba == Semana.TipoAba.Aulas
                                              ? Util.Log.MsProLog_Tela.MainAula
                                              : aba == Semana.TipoAba.Questoes
                                                  ? Util.Log.MsProLog_Tela.MainQuestao
                                                  : 0,
                    Acao = Util.Log.MsProLog_Acao.Abriu
                });

                var anoVigente = Utilidades.GetYear();


                using (var ctx = new DesenvContext())
                {
                    var lSemanas = new List<Semana>();

                    var semanaAtual = Utilidades.GetNumeroSemanaAtual(DateTime.Now);
                    
                    var listaEntidades = ctx.Set<msp_API_ListaEntidades_Result>().FromSqlRaw("msp_API_ListaEntidades @intProductGroup = {0}, @intYear = {1}, @matricula = {2}", idProduto, ano, matricula)
                                            .ToList();

                    int prod = ProdutoEntity.GetProductByCourse(idProduto);

                    var temas = listaEntidades.Select(y => y.intLessonTitleID);
                    var entidades = listaEntidades.Select(y => y.intID)
                                                  .ToArray();

                    var acessoAntecipado = AlunoTemAcessoAntecipado(matricula);
                    var semanas = listaEntidades.GroupBy(x => new { entidade = x.intID, semana = x.intSemana, dataInicio = x.dataInicio, datafim = x.datafim })
                                                .ToList();

                    var mesesCursados = ctx.csp_loadMesesCursados(matricula, prod);

                    var mesesCursadosAnoAnterior = mesesCursados.Where(x => x.intYear < anoVigente).Select(x => (int)x.intMonth).ToArray();

                    var mesesCursadosAnoAtual = mesesCursados.Where(x => x.intYear == anoVigente).Select(x => (int)x.intMonth).ToArray();


                    if (anoVigente > DateTime.Now.Year)
                        semanaAtual = 1;


                    var isSemanasDisabled = false;
                    if (!semanas.Any())
                    {
                        isSemanasDisabled = true;
                        const int matriculaGlobal = 96409;
                        
                        semanas.AddRange(ctx.Set<msp_API_ListaEntidades_Result>().FromSqlRaw("msp_API_ListaEntidades @intProductGroup = {0}, @intYear = {1}, @matricula = {2}", idProduto, ano, matriculaGlobal).AsEnumerable()
                            .GroupBy(x => new { entidade = x.intID, semana = x.intSemana, dataInicio = x.dataInicio, datafim = x.datafim })
                            .ToList());
                    }

                    var dicProgressos = new Dictionary<int, int>();
                    var dicProgressosQuestoes = new Dictionary<long, int>();

                    switch (aba)
                    {
                        case Semana.TipoAba.Aulas:
                            dicProgressos = new MednetEntity().GetProgressoAulas(temas.ToArray(), matricula);
                            break;
                        case Semana.TipoAba.Materiais:
                            break;
                        case Semana.TipoAba.Questoes:
                            dicProgressosQuestoes = GetProgressoQuestoes(entidades, matricula);
                            break;
                        case Semana.TipoAba.Revalida:
                            break;
                        default:
                            break;
                    }

                    foreach (var itSemana in semanas)
                    {
                        var numeroSemana = itSemana.Key.semana ?? 0;
                        var semana = new Semana();

                        if (idProduto == produtoAdaptamed)
                        {
                            var currentYear = ano;
                            if (currentYear == 0)
                                currentYear = DateTime.Now.Year;

                            var dataTextoInicio = itSemana.Key.dataInicio + "/" + currentYear;
                            var dataInicio = DateTime.ParseExact(dataTextoInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var liberacao = dataInicio <= DateTime.Now;
                            semana.Ativa = isSemanasDisabled ? 0 : Convert.ToInt32(liberacao);
                            semana.Numero = numeroSemana;
                            semana.DataInicio = itSemana.Key.dataInicio;
                            semana.DataFim = itSemana.Key.datafim;
                            semana.Apostilas = new List<Apostila>();
                            semana.SemanaAtiva = Convert.ToInt32((dataTextoInicio == "15/09/2017"));
                        }
                        else
                        {

                            semana.Ativa = VerificaLiberacaoSemana(acessoAntecipado, isSemanasDisabled, semanaAtual, itSemana.Key.semana, mesesCursadosAnoAnterior, mesesCursadosAnoAtual, itSemana.Key.dataInicio);
                            semana.Numero = numeroSemana;
                            semana.DataInicio = itSemana.Key.dataInicio;
                            semana.DataFim = itSemana.Key.datafim;
                            semana.Apostilas = new List<Apostila>();

                            semana.SemanaAtiva = semanaAtual == numeroSemana ? ativo : inativo;


                        }

                        if (itSemana.Count() > 2)
                        {
                            var entidadeAgrupada = itSemana.GroupBy(y => new { idEntidade = y.intID, entidade = y.entidade }).ToList();
                            foreach (var entidade in entidadeAgrupada)
                            {
                                var percentlido = 0;
                                switch (aba)
                                {
                                    case Semana.TipoAba.Aulas:
                                        percentlido = semana.Ativa == 1 ? (int?)dicProgressos.FirstOrDefault(x => x.Key == entidade.First().intLessonTitleID).Value ?? 0 : 0;  //GetProgressoAula(entidade.First().intLessonTitleID, matricula) : 0;
                                        break;
                                    case Semana.TipoAba.Materiais:
                                        percentlido = semana.Ativa == 1 ? new MaterialApostilaEntity().GetProgressoMaterial(entidade.First().intID, matricula) : 0;
                                        break;
                                    case Semana.TipoAba.Questoes:
                                        percentlido = semana.Ativa == 1 ? (int?)dicProgressosQuestoes.FirstOrDefault(x => x.Key == entidade.First().intID).Value ?? 0 : 0;  //GetPercentQuestoes((int)entidade.First().intID, matricula) : 0;
                                        break;
                                    default:
                                        break;
                                }

                                var apostila = new Apostila
                                {
                                    IdEntidade = (int)entidade.Key.idEntidade,
                                    Nome = entidade.Key.entidade,
                                    PercentLido = percentlido,
                                    Temas = new List<AulaTema>()
                                };

                                foreach (var tema in entidade)
                                {
                                    apostila.Temas.Add(new AulaTema { TemaID = tema.intLessonTitleID });
                                }

                                semana.Apostilas.Add(apostila);
                                if (entidadeAgrupada.Count() == 1)
                                {
                                    semana.Apostilas.Add(apostila);
                                }
                            }
                        }
                        else
                        {
                            foreach (var tema in itSemana)
                            {
                                var percentlido = 0;
                                switch (aba)
                                {
                                    case Semana.TipoAba.Aulas:
                                        //percentlido = semana.Ativa == 1 ? GetProgressoAula(tema.intLessonTitleID, matricula) : 0;
                                        percentlido = semana.Ativa == 1 ? (int?)dicProgressos.FirstOrDefault(x => x.Key == tema.intLessonTitleID).Value ?? 0 : 0;
                                        break;
                                    case Semana.TipoAba.Materiais:
                                        //percentlido = semana.Ativa == 1 ? new MaterialApostilaEntity().GetProgressoMaterial(semana.Apostilas.FirstOrDefault().IdEntidade, matricula) : 0;
                                        percentlido = new MaterialApostilaEntity().GetProgressoMaterial(tema.intID, matricula);
                                        break;
                                    case Semana.TipoAba.Questoes:
                                        percentlido = semana.Ativa == 1 ? (int?)dicProgressosQuestoes.FirstOrDefault(x => x.Key == tema.intID).Value ?? 0 : 0;  // GetPercentQuestoes((int)tema.intID, matricula) : 0;
                                        break;
                                    default:
                                        break;
                                }

                                var apostila = new Apostila
                                {
                                    IdEntidade = (int)tema.intID,
                                    Nome = tema.entidade,
                                    PercentLido = percentlido,
                                    Temas = new List<AulaTema>()
                                };

                                apostila.Temas.Add(new AulaTema { TemaID = tema.intLessonTitleID });

                                semana.Apostilas.Add(apostila);
                                if (itSemana.Count() == 1)
                                {
                                    semana.Apostilas.Add(apostila);
                                }

                            }
                        }

                        lSemanas.Add(semana);
                    }
                    //Se nenhuma semana estiver marcada como semana atual, marca a ultima
                    if (lSemanas.Any() && lSemanas.All(x => x.SemanaAtiva != 1))
                        lSemanas.Last().SemanaAtiva = 1;
                    return lSemanas;
                }
            }
            catch
            {
                throw;
            }
        }
        public List<AlunoAulaDTO> GetAlunosAulaTurma(List<CursoAulaDTO> turmasPrimeiraAula)
        {
            var statusOv = new List<int> { (int)Utilidades.ESellOrderStatus.Pendente, (int)Utilidades.ESellOrderStatus.Ativa };
            List<int> chamadoTrocaTemporaria = new List<int>() { 1845, 1844 };
            int statusCallCenterExcluido = 7;
            var alunosMeioDeAno = Utilidades.AlunosMeioDeAnoAtuais();

            var alunosAulaDto = new List<AlunoAulaDTO>();

            if (turmasPrimeiraAula.Any())
            {
                var data = turmasPrimeiraAula.FirstOrDefault().LessonDatetime.Date;
                var turmasIds = turmasPrimeiraAula.Select(x => x.CourseId).ToList();

                using (var ctx = new DesenvContext())
                {
                    alunosAulaDto = (from so in ctx.tblSellOrders
                                     join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                     join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                                     join p1 in ctx.tblProductGroups1 on p.intProductGroup1 equals p1.intProductGroup1ID
                                     join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                     join dt in ctx.tblDeviceToken on so.intClientID equals dt.intClientID
                                     where dt.bitAtivo == true && (dt.intApplicationId == null
                                        || (dt.intApplicationId.HasValue && dt.intApplicationId.Value == (int)Aplicacoes.MsProMobile)
                                        )
                                     && (statusOv.Contains(so.intStatus ?? 0) || (alunosMeioDeAno.Contains(so.intClientID) && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada))
                                     && turmasIds.Contains(c.intCourseID)
                                     select new AlunoAulaDTO
                                     {
                                         ClientId = so.intClientID,
                                         CourseId = c.intCourseID,
                                         ProductId = p1.intProductGroup1ID,
                                         ProductName = p1.txtDescription.Trim(),
                                         ClientDeviceToken = dt.txtOneSignalToken
                                     })
                                     .Union(from ccc in ctx.tblCallCenterCalls
                                            join p in ctx.tblProducts on ccc.intCourseID equals p.intProductID
                                            join p1 in ctx.tblProductGroups1 on p.intProductGroup1 equals p1.intProductGroup1ID
                                            join c in ctx.tblCourses on ccc.intCourseID equals c.intCourseID
                                            join dt in ctx.tblDeviceToken on ccc.intClientID equals dt.intClientID
                                            where dt.bitAtivo == true && (dt.intApplicationId == null
                                                || (dt.intApplicationId.HasValue && dt.intApplicationId.Value == (int)Aplicacoes.MsProMobile)
                                                )
                                            && chamadoTrocaTemporaria.Contains(ccc.intCallCategoryID)
                                            && ccc.intStatusID != statusCallCenterExcluido
                                            && turmasIds.Contains(c.intCourseID)
                                            && data >= (ccc.dteDataPrevisao1 ?? DateTime.MaxValue)
                                            && data <= (ccc.dteDataPrevisao2 ?? (ccc.dteDataPrevisao1 ?? DateTime.MaxValue))

                                            select new AlunoAulaDTO
                                            {
                                                ClientId = ccc.intClientID,
                                                CourseId = c.intCourseID,
                                                ProductId = p1.intProductGroup1ID,
                                                ProductName = p1.txtDescription.Trim(),
                                                ClientDeviceToken = dt.txtOneSignalToken
                                            }
                                    ).ToList();

                }

                foreach (var aluno in alunosAulaDto)
                {
                    var turmaAluno = turmasPrimeiraAula.FirstOrDefault(x => x.CourseId == aluno.CourseId);
                    aluno.LessonDatetime = turmaAluno.LessonDatetime;
                }
            }

            return alunosAulaDto;
        }

        public int VerificaLiberacaoSemana(bool acessoAntecipado, bool isDisabled, int semanaAtual, int? semana, int[] mesesCursadosAnterior, int[] mesesCursadosAnoAtual, string dataInicio)
        {
            var mes = Convert.ToInt32(dataInicio.Substring(3, 2));
            var liberacao = !isDisabled;


            if (mesesCursadosAnterior.Contains(mes))
            {
                liberacao = true;
            }
            else
            {
                if (!acessoAntecipado)
                    liberacao = !isDisabled && semanaAtual >= semana;
            }

            return liberacao ? 1 : 0;
        }

        public Dictionary<long, int> GetProgressoQuestoes(long[] entidades, int matricula)
        {
            Dictionary<long, int> dicProgresso = new Dictionary<long, int>();

            string strEntidades = string.Join(",", entidades);
            using (var ctxMat = new DesenvContext())
            {
                using (var ctx = new AcademicoContext())
                {
                    var anoAtual = Utilidades.GetYear();
                    var listaQuestoesApostila = (from cq in ctxMat.tblConcursoQuestoes
                                                 join cca in ctxMat.tblConcursoQuestao_Classificacao_Autorizacao on cq.intQuestaoID equals cca.intQuestaoID
                                                 join ccaa in ctxMat.tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada on cca.intMaterialID equals ccaa.intProductID
                                                 join b in ctxMat.tblBooks on cca.intMaterialID equals b.intBookID
                                                 join cc in ctxMat.tblCodigoComentario on new { materialID = cca.intMaterialID, questaoID = cca.intQuestaoID }
                                                                                       equals new { materialID = cc.intApostilaID, questaoID = cc.intQuestaoID }
                                                 join be in ctxMat.tblBooks_Entities on b.intBookEntityID equals be.intID
                                                 join p in ctxMat.tblProducts on b.intBookID equals p.intProductID
                                                 where ccaa.bitActive == true
                                                    && cca.bitAutorizacao == true
                                                    && b.intYear == anoAtual
                                                    && entidades.Contains(be.intID)
                                                 select new
                                                 {
                                                     cq.intQuestaoID,
                                                     be.intID
                                                 }).ToList();

                    var respostas = GetRespostas_PorMatricula(matricula);

                    var groupListQuestoes = (from resp in respostas
                                             join lq in listaQuestoesApostila on resp equals lq.intQuestaoID
                                             select new
                                             {
                                                 intQuestao = lq.intQuestaoID,
                                                 intQuestaoResposta = resp,
                                                 lq.intID
                                             }).GroupBy(x => x.intID).ToList();

                    foreach (var q in groupListQuestoes)
                    {
                        var qtdQuestoes = listaQuestoesApostila.Where(x => q.Key == x.intID).Count();
                        decimal percentual = ((100 * q.Count()) / qtdQuestoes);
                        dicProgresso.Add(q.Key, (int)Math.Ceiling(percentual));
                    }
                }
            }

            return dicProgresso;
        }

        public List<IGrouping<int?, msp_API_ListaEntidades_Result>> ObterCronograma(int cursoId, int ano, int matricula = 0)
        {
            if (matricula == 0)
                matricula  = Constants.CONTACTID_ACADEMICO;

            List<IGrouping<int?, msp_API_ListaEntidades_Result>> cronogramaPorSemana;

            using (var ctx = new DesenvContext())
            {
                cronogramaPorSemana = ctx.Set<msp_API_ListaEntidades_Result>().FromSqlRaw("msp_API_ListaEntidades  @intProductGroup = {0}, @intYear = {1}, @matricula = {2}", cursoId, ano, matricula).ToList()
                .GroupBy(x => x.intSemana)
                .ToList();
            }

            return cronogramaPorSemana;
        }

        public List<MaterialDireitoDTO> ObterMaterialDireitoAluno(int matricula, int produtoId, int cursoId, int anoVigente, bool acessoAntecipado, int anoMaterial = 0)
        {
            List<MaterialDireitoDTO> materialDireitoPorEntidade;
            List<csp_ListaMaterialDireitoAluno_Result> materialComApostilas;

            using (var ctx = new DesenvContext())
            {
                if (anoMaterial == 0) anoMaterial = anoVigente;

                var isMaterialBonusMedMaster = IsMaterialBonusMedMaster(matricula, anoMaterial, produtoId);

                if (isMaterialBonusMedMaster)
                {
                    const int matriculaGlobal = Constants.CONTACTID_ACADEMICO;
                    materialComApostilas = ctx.csp_ListaMaterialDireitoAluno(matriculaGlobal, anoMaterial, produtoId).ToList();
                }
                else
                {
                    materialComApostilas = ctx.csp_ListaMaterialDireitoAluno(matricula, anoVigente, produtoId).ToList();
                }
             
                var apostilas = RetornaApostilasDeAcordoComMatricula(matricula);

                var apostilasLiberadas = (from tbl in apostilas
                                          join ma in materialComApostilas on tbl.intBookId equals ma.intMaterialID
                                          select new
                                          {
                                              idApostila = tbl.intBookId,
                                              idEntidade = ma.intBookEntityID
                                          }).ToList();

                var materialDireitoPorEntidadeMuitos = (from ma in materialComApostilas
                                                        select new
                                                        {
                                                            ma.intBookEntityID,
                                                            ma.txtName,
                                                            ma.intSemana,
                                                            ma.dataInicio,
                                                            ma.datafim,
                                                            ma.intLessonTitleID,
                                                            Ativa = (ma.blnPermitido == 1 || (ma.anoCursado == anoVigente && acessoAntecipado))
                                                        }).Distinct().ToList();

                materialDireitoPorEntidade = materialDireitoPorEntidadeMuitos.Select(x => new MaterialDireitoDTO
                {
                    Id = x.intBookEntityID,
                    Entidade = x.txtName,
                    IntSemana = x.intSemana,
                    DataInicio = x.dataInicio,
                    DataFim = x.datafim,
                    IntLessonTitleId = x.intLessonTitleID,
                    intBookEntityId = x.intBookEntityID,
                    Ativa = x.Ativa
                }).ToList();

                List<int?> listaApostilasAprovadas = null;

                foreach (var md in materialDireitoPorEntidade.ToList())
                {
                    md.IntYear = materialComApostilas.Where(X => X.intBookEntityID == md.Id).Min(y => y.anoCursado);
                    if (anoVigente != anoMaterial || isMaterialBonusMedMaster)
                    {

                        if (listaApostilasAprovadas == null)
                            listaApostilasAprovadas = GetApostilasLiberadasSeHouveAulaCronograma(anoMaterial);

                        md.ApostilasAprovadas = listaApostilasAprovadas;
                    }
                    else
                    {
                        var visualizacaoLiberada = VisualizacaoLiberada(anoMaterial, matricula, cursoId, anoVigente, (int)(md.IntSemana ?? 0));
                        md.ApostilasAprovadas = visualizacaoLiberada ? apostilasLiberadas.Where(x => x.idEntidade == md.Id).Select(x => x.idApostila).ToList() : null;
                        md.QuestoesAprovadas = visualizacaoLiberada ? materialComApostilas.Where(x => x.intBookEntityID == md.Id && x.blnPermitido == 1).Select(x => x.intMaterialID).ToList() : null;
                    }
                }

            }

            return materialDireitoPorEntidade;
        }

        public List<int?> GetApostilasLiberadasSeHouveAulaCronograma(int anoMaterial)
        {
            using (var ctx = new DesenvContext())
            {

                var apostilas = (from b in ctx.tblBooks
                        join l in ctx.tblLiberacaoApostila on b.intBookID equals l.intBookId
                        join lm in ctx.tblLesson_Material on b.intBookID equals lm.intMaterialID
                        join mc in ctx.mview_Cronograma on lm.intLessonID equals mc.intLessonID
                        join lt in ctx.tblLessonTypes on mc.intLessonType equals lt.intLessonType
                        where
                        lt.bitAulaPresencialMedicine
                         && (mc.intStoreID == (int)Utilidades.Filiais.SãoPaulo || mc.intStoreID == (int)Utilidades.Filiais.Salvador)
                        && b.intYear == anoMaterial
                        && l.bitLiberado
                        && mc.dteDateTime < DateTime.Now
                        select new
                        {
                            b.intBookID
                        }).ToList().Select(x => (int?)x.intBookID).Distinct().ToList();

                return apostilas;
            }  
        }

        public bool IsMaterialBonusMedMaster(int matricula, int ano, int produtoId)
        {
            return Utilidades.ProdutosExtensivo().Contains(produtoId) && AlunoPossuiMedMaster(matricula, ano + 1);
        }

        public List<int?> GetApostilasLiberadasSeHouveAulaCronograma(long? intBookEntityId, int anoMaterial)
        {
            using (var ctx = new DesenvContext())
            {

                var apostilas = (from b in ctx.tblBooks
                        join l in ctx.tblLiberacaoApostila on b.intBookID equals l.intBookId
                        join lm in ctx.tblLesson_Material on b.intBookID equals lm.intMaterialID
                        join mc in ctx.mview_Cronograma on lm.intLessonID equals mc.intLessonID
                        join lt in ctx.tblLessonTypes on mc.intLessonType equals lt.intLessonType
                        where
                        lt.bitAulaPresencialMedicine
                         && (mc.intStoreID == (int)Utilidades.Filiais.SãoPaulo || mc.intStoreID == (int)Utilidades.Filiais.Salvador)
                        && b.intBookEntityID == intBookEntityId 
                        && b.intYear == anoMaterial
                        && l.bitLiberado
                        && mc.dteDateTime < DateTime.Now
                        select new
                        {
                            b.intBookID
                        }).ToList().Select(x => (int?)x.intBookID).Distinct().ToList();

                return apostilas;
            }  
        }

        public bool VisualizacaoLiberada(int ano, int matricula, int cursoId, int anoAtual, int semana)
        {
            bool liberado = true;

            var idsExtensivos = new int[] { (int)Produto.Cursos.MED, (int)Produto.Cursos.MEDCURSO};

            var alunoCancelado_UltimaSemanaPaga = -1;
            if (idsExtensivos.Contains(cursoId))
            {
                alunoCancelado_UltimaSemanaPaga = BuscarSemanaPagaAlunoCancelado(ano, matricula, anoAtual, cursoId);
            }

            if (alunoCancelado_UltimaSemanaPaga >= 0 && semana > alunoCancelado_UltimaSemanaPaga)
            {
                liberado = false;
            }

            return liberado;
        }

        public List<CursoAulaDTO> GetPrimeiraAulaTurma(DateTime data, int courseId = 0)
        {
            var anoLetivo = data.Year;
            List<CursoAulaDTO> primeiraAulaTurma = new List<CursoAulaDTO>();
            using (var ctx = new DesenvContext())
            {
                primeiraAulaTurma = (from mc in ctx.mview_Cronograma
                                     join lt in ctx.tblLessonTypes on mc.intLessonType equals lt.intLessonType
                                     join c in ctx.tblCourses 
                                     on new
                                     {
                                         intCourseID = mc.intCourseID,
                                         intClassRoomID = mc.intClassRoomID
                                     }
                                     equals new
                                     {
                                         intCourseID = c.intCourseID,
                                         intClassRoomID = c.intPrincipalClassRoomID ?? 0
                                     }
                                     where c.intYear == anoLetivo
                                     && (c.intCourseID == courseId || courseId == 0)
                                     && lt.bitAulaPresencialMedicine
                                     group mc by mc.intCourseID into grp
                                     select new { CourseId = grp.Key, Date = grp.Min(x => x.dteDateTime) }
                                         ).Where(x => x.Date == data)
                                         .Select(y => new CursoAulaDTO
                                         {
                                             CourseId = y.CourseId,
                                             LessonDatetime = y.Date
                                         }).ToList();
            }
            return primeiraAulaTurma;
        }

        public DataTable GetCronogramaTurma(int idTurma, int ano)
        {
            var parametros = new SqlParameter[] { new SqlParameter("@intClassRoomID", idTurma), new SqlParameter("intYear", ano) };
            var dtCronograma = new DBQuery().ExecuteStoredProcedure("emed_Cronograma_LocalAula_Rel", parametros).Tables[0];

            return dtCronograma;
        }
    }
}