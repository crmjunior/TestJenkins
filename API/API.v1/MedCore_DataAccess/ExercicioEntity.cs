using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_API.Academico;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Repository;
using Microsoft.Data.SqlClient;
using StackExchange.Profiling;
using Microsoft.AspNetCore.Hosting;

namespace MedCore_DataAccess.Entidades
{
    public class ExercicioEntity : IDataAccess<Exercicio>, IExercicioData
    {
        public List<Exercicio> GetByFilters(Exercicio exercicio)
        {
            throw new NotImplementedException();
        }

        public List<Exercicio> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Insert(Exercicio exercicio)
        {
            throw new NotImplementedException();
        }

        public int Update(Exercicio exercicio)
        {
            throw new NotImplementedException();
        }

        public int Delete(Exercicio exercicio)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retorna todos os exercícios de determinado tipo
        /// Teste Unitário OK
        /// </summary>
        /// <param name="tipoExercicio"></param>
        /// <returns></returns>
        private List<Exercicio> GetByFilters(Exercicio.tipoExercicio tipoExercicio)
        {
            var e = new List<Exercicio>();
            switch (tipoExercicio)
            {
                case Exercicio.tipoExercicio.SIMULADO:
                    // e = GetSimulados();
                    break;

                case Exercicio.tipoExercicio.CONCURSO:
                    e = GetConcursos();
                    break;
            }

            return e;
        }

        public List<Exercicio> GetSimuladosByFilters(Int32 anoExercicio, int matricula, int idAplicacao = 1, bool getOnline = false, int idTipoSimulado = (int)Constants.TipoSimulado.Extensivo)
        {
            var temasPermitidos = new PermissaoSimuladoDTO();

            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetSimuladosByFilters))
                temasPermitidos = RedisCacheManager.GetItemObject<PermissaoSimuladoDTO>(RedisCacheConstants.DadosFakes.KeyGetSimuladosByFilters);
            else
                temasPermitidos = ListarTemasDeSimuladoPermitidosAnoAtualComCache(matricula);

            return GetSimuladosPermitidosAnoAtual(temasPermitidos, anoExercicio, matricula, getOnline, idTipoSimulado);
        }

        /// <summary>
        /// Retorna os exercícios de determinado tipo e por ano
        /// Teste Unitário OK
        /// </summary>
        /// <param name="tipoExercicio"></param>
        /// <param name="anoExercicio"></param>
        /// <returns></returns>
        public List<Exercicio> GetByFilters(Exercicio.tipoExercicio tipoExercicio, Int32 anoExercicio, int matricula, int idAplicacao = 1, bool getOnline = false)
        {
            List<Exercicio> e = new List<Exercicio>();

            switch (tipoExercicio)
            {
                case Exercicio.tipoExercicio.SIMULADO:
                    e = GetSimulados(anoExercicio, matricula, idAplicacao, getOnline);
                    break;

                case Exercicio.tipoExercicio.CONCURSO:
                    e = GetConcursos(anoExercicio);
                    break;
            }

            return e;
        }

        //public List<Exercicio> GetSimuladosPermitidos(int matricula, int idAplicacao = 1, bool getOnline = false, int idTipoSimulado = (int) Constants.TipoSimulado.Extensivo)
        public List<Exercicio> GetSimuladosPermitidos(int matricula, int idAplicacao, bool getOnline, int idTipoSimulado = (int)Constants.TipoSimulado.Extensivo)
        {
            using (var ctxMat = new DesenvContext())
            {
                using (var ctx = new AcademicoContext())
                {
                    var data = DateTime.Now;
                    var isEmployee = ctxMat.tblEmployees.Any(e => e.intEmployeeID == matricula && e.txtLogin != string.Empty && e.intStatus == Constants.StatusEmpregadoAtivo);
                    var isMatriculasAntecedencia = ctxMat.tblAPI_VisualizarAntecedencia.Any(e => e.intContactID == matricula);

                    if (matricula == Constants.CONTACTID_ACADEMICO || isEmployee || isMatriculasAntecedencia)
                        data = DateTime.Now.AddMonths(4);

                    var anoAtual = Utilidades.GetYear();
                    var anos = OrdemVendaEntity.AnosValidos(idAplicacao, data);

                    var simAntecipado = ctx.tblSimulado.Where(x => x.intAno == anoAtual && x.intSimuladoOrdem == 10).Select(y => y.intSimuladoID).ToList(); // sim 9, sim 10

                    var statusComPermissao = new[] { (int)Utilidades.ESellOrderStatus.Pendente, (int)Utilidades.ESellOrderStatus.Ativa };

                    var isAtivo =
                        isEmployee
                            ? true
                            : (from so in ctxMat.tblSellOrders
                               join sod in ctxMat.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                               join c in ctxMat.tblCourses on sod.intProductID equals c.intCourseID
                               where (statusComPermissao).Contains(so.intStatus.Value)
                                     && so.intClientID == matricula
                                     && anos.Contains(c.intYear ?? 0)
                               select so.intOrderID).Any();

                    var hasCancelada = (from so in ctxMat.tblSellOrders
                                        join sod in ctxMat.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                        join c in ctxMat.tblCourses on sod.intProductID equals c.intCourseID
                                        where (new[] { (int)Utilidades.ESellOrderStatus.Cancelada }).Contains(so.intStatus.Value)
                                              && so.intClientID == matricula
                                              && anos.Contains(c.intYear ?? 0)
                                        select so.intOrderID).Any();


                    if (!isAtivo && (!hasCancelada && !Utilidades.IsSimuladoRMais(idTipoSimulado)))
                        return new List<Exercicio>();

                    var produtos = new[] {
                    (int) Utilidades.ProductGroups.R3_CLINICA,
                    (int) Utilidades.ProductGroups.R3_CIRURGIA,
                    (int) Utilidades.ProductGroups.R3_PEDIATRIA,
                    (int) Utilidades.ProductGroups.R4_GO,
                    (int) Utilidades.ProductGroups.MED,
                    (int) Utilidades.ProductGroups.MEDCURSO,
                    (int) Utilidades.ProductGroups.MEDCURSOEAD,
                    (int) Utilidades.ProductGroups.MEDEAD
                }.ToList();

                    var isProdutoPermitido = new OrdemVendaEntity()
                                                .GetResumed(matricula, anos, lgroup: produtos)
                                                .Where(x => (statusComPermissao)
                                                .Contains((int)x.Status)).Any();

                    //pegar os simulados
                    var simulados = (from s in ctx.tblSimulado
                                     where s.intTipoSimuladoID == idTipoSimulado &&
                                           (anos.Contains((int)s.intAno) || ((int)s.intAno < DateTime.Now.Year || (isAtivo)))
                                     select s
                                    ).ToList();

                    var simuladosBooks = simulados.Select(x => x.intBookID ?? 0).ToList();
                    var simuladosLesson = simulados.Select(x => x.intLessonTitleID ?? 0).ToList();


                    var cronogramaSimuladosU1 = (from mvc2 in ctxMat.mview_Cronograma
                                                 join lm2 in ctxMat.tblLesson_Material on mvc2.intLessonID equals lm2.intLessonID
                                                 where mvc2.intStoreID != (int)Utilidades.Filiais.EADAssinaturas
                                                       && simuladosBooks.Contains(lm2.intMaterialID)
                                                 select new
                                                 {
                                                     lm2.intMaterialID,
                                                     data = mvc2.dteDateTime,
                                                     mvc2.intSequence
                                                 }).ToList();

                    var cronogramaSimuladosU2 = (from mvc3 in ctxMat.mview_Cronograma
                                                 join lm3 in ctxMat.tblLesson_Material on mvc3.intLessonID equals lm3.intLessonID
                                                 join sod2 in ctxMat.tblSellOrderDetails on mvc3.intCourseID equals sod2.intProductID
                                                 join so2 in ctxMat.tblSellOrders on sod2.intOrderID equals so2.intOrderID
                                                 join ad2 in ctxMat.tblAccountData on so2.intOrderID equals (ad2.intOrderID ?? 0)
                                                 join c2 in ctxMat.tblClients on so2.intClientID equals c2.intClientID
                                                 join p in ctxMat.tblPersons on c2.intClientID equals p.intContactID
                                                 join cs in ctxMat.tblCourses on sod2.intProductID equals cs.intCourseID
                                                 join lma in ctxMat.tblLogMesesBlocoMaterialAnteriorAvulso on so2.intOrderID equals lma.intOrderID into LMA
                                                 from lma in LMA.DefaultIfEmpty()
                                                 where simuladosBooks.Contains(lm3.intMaterialID)
                                                 && simuladosLesson.Contains(mvc3.intLessonTitleID)
                                                 && so2.intClientID == matricula
                                                 && c2.intAccountID == ad2.intDebitAccountID
                                                 && (ad2.intStatusID == (int)Utilidades.ESellOrderStatus.Ativa || isEmployee || isMatriculasAntecedencia)
                                                 && (isProdutoPermitido || hasCancelada)
                                                 select new
                                                 {
                                                     intMaterialID = lm3.intMaterialID,
                                                     data = mvc3.dteDateTime,
                                                     intSequence = (int)mvc3.intSequence,
                                                     intMes = lma.intMes > 0 ? lma.intMes : 0

                                                 }).ToList();
                    var cronogramaSimulados = (from s in simulados
                                               join c1 in cronogramaSimuladosU1 on s.intBookID equals c1.intMaterialID
                                               where (c1.intSequence > -1 || (s.intSimuladoOrdem == 10 || (s.bitOnline && (getOnline || (s.dteReleaseGabarito ?? DateTime.MaxValue) < data))))
                                               select new
                                               {
                                                   idsimulado = s.intSimuladoID,
                                                   nome = s.txtSimuladoName,
                                                   mes = s.intSimuladoOrdem,
                                                   ano = s.intAno.Value,
                                                   data = c1.data,
                                                   online = s.bitOnline,
                                                   dataRanking = s.dteInicioConsultaRanking
                                               }
                                               ).Union(from s in simulados
                                                       join c2 in cronogramaSimuladosU2 on s.intBookID equals c2.intMaterialID
                                                       where (c2.intSequence == c2.data.Month || (s.dteReleaseGabarito ?? DateTime.Now).Month == c2.intSequence || (c2.intMes == c2.data.Month) || c2.intMes == (s.dteReleaseGabarito ?? DateTime.Now).Month)
                                                               && (c2.intSequence > -1 || s.intSimuladoOrdem == 10 || (s.bitOnline && (getOnline || (s.dteReleaseGabarito ?? DateTime.MaxValue) < data)))

                                                       select new
                                                       {
                                                           idsimulado = s.intSimuladoID,
                                                           nome = s.txtSimuladoName,
                                                           mes = s.intSimuladoOrdem,
                                                           ano = s.intAno.Value,
                                                           data = c2.data,
                                                           online = s.bitOnline,
                                                           dataRanking = s.dteInicioConsultaRanking
                                                       }
                                                 ).Distinct().GroupBy(x => x.idsimulado).ToList();


                    var ex = new List<Exercicio>();

                    foreach (var g in cronogramaSimulados)
                    {

                        var e = new Exercicio
                        {
                            ID = g.Key,
                            Ano = g.Select(y => y.ano).FirstOrDefault(),
                            ExercicioName = g.Select(y => y.nome).FirstOrDefault(),
                            Online = Convert.ToInt32(g.Select(y => y.online).FirstOrDefault()),
                            DtLiberacaoRanking = g.Select(y => y.dataRanking).FirstOrDefault() ?? data.AddDays(1)
                        };

                        ex.Add(e);
                    }
                    return ex;
                }
            }
        }

        public Dictionary<bool, List<int>> GetIdsExerciciosRealizadosAluno(int matricula, int idTipoSimulado = (int)Constants.TipoSimulado.Extensivo)
        {
            try
            {
                using (var ctx = new AcademicoContext())
                {
                    var dteDateRefazer = new DateTime(1900, 01, 01);
                    
                    var consulta = (from a in ctx.tblExercicio_Historico
                                    where a.intClientID == matricula && a.intExercicioTipo == 1
                                            && (a.dteDateFim != null || !a.dteDateInicio.Equals(dteDateRefazer))
                                    select new
                                    {
                                        id = a.intExercicioID,
                                        status = a.dteDateFim != null
                                    }).ToList();

                    var ids = consulta.GroupBy(x => x.status).ToDictionary(x => x.FirstOrDefault().status, y => y.Select(x => x.id).ToList());

                    return ids;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }


        //Concurso Recursos_________________________________________________________________________________

        public ProvasRecurso GetConcursosRecursos(int ano, int matricula)
        {
            throw new NotImplementedException();
            // using (var ctx = new DesenvContext())
            // {
            //     var status = ctx.tblConcurso_Recurso_Status.Where(tipo => tipo.txtCategoria == "P").ToList();

            //     var lstRecursoProvas = new List<ProvaRecurso>();
            //     var parametros = new SqlParameter[] {
            //                                             new SqlParameter("intStatus1", "0"),
            //                                             new SqlParameter("intStatus2", "0"),
            //                                             new SqlParameter("intYear", ano),
            //                                             new SqlParameter("txtBusca", ""),
            //                                         };
            //     var ds = new DBQuery().ExecuteStoredProcedure("msp_RecursoListaConcurso", parametros);

            //     if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0)

            //         for (int i = 0; i < 3; i++)
            //         {
            //             foreach (DataRow row in ds.Tables[i].Rows)
            //                 lstRecursoProvas.Add(new ProvaRecurso
            //                 {
            //                     ID = Convert.ToInt32(row["intProvaID"]),
            //                     ExercicioName = row["NM_CONCURSO"].ToString(),
            //                     SiglaEstado = row["uf"].ToString(),
            //                     TipoConcursoProva = row["txtName"].ToString(),
            //                     Ordem = Convert.ToInt32(row["intOrderTipoProva"]),

            //                     SiglaConcurso = row["SG_CONCURSO"].ToString(),
            //                     ConcursoId = Convert.ToInt32(row["ID_CONCURSO"]),
            //                     IdStatus = Convert.ToInt32(row["idStatus"]),
            //                     Status = status.FirstOrDefault(s => s.ID_CONCURSO_RECURSO_STATUS == Convert.ToInt32(row["idStatus"])).txtConcursoQuestao_Status,
            //                     Descricao = row["concurso_description"].ToString(),
            //                     Grupo = GetGrupoConcurso(Convert.ToInt32(row["idStatus"])).ToString(),
            //                     IdGrupo = Convert.ToInt32(GetGrupoConcurso(Convert.ToInt32(row["idStatus"]))),
            //                     TemVereditoBanca = Convert.ToBoolean(row["bitvereditoBanca"]),
            //                     SemQuestoes = Convert.ToBoolean(row["bitSemQuestoes"]),
            //                     TemGabaritoPos = Convert.ToBoolean(row["bitGabaritoPos"]),
            //                     Ano = ano
            //                 });
            //         }

            //     var provasComunicados = GetProvasComunicados(ano);

            //     foreach (var provaComunicado in provasComunicados)
            //     {
            //         var prova = lstRecursoProvas.Where(a => a.ID == provaComunicado.ID).FirstOrDefault();
            //         prova.StatusComunicado = provaComunicado.StatusComunicado;
            //         prova.Comunicado = provaComunicado.Comunicado;
            //     }

            //     var provasFavoritas = GetProvasFavoritas(matricula);

            //     foreach (var provaFavorita in provasFavoritas)
            //     {
            //         var prova = lstRecursoProvas.Where(a => a.ID == provaFavorita.ConcursoId).FirstOrDefault();
            //         if (prova != null)
            //             prova.Favorita = provaFavorita.Favorita;
            //     }

            //     var pv = new ProvasRecurso();
            //     pv.AddRange(lstRecursoProvas.OrderBy(s => s.SemQuestoes));
            //     return pv;
            // }
        }

        public enum GrupoConcurso
        {
            SemGrupo = 0,
            Analise = 1,
            Proximos = 2,
            Expirados = 3
        }
        public GrupoConcurso GetGrupoConcurso(int idStatus)
        {
            switch (idStatus)
            {
                case 1:
                case 2:
                case 13:
                    return GrupoConcurso.Analise;

                case 6:
                    return GrupoConcurso.Proximos;

                case 9:
                    return GrupoConcurso.Expirados;

                default:
                    return GrupoConcurso.SemGrupo;
            }
        }

        /// <summary>
        /// Retorna uma lista de anos de determinado tipo de exercicio
        /// SIMULADO - DESENVOLVIDO
        /// PROVA - PENDENTE
        /// Teste Unitário OK
        /// </summary>
        /// <param name="tipoExercicio"></param>
        /// <returns></returns>
        public List<Int32> GetAnosExerciciosPermitidos(Exercicio.tipoExercicio tipoExercicio, int matricula, bool getOnline = false, int idAplicacao = 1)
        {
            //======================== LOG
            new Util.Log().SetLog(new LogMsPro
            {
                Matricula = matricula,
                IdApp = (Aplicacoes)idAplicacao,
                Tela = Util.Log.MsProLog_Tela.SimuladoLista,
                Acao = Util.Log.MsProLog_Acao.Abriu
            });
            // ======================== 

            List<Int32> anos = new List<Int32>();

            if (tipoExercicio == Exercicio.tipoExercicio.SIMULADO)
                anos = GetAnosSimulados(matricula, getOnline, idAplicacao);

            var produtosContratados = ProdutoEntity.GetProdutosContratadosPorAnoComCache(matricula);

            int ano;
            try
            {
                ano = Utilidades.GetYear();
            }
            catch
            {
                ano = DateTime.Today.Year;
            }
            var produtosAnoCorrente = produtosContratados.FindAll(x => x.Ano == ano);

            return anos;
        }

        public bool IsProdutoSomenteMedeletro(List<Produto> produtosContratados)
        {
            return produtosContratados.Count == 1 && produtosContratados.FirstOrDefault().IDProduto == (int)Produto.Produtos.MEDELETRO;
        }

        private bool IsProdutosSomenteIntensivo(List<Produto> produtosContratados)
        {
            var produtosIntensivo = new List<int>();
            produtosIntensivo.Add((int)Produto.Produtos.INTENSIVAO);
            produtosIntensivo.Add((int)Produto.Produtos.RAC);
            produtosIntensivo.Add((int)Produto.Produtos.RACIPE);
            produtosIntensivo.Add((int)Produto.Produtos.RAC_IMED);
            produtosIntensivo.Add((int)Produto.Produtos.RACIPE_IMED);

            var produtos = produtosContratados.Select(x => x.IDProduto).ToList();

            return produtos.Except(produtosIntensivo).ToList().Count == 0;
        }

        private PermissaoSimuladoDTO ListarTemasDeSimuladoPermitidosAnoAtual(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                var permissao = new PermissaoSimuladoDTO();
                permissao.IsEmployee = ctx.tblEmployees.Any(e => e.intEmployeeID == matricula && e.txtLogin != string.Empty && e.intStatus == Constants.StatusEmpregadoAtivo);
                permissao.IsMatriculasAntecedencia = ctx.tblAPI_VisualizarAntecedencia.Any(e => e.intContactID == matricula);

                var isUsuarioMeioDeAno = ctx.tblAlunosAnoAtualMaisAnterior.Any(e => e.intClientID == matricula);

                permissao.Temas = (
                    from ov in ctx.tblSellOrders
                    join detalheOv in ctx.tblSellOrderDetails on ov.intOrderID equals detalheOv.intOrderID
                    join turma in ctx.tblCourses on detalheOv.intProductID equals turma.intCourseID
                    join crono in ctx.mview_Cronograma on turma.intCourseID equals crono.intCourseID
                    where turma.intYear == DateTime.Today.Year
                        && ov.intClientID == matricula
                        && crono.intLessonType == (int)Constants.LessonTypes.SimuladoOnline
                        && (ov.intStatus == (int)Utilidades.ESellOrderStatus.Ativa || isUsuarioMeioDeAno)
                    select new TemasPermitidosDTO()
                    {
                        IntLessonTitleID = crono.intLessonTitleID,
                        IntCourseID = turma.intCourseID
                    }
                ).Distinct()
                .ToList();

                return permissao;
            }
        }

        public PermissaoSimuladoDTO ListarTemasDeSimuladoPermitidosAnoAtualComCache(int matricula)
        {
            try
            {
                if (RedisCacheManager.CannotCache(RedisCacheConstants.Permissao.ListarTemasDeSimuladoPermitidosAnoAtual))
                    return ListarTemasDeSimuladoPermitidosAnoAtual(matricula);

                var key = String.Format("{0}:{1}", RedisCacheConstants.Permissao.ListarTemasDeSimuladoPermitidosAnoAtual, matricula);
                var temas = RedisCacheManager.GetItemObject<PermissaoSimuladoDTO>(key);

                if (temas == null)
                {
                    temas = ListarTemasDeSimuladoPermitidosAnoAtual(matricula);
                    if (temas != null)
                    {
                        RedisCacheManager.SetItemObject(key, temas, TimeSpan.FromMinutes(RedisCacheConstants.Permissao.ValidadeListarTemasDeSimuladoPermitidosAnoAtual));
                    }
                }

                return temas;
            }
            catch 
            {
                return ListarTemasDeSimuladoPermitidosAnoAtual(matricula);
            }
        }

        public List<ExercicioHistoricoDTO> ObterSimuladoOnlineEmAndamento(int matricula)
        {
            var retorno = new List<ExercicioHistoricoDTO>();

            using (var ctx = new AcademicoContext())
            {
                retorno = ctx.tblExercicio_Historico.Where(x => x.intClientID == matricula
                && (x.intTipoProva == (int)TipoProvaEnum.ModoOnline || x.intTipoProva == (int)TipoProvaEnum.ModoProva)
                && !x.dteDateFim.HasValue
                && x.bitRealizadoOnline == true
                )
                .Select(x => new ExercicioHistoricoDTO()
                {
                    intHistoricoExercicioID = x.intHistoricoExercicioID,
                    intExercicioID = x.intExercicioID,
                    intExercicioTipo = x.intExercicioTipo,
                    dteDateInicio = x.dteDateInicio,
                    dteDateFim = x.dteDateFim,
                    bitRanqueado = x.bitRanqueado,
                    intTempoExcedido = x.intTempoExcedido,
                    intClientID = x.intClientID,
                    intApplicationID = x.intApplicationID,
                    bitRealizadoOnline = x.bitRealizadoOnline,
                    bitPresencial = x.bitPresencial,
                    intVersaoID = x.intVersaoID,
                    intTipoProva = x.intTipoProva
                }).ToList();
            }

            return retorno;
        }

        private List<Exercicio> GetSimuladosPermitidosAnoAtual(PermissaoSimuladoDTO permissao, int ano, int matricula, bool getOnline, int intSimuladoTipo)
        {
            bool isEmployee;
            bool isMatriculasAntecedencia;
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetSimuladosByFilters))
            {
                isEmployee = false;
                isMatriculasAntecedencia = false;
            }
            else
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    isEmployee = ctxMatDir.tblEmployees.Any(e => e.intEmployeeID == matricula && e.txtLogin != string.Empty && e.intStatus == Constants.StatusEmpregadoAtivo);
                    isMatriculasAntecedencia = ctxMatDir.tblAPI_VisualizarAntecedencia.Any(e => e.intContactID == matricula);
                }
            }

            using (var ctx = new AcademicoContext())
            {
                var data = DateTime.Now;

                if (matricula == Constants.CONTACTID_ACADEMICO || permissao.IsEmployee || permissao.IsMatriculasAntecedencia)
                    data = DateTime.Now.AddMonths(4);

                var temasPermitidos = permissao.Temas.Select(x => x.IntLessonTitleID).Distinct().ToList();
                var cursosPermitidos = permissao.Temas.Select(x => x.IntCourseID).Distinct().ToList();

                var simuladoExcecao = ctx.tblSimuladoOnline_Excecao
                    .Where(ex => ex.intClientID == matricula || cursosPermitidos.Contains(ex.intCourseID))
                    .ToList();

                var simulado = (
                    from sim in ctx.tblSimulado
                    join espSim in ctx.tblEspecialidadesSimulado on sim.intSimuladoID equals espSim.intSimuladoID
                    join esp in ctx.tblEspecialidades on espSim.intEspecialidadeID equals esp.intEspecialidadeID
                    where sim.intTipoSimuladoID == intSimuladoTipo
                            && (ano <= 0 || sim.intAno == ano)
                            && sim.intAno > 2007
                            && sim.bitOnline
                            && (
                                (
                                    sim.intAno == DateTime.Now.Year
                                    && temasPermitidos.Contains(sim.intLessonTitleID ?? 0)
                                )
                                || sim.intAno < DateTime.Now.Year
                            )
                    orderby sim.intAno descending, sim.intSimuladoOrdem descending
                    select new
                    {
                        sim.intSimuladoID,
                        sim.txtSimuladoName,
                        sim.txtSimuladoDescription,
                        sim.intAno,
                        sim.intTipoSimuladoID,
                        sim.dteDataHoraInicioWEB,
                        sim.dteDataHoraTerminoWEB,
                        sim.dteInicioConsultaRanking,
                        sim.bitOnline,
                        sim.intDuracaoSimulado,
                        Especialidade = new Especialidade { Id = esp.intEspecialidadeID, Descricao = esp.CD_ESPECIALIDADE },
                    }
                ).ToList();

                var exercicioAndamento = ObterSimuladoOnlineEmAndamento(matricula);

                var listaExercicio = (
                    from sim in simulado
                    from simExc in
                        simuladoExcecao.Where(x =>
                            x.intSimuladoExcecaoID == (from ex in simuladoExcecao
                                                       where ex.intSimuladoID == sim.intSimuladoID
                                                       orderby ex.intSimuladoExcecaoID descending
                                                       select ex.intSimuladoExcecaoID).FirstOrDefault()
                            ).DefaultIfEmpty()
                    select new
                    {
                        ID = sim.intSimuladoID,
                        ExercicioName = sim.txtSimuladoName,
                        Descricao = sim.txtSimuladoDescription,
                        Ano = sim.intAno ?? 0,
                        TipoId = sim.intTipoSimuladoID,
                        DataInicio = simExc != null && simExc.dteDataHoraInicioWEB != null ? simExc.dteDataHoraInicioWEB : sim.dteDataHoraInicioWEB,
                        DataFim = simExc != null && simExc.dteDataHoraTerminoWEB != null ? simExc.dteDataHoraTerminoWEB : sim.dteDataHoraTerminoWEB,
                        DtLiberacaoRanking = simExc != null && simExc.dteInicioConsultaRanking != null ? simExc.dteInicioConsultaRanking : sim.dteInicioConsultaRanking,
                        Online = simExc != null ? simExc.bitRefazer : sim.bitOnline,
                        Duracao = sim.intDuracaoSimulado,
                        Especialidade = sim.Especialidade,
                    })
                    .Where(x => x.DtLiberacaoRanking.Value.AddHours(2) < data || (getOnline && x.Online))
                    .Select(x => new Exercicio
                    {
                        DataInicio = x.DataInicio == null ? 0 : Utilidades.DateTimeToUnixTimestamp(Convert.ToDateTime(x.DataInicio)),
                        DataFim = x.DataFim == null ? 0 : Utilidades.DateTimeToUnixTimestamp(Convert.ToDateTime(x.DataFim)),
                        ID = x.ID,
                        ExercicioName = x.ExercicioName,
                        Descricao = x.Descricao,
                        Ano = x.Ano,
                        Especialidade = x.Especialidade,
                        Online = Convert.ToInt32(x.Online),
                        Duracao = x.Duracao,
                        DtLiberacaoRanking = x.DtLiberacaoRanking.Value,
                        Atual = x.Ano == DateTime.Now.Year && x.DataInicio <= DateTime.Now && x.DataFim >= DateTime.Now ? 1 : 0,
                        TipoId = x.TipoId,
                        BitAndamento = exercicioAndamento.Where(s => s.intExercicioID == x.ID).Any()
                    }).ToList();

                return listaExercicio;
            }
        }

        /// <summary>
        /// Retorna uma lista de anos de exercícios do tipo simulado
        /// Teste Unitário OK - Pelo método GetAnos
        /// </summary>
        /// <returns></returns>
        public List<Int32> GetAnosSimulados(int matricula, bool getOnline = false, int idAplicacao = 1, int idTipoSimulado = (int) Constants.TipoSimulado.Extensivo)
        {
            var permitidos = ListarTemasDeSimuladoPermitidosAnoAtualComCache(matricula);
            var simulados = GetSimuladosPermitidosAnoAtual(permitidos, 0, matricula, getOnline, idTipoSimulado);

            return simulados.Select(x => x.Ano).Distinct().OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Retorna os simulados de determinado ano
        /// Teste unitário Ok Pelo método GetAnosSimulados
        /// </summary>
        /// <param name="ano"></param>
        /// <returns></returns>
        private List<Exercicio> GetSimulados(int ano, int matricula, int idAplicacao = 1, bool getOnline = false, int idTipoSimulado = (int) Constants.TipoSimulado.Extensivo)
        {
            return GetSimulados(matricula, idAplicacao, getOnline, idTipoSimulado, ano);
        }

        /// <summary>
        /// Retorna uma lista de todos os simulados
        /// Teste unitário Ok Pelo método GetSimulados(Int32 ano)
        /// </summary>
        /// <returns></returns>
        public List<Exercicio> GetSimulados(int matricula, int idAplicacao = 1, bool getOnline = false, int idTipoSimulado = (int)Constants.TipoSimulado.Extensivo, int anoExercicio = 0)
        {
            try
            {
                using (var ctx = new AcademicoContext())
                {
                    using (var ctxMatDir = new DesenvContext())
                    {
                        var statusPermitidos = new int[] { (int)Utilidades.ESellOrderStatus.Ativa }.ToList();
                        var listaIntProductID = (from so in ctxMatDir.tblSellOrders
                                                 join sod in ctxMatDir.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                                 where so.intClientID == matricula
                                                   && statusPermitidos.Contains(so.intStatus ?? -1)
                                                 select sod.intProductID).ToList();
                        var simExcecao = (from ex in ctx.tblSimuladoOnline_Excecao
                                          where listaIntProductID.Contains(ex.intCourseID)
                                          select ex
                                          ).Union(
                                          from ex in ctx.tblSimuladoOnline_Excecao
                                          where ex.intClientID == matricula
                                          select ex);

                        var anosDisponiveis = new List<int>();

                        if (anoExercicio > 0)
                            anosDisponiveis.Add(anoExercicio);
                        else
                            anosDisponiveis.AddRange(ctxMatDir.tblBooks.Where(b => b.intYear != null).Select(x => x.intYear.Value).ToList());

                        var listaBook = (from p in ctxMatDir.tblProducts
                                         join book in ctxMatDir.tblBooks on p.intProductID equals book.intBookID
                                         where book.intYear != null && anosDisponiveis.Contains(book.intYear.Value)
                                         select book).ToList();
                        var listaIntBookID = listaBook.Select(x => x.intBookID).ToList();
                        var query = (from s in ctx.tblSimulado
                                     join es in ctx.tblEspecialidadesSimulado on s.intSimuladoID equals es.intSimuladoID
                                     join e in ctx.tblEspecialidades on es.intEspecialidadeID equals e.intEspecialidadeID
                                     join ex in simExcecao on s.intSimuladoID equals ex.intSimuladoID into simEx
                                     from excecao in simEx.DefaultIfEmpty()
                                     where listaIntBookID.Contains(s.intBookID ?? 0) && s.intTipoSimuladoID == idTipoSimulado
                                     orderby s.intSimuladoID
                                     select new { s, e, excecao }).ToList();

                        var todosSimulados = query.Select(item => new
                        {
                            ID = item.s.intSimuladoID,
                            ExercicioName = item.s.txtSimuladoName,
                            Descricao = item.s.txtSimuladoDescription,
                            Ano = listaBook.Where(x => item.s.intBookID == x.intBookID).Select(x => x.intYear ?? 0).FirstOrDefault(),
                            DataInicio = item.excecao != null && item.excecao.dteDataHoraInicioWEB != null ? item.excecao.dteDataHoraInicioWEB : item.s.dteDataHoraInicioWEB,
                            DataFim = item.excecao != null && item.excecao.dteDataHoraTerminoWEB != null ? item.excecao.dteDataHoraTerminoWEB : item.s.dteDataHoraTerminoWEB,
                            TipoId = item.s.intTipoSimuladoID,
                            Especialidade = new Especialidade
                            {
                                Id = item.e.intEspecialidadeID,
                                Descricao = item.e.CD_ESPECIALIDADE
                            },
                            Online = item.s.bitOnline,
                            Duracao = item.s.intDuracaoSimulado,
                            DtLiberacaoRanking = item.excecao != null && item.excecao.dteInicioConsultaRanking != null ? item.excecao.dteInicioConsultaRanking : item.s.dteInicioConsultaRanking
                        }).ToList();

                        var simuladosPermitidos = GetSimuladosPermitidos(matricula, idAplicacao, getOnline, idTipoSimulado);
                        var result = new List<Exercicio>();
                        foreach (var exerc in todosSimulados)
                        {
                            var simuladoPermitido = simuladosPermitidos.Where(e => e.ID == exerc.ID);
                            if (simuladoPermitido.Any())
                            {
                                result.Add(new Exercicio
                                {
                                    DataInicio = exerc.DataInicio == null ? 0 : Utilidades.DateTimeToUnixTimestamp(Convert.ToDateTime(exerc.DataInicio)),
                                    DataFim = exerc.DataFim == null ? 0 : Utilidades.DateTimeToUnixTimestamp(Convert.ToDateTime(exerc.DataFim)),
                                    ID = exerc.ID,
                                    ExercicioName = exerc.ExercicioName,
                                    Descricao = exerc.Descricao,
                                    Ano = exerc.Ano,
                                    Especialidade = new Especialidade { Id = exerc.Especialidade.Id, Descricao = exerc.Especialidade.Descricao },
                                    Online = Convert.ToInt32(exerc.Online),
                                    Duracao = exerc.Duracao,
                                    DtLiberacaoRanking = exerc.DtLiberacaoRanking ?? simuladosPermitidos.FirstOrDefault().DtLiberacaoRanking,
                                    Atual = exerc.Ano == DateTime.Now.Year && exerc.DataInicio <= DateTime.Now && exerc.DataFim >= DateTime.Now ? 1 : 0
                                });
                            }
                        }

                        return result;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Exercicio> GetConcursosConsiderandoBloqueio()
        {
            throw new NotImplementedException();
            // var lista = GetConcursos();
            // var ctx = new DesenvContext();

            // var listaConcursoBloqueados = ctx.tblBloqueioConcurso.Where(b => b.intBloqueioAreaId == (int)BloqueioConcursoArea.PortalProfessor).Select(b => b.intProvaId).ToList();

            // return lista.Where(l => !listaConcursoBloqueados.Contains(l.ID)).ToList();
        }


        public List<Exercicio> GetConcursos()
        {
            throw new NotImplementedException();
            // var ctx = new DesenvContext();

            // var query = (from cp in ctx.tblConcurso_Provas
            //              join cq in ctx.tblConcursoQuestoes on cp.intProvaID equals cq.intProvaID
            //              join c in ctx.tblConcursoes on cp.ID_CONCURSO equals c.ID_CONCURSO
            //              join st in ctx.tblStates on c.CD_UF equals st.txtCaption
            //              join reg in ctx.tblRegions on st.intRegionID equals reg.intRegionID
            //              //where c.VL_ANO_CONCURSO > 2008
            //              select new Exercicio()
            //              {
            //                  ID = cp.intProvaID,
            //                  ExercicioName = c.NM_CONCURSO,
            //                  Descricao = c.SG_CONCURSO,
            //                  EstadoID = st.intStateID,
            //                  Estado = st.txtDescription,
            //                  SiglaEstado = st.txtCaption,

            //                  RegiaoID = st.intRegionID ?? 0,
            //                  Regiao = reg.txtDescription.Trim(),
            //                  Ano = c.VL_ANO_CONCURSO ?? 0,
            //                  IdConcurso = c.ID_CONCURSO,
            //                  IsPremium = ctx.tblConcursoPremium.Any(premium => c.SG_CONCURSO.Trim().ToLower() == premium.txtSigla.Trim().ToLower())
            //              })
            //     .Distinct()
            //     .OrderBy(e => e.Ano)
            //     .ThenBy(e => e.Descricao);

            // return query.ToList();
        }

        public List<Exercicio> GetConcursos(int matricula)
        {
            throw new NotImplementedException();
            // var ctx = new DesenvContext();
            // List<int> lstAcessosConcurso = new List<int> { 55, 103 };

            // var isAlunoPermissao = new OrdemVendaEntity().IsOrdemDeVendaAtiva(matricula);

            // if (!isAlunoPermissao)
            //     return new List<Exercicio>();

            // var query = (from cp in ctx.tblConcurso_Provas
            //              join vv in ctx.mview_ConcursoProvas_Recursos on cp.intProvaID equals vv.intProvaID
            //              join st in ctx.tblStates on vv.CD_UF equals st.txtCaption
            //              join reg in ctx.tblRegions on st.intRegionID equals reg.intRegionID
            //              where !lstAcessosConcurso.Contains(cp.intProvaTipoID) && vv.VL_ANO_CONCURSO > 2008 && cp.bitVendaLiberada.Value
            //              orderby vv.VL_ANO_CONCURSO, vv.SG_CONCURSO
            //              select new Exercicio()
            //              {
            //                  ID = vv.intProvaID,
            //                  ExercicioName = vv.NM_CONCURSO,
            //                  Descricao = vv.SG_CONCURSO,
            //                  EstadoID = st.intStateID,
            //                  Estado = st.txtDescription,
            //                  SiglaEstado = st.txtCaption,

            //                  RegiaoID = st.intRegionID ?? 0,
            //                  Regiao = reg.txtDescription.Trim(),
            //                  Ano = vv.VL_ANO_CONCURSO ?? 0
            //              });

            // var lstEx = query.ToList();

            // //__________REMOVER CONCURSO (USP – SP 2011) Por ser Disc e estar Acesso 1
            // lstEx.RemoveAll(c => c.ID == 794157);
            // //________________________________________________________________________

            // for (int i = 0; i < lstEx.Count(); i++)
            // {
            //     var arrayNome = lstEx[i].Descricao.ToString().Split('_');

            //     if (Convert.ToInt32(arrayNome.ElementAtOrDefault(1)) == 0)
            //     {
            //         lstEx[i].Descricao = arrayNome[0];

            //         if (lstEx.Count() == i + 1)
            //         {
            //             lstEx[i].Ordem = 0;
            //             break;
            //         }

            //         var arrayProximoNome = lstEx[i + 1].Descricao.ToString().Split('_');
            //         if (arrayProximoNome[0] == arrayNome[0])
            //             lstEx[i].Ordem = 1;
            //         else
            //             lstEx[i].Ordem = 0;

            //     }
            //     else
            //     {
            //         lstEx[i].Descricao = arrayNome[0];
            //         lstEx[i].Ordem = Convert.ToInt32(arrayNome[1]);
            //     }
            // }


            // return lstEx;
        }

        /*  public List<Exercicio> GetConcursos(int ano)
          {
              var exercicios = new List<Exercicio>();
              exercicios.AddRange((from e in GetConcursos() where e.Ano == ano orderby e.ExercicioName select e).ToList());

              return exercicios;
          }*/
        public List<Exercicio> GetConcursosPorEstados(string idsEstados)
        {
            var lstIdsEstados = idsEstados.Split(',').Select(x => Int32.Parse(x)).ToList();

            var exercicios2 = (from c in GetConcursos()
                               where lstIdsEstados.Contains(c.EstadoID)
                               orderby c.ExercicioName
                               select c).ToList();
            //--------------------------------------------------------------------------------------
            var exercicioDymanic = new List<dynamic>();
            exercicioDymanic.AddRange((from c in GetConcursos()
                                       where lstIdsEstados.Contains(c.EstadoID)
                                       orderby c.ExercicioName
                                       select c));

            var exercicios = new List<Exercicio>();

            exercicios.AddRange(exercicioDymanic.Select(e => new Exercicio()
            {
                Descricao = e.Descricao,
                Estado = e.Estado,
                RegiaoID = e.RegiaoID,
                ID = e.ID
            }));

            return exercicios2;
        }

        /// <summary>
        /// Método GetAtualizacaoDados, carrega os dados para a atualização do MedSoft, a partir do GUID (Coluna)
        /// Teste Unitário OK
        /// </summary>
        /// <param name="Coluna"></param>
        public Exercicio GetAtualizacaoDados(String guid)
        {
            throw new NotImplementedException();
            // DBQuery q = new DBQuery("msp_Medsoft_SelectAtualizacao_Dados 'EXERCICIO', '" + guid + "'");
            // System.Data.Common.DbDataReader rs = q.reader;
            // Exercicio ex = new Exercicio();

            // if (rs.NextResult())
            // {
            //     if (rs.Read())
            //     {
            //         ex.Guid = rs["guidExercicioID"].ToString();
            //         ex.ID = Convert.ToInt32(rs["intExercicioID"]);
            //         ex.ExercicioName = rs["txtExercicioName"].ToString();
            //         ex.TipoId = Convert.ToInt32(rs["intExercicioTipo"]);
            //         ex.Tipo = rs["txtExercicioTipo"].ToString();
            //         ex.Estado = (rs["txtEstado"] == DBNull.Value) ? null : rs["txtEstado"].ToString();
            //         ex.RegiaoID = (rs["intRegiaoID"] == DBNull.Value) ? 0 : Convert.ToInt32(rs["intRegiaoID"]);
            //         ex.Regiao = (rs["txtRegiao"] == DBNull.Value) ? null : rs["txtRegiao"].ToString();
            //         ex.Descricao = rs["txtExercicioDescription"].ToString();
            //         if (rs["intExercicioOrdem"] != DBNull.Value) Convert.ToInt32(rs["intExercicioOrdem"]);
            //         ex.Ano = Convert.ToInt32(rs["intAno"]);
            //         ex.Duracao = (rs["intExercicioDuracao"] == DBNull.Value) ? 0 : Convert.ToInt32(rs["intExercicioDuracao"]);
            //         ex.DataLiberacao = Utilidades.FormatDataTime(DateTime.Now);
            //         ex.QtdQuestoes = Convert.ToInt32(rs["intQtdQuestoes"]);
            //         ex.Especialidade = new Especialidade { Id = Convert.ToInt32(rs["intExercicioEspecialidadeID"]), Descricao = (rs["txtExercicioEspecialidade"] == DBNull.Value) ? null : rs["txtExercicioEspecialidade"].ToString() };
            //         ex.HoraInicio = (rs["dteDataHoraInicio"] == DBNull.Value) ? null : Utilidades.FormatDataTime(Convert.ToDateTime(rs["dteDataHoraInicio"]));
            //         ex.HoraTermino = (rs["dteDataHoraTermino"] == DBNull.Value) ? null : Utilidades.FormatDataTime(Convert.ToDateTime(rs["dteDataHoraTermino"]));
            //         ex.LastUpdate = (rs["dteDateTimeLastUpdate"] == DBNull.Value) ? null : Utilidades.FormatDataTime(Convert.ToDateTime(rs["dteDateTimeLastUpdate"]));
            //         ex.CapaApostila = (rs["txtExercicioImagemUrl"] == DBNull.Value) ? null : rs["txtExercicioImagemUrl"].ToString();
            //         ex.CapaApostilaThumb = (rs["txtExercicioImagemUrlThumb"] == DBNull.Value) ? null : rs["txtExercicioImagemUrlThumb"].ToString();
            //         ex.TempoTolerancia = Convert.ToInt32(rs["intTempoTolerancia"]);
            //         ex.TipoApostilaId = (rs["intExercicioGrupoID"] == DBNull.Value) ? 0 : Convert.ToInt32(rs["intExercicioGrupoID"]);
            //         ex.StatusId = Convert.ToInt32(rs["bitStatus"]);
            //         ex.Ativo = Convert.ToBoolean(rs["bitAtivo"]);
            //         ex.EntidadeApostilaID = (rs["intEntidadeApostilaID"] == DBNull.Value) ? 0 : Convert.ToInt32(rs["intEntidadeApostilaID"]);
            //         ex.TipoConcursoProva = (rs["txtTipoProva"] == DBNull.Value) ? null : rs["txtTipoProva"].ToString();

            //         //Questoes
            //         List<Questao> questoes = new List<Questao>();

            //         if (rs.NextResult())
            //         {
            //             while (rs.Read())
            //             {
            //                 Questao questao = new Questao();

            //                 questao.Guid = rs["guidQuestaoID"].ToString();
            //                 questao.GrupoId = Convert.ToInt32(rs["intGrupoID"]);
            //                 questao.Id = Convert.ToInt32(rs["intQuestaoID"]);
            //                 questao.Ordem = Convert.ToInt32(rs["intOrder"]);
            //                 questao.Auto = Convert.ToBoolean(rs["bitAuto"]);
            //                 questao.Impressa = Convert.ToBoolean(rs["bitImpressa"]);
            //                 questao.OrderImpressa = (rs["intOrderImpressa"] == DBNull.Value) ? 0 : Convert.ToInt32(rs["intOrderImpressa"]);

            //                 questoes.Add(questao);
            //             }
            //         }

            //         //Grupo
            //         List<Grupo> grupos = new List<Grupo>();

            //         if (rs.NextResult())
            //         {
            //             while (rs.Read())
            //             {
            //                 Grupo grupo = new Grupo();

            //                 grupo.Id = Convert.ToInt32(rs["intGrupoID"]);
            //                 grupo.Descricao = rs["txtGrupoDescricao"].ToString();
            //                 grupo.Duracao = Convert.ToInt32(rs["intDuracao"]);
            //                 grupo.Ordem = Convert.ToInt32(rs["intOrdem"]);
            //                 grupo.DataHoraInicio = (rs["dteDataHoraInicio"] == DBNull.Value) ? null : Utilidades.FormatDataTime(Convert.ToDateTime(rs["dteDataHoraInicio"]));

            //                 grupo.Questoes = (from qt in questoes where qt.GrupoId == grupo.Id select qt).ToList();

            //                 grupos.Add(grupo);
            //             }
            //         }

            //         ex.Grupos = grupos;


            //         //Grupo Data

            //         List<GrupoData> gruposdata = new List<GrupoData>();

            //         if (rs.NextResult())
            //         {
            //             while (rs.Read())
            //             {
            //                 GrupoData gd = new GrupoData()
            //                 {
            //                     GrupoDataId = Convert.ToInt32(rs["intGrupoDataId"]),
            //                     Id = Convert.ToInt32(rs["intGrupoID"]),
            //                     DataHoraInicio = (rs["dteDataHoraInicio"] == DBNull.Value) ? null : Utilidades.FormatDataTime(Convert.ToDateTime(rs["dteDataHoraInicio"]))
            //                 };

            //                 gruposdata.Add(gd);
            //             }
            //         }

            //         if (ex.Grupos != null)
            //             foreach (Grupo gp in ex.Grupos)
            //             {
            //                 gp.GrupoDatas = (from gd in gruposdata where gd.Id == gp.Id select gd).ToList();
            //             }

            //     }
            // }

            // return ex;
        }

        public List<ForumProva.Acerto> GetForumAcertos(int idProva, int idEspecialidade, int matricula = 0)
        {
            throw new NotImplementedException();
            // try
            // {
            //     using (var ctx = new DesenvContext())
            //     {
            //         var acertos = new List<ForumProva.Acerto>();
            //         var parametros = new SqlParameter[] {
            //                                                 new SqlParameter("op", 1),
            //                                                 new SqlParameter("intProvaID", idProva),
            //                                                 new SqlParameter("intClientID", matricula),
            //                                                 new SqlParameter("intEspecialidadeID", idEspecialidade)
            //                                             };

            //         var dt = new Util.DBQuery().ExecuteStoredProcedure("msp_RecursosForum", parametros).Tables[0];

            //         foreach (DataRow dRow in dt.Rows)
            //             acertos.Add(new ForumProva.Acerto
            //             {
            //                 Matricula = dRow.IsNull("intContactID") ? 0 : Convert.ToInt32(dRow["intContactID"]),
            //                 Acertos = Convert.ToInt32(dRow["intAcertos"]),
            //                 Nome = dRow["tooltip"].ToString().Split('>')[0],
            //                 UF = dRow["tooltip"].ToString().Split('>')[1],
            //                 Especialidade = new Especialidade
            //                 {
            //                     Descricao = dRow["DE_ESPECIALIDADE"].ToString(),
            //                     Id = Convert.ToInt32(dRow["intEspecialidadeID"])
            //                 }
            //             });

            //         return acertos;
            //     }
            // }
            // catch (Exception ex)
            // {
            //     throw;
            // }
        }

        public List<ForumProva.Acerto> GetForumProvaDashboardAcertos(int idProva, int idEspecialidade)
        {
            throw new NotImplementedException();
            // try
            // {
            //     using (var ctx = new DesenvContext())
            //     {
            //         using (var ctxAcad = new AcademicoContext())
            //         {
            //             var provaAcertos = (from a in ctx.tblConcurso_Provas_Acertos
            //                                 join p in ctx.tblPersons on a.intContactID equals p.intContactID
            //                                 join c in ctx.tblCities on p.intCityID equals c.intCityID
            //                                 join s in ctx.tblStates on c.intState equals s.intStateID
            //                                 where (a.intProvaID == idProva)
            //                                 select new
            //                                 {
            //                                     intEspecialidadeID = a.intEspecialidadeID,
            //                                     Data = a.dteCadastro,
            //                                     Nome = p.txtName,
            //                                     UF = s.txtCaption,
            //                                     Acertos = a.intAcertos
            //                                 }).OrderByDescending(a => a.Data).ToList();

            //             var especialidades = (from e in ctxAcad.tblEspecialidades
            //                                   where e.intEspecialidadeID == idEspecialidade
            //                                   select new
            //                                   {
            //                                       e.intEspecialidadeID,
            //                                       e.DE_ESPECIALIDADE
            //                                   }).ToList();

            //             var acertos = (
            //               from a in provaAcertos
            //               join e in especialidades on a.intEspecialidadeID equals e.intEspecialidadeID into ae
            //               from especialidade in ae.DefaultIfEmpty()
            //               select new ForumProva.Acerto
            //               {
            //                   Data = a.Data,
            //                   Nome = a.Nome,
            //                   UF = a.UF,
            //                   Especialidade = new Especialidade() { Id = ((especialidade == null) ? 0 : especialidade.intEspecialidadeID), Descricao = ((especialidade == null) ? String.Empty : especialidade.DE_ESPECIALIDADE) },
            //                   Acertos = a.Acertos
            //               }
            //             )
            //             .OrderByDescending(a => a.Data)
            //             .ToList();

            //             if (idEspecialidade > 0) acertos = acertos.Where(a => a.Especialidade.Id == idEspecialidade).ToList();

            //             return acertos;
            //         }
            //     }
            // }
            // catch (Exception ex)
            // {
            //     throw;
            // }
        }

        public int GetForumProvaTotalAcertosEnviados(int idProva, int matricula = 0)
        {
            throw new NotImplementedException();
            // try
            // {
            //     var total = 0;
            //     var parametros = new SqlParameter[] {
            //                                             new SqlParameter("op", 1),
            //                                             new SqlParameter("intProvaID", idProva),
            //                                             new SqlParameter("intClientID", matricula)
            //                                         };

            //     var dt = new Util.DBQuery().ExecuteStoredProcedure("msp_RecursosForum", parametros).Tables[0];
            //     total = dt.Rows.Count;
            //     return total;
            // }
            // catch (Exception ex)
            // {
            //     throw;
            // }
        }

        public int GetTotalAcertosDisponiveis(int idProva)
        {
            throw new NotImplementedException();
            // try
            // {
            //     var qtdRetorno = 0;
            //     var dt = new Util.DBQuery().ExecuteQuery(string.Format("select dbo.medfn_Concursos_Forum_QtdeAcertos({0})", idProva)).Tables[0];
            //     if (dt.Rows.Count > 0)
            //         qtdRetorno = Convert.ToInt32(dt.Rows[0][0]);

            //     return qtdRetorno;
            // }
            // catch (Exception ex)
            // {
            //     throw;
            // }
        }

        public List<ForumProva.Comentario> GetForumComentarios(int idProva, int qtdComentarios, double ultimaDataComentario, int matricula = 0)
        {
            throw new NotImplementedException();
            // try
            // {
            //     using (var ctx = new DesenvContext())
            //     {
            //         var comentarios = new List<ForumProva.Comentario>();
            //         var parametros = new SqlParameter[] {
            //                                                 new SqlParameter("op", "0"),
            //                                                 new SqlParameter("intProvaID", idProva),
            //                                                 new SqlParameter("numQuestoes", qtdComentarios),
            //                                                 new SqlParameter("dteCadastro", null),
            //                                                 new SqlParameter("intClientID", matricula)

            //                                             };
            //         if (ultimaDataComentario != 0.0) parametros[3].Value = Utilidades.UnixTimeStampToDateTime(ultimaDataComentario);

            //         var dt = new Util.DBQuery().ExecuteStoredProcedure("msp_RecursosForum_scroll", parametros).Tables[0];

            //         foreach (DataRow dRow in dt.Rows)
            //             comentarios.Add(new ForumProva.Comentario
            //             {
            //                 NickName = dRow["txtNickName"].ToString(),
            //                 Uf = dRow["txtUF"].ToString(),
            //                 UrlAvatar = dRow["txtPicturePath"].ToString(),
            //                 DataAmigavel = dRow["dataAmigavel"].ToString(),
            //                 DataCadastro = Utilidades.FormatDataTime(Convert.ToDateTime(dRow["dteCadastro"])),
            //                 ComentarioTexto = dRow["txtComentario"].ToString(),
            //                 Especialidade = new Especialidade { Descricao = dRow["DE_ESPECIALIDADE"].ToString() },
            //                 Matricula = dRow.IsNull("intContactID") ? 0 : Convert.ToInt32(dRow["intContactID"])
            //             });

            //         return comentarios;
            //     }
            // }
            // catch (Exception ex)
            // {
            //     throw;
            // }
        }

        public int GetForumProvaTotalComentariosEnviados(int idProva, int matricula)
        {
            throw new NotImplementedException();
            // try
            // {
            //     using (var ctx = new DesenvContext())
            //     {
            //         var total = (from fp in ctx.tblConcurso_Provas_Forum
            //                      where fp.intProvaID == idProva
            //                            && (fp.bitActive == true || fp.intContactID == matricula)
            //                      select fp.intProvaForumID).ToList();

            //         var totalLog = (from fl in ctx.tblConcurso_Provas_Forum_log
            //                         where fl.intProvaID == idProva && fl.intContactID == matricula && fl.intProvaForumId != 0
            //                         select fl.intProvaForumId ?? 0).ToList();

            //         total.AddRange(totalLog);

            //         return total.Distinct().Count();
            //     }
            // }
            // catch (Exception ex)
            // {
            //     throw;
            // }
        }

        public List<int> GetAnosConcursosRecursos()
        {
            throw new NotImplementedException();

            // var anos = WebConfigurationManager.AppSettings["anosRecursos"].ToString().Split(',').Select(Int32.Parse).ToList();
            // return anos;

            //using (var ctx = new DesenvContext())
            //{
            //    var ultimoAno = Convert.ToInt32(ctx.msp_RecursosForum_CurrentYear().FirstOrDefault());
            //    var anos = new List<int>();
            //    for (var ano = (ultimoAno - 3); ano <= ultimoAno; ano++)
            //        anos.Add(ano);

            //    return anos;
            //}
        }

        public ProvaRecurso GetProvaPrazo(int idProva)
        {
            throw new NotImplementedException();
            // using (var ctx = new DesenvContext())
            // {
            //     var padraoData = "dd/MM/yyyy";

            //     try
            //     {

            //         var prazoTabelaProvas = ((DateTime)ctx.tblConcurso_Provas.SingleOrDefault(a => a.intProvaID == idProva).dteExpiracao);
            //         if (prazoTabelaProvas != null && prazoTabelaProvas != Convert.ToDateTime("1900-01-01"))
            //             return new ProvaRecurso { Prazo = prazoTabelaProvas.ToString(padraoData) };
            //         else
            //             return new ProvaRecurso { Prazo = Constants.RECURSOS_PRAZO_INDEFINIDO };

            //     }
            //     catch { }

            //     try
            //     {
            //         var prazoTabelaConcursos = ((DateTime)ctx.tblConcursoes.SingleOrDefault(a => a.ID_CONCURSO == idProva).PRAZO_RECURSO_ATE);
            //         if (prazoTabelaConcursos != null)
            //             return new ProvaRecurso { Prazo = prazoTabelaConcursos.ToString(padraoData) };

            //     }
            //     catch { }

            //     return new ProvaRecurso();

            // }
        }

        public Int32 GetHistoricoID(Int32 ExercicioID, Int32 ExercicioTipo, Int32 ApplicationID, Int32 ClientID)
        {
            using (var ctx = new AcademicoContext())
            {
                //Inicialmente verificamos qual é o último histórico do aluno
                var historico = (from h in ctx.tblExercicio_Historico
                                 where h.intClientID == ClientID
                                       && h.intExercicioID == ExercicioID
                                       && h.intExercicioTipo == ExercicioTipo
                                       && h.intApplicationID == ApplicationID
                                 orderby h.intHistoricoExercicioID descending
                                 select h).FirstOrDefault();
                Int32 HistoricoID = 0;

                if (historico == null)
                {
                    var newHistorico = new tblExercicio_Historico
                    {
                        intExercicioID = ExercicioID,
                        intExercicioTipo = ExercicioTipo,
                        dteDateInicio = DateTime.Now,
                        bitRanqueado = false,
                        intTempoExcedido = 0,
                        intClientID = ClientID,
                        intApplicationID = ApplicationID
                    };
                    ctx.tblExercicio_Historico.Add(newHistorico);
                    ctx.SaveChanges();

                    HistoricoID = newHistorico.intHistoricoExercicioID;
                }
                else
                {
                    HistoricoID = historico.intHistoricoExercicioID;
                }

                return HistoricoID;
            }
        }

        public static string GetNomeConcurso(int idprova)
        {
            var ctx = new DesenvContext();
            var consulta = (from c in ctx.tblConcurso
                            join cp in ctx.tblConcurso_Provas on new { ID = c.ID_CONCURSO } equals new { ID = (int)cp.ID_CONCURSO }
                            where cp.intProvaID == idprova
                            select new
                            {
                                Nome = c.NM_CONCURSO,
                                Sigla = c.SG_CONCURSO,
                                Ano = c.VL_ANO_CONCURSO ?? 0
                            }).FirstOrDefault();

            var nome = String.Format("{0} - {1} {2}", consulta.Nome.Trim(), consulta.Sigla.Trim(), consulta.Ano.ToString().Trim());
            return nome;
        }

        public static string GeraPdfExercicio(int idexercicio, string tipoexercicio)
        {
            throw new NotImplementedException();
            // var urlImpressa = tipoexercicio.ToUpper() == "CONCURSO" ? Constants.URLPROVAIMPRESSA : Constants.URLSIMULADOIMPRESSO;
            // var urlOrigem = urlImpressa.Replace("IDEXERCICIO", idexercicio.ToString()).Replace("TIPOEXERCICIO", tipoexercicio).Replace("NOME", Uri.EscapeDataString(GetNomeConcurso(idexercicio).Replace("'", @"")));
            // return Utilidades.GerarPdf(urlOrigem, idexercicio.ToString());
        }

        public static string GeraPdfConcurso(int idconcurso)
        {
            return GeraPdfExercicio(idconcurso, "Concurso");

        }

        public static string GetUrlProvaImpressa(int idexercicio, string tipoexercicio)
        {
            var urlImpressa = tipoexercicio.ToUpper() == "CONCURSO" ? Constants.URLPROVAIMPRESSA : Constants.URLSIMULADOIMPRESSO;
            var urlOrigem = urlImpressa.Replace("IDEXERCICIO", idexercicio.ToString()).Replace("TIPOEXERCICIO", tipoexercicio).Replace("NOME", Uri.EscapeDataString(GetNomeConcurso(idexercicio).Replace("'", @"")));
            return urlOrigem;
        }

        public static string GeraPdfSimulado(int idsimulado)
        {
            throw new NotImplementedException();
            //return Utilidades.GerarPdf("http://static.medgrupo.com.br/_files/Inscricoes/CPMED/2016_CPMED_CONTRATO.pdf", idsimulado.ToString());
            //return GeraPdfExercicio(idsimulado, "Simulado");

        }

        public string GetPdfSimuladoImpressa(int IdExercicio, int Ano, string environmentRootPath)
        {
            var urlImpressa = Constants.URLSIMULADOIMPRESSO_COM_ANO;
            var urlOrigem = urlImpressa.Replace("ANO", Ano.ToString()).Replace("IDEXERCICIO", IdExercicio.ToString());
            return Utilidades.GerarPdf(urlOrigem, IdExercicio.ToString(), environmentRootPath);
        }

        public List<Prova> GetProvasConcursoConsiderandoBloqueio(int idConcurso, string tipoProva)
        {
            var lista = GetProvasConcurso(idConcurso, tipoProva);
            var ctx = new DesenvContext();

            var listaConcursoBloqueados = ctx.tblBloqueioConcurso.Where(b => b.intBloqueioAreaId == (int)BloqueioConcursoArea.PortalProfessor).Select(b => b.intProvaId).ToList();

            return lista.Where(l => !listaConcursoBloqueados.Contains(l.ID)).ToList();
        }

        public List<CatalogoProvasDTO> GetCatalogoDeProvas()
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblConcurso_Provas_Tipos.Select(x => new CatalogoProvasDTO()
                {
                    ID = x.intProvaTipoID,
                    Nome = x.txtDescription
                }).ToList();
            }
        }

        public List<int> GetConcursoAnos()
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblConcurso.Select(x => x.VL_ANO_CONCURSO.Value).Distinct().ToList();
            }
        }

        public List<int> GetConcursoAnosQuestoesConsiderandoBloqueio()
        {
            using (var ctx = new DesenvContext())
            {
                var listaConcursoBloqueados = ctx.tblBloqueioConcurso
                    .Where(b => b.intBloqueioAreaId == (int)BloqueioConcursoArea.PortalProfessor)
                    .Select(b => b.intProvaId);

                var anosPermitidos = (from c in ctx.tblConcurso
                                      join p in ctx.tblConcurso_Provas on c.ID_CONCURSO equals p.ID_CONCURSO
                                      join q in ctx.tblConcursoQuestoes on p.intProvaID equals q.intProvaID
                                      where !listaConcursoBloqueados.Contains(p.intProvaID)
                                      select c.VL_ANO_CONCURSO.Value).Distinct().ToList().OrderBy(x => x).ToList();

                return anosPermitidos;
            }
        }

        public List<Prova> GetProvasPorTipo(string tipoProva)
        {
            var filtrarTipoProva = !string.IsNullOrEmpty(tipoProva);

            using (var ctx = new DesenvContext())
            {
                var consulta = (from concurso in ctx.tblConcurso
                                join provas in ctx.tblConcurso_Provas on concurso.ID_CONCURSO equals provas.ID_CONCURSO
                                join provasTipo in ctx.tblConcurso_Provas_Tipos on provas.intProvaTipoID equals provasTipo.intProvaTipoID
                                where (!filtrarTipoProva || (filtrarTipoProva && provasTipo.txtDescription.Trim().ToLower() == tipoProva.Trim().ToLower()))
                                orderby provas.txtName
                                select new Prova()
                                {
                                    ID = provas.intProvaID,
                                    Nome = provas.txtName,
                                    Descricao = provas.txtDescription,
                                    TipoProva = new TipoProva()
                                    {
                                        ID = provasTipo.intProvaTipoID,
                                        Descricao = provasTipo.txtDescription,
                                        Tipo = provasTipo.txtDescription.ToUpper().Contains("R3") ? "R3" : "R1",
                                        Ordem = provasTipo.IntOrder.Value,
                                        Discursiva = provasTipo.bitDiscursiva.Value
                                    }
                                });

                return consulta.ToList();
            }
        }

        public List<Prova> GetProvasConcurso(int idConcurso, string tipoProva)
        {
            using (var ctx = new DesenvContext())
            {
                var filtrarTipoProva = !string.IsNullOrEmpty(tipoProva);

                var consulta = (from concurso in ctx.tblConcurso
                                join provas in ctx.tblConcurso_Provas on concurso.ID_CONCURSO equals provas.ID_CONCURSO
                                join provasTipo in ctx.tblConcurso_Provas_Tipos on provas.intProvaTipoID equals provasTipo.intProvaTipoID
                                where concurso.ID_CONCURSO == idConcurso && (!filtrarTipoProva || (filtrarTipoProva && provasTipo.txtDescription.Trim().ToLower() == tipoProva.Trim().ToLower()))
                                orderby provas.txtName
                                select new Prova()
                                {
                                    ID = provas.intProvaID,
                                    Nome = provas.txtName,
                                    Descricao = provas.txtDescription,
                                    TipoProva = new TipoProva()
                                    {
                                        ID = provasTipo.intProvaTipoID,
                                        Descricao = provasTipo.txtDescription,
                                        Tipo = provasTipo.txtDescription.ToUpper().Contains("R3") ? "R3" : "R1",
                                        Ordem = provasTipo.IntOrder.Value,
                                        Discursiva = provasTipo.bitDiscursiva.Value
                                    }
                                });

                return consulta.ToList();
            }
        }

        public bool IsforumProvaComentarioRepetido(int idProva, int matricula, string comentario)
        {
            using (var ctx = new DesenvContext())
            {
                var coments = (from fp in ctx.tblConcurso_Provas_Forum
                               where fp.intProvaID == idProva && fp.bitActive == true && fp.intContactID == matricula
                               select fp.txtComentario).ToList();

                var comentsLog = (from fl in ctx.tblConcurso_Provas_Forum_log
                                  where fl.intProvaID == idProva && fl.intContactID == matricula && fl.intProvaForumId != 0
                                  select fl.txtComentario).ToList();

                coments.AddRange(comentsLog);

                return coments.Contains(comentario.Trim());
            }
        }

        public int SetComentarioForumProva(ForumProva forum)
        {
            try
            {
                if (Utilidades.IsActiveFunction(Utilidades.Funcionalidade.RecursosIphone_BloqueiaPosts)) return 1;
                if (IsforumProvaComentarioRepetido(forum.Prova.ID, forum.Matricula, forum.Comentarios[0].ComentarioTexto)) return 1;

                using (var ctx = new DesenvContext())
                {
                    var comentarioForum = new tblConcurso_Provas_Forum()
                    {
                        intProvaID = forum.Prova.ID,
                        txtComentario = forum.Comentarios[0].ComentarioTexto,
                        intEspecialidadeID = forum.Comentarios[0].Especialidade.Id,
                        intContactID = forum.Matricula,
                        txtIP_Usuario = forum.Ip,
                        txtNickname = "", // forum.Comentarios[0].NickName,
                        dteCadastro = DateTime.Now,
                        bitActive = false
                    };

                    ctx.tblConcurso_Provas_Forum.Add(comentarioForum);
                    ctx.SaveChanges();


                    var comentarioForumModerada = new tblConcurso_Provas_Forum_Moderadas
                    {
                        bitModerado = false,
                        intProvaForumId = comentarioForum.intProvaForumID
                    };

                    ctx.tblConcurso_Provas_Forum_Moderadas.Add(comentarioForumModerada);
                    ctx.SaveChanges();
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return 1;
        }

        public int SetAcertosForumProva(ForumProva forum)
        {
            try
            {
                if (Utilidades.IsActiveFunction(Utilidades.Funcionalidade.RecursosIphone_BloqueiaPosts)) return 1;

                //using (var ctx = new DesenvContext())
                //{
                //    var retorno = ctx.msp_RecursosForumTrasacao(0,
                //        forum.Matricula,
                //        "",
                //        forum.Ip,
                //        true,
                //        forum.Prova.ID,
                //        "",
                //        forum.Acertos[0].Acertos,
                //        forum.Acertos[0].Especialidade.Id,
                //        true,
                //        0);
                //}

                using (var ctx = new DesenvContext())
                {
                    var tblAcertosForum = new tblConcurso_Provas_Acertos()
                    {
                        intProvaID = forum.Prova.ID,
                        intAcertos = forum.Acertos[0].Acertos,
                        intEspecialidadeID = forum.Acertos[0].Especialidade.Id,
                        intContactID = forum.Matricula,
                        txtIP_Usuario = forum.Ip,
                        txtNickname = "",   //forum.Comentarios[0].NickName,
                        dteCadastro = DateTime.Now,
                        bitActive = true,
                        txtComentario = ""
                    };

                    ctx.tblConcurso_Provas_Acertos.Add(tblAcertosForum);
                    ctx.SaveChanges();
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return 1;
        }

        public ProvasRecurso GetProvasComunicados(int ano)
        {
            throw new NotImplementedException();
            // using (var ctx = new DesenvContext())
            // {
            //     var comunicadosLista = ctx.msp_RecursoLightboxes(ano).ToList();
            //     if (comunicadosLista != null && comunicadosLista.Any())
            //     {
            //         var provasRecurso = new ProvasRecurso();

            //         foreach (var c in comunicadosLista)
            //         {
            //             provasRecurso.Add(new ProvaRecurso
            //             {
            //                 ID = c.intProvaID,
            //                 SiglaConcurso = c.sg_concurso,
            //                 Comunicado = new MensagemRecurso { Texto = c.txtLightBox },
            //                 StatusComunicado = c.bitActiveLightBox ?? false
            //             });
            //         }

            //         return provasRecurso;
            //     }
            //     else
            //         return new ProvasRecurso();
            // }
        }

        public ProvasRecurso GetProvasFavoritas(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                var provasRecurso = new ProvasRecurso();
                var provasFavoritas = ctx.tblConcursoRecursoFavoritado.Where(o => o.intClienteId == matricula).ToList();

                foreach (var prova in provasFavoritas)
                    provasRecurso.Add(new ProvaRecurso { ConcursoId = prova.intConcursoId, Favorita = true });

                return provasRecurso;
            }
        }

        public int SetStatusProvaFavorita(int provaId, int matricula)
        {
            throw new NotImplementedException();
            // if (!string.IsNullOrEmpty(provaId.ToString()) && !string.IsNullOrEmpty(matricula.ToString()))
            // {
            //     using (var ctx = new DesenvContext())
            //     {
            //         var provaFavorita = ctx.tblConcursoRecursoFavoritadoes.Where(o => o.intClienteId == matricula && o.intConcursoId == provaId).FirstOrDefault();

            //         if (provaFavorita == null)
            //         {
            //             ctx.tblConcursoRecursoFavoritadoes.Add(new tblConcursoRecursoFavoritado
            //             {
            //                 intClienteId = matricula,
            //                 intConcursoId = provaId,
            //                 dteCadastro = DateTime.Now
            //             });

            //         }
            //         else
            //         {
            //             if (provaFavorita != null)
            //                 ctx.tblConcursoRecursoFavoritadoes.Remove(provaFavorita);
            //         }

            //         ctx.SaveChanges();
            //         return 1;
            //     }
            // }
            // return 0;
        }

        public bool AlunoFavoritouProva(int idProva, int matricula)
        {
            throw new NotImplementedException();
            // using (var ctx = new DesenvContext())
            // {
            //     return ctx.tblConcursoRecursoFavoritadoes.Any(
            //         o => o.intClienteId == matricula && o.intConcursoId == idProva
            //         );
            // }
        }

        public int GetAcertosForumProvaPermissao(int idProva, int matricula, int visitante = 0)
        {
            throw new NotImplementedException();
            // try
            // {
            //     if (Utilidades.IsActiveFunction(Utilidades.Funcionalidade.RecursosIphone_BloqueiaPosts) || Convert.ToBoolean(visitante))
            //         return 0;

            //     using (var ctx = new DesenvContext())
            //     {
            //         var permissaoAcertos = !ctx.tblConcurso_Provas_Acertos
            //             .Where(p => p.intProvaID == idProva && p.intContactID == matricula).Any();

            //         var pemissaoBit = ctx.tblConcurso_Provas.Where(p => p.intProvaID == idProva && p.bitRecursoForumAcertosLiberado).Any();


            //         return Convert.ToInt32(permissaoAcertos && pemissaoBit);
            //     }
            // }
            // catch (Exception ex)
            // {
            //     throw;
            // }
        }

        public int GetForumProvaComentarioPermissao(int idProva, int visitante = 0)
        {
            throw new NotImplementedException();
            // try
            // {
            //     using (var ctx = new DesenvContext())
            //     {
            //         if (Utilidades.IsActiveFunction(Utilidades.Funcionalidade.RecursosIphone_BloqueiaPosts) || Convert.ToBoolean(visitante))
            //             return 0;

            //         var prova = (from c in ctx.tblConcursoes
            //                      join cp in ctx.tblConcurso_Provas on c.ID_CONCURSO equals cp.ID_CONCURSO
            //                      where cp.intProvaID == idProva
            //                      select new { ano = c.VL_ANO_CONCURSO, data = cp.dteDateTimeForumComentBlocked ?? DateTime.MinValue }).FirstOrDefault();
            //         if (prova.ano < Constants.anoRecursos || prova.data > DateTime.Now)
            //             return 0;

            //         return 1;
            //     }
            // }
            // catch (Exception ex)
            // {
            //     throw;
            // }
        }

        //public virtual Simulado GetSimuladoConfiguracao(int idSimulado, int matricula, int idAplicacao, string appVersion = "0.0.0")
        public Simulado GetSimuladoConfiguracao(int idSimulado, int matricula, int idAplicacao, string appVersion)
        {
            try
            {
                using(MiniProfiler.Current.Step("Obtendo simulado configuração"))
                {
                    new Util.Log().SetLog(new LogMsPro
                    {
                        Matricula = matricula,
                        IdApp = (Aplicacoes)idAplicacao,
                        Tela = Util.Log.MsProLog_Tela.SimuladoRealizaProva,
                        Acao = Util.Log.MsProLog_Acao.Abriu
                    });
                    
                    using (var ctx = new AcademicoContext())
                    {
                        var simu = ctx.tblSimulado.Where(s => s.intSimuladoID == idSimulado).ToList().FirstOrDefault();
                        var historico = ctx.tblExercicio_Historico.Add(new tblExercicio_Historico
                        {
                            intApplicationID = idAplicacao,
                            intClientID = matricula,
                            intExercicioID = idSimulado,
                            intExercicioTipo = (int)Exercicio.tipoExercicio.SIMULADO,
                            dteDateInicio = DateTime.Now,
                            intTipoProva = (int)TipoProvaEnum.ModoEstudo
                        });
                        ctx.SaveChanges();
                        var idHistorico = historico.Entity.intHistoricoExercicioID;

                        var versaoMinimaComparacao = Utilidades.VersaoMinimaImpressaoSimulado(idAplicacao);

                        var exercicio = new Simulado
                        {
                            IdExercicio = idSimulado,
                            Nome = simu.txtSimuladoDescription,
                            //Duracao = simu.intDuracaoSimulado,      
                            QtdQuestoes = Convert.ToInt32(simu.intQtdQuestoes),
                            QtdQuestoesDiscursivas = Convert.ToInt32(simu.intQtdQuestoesCasoClinico),
                            CartoesResposta = new CartoesResposta { HistoricoId = idHistorico },
                            PermissaoProva = new PermissaoProva
                            {
                                ComentarioTexto = 1,
                                ComentarioVideo = 1,
                                Estatística = 1,
                                Favorito = 1,
                                Gabarito = 1,
                                Recursos = 0,
                                Cronometro = 0,
                                Impressao = Utilidades.VersaoMenorOuIgual(appVersion, versaoMinimaComparacao) ? 0 : 1
                            }
                        };

                        return exercicio;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public Simulado GetSimuladoOnlineConfiguracao(int idSimulado, int matricula, int idAplicacao, string appVersion = "0.0.0")
        {
            try
            {
                using(MiniProfiler.Current.Step("Obtendo configuração simulado online"))
                {
                    using (var ctx = new AcademicoContext())
                    {
                        //Duracao Em Minutos - Aprox.
                        var duracao = 0;

                        //Duracao em Seg - Exato.
                        var duracaoSeg = 0;

                        var idHistorico = 0;

                        var simu = ctx.tblSimulado.Where(s => s.intSimuladoID == idSimulado).ToList().FirstOrDefault();

                        var exercicios = ctx.tblExercicio_Historico.Where(s => s.intClientID == matricula && s.intExercicioID == idSimulado && s.intExercicioTipo == 1);
                        var jaInicouOnline = exercicios.Any();

                        if (jaInicouOnline)
                        {
                            var exercicioHistorico = exercicios
                                .FirstOrDefault();
                            var dataInicial = (new DateTime(1900, 01, 01));
                            var deveRefazer = ctx.tblSimuladoOnline_Excecao.Any(x => x.intClientID == matricula && x.bitRefazer) && exercicioHistorico.dteDateInicio.Equals(dataInicial);
                            if (deveRefazer)
                            {
                                exercicioHistorico.dteDateInicio = DateTime.Now;
                                ctx.SaveChanges();
                            }



                            var tempoRestante = exercicioHistorico.dteDateInicio.AddMinutes(simu.intDuracaoSimulado) - DateTime.Now;

                            var tempoRestanteSeg = exercicioHistorico.dteDateInicio.AddSeconds(simu.intDuracaoSimulado * 60) - DateTime.Now;

                            var totalMinutes = tempoRestante.TotalMinutes;

                            duracaoSeg = Convert.ToInt32(tempoRestante.TotalSeconds);
                            duracao = Convert.ToInt32(tempoRestante.TotalMinutes);


                            idHistorico = exercicioHistorico.intHistoricoExercicioID;
                        }
                        else
                        {
                            duracao = simu.intDuracaoSimulado;
                            duracaoSeg = simu.intDuracaoSimulado * 60;

                            var historico = ctx.tblExercicio_Historico.Add(new tblExercicio_Historico
                            {
                                intApplicationID = idAplicacao,
                                intClientID = matricula,
                                intExercicioID = idSimulado,
                                intExercicioTipo = (int)Exercicio.tipoExercicio.SIMULADO,
                                dteDateInicio = DateTime.Now,
                                intTipoProva = (int)TipoProvaEnum.ModoOnline,
                                bitRealizadoOnline = true,
                                intVersaoID = 1
                            });
                            ctx.SaveChanges();
                            idHistorico = historico.Entity.intHistoricoExercicioID;
                        }

                        var versaoMinimaComparacao = Utilidades.VersaoMinimaImpressaoSimulado(idAplicacao);

                        var exercicio = new Simulado
                        {
                            IdExercicio = idSimulado,
                            Nome = simu.txtSimuladoDescription,
                            Duracao = duracao,      // TODO: Se houver realizaçao (caso o aluno tenha fechado e voltado), debitar o tempo. Buscar da tbl Histórico se ele ja realizou, e se realizou debitar
                            DuracaoEmSeg = duracaoSeg,
                            QtdQuestoes = Convert.ToInt32(simu.intQtdQuestoes),
                            QtdQuestoesDiscursivas = Convert.ToInt32(simu.intQtdQuestoesCasoClinico),
                            CartoesResposta = new CartoesResposta { HistoricoId = idHistorico },
                            PermissaoProva = new PermissaoProva
                            {
                                Cronometro = 1,
                                Impressao = Utilidades.VersaoMenorOuIgual(appVersion, versaoMinimaComparacao) ? 0 : 1
                            }
                        };

                        return exercicio;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public ExercicioHistoricoDTO InserirExercicioSimulado(int idSimulado, int matricula, int idAplicacao, TipoProvaEnum tipoProva)
        {
            var historico = new tblExercicio_Historico();

            using (var ctx = new AcademicoContext())
            {
                historico = ctx.tblExercicio_Historico.Add(new tblExercicio_Historico
                {
                    intApplicationID = idAplicacao,
                    intClientID = matricula,
                    intExercicioID = idSimulado,
                    intExercicioTipo = (int)Exercicio.tipoExercicio.SIMULADO,
                    dteDateInicio = DateTime.Now,
                    intTipoProva = (int)tipoProva
                }).Entity;

                ctx.SaveChanges();
            }

            return new ExercicioHistoricoDTO()
            {
                intApplicationID = historico.intApplicationID,
                intClientID = historico.intClientID,
                intExercicioID = historico.intExercicioID,
                intExercicioTipo = historico.intExercicioTipo,
                dteDateInicio = historico.dteDateInicio,
                intTipoProva = historico.intTipoProva,
                intHistoricoExercicioID = historico.intHistoricoExercicioID
            };
        }

        public List<ExercicioHistoricoDTO> GetComboSimuladosRealizados(int matricula, int simuladoId, int idAplicacao)
        {
            var retorno = new List<ExercicioHistoricoDTO>();

            using (var ctx = new AcademicoContext())
            {
                retorno = ctx.tblExercicio_Historico
                    .Where(x => x.intExercicioID == simuladoId
                    && x.intClientID == matricula
                    && (x.intTipoProva == (int)TipoProvaEnum.ModoOnline || x.intTipoProva == (int)TipoProvaEnum.ModoProva)
                    && (x.intApplicationID == (int)Aplicacoes.MsProDesktop || x.intApplicationID == (int)Aplicacoes.MsProMobile)
                    ).Select(x => new ExercicioHistoricoDTO()
                    {
                        intHistoricoExercicioID = x.intHistoricoExercicioID,
                        intExercicioID = x.intExercicioID,
                        intExercicioTipo = x.intExercicioTipo,
                        dteDateInicio = x.dteDateInicio,
                        dteDateFim = x.dteDateFim,
                        bitRanqueado = x.bitRanqueado,
                        intTempoExcedido = x.intTempoExcedido,
                        intClientID = x.intClientID,
                        intApplicationID = x.intApplicationID,
                        bitRealizadoOnline = x.bitRealizadoOnline,
                        bitPresencial = x.bitPresencial,
                        intVersaoID = x.intVersaoID,
                        intTipoProva = x.intTipoProva
                    }).ToList();
            }

            return retorno;
        }

        public int ObterAcertosDoAlunoNoSimulado(int idSimulado, int matricula, int idAplicacao)
        {
            throw new NotImplementedException();
            // var acertos = 0;

            // using (var ctx = new AcademicoContext())
            // {
            //     acertos = (from eh in ctx.tblExercicio_Historico
            //                join cro in ctx.tblCartaoResposta_objetiva on eh.intHistoricoExercicioID equals cro.intHistoricoExercicioID
            //                join qa in ctx.tblQuestaoAlternativas on cro.intQuestaoID equals qa.intQuestaoID
            //                where eh.intExercicioID == idSimulado && eh.intClientID == matricula && qa.bitCorreta.Value && cro.txtLetraAlternativa == qa.txtLetraAlternativa && eh.intApplicationID == idAplicacao
            //                select new { questoesCorretas = cro.intQuestaoID }
            //                ).Count();
            // }

            // return acertos;
        }

        public int ObterQuantidadeParticipantesSimuladoOnline(int idSimulado)
        {
            throw new NotImplementedException();
            // var retorno = 0;

            // using (var ctx = new AcademicoContext())
            // {
            //     retorno = ctx.tblSimuladoRanking_Fase01
            //         .Where(x => x.intSimuladoID == idSimulado).Count();
            // }

            // return retorno;
        }

        public int ObterQuantidadeQuestoesSimuladoOnline(int idSimulado)
        {
            var retorno = 0;

            using (var ctx = new AcademicoContext())
            {
                retorno = (from s in ctx.tblSimulado
                           join qs in ctx.tblQuestao_Simulado on s.intSimuladoID equals qs.intSimuladoID
                           join q in ctx.tblQuestoes on qs.intQuestaoID equals q.intQuestaoID
                           where s.intSimuladoID == idSimulado
                           select q.intQuestaoID
                           ).Count();
            }

            return retorno;
        }

        public List<PosicaoRankingDTO> ObterRankingPorSimulado(int idSimulado, string especialidade, string unidades, string localidade)
        {
            var retorno = new List<PosicaoRankingDTO>();

            using (var ctx = new AcademicoContext())
            {
                var consulta = ctx.tblSimuladoRanking_Fase01
                    .Where(x => x.intSimuladoID == idSimulado);

                if (!string.IsNullOrEmpty(especialidade))
                    consulta = consulta.Where(x => x.txtEspecialidade.Contains(especialidade));

                if (!string.IsNullOrEmpty(localidade) && !localidade.ToUpper().Equals("TODOS"))
                {
                    localidade = (localidade.IndexOf('(') > 0 ? localidade.Substring(0, localidade.IndexOf('(')).Trim() : localidade);
                    consulta = consulta.Where(x => x.txtUnidade.Contains(localidade));
                }


                if (!string.IsNullOrEmpty(unidades) && !unidades.ToUpper().Equals("TODOS"))
                {
                    var codUnidade = Convert.ToInt32(unidades);
                    consulta = consulta.Where(x => x.intStateID == codUnidade);
                }

                retorno = consulta.Distinct().OrderByDescending(x => x.intAcertos).Select(x => new PosicaoRankingDTO() { Posicao = x.txtPosicao, Acertos = x.intAcertos.Value, Nota = x.dblNotaFinal.Value }).ToList();
            }

            return retorno;
        }

        public Concurso GetConcursoConfiguracao(int idConcurso, int matricula, int idAplicacao)
        {
            
            try
            {
                // ======================== LOG
                new Util.Log().SetLog(new LogMsPro
                {
                    Matricula = matricula,
                    IdApp = (Aplicacoes)idAplicacao,
                    Tela = Util.Log.MsProLog_Tela.CIRealizaProva,
                    Acao = Util.Log.MsProLog_Acao.Abriu
                });
                // ======================== 
                using (var ctx = new AcademicoContext())
                {
                    using (var ctxMatDir = new DesenvContext())
                    {
                        var conc = (from c in ctxMatDir.tblConcurso
                                    join p in ctxMatDir.tblConcurso_Provas on c.ID_CONCURSO equals p.ID_CONCURSO
                                    join cpt in ctxMatDir.tblConcurso_Provas_Tipos on p.intProvaTipoID equals cpt.intProvaTipoID
                                    where p.intProvaID == idConcurso
                                    select new
                                    {
                                        IdProva = p.intProvaID,
                                        IdConcurso = c.ID_CONCURSO,
                                        Nome = c.NM_CONCURSO,
                                        Sigla = c.SG_CONCURSO,
                                        Duracao = 0,
                                        QtdQuestoes = p.intRecursoForumAcertosQtdQuestoes,
                                        Ano = c.VL_ANO_CONCURSO,
                                        Tipo = cpt.txtDescription
                                    }).ToList().FirstOrDefault();

                        var permiteImpresao = conc.Tipo.ToUpper().Contains("R3") || conc.Tipo.ToUpper().Contains("ACESSO DIRETO") ? 1 : 0;

                        var historico = ctx.tblExercicio_Historico.Add(new tblExercicio_Historico
                        {
                            intApplicationID = idAplicacao,
                            intClientID = matricula,
                            intExercicioID = idConcurso,
                            intExercicioTipo = (int)Exercicio.tipoExercicio.CONCURSO,
                            dteDateInicio = DateTime.Now
                        }).Entity;
                        
                        ctx.SaveChanges();
                        var idHistorico = historico.intHistoricoExercicioID;

                        var exercicio = new Concurso
                        {
                            IdExercicio = idConcurso,
                            Nome = string.Concat(conc.Sigla, "|", conc.Nome),
                            QtdQuestoes = Convert.ToInt32(conc.QtdQuestoes),
                            CartoesResposta = new CartoesResposta { HistoricoId = idHistorico },
                            Ano = Convert.ToInt32(conc.Ano),
                            TipoProva = conc.Tipo,
                            PermissaoProva = new PermissaoProva { ComentarioTexto = 1, ComentarioVideo = 1, Estatística = 1, Favorito = 1, Gabarito = 1, Recursos = 1, Cronometro = 0, Impressao = permiteImpresao }
                        };

                        return exercicio;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public Concurso GetProvaPersonalizadaConfiguracao(int idProva, int matricula, int idAplicacao)
        {
            try
            {
                // ======================== LOG
                new Util.Log().SetLog(new LogMsPro
                {
                    Matricula = matricula,
                    IdApp = (Aplicacoes)idAplicacao,
                    Tela = Util.Log.MsProLog_Tela.CIRealizaProva,
                    Acao = Util.Log.MsProLog_Acao.Abriu
                });
                // ======================== 


                using (var ctx = new AcademicoContext())
                {
                    using (var ctxMatDir = new DesenvContext())
                    {
                        var idFiltro = ctxMatDir.tblExercicio_MontaProva.FirstOrDefault(x => x.intID == idProva).intFiltroId;
                        var nomeFiltro = ctxMatDir.tblFiltroAluno_MontaProva.FirstOrDefault(x => x.intID == idFiltro).txtNome;

                        var prova = ctxMatDir.tblExercicio_MontaProva.Where(x => x.intID == idProva).Select(x => new
                        {
                            IdProva = x.intID,
                            Nome = nomeFiltro,
                            Sigla = "",
                            Duracao = 0,
                            Ano = x.dteDataCriacao.Year,
                            Tipo = ""
                        }).FirstOrDefault();

                        var qtdeQuestoes = ctxMatDir.tblQuestao_MontaProva.Count(x => x.intProvaId == prova.IdProva);

                        var historico = ctx.tblExercicio_Historico.Add(new tblExercicio_Historico
                        {
                            intApplicationID = idAplicacao,
                            intClientID = matricula,
                            intExercicioID = prova.IdProva,
                            intExercicioTipo = (int)Exercicio.tipoExercicio.MONTAPROVA,
                            dteDateInicio = DateTime.Now
                        });
                        ctx.SaveChanges();
                        var idHistorico = historico.Entity.intHistoricoExercicioID;

                        var exercicio = new Concurso
                        {
                            IdExercicio = prova.IdProva,
                            Nome = prova.Nome,
                            QtdQuestoes = Convert.ToInt32(qtdeQuestoes),
                            CartoesResposta = new CartoesResposta { HistoricoId = idHistorico },
                            TipoProva = prova.Tipo,
                            PermissaoProva = new PermissaoProva { ComentarioTexto = 1, ComentarioVideo = 1, Estatística = 1, Favorito = 1, Gabarito = 1, Recursos = 1, Cronometro = 0 }
                        };

                        return exercicio;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void RegistrarSimuladoOnline(int historicoId)
        {
            using (var ctx = new AcademicoContext())
            {
                var exercicio = ctx.tblExercicio_Historico.FirstOrDefault(e => e.intHistoricoExercicioID == historicoId);

                exercicio.bitRealizadoOnline = true;
                exercicio.intVersaoID = 1;

                ctx.SaveChanges();
            }
        }

        public List<CartaoRespostaObjetivaDTO> ObterQuestoes(int exercicioHistoricoId)
        {
            throw new NotImplementedException();

            // using (var ctx = new Medgrupo.DataAccessEntity.Academico.AcademicoContext())
            // {
            //     return ctx.tblCartaoResposta_objetiva.Where(x => x.intHistoricoExercicioID == exercicioHistoricoId)
            //         .Select(x => new CartaoRespostaObjetivaDTO()
            //         {
            //             intID = x.intID,
            //             intQuestaoID = x.intQuestaoID,
            //             intHistoricoExercicioID = x.intHistoricoExercicioID,
            //             txtLetraAlternativa = x.txtLetraAlternativa,
            //             guidQuestao = x.guidQuestao,
            //             intExercicioTipoId = x.intExercicioTipoId,
            //             dteCadastro = x.dteCadastro
            //         }).ToList();
            // }
        }

        public List<CartaoRespostaObjetivaDTO> ObterQuestoesOnline(int exercicioHistoricoId)
        {
            using (var ctx = new AcademicoContext())
            {
                return ctx.tblCartaoResposta_objetiva_Simulado_Online.Where(x => x.intHistoricoExercicioID == exercicioHistoricoId)
                    .Select(x => new CartaoRespostaObjetivaDTO()
                    {
                        intID = x.intID,
                        intQuestaoID = x.intQuestaoID,
                        intHistoricoExercicioID = x.intHistoricoExercicioID,
                        txtLetraAlternativa = x.txtLetraAlternativa,
                        guidQuestao = x.guidQuestao,
                        intExercicioTipoId = x.intExercicioTipoId,
                        dteCadastro = x.dteCadastro
                    }).ToList();
            }
        }

        public SimuladoOnlineExcecaoDTO ObterSimuladoAlunoExcecao(int clientId, int exercicioId)
        {
            using (var ctx = new AcademicoContext())
            {
                return ctx.tblSimuladoOnline_Excecao.Where(x => x.intClientID == clientId && x.intSimuladoID == exercicioId)
                    .Select(x => new SimuladoOnlineExcecaoDTO()
                    {
                        intSimuladoExcecaoID = x.intSimuladoExcecaoID,
                        intSimuladoID = x.intSimuladoID,
                        intClientID = x.intClientID,
                        intCourseID = x.intCourseID,
                        dteDataHoraInicioWEB = x.dteDataHoraInicioWEB,
                        dteDataHoraTerminoWEB = x.dteDataHoraTerminoWEB,
                        bitRefazer = x.bitRefazer,
                        dteInicioConsultaRanking = x.dteInicioConsultaRanking,
                    })
                    .OrderByDescending(x => x.intSimuladoExcecaoID)
                    .FirstOrDefault();
            }
        }

        public TblSimuladoDTO ObterSimulado(int exercicioId)
        {
            using (var ctx = new AcademicoContext())
            {
                return ctx.tblSimulado.Where(x => x.intSimuladoID == exercicioId).Select(x => new TblSimuladoDTO()
                {
                    intSimuladoID = x.intSimuladoID,
                    intLessonTitleID = x.intLessonTitleID,
                    intBookID = x.intBookID,
                    txtOrigem = x.txtOrigem,
                    txtSimuladoName = x.txtSimuladoName,
                    txtSimuladoDescription = x.txtSimuladoDescription,
                    intSimuladoOrdem = x.intSimuladoOrdem,
                    intDuracaoSimulado = x.intDuracaoSimulado,
                    intConcursoID = x.intConcursoID,
                    intAno = x.intAno,
                    bitParaWEB = x.bitParaWEB,
                    dteDataHoraInicioWEB = x.dteDataHoraInicioWEB,
                    dteDataHoraTerminoWEB = x.dteDataHoraTerminoWEB,
                    dteReleaseSimuladoWeb = x.dteReleaseSimuladoWeb,
                    dteReleaseGabarito = x.dteReleaseGabarito,
                    dteReleaseComentario = x.dteReleaseComentario,
                    dteInicioConsultaRanking = x.dteInicioConsultaRanking,
                    dteLimiteParaRanking = x.dteLimiteParaRanking,
                    bitIsDemo = x.bitIsDemo,
                    CD_ESPECIALIDADE = x.CD_ESPECIALIDADE,
                    ID_INSTITUICAO = x.ID_INSTITUICAO,
                    txtPathGabarito = x.txtPathGabarito,
                    intQtdQuestoes = x.intQtdQuestoes,
                    bitRankingWeb = x.bitRankingWeb,
                    bitGabaritoWeb = x.bitGabaritoWeb,
                    bitRankingFinalWeb = x.bitRankingFinalWeb,
                    txtCodQuestoes = x.txtCodQuestoes,
                    bitVideoComentariosWeb = x.bitVideoComentariosWeb,
                    intQtdQuestoesCasoClinico = x.intQtdQuestoesCasoClinico,
                    guidSimuladoID = x.guidSimuladoID,
                    dteDateTimeLastUpdate = x.dteDateTimeLastUpdate,
                    bitCronogramaAprovado = x.bitCronogramaAprovado,
                    intTipoSimuladoID = x.intTipoSimuladoID,
                    bitSimuladoGeral = x.bitSimuladoGeral,
                    bitOnline = x.bitOnline,
                    intPesoProvaObjetiva = x.intPesoProvaObjetiva,
                    dteDateInicio = x.dteDateInicio,
                    dteDateFim = x.dteDateFim
                }).FirstOrDefault();
            }
        }

        public ExercicioHistoricoDTO ObterExercicio(int historicoId)
        {
            using (var ctx = new AcademicoContext())
            {
                return ctx.tblExercicio_Historico
                    .Where(e => e.intHistoricoExercicioID == historicoId)
                    .Select(x => new ExercicioHistoricoDTO()
                    {
                        intHistoricoExercicioID = x.intHistoricoExercicioID,
                        intExercicioID = x.intExercicioID,
                        intExercicioTipo = x.intExercicioTipo,
                        dteDateInicio = x.dteDateInicio,
                        dteDateFim = x.dteDateFim,
                        bitRanqueado = x.bitRanqueado,
                        intTempoExcedido = x.intTempoExcedido,
                        intClientID = x.intClientID,
                        intApplicationID = x.intApplicationID,
                        bitRealizadoOnline = x.bitRealizadoOnline,
                        bitPresencial = x.bitPresencial,
                        intVersaoID = x.intVersaoID,
                        intTipoProva = x.intTipoProva
                    }).FirstOrDefault();
            }
        }

        public ExercicioHistoricoDTO ObterUltimoExercicioSimuladoModoProva(int matricula, int simuladoId)
        {
            using (var ctx = new AcademicoContext())
            {
                return ctx.tblExercicio_Historico
                    .Where(x => x.intClientID == matricula && x.intExercicioID == simuladoId && (TipoProvaEnum)x.intTipoProva == TipoProvaEnum.ModoProva && x.dteDateFim == null)
                    .OrderByDescending(x => x.dteDateInicio)
                    .Select(x => new ExercicioHistoricoDTO()
                    {
                        intHistoricoExercicioID = x.intHistoricoExercicioID,
                        intExercicioID = x.intExercicioID,
                        intExercicioTipo = x.intExercicioTipo,
                        dteDateInicio = x.dteDateInicio,
                        dteDateFim = x.dteDateFim,
                        bitRanqueado = x.bitRanqueado,
                        intTempoExcedido = x.intTempoExcedido,
                        intClientID = x.intClientID,
                        intApplicationID = x.intApplicationID,
                        bitRealizadoOnline = x.bitRealizadoOnline,
                        bitPresencial = x.bitPresencial,
                        intVersaoID = x.intVersaoID,
                        intTipoProva = x.intTipoProva
                    }).FirstOrDefault();
            }
        }

        public void FinalizarExercicio(Exercicio exercicio)
        {
            using (var ctx = new AcademicoContext())
            {
                var exerc = ctx.tblExercicio_Historico.FirstOrDefault(e => e.intHistoricoExercicioID == exercicio.HistoricoId);

                exerc.dteDateFim = DateTime.Now;
                exerc.bitRanqueado = Convert.ToBoolean(exercicio.Ranqueado);
                exerc.intTempoExcedido = exercicio.TempoExcedido;

                ctx.SaveChanges();
            }
        }

        public AlunoTurmaDTO GetDadosAlunoTurma(int intClientId)
        {
            var listaIntProductGroup1 = new List<int>(){
                (int)Utilidades.ProductGroups.MEDCURSO,
                (int)Utilidades.ProductGroups.MED,
                (int)Utilidades.ProductGroups.MEDEAD,
                (int)Utilidades.ProductGroups.MEDCURSOEAD,
                (int)Utilidades.ProductGroups.R3_CLINICA,
                (int)Utilidades.ProductGroups.R3_CIRURGIA,
                (int)Utilidades.ProductGroups.R3_PEDIATRIA,
                (int)Utilidades.ProductGroups.R4_GO,
                (int)Utilidades.ProductGroups.INTENSIVO
            };

            var sortOrder = new Dictionary<int, int>
            {
                {(int)Utilidades.ProductGroups.MED, 1},
                {(int)Utilidades.ProductGroups.MEDEAD, 2},   
                {(int)Utilidades.ProductGroups.MEDCURSO, 3},  
                {(int)Utilidades.ProductGroups.MEDCURSOEAD, 4},  
                {(int)Utilidades.ProductGroups.R3_CLINICA, 5}, 
                {(int)Utilidades.ProductGroups.R3_CIRURGIA, 6}, 
                {(int)Utilidades.ProductGroups.R3_PEDIATRIA, 7},
                {(int)Utilidades.ProductGroups.R4_GO, 8},
                {(int)Utilidades.ProductGroups.INTENSIVO, 9}
            };

            int anoAtual = Utilidades.GetServerDate().Year;

            using (var ctxMatDir = new DesenvContext())
            {
                var alunosAno = ctxMatDir.tblAlunosAnoAtualMaisAnterior.Any(x => x.intClientID == intClientId);
                int order;
                var permissaoAluno = (
                    from sellOrders in ctxMatDir.tblSellOrders
                    join sellOrdersDet in ctxMatDir.tblSellOrderDetails on sellOrders.intOrderID equals sellOrdersDet.intOrderID
                    join prod in ctxMatDir.tblProducts on sellOrdersDet.intProductID equals prod.intProductID
                    join courses in ctxMatDir.tblCourses on sellOrdersDet.intProductID equals courses.intCourseID
                    join stores in ctxMatDir.tblStores on sellOrders.intStoreID equals stores.intStoreID
                    join cli in ctxMatDir.tblClients on sellOrders.intClientID equals cli.intClientID
                    join cit in ctxMatDir.tblCities on stores.intCityID equals cit.intCityID into cijoin
                    from cit in cijoin.DefaultIfEmpty()

                    where
                        (
                            sellOrders.intStatus == (int)OrdemVenda.StatusOv.Ativa
                            || (sellOrders.intStatus == (int)OrdemVenda.StatusOv.Cancelada && alunosAno)
                        )
                        && listaIntProductGroup1.Contains(prod.intProductGroup1 ?? 0)
                        && courses.intYear == anoAtual
                        && sellOrders.intClientID == intClientId
                    select new
                    {
                        sellOrders.intOrderID,
                        sellOrders.intClientID,
                        courses.intCourseID,
                        prod.txtName,
                        courses.intPrincipalClassRoomID,
                        stores.txtStoreName,
                        intState = cit != null ? cit.intState : (int?)null,
                        cli.intEspecialidadeID,
                        intProductGroup1 = prod.intProductGroup1 ?? 0
                    }
                ).AsEnumerable()
                .OrderBy(p => sortOrder.TryGetValue(p.intProductGroup1, out order) ? order : 3)
                .FirstOrDefault();

                AlunoTurmaDTO dadosLogSimulado = null;
                if (permissaoAluno != null)
                {
                    dadosLogSimulado = new AlunoTurmaDTO()
                    {
                        IntOrderID = permissaoAluno.intOrderID,
                        IntClientID = permissaoAluno.intClientID,
                        IntCourseID = permissaoAluno.intCourseID,
                        TxtCourseName = permissaoAluno.txtName,
                        IntPrincipalClassRoomID = permissaoAluno.intPrincipalClassRoomID,
                        TxtStoreName = permissaoAluno.txtStoreName,
                        IntCitState = permissaoAluno.intState,
                        IntEspecialidadeID = permissaoAluno.intEspecialidadeID
                    };

                    dadosLogSimulado.IsEmploye = (
                        from emp in ctxMatDir.tblEmployees
                        where emp.intEmployeeID == intClientId
                        select emp.intEmployeeID
                    ).Any();

                    dadosLogSimulado.IsPerson = (
                        from emp in ctxMatDir.tblPersons
                        where emp.intContactID == intClientId
                        select emp.intContactID
                    ).Any();
                }

                return dadosLogSimulado;
            }
        }

        public AlunoTurmaDTO GetDadosAlunoTurmaComCache(int intClientId)
        {
            try
            {
                if (RedisCacheManager.CannotCache(RedisCacheConstants.Exercicio.KeyGetDadosAlunoTurma))
                    return GetDadosAlunoTurma(intClientId);

                var key = String.Format("{0}:{1}", RedisCacheConstants.Exercicio.KeyGetDadosAlunoTurma, intClientId);
                var alunoTurma = RedisCacheManager.GetItemObject<AlunoTurmaDTO>(key);

                if (alunoTurma == null)
                {
                    alunoTurma = GetDadosAlunoTurma(intClientId);
                    if (alunoTurma != null)
                    {
                        RedisCacheManager.SetItemObject(key, alunoTurma, TimeSpan.FromMinutes(RedisCacheConstants.Exercicio.ValidadeGetDadosAlunoTurma));
                    }
                }

                return alunoTurma;
            }
            catch
            {
                return GetDadosAlunoTurma(intClientId);
            }
        }

        public List<SimuladoOnlineRespostasAlunoDTO> SimuladoOnlineRespostasAluno(int intSimuladoId, int intClientId)
        {
            
            using (var ctx = new AcademicoContext())
            {
                DateTime? dteInicioConsultaRanking;
                var simuladoOnlineExcecao = (
                    from simEx in ctx.tblSimuladoOnline_Excecao
                    where simEx.intClientID == intClientId
                        && simEx.intSimuladoID == intSimuladoId
                    orderby simEx.intSimuladoExcecaoID descending
                    select simEx
                ).FirstOrDefault();

                if (simuladoOnlineExcecao != null)
                {
                    dteInicioConsultaRanking = simuladoOnlineExcecao.dteInicioConsultaRanking;
                }
                else
                {
                    dteInicioConsultaRanking = (from sim in ctx.tblSimulado where sim.intSimuladoID == intSimuladoId select sim.dteInicioConsultaRanking).FirstOrDefault();
                }

                var intHistoricoExercicioID = (
                    from eh in ctx.tblExercicio_Historico
                    where eh.intClientID == intClientId
                        && (eh.bitPresencial == true || eh.bitRealizadoOnline == true)
                        && eh.intExercicioID == intSimuladoId
                    orderby eh.dteDateInicio
                    select eh.intHistoricoExercicioID
                ).FirstOrDefault();

                var queryRespostas = (
                    from sv in ctx.tblSimuladoVersao
                    join eh in ctx.tblExercicio_Historico on new { intSimuladoID = sv.intSimuladoID, intExercicioTipo = (int)Exercicio.tipoExercicio.SIMULADO, intVersaoID = sv.intVersaoID } equals new { intSimuladoID = eh.intExercicioID, intExercicioTipo = eh.intExercicioTipo, intVersaoID = eh.intVersaoID }
                    join qa in ctx.tblQuestaoAlternativas on sv.intQuestaoID equals qa.intQuestaoID
                    from co in ctx.tblCartaoResposta_objetiva_Simulado_Online
                    where
                        sv.intQuestaoID == co.intQuestaoID
                        && eh.intHistoricoExercicioID == co.intHistoricoExercicioID
                        && (co.dteCadastro ?? new DateTime(9999, 12, 31)) < dteInicioConsultaRanking
                    where sv.intSimuladoID == intSimuladoId
                        && eh.intHistoricoExercicioID == intHistoricoExercicioID
                        && eh.intClientID == intClientId
                        && qa.bitCorreta == true
                    group new { sv, co } by new { eh.intClientID, sv.intSimuladoID, sv.intVersaoID, sv.intQuestao } into g
                    select new
                    {
                        intClientID = g.Key.intClientID,
                        intSimuladoID = g.Key.intSimuladoID,
                        intVersaoID = g.Key.intVersaoID,
                        intQuestao = g.Key.intQuestao,
                        intID = g.Max(x => x.co.intID),
                        dteCadastro = g.Max(x => x.co.dteCadastro)
                    });

                return (
                    from resp in queryRespostas
                    join cartao in ctx.tblCartaoResposta_objetiva_Simulado_Online on resp.intID equals cartao.intID
                    select new SimuladoOnlineRespostasAlunoDTO()
                    {
                        IntClientID = resp.intClientID,
                        IntSimuladoID = resp.intSimuladoID,
                        IntVersaoID = resp.intVersaoID,
                        IntQuestao = resp.intQuestao,
                        TxtLetraResposta = (cartao.txtLetraAlternativa ?? "?")
                    }).Distinct().OrderBy(x => x.IntQuestao).ToList();
            }
        }

        public void InserirLogSimuladoAlunoTurma(tblLogSimuladoAlunoTurma log)
        {
            var sql = String.Format(@"
                INSERT INTO tblLogSimuladoAlunoTurma(
                    intSimuladoID,
                    intClientID,
                    intOrderID,
                    txtUnidade,
                    intState,
                    txtTurma,
                    txtEspecialidade
                ) VALUES (
                    {0},
                    {1},
                    {2},
                    '{3}',
                    {4},
                    '{5}',
                    '{6}'
                )",
                log.intSimuladoID,
                log.intClientID,
                log.intOrderID,
                log.txtUnidade,
                log.intState,
                log.txtTurma,
                log.txtEspecialidade
            );
            new DBQuery().ExecuteQuery(sql, Utilidades.GetChaveamento());
        }

        public bool SimuladoOnlineSetRespostasObjetivas(List<SimuladoOnlineRespostasAlunoDTO> listaRespostasNovas, int intSimuladoId, int intClientId, int intArquivoID)
        {
            bool sucesso = false;

            using (var ctx = new AcademicoContext())
            {
                var listaRespostasAntigas = ctx.tblSimuladoRespostas
                    .Where(x => x.intSimuladoID == intSimuladoId && x.intClientID == intClientId)
                    .ToList();

                if (listaRespostasAntigas.Count > 0)
                {
                    foreach (var respostaAntigas in listaRespostasAntigas)
                    {
                        ctx.tblSimuladoRespostas.Remove(respostaAntigas);
                    }
                    ctx.SaveChanges();
                }

                foreach (var respostaNova in listaRespostasNovas)
                {
                    ctx.tblSimuladoRespostas.Add(new tblSimuladoRespostas()
                    {
                        intClientID = respostaNova.IntClientID,
                        intSimuladoID = respostaNova.IntSimuladoID,
                        intVersaoID = respostaNova.IntVersaoID,
                        intQuestao = respostaNova.IntQuestao,
                        txtLetraResposta = respostaNova.TxtLetraResposta,
                        intArquivoID = intArquivoID
                    });
                }
                ctx.SaveChanges();

                sucesso = true;
            }

            return sucesso;
        }

        public bool SimuladoOnlineSetResultadosObjetivos(int intSimuladoId, int intClientId)
        {
            throw new NotImplementedException();
            // bool sucesso = false;
            // using (var ctx = new AcademicoContext())
            // {
            //     var listaResultadosAntigos = ctx.tblSimuladoResultados
            //         .Where(x => x.intSimuladoID == intSimuladoId && x.intClientID == intClientId)
            //         .ToList();

            //     if (listaResultadosAntigos.Count > 0)
            //     {
            //         foreach (var resultadosAntigos in listaResultadosAntigos)
            //         {
            //             ctx.tblSimuladoResultados.Remove(resultadosAntigos);
            //         }
            //         ctx.SaveChanges();
            //     }

            //     var totalQuestoesAnuladas = (
            //         from q in ctx.tblQuestao_Simulado
            //         where q.bitAnulada == true && q.intSimuladoID == intSimuladoId
            //         select q
            //     ).Count();

            //     var listaResultadoNovo = (
            //         from sim in ctx.tblSimulado
            //         join qs in ctx.tblQuestao_Simulado on sim.intSimuladoID equals qs.intSimuladoID
            //         join questao in ctx.tblQuestoes on qs.intQuestaoID equals questao.intQuestaoID
            //         join alternativa in ctx.tblQuestaoAlternativas on questao.intQuestaoID equals alternativa.intQuestaoID
            //         join versao in ctx.tblSimuladoVersao on new { sim.intSimuladoID, questao.intQuestaoID } equals new { versao.intSimuladoID, versao.intQuestaoID }
            //         join resp in ctx.tblSimuladoRespostas on new { versao.intSimuladoID, versao.intVersaoID, versao.intQuestao } equals new { resp.intSimuladoID, resp.intVersaoID, resp.intQuestao }
            //         where
            //             resp.txtLetraResposta == alternativa.txtLetraAlternativa
            //             && alternativa.bitCorreta == true
            //             && sim.intSimuladoID == intSimuladoId
            //             && resp.intClientID == intClientId
            //             && qs.bitAnulada == false
            //         group questao.intQuestaoID by new { resp.intClientID, sim.intSimuladoID, versao.intVersaoID, resp.intArquivoID } into g
            //         select new
            //         {
            //             g.Key.intClientID,
            //             g.Key.intSimuladoID,
            //             g.Key.intVersaoID,
            //             g.Key.intArquivoID,
            //             intAcertos = g.Count() + totalQuestoesAnuladas
            //         }
            //     ).OrderByDescending(x => x.intAcertos)
            //     .ToList();

            //     foreach (var resultadoNovo in listaResultadoNovo)
            //     {
            //         ctx.tblSimuladoResultados.Add(new RDS_tblSimuladoResultados()
            //         {
            //             intClientID = resultadoNovo.intClientID,
            //             intSimuladoID = resultadoNovo.intSimuladoID,
            //             intVersaoID = resultadoNovo.intVersaoID,
            //             intArquivoID = resultadoNovo.intArquivoID,
            //             intAcertos = resultadoNovo.intAcertos
            //         });
            //     }

            //     ctx.SaveChanges();
            //     sucesso = true;
            // }
            // return sucesso;
        }

        public void ReplicarSimuladoOnlineTabelasMGE(int clientId, int simuladoId)
        {
            var txtNomeArquivo = "MsPro Automático único aluno";

            var dadosLogSimulado = GetDadosAlunoTurmaComCache(clientId);
            if (dadosLogSimulado != null)
            {
                var respostasAluno = dadosLogSimulado.IsPerson && !dadosLogSimulado.IsEmploye ?
                    SimuladoOnlineRespostasAluno(simuladoId, clientId) :
                    new List<SimuladoOnlineRespostasAlunoDTO>();

                using (var ctx = new AcademicoContext())
                {
                    //gerando id de arquivo
                    var logLeituraCartaoRespostaSimulados = new tblLogLeituraCartaoRespostaSimulados()
                    {
                        intEmployeeID = 131220,
                        dteDate = DateTime.Now,
                        txtSimuladoID = simuladoId.ToString(),
                        txtNomeArquivo = txtNomeArquivo,
                        bitInicioImp = true,
                        bitFimImp = true,
                        intQtdLidos = 1,
                        intClassRoomID = dadosLogSimulado.IntPrincipalClassRoomID,
                        dteFimImportacao = DateTime.Now
                    };
                    ctx.tblLogLeituraCartaoRespostaSimulados.Add(logLeituraCartaoRespostaSimulados);
                    ctx.SaveChanges();
                    var intArquivoID = logLeituraCartaoRespostaSimulados.intLogID;

                    var txtResposta = string.Join("", respostasAluno.Select(x => x.TxtLetraResposta).ToArray());
                    var intVersaoID = respostasAluno.Select(x => x.IntVersaoID).FirstOrDefault();

                    var exiteLog = ctx.tblLogSimuladoAlunoTurma
                        .Where(x => x.intClientID == clientId && x.intSimuladoID == simuladoId)
                        .Any();

                    // Gravando o CartaoResposta
                    if (!exiteLog)
                    {
                        var listaLog = new List<AlunoTurmaDTO>() { dadosLogSimulado };
                        var especialidades = ctx.tblEspecialidades.Where(x => x.intEspecialidadeID == dadosLogSimulado.IntEspecialidadeID).ToList();
                        var listaLogSimuladoAlunoTurma = (
                            from log in listaLog
                            join es in especialidades on log.IntEspecialidadeID equals es.intEspecialidadeID
                            select new tblLogSimuladoAlunoTurma()
                            {
                                intSimuladoID = simuladoId,
                                intClientID = log.IntClientID,
                                intOrderID = log.IntOrderID,
                                txtUnidade = log.TxtStoreName.Replace("Medcurso ", string.Empty).Replace("Medicine ", string.Empty),
                                intState = log.IntCitState ?? -1,
                                txtTurma = log.TxtCourseName,
                                txtEspecialidade = es.DE_ESPECIALIDADE
                            }
                        ).ToList();

                        foreach (var log in listaLogSimuladoAlunoTurma)
                        {
                            InserirLogSimuladoAlunoTurma(log);
                        }
                    }

                    var txtTurma = ctx.tblLogSimuladoAlunoTurma
                        .Where(x => x.intClientID == clientId && x.intSimuladoID == simuladoId)
                        .Select(x => x.txtTurma)
                        .FirstOrDefault();

                    var simuladoImportacaoCartaoResposta = new tblSimuladoImportacaoCartaoResposta()
                    {
                        txtClientID = clientId.ToString(),
                        txtSimuladoID = simuladoId.ToString(),
                        txtColVazia = "?",
                        txtVersaoID = intVersaoID.ToString(),
                        txtResposta = txtResposta,
                        intArquivoID = intArquivoID,
                        txtTurma = txtTurma
                    };
                    ctx.tblSimuladoImportacaoCartaoResposta.Add(simuladoImportacaoCartaoResposta);
                    ctx.SaveChanges();
                    var idImportacao = simuladoImportacaoCartaoResposta.intLeituraID;

                    // Setando As Respostas
                    SimuladoOnlineSetRespostasObjetivas(respostasAluno, simuladoId, clientId, intArquivoID);

                    // Gerando o gabarito
                    SimuladoOnlineSetResultadosObjetivos(simuladoId, clientId);
                }
            }
        }

        public void InserirQuestoesSimulado(List<CartaoRespostaObjetivaDTO> questoes, int intClientId)
        {
            using (var ctx = new AcademicoContext())
            {
                foreach (var questao in questoes)
                {
                    var item = new tblCartaoResposta_objetiva()
                    {
                        intQuestaoID = questao.intQuestaoID,
                        intHistoricoExercicioID = questao.intHistoricoExercicioID,
                        txtLetraAlternativa = questao.txtLetraAlternativa,
                        guidQuestao = questao.guidQuestao,
                        intExercicioTipoId = questao.intExercicioTipoId,
                        dteCadastro = questao.dteCadastro,
                        intClientID = intClientId
                    };

                    ctx.tblCartaoResposta_objetiva.Add(item);
                }

                ctx.SaveChanges();
            }
        }

        public void InserirQuestoesSimuladoOnline(List<CartaoRespostaObjetivaDTO> questoes)
        {
            throw new NotImplementedException();
            // using (var ctx = new Medgrupo.DataAccessEntity.Academico.AcademicoContext())
            // {
            //     foreach (var questao in questoes)
            //     {
            //         var item = new Academico.RDS_tblCartaoResposta_objetiva_Simulado_Online()
            //         {
            //             intQuestaoID = questao.intQuestaoID,
            //             intHistoricoExercicioID = questao.intHistoricoExercicioID,
            //             txtLetraAlternativa = questao.txtLetraAlternativa,
            //             guidQuestao = questao.guidQuestao,
            //             intExercicioTipoId = questao.intExercicioTipoId,
            //             dteCadastro = questao.dteCadastro
            //         };

            //         ctx.tblCartaoResposta_objetiva_Simulado_Online.Add(item);
            //     }

            //     ctx.SaveChanges();
            // }
        }

        public bool AlunoJaRealizouSimuladoOnline(int clientId, int simuladoId)
        {
            var jaRealizou = false;

            using (var ctx = new AcademicoContext())
            {
                jaRealizou = ctx.tblExercicio_Historico.Any(e => e.intClientID == clientId
                            && e.intExercicioTipo == (int)Exercicio.tipoExercicio.SIMULADO
                            && e.intExercicioID == simuladoId
                            && (e.bitRealizadoOnline.HasValue && e.bitRealizadoOnline.Value)
                            && e.dteDateFim != null);

            }

            return jaRealizou;
        }

        public Apostila GetApostilaConfiguracaoCache(int idEntidade, int matricula, int idAplicacao)
        {
            try
            {
                using(MiniProfiler.Current.Step("Obter configuração da apostila"))
                {
                    var cacheDesabilitadoCountQuestoes = RedisCacheManager.CannotCache(RedisCacheConstants.Questao.keyCountQuestoes);
                    var cacheDesabilitadoCountQuestoesDiscursivas = RedisCacheManager.CannotCache(RedisCacheConstants.Questao.keyCountQuestoesDiscursivas);
                    var cacheDesabilitadoNomeApostila = RedisCacheManager.CannotCache(RedisCacheConstants.Questao.KeyNomeApostila);

                    if (cacheDesabilitadoCountQuestoes || cacheDesabilitadoCountQuestoesDiscursivas || cacheDesabilitadoNomeApostila)
                        return GetApostilaConfiguracao(idEntidade, matricula, idAplicacao);

                    using (var ctx = new DesenvContext())
                    {
                        var keycountQuestoes = String.Format("{0}:{1}", RedisCacheConstants.Questao.keyCountQuestoes, idEntidade);
                        var countQuestoes = RedisCacheManager.GetItemObject<int>(keycountQuestoes);

                        var keycountQuestoesDiscursivas = String.Format("{0}:{1}", RedisCacheConstants.Questao.keyCountQuestoesDiscursivas, idEntidade);
                        var countQuestoesDiscursivas = RedisCacheManager.GetItemObject<int>(keycountQuestoesDiscursivas);

                        if (countQuestoes == 0 || countQuestoesDiscursivas == 0)
                        {
                            var ppQuestoes = new QuestaoEntity().GetQuestoesComComentarioApostila(idEntidade);

                            var questoesOrdenadas = new PortalProfessorEntity().OrdenarQuestoes(ppQuestoes);

                            var idsPPQuestoes = questoesOrdenadas.Select(p => p.Id).ToList();

                            // _________________________________ QUESTÕES SOMENTE IMPRESSAS, COM OU SEM VÍDEO
                            var questoesImpressas = (from q in ctx.tblConcursoQuestoes
                                                    join cqca in ctx.tblConcursoQuestao_Classificacao_Autorizacao on q.intQuestaoID equals cqca.intQuestaoID
                                                    join al in ctx.tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada on cqca.intMaterialID equals al.intProductID
                                                    join b in ctx.tblBooks on cqca.intMaterialID equals b.intBookID
                                                    join be in ctx.tblBooks_Entities on b.intBookEntityID equals be.intID
                                                    where be.intID == idEntidade
                                                        && (bool)cqca.bitAutorizacao.Value
                                                        && al.bitActive
                                                        && idsPPQuestoes.Contains(q.intQuestaoID)
                                                    select new Questao()
                                                    {
                                                        Id = q.intQuestaoID,
                                                        Tipo = q.bitDiscursiva == true ? 2 : 1
                                                    }).Distinct().ToList();

                            var idsQuestoesImpressas = questoesImpressas.Select(a => a.Id).ToList();

                            // _________________________________ QUESTÕES COM VÍDEOS
                            var questoesVideos = (from q in ctx.tblConcursoQuestoes
                                                join v in ctx.tblVideo_Questao_Concurso on q.intQuestaoID equals v.intQuestaoID
                                                where idsPPQuestoes.Contains(q.intQuestaoID)
                                                        && !idsQuestoesImpressas.Contains(q.intQuestaoID)
                                                select new Questao()
                                                {
                                                    Id = q.intQuestaoID,
                                                    Tipo = q.bitDiscursiva == true ? 2 : 1
                                                }).Distinct().ToList();

                            // _________________________________ UNION QUESTÕES
                            var questoes = questoesImpressas.Union(questoesVideos).Distinct().ToList();

                            countQuestoesDiscursivas = questoes.Where(a => a.Tipo == 2).Count();

                            countQuestoes = questoes.Where(a => a.Tipo == 1).Count();

                            RedisCacheManager.SetItemObject(keycountQuestoes, countQuestoes);
                            RedisCacheManager.SetItemObject(keycountQuestoesDiscursivas, countQuestoesDiscursivas);
                        }

                        var key = String.Format("{0}:{1}", RedisCacheConstants.Questao.KeyNomeApostila, idEntidade);
                        var nomeApostila = RedisCacheManager.GetItemString(key);

                        if (string.IsNullOrEmpty(nomeApostila))
                        {
                            nomeApostila = (from b in ctx.tblBooks
                                            join p in ctx.tblProducts
                                            on b.intBookID equals p.intProductID
                                            where b.intBookEntityID == idEntidade
                                            orderby b.intYear descending
                                            select p.txtCode.Remove(0, 5)
                                            ).FirstOrDefault().Trim();

                            RedisCacheManager.SetItemString(key, nomeApostila);
                        }

                        using (var ctxAcad = new AcademicoContext())
                        {
                            var historico = new tblExercicio_Historico
                            {
                                intApplicationID = idAplicacao,
                                intClientID = matricula,
                                intExercicioID = idEntidade,
                                intExercicioTipo = (int)Exercicio.tipoExercicio.APOSTILA,
                                dteDateInicio = DateTime.Now
                            };

                            ctxAcad.tblExercicio_Historico.Add(historico);
                            ctxAcad.SaveChanges();

                            var idHistorico = historico.intHistoricoExercicioID;

                            var exercicio = new Apostila
                            {
                                IdExercicio = idEntidade,
                                Nome = nomeApostila.Replace(" - MATERIAL VIRTUAL", string.Empty).ToUpper(),
                                QtdQuestoes = countQuestoes,
                                QtdQuestoesDiscursivas = countQuestoesDiscursivas,
                                CartoesResposta = new CartoesResposta { HistoricoId = idHistorico },
                                PermissaoProva = new PermissaoProva { ComentarioTexto = 1, ComentarioVideo = 1, Estatística = 1, Favorito = 1, Gabarito = 1, Recursos = 1, Cronometro = 0 }
                            };

                            return exercicio;
                        }
                    }
                }
            }
            catch
            {
                return GetApostilaConfiguracao(idEntidade, matricula, idAplicacao);
            }
        }

        public Apostila GetApostilaConfiguracao(int idEntidade, int matricula, int idAplicacao)
        {
            try
            {
                using (var ctx = new AcademicoContext())
                {
                    using (var ctxMatDir = new DesenvContext())
                    {
                        var ppQuestoes = new QuestaoEntity().GetQuestoesComComentarioApostila(idEntidade);

                        var questoesOrdenadas = new PortalProfessorEntity().OrdenarQuestoes(ppQuestoes);

                        var idsPPQuestoes = questoesOrdenadas.Select(p => p.Id).ToList();

                        // _________________________________ QUESTÕES SOMENTE IMPRESSAS, COM OU SEM VÍDEO
                        var questoesImpressas = (from q in ctxMatDir.tblConcursoQuestoes
                                                 join cqca in ctxMatDir.tblConcursoQuestao_Classificacao_Autorizacao on q.intQuestaoID equals cqca.intQuestaoID
                                                 join al in ctxMatDir.tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada on cqca.intMaterialID equals al.intProductID
                                                 join b in ctxMatDir.tblBooks on cqca.intMaterialID equals b.intBookID
                                                 join be in ctxMatDir.tblBooks_Entities on b.intBookEntityID equals be.intID
                                                 where be.intID == idEntidade
                                                       && (bool)cqca.bitAutorizacao.Value
                                                       && al.bitActive
                                                       && idsPPQuestoes.Contains(q.intQuestaoID)
                                                 select new Questao()
                                                 {
                                                     Id = q.intQuestaoID,
                                                     Tipo = q.bitDiscursiva == true ? 2 : 1
                                                 }).Distinct().ToList();

                        var idsQuestoesImpressas = questoesImpressas.Select(a => a.Id).ToList();

                        // _________________________________ QUESTÕES COM VÍDEOS
                        var questoesVideos = (from q in ctxMatDir.tblConcursoQuestoes
                                              join v in ctxMatDir.tblVideo_Questao_Concurso on q.intQuestaoID equals v.intQuestaoID
                                              where idsPPQuestoes.Contains(q.intQuestaoID)
                                                    && !idsQuestoesImpressas.Contains(q.intQuestaoID)
                                              select new Questao()
                                              {
                                                  Id = q.intQuestaoID,
                                                  Tipo = q.bitDiscursiva == true ? 2 : 1
                                              }).Distinct().ToList();

                        // _________________________________ UNION QUESTÕES
                        var questoes = questoesImpressas.Union(questoesVideos).Distinct().ToList();

                        var countQuestoesDiscursivas = questoes.Where(a => a.Tipo == 2).Count();

                        var countQuestoes = questoes.Where(a => a.Tipo == 1).Count();

                        var nomeApostila = (from b in ctxMatDir.tblBooks
                                            join p in ctxMatDir.tblProducts
                                            on b.intBookID equals p.intProductID
                                            where b.intBookEntityID == idEntidade
                                            orderby b.intYear descending
                                            select p.txtCode.Remove(0, 5)
                                            ).FirstOrDefault().Trim();


                        var historico = new tblExercicio_Historico
                        {
                            intApplicationID = idAplicacao,
                            intClientID = matricula,
                            intExercicioID = idEntidade,
                            intExercicioTipo = (int)Exercicio.tipoExercicio.APOSTILA,
                            dteDateInicio = DateTime.Now
                        };

                        ctx.tblExercicio_Historico.Add(historico);
                        ctx.SaveChanges();

                        var idHistorico = historico.intHistoricoExercicioID;

                        var exercicio = new Apostila
                        {
                            IdExercicio = idEntidade,
                            Nome = nomeApostila.Replace(" - MATERIAL VIRTUAL", string.Empty).ToUpper(),
                            QtdQuestoes = countQuestoes,
                            QtdQuestoesDiscursivas = countQuestoesDiscursivas,
                            CartoesResposta = new CartoesResposta { HistoricoId = idHistorico },
                            PermissaoProva = new PermissaoProva { ComentarioTexto = 1, ComentarioVideo = 1, Estatística = 1, Favorito = 1, Gabarito = 1, Recursos = 1, Cronometro = 0 }
                        };

                        return exercicio;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public Exercicio GetExercicio(int idExercicio)
        {
            using (var ctx = new DesenvContext())
            {
                var lista = GetConcursos();

                var listaConcursoBloqueados = ctx.tblBloqueioConcurso.Where(b => b.intBloqueioAreaId == (int)BloqueioConcursoArea.PortalProfessor).Select(b => b.intProvaId).ToList();

                var lst = lista.Where(l => !listaConcursoBloqueados.Contains(l.ID)).ToList();

                var ret = lst.Where(a => a.ID == idExercicio).FirstOrDefault();

                return ret;
            }
        }

        public Exercicio GetSimuladoOnlineCorrente()
        {
            Exercicio simulado = null;
            using (var ctx = new AcademicoContext())
            {
                var simu = ctx.tblSimulado.Where(s => s.bitOnline == true && s.dteDataHoraInicioWEB <= DateTime.Now && s.dteDataHoraTerminoWEB >= DateTime.Now).FirstOrDefault();

                if (simu != null)
                {
                    simulado = new Exercicio();
                    simulado.ID = simu.intSimuladoID;
                }
            }
            return simulado;
        }

        public List<int> GetExerciciosSimuladoNaoFinalizados(int simuladoId)
        {
            throw new NotImplementedException();
            // List<int> exercicios;

            // using (var ctx = new AcademicoContext())
            // {
            //     var idsExercicios = from eh in ctx.tblExercicio_Historico
            //                         join sim in ctx.tblSimulado on eh.intExercicioID equals sim.intSimuladoID
            //                         where
            //                         sim.bitOnline == true
            //                         && sim.intSimuladoID == simuladoId
            //                         && eh.dteDateFim == null
            //                         && eh.dteDateInicio >= sim.dteDataHoraInicioWEB
            //                         && eh.dteDateInicio <= sim.dteDataHoraTerminoWEB
            //                         select eh.intHistoricoExercicioID;

            //     exercicios = new List<int>(idsExercicios.ToList());
            // }
            // return exercicios;
        }

        public List<int> GetExerciciosSimuladoNaoFinalizadosAlunosExcecao(int simuladoId)
        {
            throw new NotImplementedException();
            // List<int> exercicios;

            // using (var ctx = new AcademicoContext())
            // {
            //     var idsExercicios = from eh in ctx.tblExercicio_Historico
            //                         join sim in ctx.tblSimulado on eh.intExercicioID equals sim.intSimuladoID
            //                         join simExc in ctx.tblSimuladoOnline_Excecao on sim.intSimuladoID equals simExc.intSimuladoID
            //                         where
            //                         sim.bitOnline == true
            //                         && sim.intSimuladoID == simuladoId
            //                         && simExc.intSimuladoID == simuladoId
            //                         && eh.dteDateInicio >= simExc.dteDataHoraInicioWEB
            //                         && eh.dteDateInicio <= simExc.dteDataHoraTerminoWEB
            //                         && eh.dteDateFim == null
            //                         select eh.intHistoricoExercicioID;

            //     exercicios = new List<int>(idsExercicios.ToList());
            // }
            // return exercicios;
        }

        public Exercicio GetUltimoSimuladoAgendadoComGabaritoLiberado()
        {
            throw new NotImplementedException();
            // Exercicio simulado = null;
            // using (var ctx = new AcademicoContext())
            // {
            //     var simulados = ctx.tblSimulado.Where(s => s.bitOnline == true
            //         && s.dteDataHoraTerminoWEB < DateTime.Now //ja terminou
            //         && s.dteReleaseGabarito != null
            //         //).OrderByDescending(x => new { x.dteReleaseGabarito, x.intSimuladoID } ).ToList();
            //         ).OrderByDescending(x => new { x.intSimuladoID }).ToList();

            //     var simu = simulados.FirstOrDefault();


            //     if (simu != null)
            //     {
            //         simulado = new Exercicio();
            //         simulado.ID = simu.intSimuladoID;
            //     }
            // }
            // return simulado;
        }

        public List<ExercicioHistoricoDTO> ObterExerciciosEmAndamento(int matricula, int idAplicacao)
        {
            var retorno = new List<ExercicioHistoricoDTO>();

            using (var ctx = new AcademicoContext())
            {
                retorno = ctx.tblExercicio_Historico.Where(x => x.intClientID == matricula
                && (x.intTipoProva == (int)TipoProvaEnum.ModoOnline || x.intTipoProva == (int)TipoProvaEnum.ModoProva)
                && !x.dteDateFim.HasValue
                && x.intApplicationID == idAplicacao
                )
                .Select(x => new ExercicioHistoricoDTO()
                {
                    intHistoricoExercicioID = x.intHistoricoExercicioID,
                    intExercicioID = x.intExercicioID,
                    intExercicioTipo = x.intExercicioTipo,
                    dteDateInicio = x.dteDateInicio,
                    dteDateFim = x.dteDateFim,
                    bitRanqueado = x.bitRanqueado,
                    intTempoExcedido = x.intTempoExcedido,
                    intClientID = x.intClientID,
                    intApplicationID = x.intApplicationID,
                    bitRealizadoOnline = x.bitRealizadoOnline,
                    bitPresencial = x.bitPresencial,
                    intVersaoID = x.intVersaoID,
                    intTipoProva = x.intTipoProva
                }).ToList();
            }

            return retorno;
        }

        [Obsolete]
        public List<int> ListarHistoricoExercicioIdAbertos(DateTime dataReferencia, int tolerancia)
        {
            throw new NotImplementedException();
            // using (var ctx = new AcademicoContext())
            // {
            //     var qry = (
            //         from hist in ctx.tblExercicio_Historico
            //         join sim in ctx.tblSimulado
            //             on hist.intExercicioID equals sim.intSimuladoID
            //         join simExc in ctx.tblSimuladoOnline_Excecao
            //             on new { sim.intSimuladoID, hist.intClientID } equals new { simExc.intSimuladoID, simExc.intClientID } into lj
            //         from simExc in lj.DefaultIfEmpty()
            //         where
            //             (simExc.dteInicioConsultaRanking ?? sim.dteInicioConsultaRanking) >= dataReferencia
            //             && hist.intExercicioTipo == (int)Exercicio.tipoExercicio.SIMULADO
            //             && hist.bitRealizadoOnline.HasValue
            //             && hist.bitRealizadoOnline.Value
            //             && hist.dteDateFim == null
            //             && EntityFunctions.AddMinutes(hist.dteDateInicio, sim.intDuracaoSimulado + tolerancia) < DateTime.Now
            //         orderby hist.intHistoricoExercicioID
            //         select hist.intHistoricoExercicioID
            //     );

            //     return qry.Distinct().ToList();
            // }
        }

        public List<int> ListarHistoricoExercicioIdAbertos(int tolerancia, List<int> SimuladoVigenteIds)
        {
            throw new NotImplementedException();
            // using (var ctx = new AcademicoContext())
            // {
            //     var qry = (
            //         from hist in ctx.tblExercicio_Historico
            //         join sim in ctx.tblSimulado
            //             on hist.intExercicioID equals sim.intSimuladoID
            //         join simExc in ctx.tblSimuladoOnline_Excecao
            //             on new { sim.intSimuladoID, hist.intClientID } equals new { simExc.intSimuladoID, simExc.intClientID } into lj
            //         from simExc in lj.DefaultIfEmpty()
            //         where
            //             SimuladoVigenteIds.Contains(sim.intSimuladoID)
            //             && hist.intExercicioTipo == (int)Exercicio.tipoExercicio.SIMULADO
            //             && hist.bitRealizadoOnline.HasValue
            //             && hist.bitRealizadoOnline.Value
            //             && hist.dteDateFim == null
            //             && EntityFunctions.AddMinutes(hist.dteDateInicio, sim.intDuracaoSimulado + tolerancia) < DateTime.Now
            //         orderby hist.intHistoricoExercicioID
            //         select hist.intHistoricoExercicioID
            //     );

            //     return qry.Distinct().ToList();
            // }
        }

        public List<SimuladoDTO> ListarIdsSimuladoVigente()
        {
            throw new NotImplementedException();
            // using (var ctx = new AcademicoContext())
            // {
            //     var data = DateTime.Now;
            //     var simuladoIds = (from s in ctx.tblSimulado
            //                        where data >= s.dteDataHoraInicioWEB
            //                          && data <= s.dteInicioConsultaRanking
            //                          && s.intTipoSimuladoID != (int)SimuladoTipos.CP_MED
            //                        select new SimuladoDTO
            //                        {
            //                            ID = s.intSimuladoID,
            //                            ExercicioName = s.txtSimuladoName,
            //                            DataInicio = s.dteDataHoraInicioWEB,
            //                            DataFim = s.dteDataHoraTerminoWEB,
            //                            DtLiberacaoRanking = s.dteInicioConsultaRanking,
            //                            Duracao = s.intDuracaoSimulado
            //                        }).ToList();

            //     return simuladoIds;
            // }
        }

        public int GetIdEspecialidade(int matricula)
        {
            throw new NotImplementedException();
            // using (var ctx = new DesenvContext())
            // {
            //     return (from e in ctx.tblClients
            //             where e.intClientID == matricula && e.intEspecialidadeID.HasValue
            //             select e.intEspecialidadeID.Value)
            //             .FirstOrDefault();
            // }
        }

        public int[] FiltrarIdsProvasDiscursivas(params int[] idsProva)
        {
            throw new NotImplementedException();
            // using (var ctx = new DesenvContext())
            // {
            //     return (from p in ctx.tblConcurso_Provas
            //             join t in ctx.tblConcurso_Provas_Tipos on p.intProvaTipoID equals t.intProvaTipoID
            //             where t.bitDiscursiva == true && idsProva.Contains(p.intProvaID)
            //             select p.intProvaID).ToArray();
            // }
        }

        public List<ProvaRecursoConcursoDTO> GetProvasConcursos(int ano, int matricula, bool modoRMais)
        {
            throw new NotImplementedException();
            // var tipoProvaDiscursiva = ProvaRecurso.TipoProva.Discursiva.GetDescription();
            // var statusProva = GetStatusPermitidosProvaRecursos();
            // var statusProvaInt = Array.ConvertAll(statusProva, value => (int)value);

            // using (var ctx = new DesenvContext())
            // {
            //     return (from c in ctx.tblConcursoes
            //             join p in ctx.tblConcurso_Provas on c.ID_CONCURSO equals p.ID_CONCURSO
            //             join t in ctx.tblConcurso_Provas_Tipos on p.intProvaTipoID equals t.intProvaTipoID
            //             where c.VL_ANO_CONCURSO == ano
            //                     && ((!modoRMais && !t.txtDescription.Contains(Constants.R3) && !t.txtDescription.Contains(Constants.R4))
            //                         || ((modoRMais && (t.txtDescription.Contains(Constants.R3) || t.txtDescription.Contains(Constants.R4)))))
            //                     && statusProvaInt.Contains(p.ID_CONCURSO_RECURSO_STATUS)
            //             orderby c.SG_CONCURSO, c.CD_UF, t.IntOrder, p.ID_CONCURSO_RECURSO_STATUS ascending
            //             select new ProvaRecursoConcursoDTO
            //             {
            //                 Id = p.intProvaID,
            //                 IdConcurso = c.ID_CONCURSO,
            //                 Nome = c.SG_CONCURSO + Constants.DASH + c.CD_UF,
            //                 NomeCompleto = c.NM_CONCURSO,
            //                 Ano = c.VL_ANO_CONCURSO,
            //                 Liberado = ctx.tblConcursoQuestoes.Any(ql => ql.intProvaID == p.intProvaID),
            //                 IdStatus = p.ID_CONCURSO_RECURSO_STATUS,
            //                 UploadLiberado = p.bitUploadAluno.HasValue && p.bitUploadAluno.Value,
            //                 AlteradaBanca = (from q in ctx.tblConcursoQuestoes
            //                                  join a in ctx.tblConcursoQuestoes_Alternativas on q.intQuestaoID equals a.intQuestaoID
            //                                  where q.intProvaID == p.intProvaID && (q != null && a != null
            //                                      && ((p.ID_CONCURSO_RECURSO_STATUS == (int)ProvaRecurso.StatusProva.RecursosExpirados
            //                                             || p.ID_CONCURSO_RECURSO_STATUS == (int)ProvaRecurso.StatusProva.BloqueadoParaNovosRecursos)
            //                                      && (a.bitCorreta == true || q.bitGabaritoPosRecursoLiberado == true))
            //                                      || (q.bitAnuladaPosRecurso == true ||
            //                                      (a.bitCorreta == true && a.bitCorretaPreliminar == false)))
            //                                  select q.intQuestaoID).Any(),
            //                 Discursiva = p.txtTipoProva == tipoProvaDiscursiva,
            //                 TipoOrder = t.IntOrder ?? 0
            //             }).ToList();
            // }
        }

        public IDictionary<int, ProvaSubespecialidade> GetSubespecialidadesProvas(int ano, bool modoRMais)
        {
            throw new NotImplementedException();
            // var result = new Dictionary<int, ProvaSubespecialidade>();
            // var statusProva = GetStatusPermitidosProvaRecursos();
            // var statusProvaInt = Array.ConvertAll(statusProva, value => (int)value);

            // using (var ctx = new DesenvContext())
            // {
            //     var listaCompleta = (from c in ctx.tblConcursoes
            //                          join p in ctx.tblConcurso_Provas on c.ID_CONCURSO equals p.ID_CONCURSO
            //                          join t in ctx.tblConcurso_Provas_Tipos on p.intProvaTipoID equals t.intProvaTipoID
            //                          join e in ctx.tblConcursoProvaEspecialidadesRecurso on p.intProvaID equals e.intProvaID
            //                          join ep in ctx.tblEspecialidadeConcursoProvaRecurso on e.intEspecialidadeID equals ep.intEspecialidadeID
            //                          join te in ctx.tblConcurso_Provas_Tipos on ep.intTipoProvaID equals te.intProvaTipoID
            //                          where c.VL_ANO_CONCURSO == ano
            //                              && ((!modoRMais && !t.txtDescription.Contains(Constants.R3) && !t.txtDescription.Contains(Constants.R4))
            //                             || ((modoRMais && (t.txtDescription.Contains(Constants.R3) || t.txtDescription.Contains(Constants.R4)))))
            //                          select new
            //                          {
            //                              IdProva = p.intProvaID,
            //                              IdSubespecialidade = e.intEspecialidadeID,
            //                              GrandeArea = te.intProvaTipoID
            //                          }).ToList();

            //     if (listaCompleta != null && listaCompleta.Any())
            //     {
            //         using (var ctxAcad = new AcademicoContext())
            //         {
            //             var especialidades = (from e in ctxAcad.tblEspecialidades
            //                                   select e).ToList();

            //             if (especialidades != null && especialidades.Any())
            //             {
            //                 foreach (var dadosProva in listaCompleta)
            //                 {
            //                     var especAtual = especialidades.FirstOrDefault(e => e.intEspecialidadeID == dadosProva.IdSubespecialidade);

            //                     if (especAtual == null)
            //                     {
            //                         continue;
            //                     }

            //                     var naoInseriuProva = !result.Any(l => l.Key == dadosProva.IdProva);
            //                     if (naoInseriuProva)
            //                     {
            //                         var provaSubs = new ProvaSubespecialidade
            //                         {
            //                             GrandeAreasProva = new List<int>(),
            //                             Subespecialidades = new List<string>()
            //                         };
            //                         result.Add(dadosProva.IdProva, provaSubs);
            //                     }

            //                     var provaEspec = result.First(l => l.Key == dadosProva.IdProva).Value;
            //                     if (!provaEspec.GrandeAreasProva.Contains(dadosProva.GrandeArea))
            //                     {
            //                         provaEspec.GrandeAreasProva.Add(dadosProva.GrandeArea);
            //                     }

            //                     if (!provaEspec.Subespecialidades.Contains(especAtual.DE_ESPECIALIDADE))
            //                     {
            //                         provaEspec.Subespecialidades.Add(especAtual.DE_ESPECIALIDADE);
            //                     }
            //                 }
            //             }
            //         }
            //     }
            // }
            // return result;
        }

        private ProvaRecurso.StatusProva[] GetStatusPermitidosProvaRecursos()
        {
            return new ProvaRecurso.StatusProva[]
            {
                ProvaRecurso.StatusProva.RecursosEmAnalise,
                ProvaRecurso.StatusProva.Bloqueado,
                ProvaRecurso.StatusProva.RecursosPróximos,
                ProvaRecurso.StatusProva.RecursosExpirados,
                ProvaRecurso.StatusProva.BloqueadoParaNovosRecursos,
            };
        }

        public ProvaAlunosFavoritoDTO GetAlunosFavoritaramProva(int idProva)
        {
            throw new NotImplementedException();

            // var favoritaram = new ProvaAlunosFavoritoDTO();
            // using (var ctx = new DesenvContext())
            // {
            //     var result = (from c in ctx.tblConcursoes
            //                   join p in ctx.tblConcurso_Provas on c.ID_CONCURSO equals p.ID_CONCURSO
            //                   join f in ctx.tblConcursoRecursoFavoritadoes on p.intProvaID equals f.intConcursoId
            //                   where p.intProvaID == idProva
            //                   select new
            //                   {
            //                       NomeProva = c.SG_CONCURSO + Constants.DASH + c.CD_UF,
            //                       Matricula = f.intClienteId
            //                   }).ToList();

            //     if (result != null && result.Any())
            //     {
            //         favoritaram.MatriculasFavoritaram = result.Select(m => m.Matricula).ToList();
            //         favoritaram.Prova = new ProvaConcursoDTO
            //         {
            //             Nome = result.ElementAt(0).NomeProva
            //         };
            //     }
            //     return favoritaram;
            // }
        }

        public List<SimuladoCronogramaDTO> GetCronogramaSimulados(int ano, int matricula)
        {
            throw new NotImplementedException();
            // int[] idTemas;

            // using (var matDir = new DesenvContext())
            // {
            //     idTemas = (from mvc in matDir.mview_Cronograma
            //                join sod in matDir.tblSellOrderDetails on mvc.intCourseID equals sod.intProductID
            //                join so in matDir.tblSellOrders on sod.intOrderID equals so.intOrderID
            //                join c in matDir.tblClients on so.intClientID equals c.intClientID
            //                where so.intClientID == matricula
            //                && mvc.intYear == ano
            //                && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
            //                select mvc.intLessonTitleID).ToArray();
            // }

            // using (var ctx = new AcademicoContext())
            // {
            //     var simulados = (from s in ctx.tblSimulado
            //                      where s.intAno == ano
            //                      && idTemas.Contains(s.intLessonTitleID ?? 0)
            //                      && s.intTipoSimuladoID != (int)Constants.TipoSimulado.CPMED
            //                      select new SimuladoCronogramaDTO
            //                      {
            //                          SimuladoID = s.intSimuladoID,
            //                          Nome = s.txtSimuladoDescription,
            //                          DataInicioWeb = s.dteDataHoraInicioWEB,
            //                          DataFimWeb = s.dteDataHoraTerminoWEB
            //                      }).ToList();

            //     return simulados;
            // }
        }

        public int GetIDSimuladoCPMED(int ano)
        {
            throw new NotImplementedException();
            // try
            // {
            //     using (var ctx = new AcademicoContext())
            //     {
            //         var idSimulado = (from s in ctx.tblSimulado
            //                           where s.intTipoSimuladoID == (int)Constants.TipoSimulado.CPMED
            //                           && !s.txtSimuladoName.Contains("CPMED R")
            //                           && s.intAno == ano
            //                           select s.intSimuladoID).FirstOrDefault();

            //         return idSimulado;
            //     }
            // }
            // catch (Exception)
            // {
            //     throw;
            // }
        }
    }
}