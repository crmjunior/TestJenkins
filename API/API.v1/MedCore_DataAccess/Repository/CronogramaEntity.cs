using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Business;
using System.Linq;
using MedCore_DataAccess.Entidades;
using System;
using System.Threading.Tasks;
using MedCore_DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using MedCore_API.Academico;
using MedCore_DataAccess.DTO.Base;
using StackExchange.Profiling;
using Medgrupo.DataAccessEntity;
using MedCore_DataAccess.Business.Enums;

namespace MedCore_DataAccess.Repository
{
    public class CronogramaEntity : ICronogramaData
    {
        public List<msp_API_ListaEntidades_Result> GetListaEntidades(int idProduto, int ano, int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                
                var entidades = ctx.Set<msp_API_ListaEntidades_Result>().FromSqlRaw("msp_API_ListaEntidades @intProductGroup = {0}, @intYear = {1}, @matricula = {2}", idProduto, ano, matricula).ToList();
                return entidades;
            }
        }

        public int UltimoAnoCursadoAluno(int matricula, int idProduto)
        {
            var anoAtual = Utilidades.GetYear();
            var maiorAnoAluno = AnosProdutoAluno(matricula, idProduto).OrderByDescending(c => c).FirstOrDefault().GetValueOrDefault();

            return (maiorAnoAluno < anoAtual) ? maiorAnoAluno : anoAtual;

        }

        public List<EspecialRevalida> RevalidaCronogramaPermissao(List<EspecialRevalida> lsRevalida, int matriculaId)
        {
            try
            {
                List<int> permissoes = new RevalidaBusiness(new MednetEntity()).ObterTemasRevalidaPermitidos(Convert.ToInt32(matriculaId)).Retorno;

                foreach (EspecialRevalida semana in lsRevalida)
                {
                    semana.Ativa = Convert.ToInt32(permissoes.Any(x => x == Convert.ToInt32(semana.Numero)));
                }

                return lsRevalida;
            }
            catch
            { 
                return new List<EspecialRevalida>();
            }
        }

        public List<EspecialRevalida> GetRevalidaCronograma(int idProduto, int ano)
        {
            List<EspecialRevalida> lsRevalida = new List<EspecialRevalida>();

            using (var ctx = new DesenvContext())
            {
                using (var ctxAcad = new AcademicoContext())
                {
                    var queryRevalida = (from ri in ctx.tblRevalidaAulaIndice
                                         join rv in ctx.tblRevalidaAulaVideo on ri.intRevalidaAulaIndiceId equals rv.intRevalidaAulaIndiceId
                                         join lt in ctx.tblLessonTitleRevalida on ri.intLessonTitleRevalidaId equals lt.intLessonTitleRevalidaId
                                         orderby ri.intOrdem
                                         select new
                                         {
                                             TemaDescricao = lt.txtName,
                                             IdRevalidaIndice = ri.intRevalidaAulaIndiceId,
                                             IdTema = ri.intRevalidaAulaIndiceId,
                                             GrupoId = lt.GrupoId,
                                             EspecialidadeId = lt.intEspecialidadeId
                                         }
                    ).Distinct().ToList();

                    List<int?> listaEspecialidadeId = queryRevalida.Select(x => x.EspecialidadeId).ToList();

                    var queryEspecialidade = (from e in ctxAcad.tblEspecialidades
                                              where listaEspecialidadeId.Contains(e.intEspecialidadeID)
                                              select new
                                              {
                                                  EspecialidadeId = e.intEspecialidadeID,
                                                  EspecialidadeSigla = e.CD_ESPECIALIDADE
                                              }).ToList();

                    var query = (from r in queryRevalida
                                 join e in queryEspecialidade on r.EspecialidadeId equals e.EspecialidadeId
                                 select new
                                 {
                                     TemaDescricao = r.TemaDescricao,
                                     IdRevalidaIndice = r.IdRevalidaIndice,
                                     IdTema = r.IdTema,
                                     GrupoId = r.GrupoId,
                                     EspecialidadeId = e.EspecialidadeId,
                                     EspecialidadeSigla = e.EspecialidadeSigla
                                 }).Distinct().ToList();

                    var temasApostila = query.Select(tema => new TemaApostila()
                    {
                        Professores = new List<Pessoa>(),
                        Id = tema.IdRevalidaIndice,
                        IdTema = tema.IdTema,
                        Descricao = tema.TemaDescricao,
                        VideosRevisao = new VideosMednet(),
                        Apostila = new Exercicio
                        {
                            ID = tema.IdTema,
                            Descricao = tema.EspecialidadeId == 110 ? "GO" : tema.EspecialidadeSigla,
                            Especialidade = new Especialidade { Id = tema.EspecialidadeId }
                        },
                        Assunto = new AssuntoTemaApostila
                        {
                            Id = tema.GrupoId.Value,
                            Descricao = string.Concat("Especial Revalida ", tema.GrupoId)
                        }
                    })
                        .ToList();

                    var revalidaAgrupado = temasApostila.GroupBy(x => x.Assunto.Id).Select(grp => new { GrpID = grp.Key, RevalidaLst = grp.ToList() }).ToList();


                    foreach (var item in revalidaAgrupado)
                    {
                        var itemRevalida = new EspecialRevalida();
                        itemRevalida.Numero = item.GrpID;
                        itemRevalida.Ativa = 1;

                        var apostilas = item.RevalidaLst.Select(x => new Apostila
                        {
                            IdEntidade = x.Apostila.Especialidade.Id,
                            Nome = x.Apostila.Descricao,
                            PercentLido = 0,
                            Temas = new List<AulaTema> { new AulaTema { TemaID = x.IdTema } }
                        }
                        );

                        itemRevalida.Apostilas.AddRange(apostilas);

                        lsRevalida.Add(itemRevalida);
                    }

                    return lsRevalida;

                }
            }
        }

        public async Task<List<CronogramaPrateleiraDTO>> GetCronogramaPrateleirasAsync(int idProduto, int ano, int matricula, int menuId)
        {
            using (var ctx = new DesenvContext())
            {

                var cronogramaProduto = await (from cp in ctx.tblCronogramaPrateleira
                                               join cplt in ctx.tblCronogramaPrateleira_LessonTitles on cp.intID equals cplt.intPrateleiraCronogramaID
                                               join mc in ctx.mview_Cronograma on cplt.intLessonTitleID equals mc.intLessonTitleID
                                               join c in ctx.tblCourses on mc.intCourseID equals c.intCourseID
                                               join p in ctx.tblProducts on c.intCourseID equals p.intProductID
                                               join sod in ctx.tblSellOrderDetails on p.intProductID equals sod.intProductID
                                               join so in ctx.tblSellOrders on sod.intOrderID equals so.intOrderID
                                               join lm in ctx.tblLesson_Material on mc.intLessonID equals lm.intLessonID
                                               join b in ctx.tblBooks on lm.intMaterialID equals b.intBookID
                                               join be in ctx.tblBooks_Entities on b.intBookEntityID equals be.intID
                                               join lt in ctx.tblLessonTitles on mc.intLessonTitleID equals lt.intLessonTitleID
                                               join p2 in ctx.tblProducts on b.intBookID equals p2.intProductID
                                               where (c.intYear ?? 0) == ano
                                               && (b.intYear ?? 0) == ano
                                               && so.intClientID == matricula
                                               && p.intProductGroup2 == idProduto
                                               && p2.intProductGroup3 != null
                                               && cp.intMenuId == menuId
                                               && mc.intStoreID != null
                                               && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                               select new CronogramaPrateleiraDTO
                                               {
                                                   ID = cp.intID,
                                                   Descricao = cp.txtDescricao,
                                                   Ordem = cp.intOrdem,
                                                   EntidadeCodigo = p2.txtCode,
                                                   EntidadeID = be.intID,
                                                   Semana = lt.intSemana ?? 0,
                                                   Ano = b.intYear ?? 0,
                                                   Data = mc.dteDateTime,
                                                   LessonTitleID = lt.intLessonTitleID,
                                                   MaterialId = lm.intMaterialID,
                                                   EspecialidadeId = p2.intProductGroup3 ?? 0,
                                                   ExibeEspecialidade = cp.bitExibeEspecialidade,
                                                   ExibeConformeCronograma = cp.bitExibeConformeCronograma
                                               }).ToListAsync();

                return cronogramaProduto;
            }
        }

        public List<long> GetBookEntitiesBloqueadosNoCronograma()
		{
			using (var ctx = new DesenvContext())
			{
				var entidadesBloqueadas = ctx.tblCronogramaExcecoesEntidades.Select(x => x.intBookEntityId).ToList();

				return entidadesBloqueadas;
			}
		}

		public List<ApostilaCodigoDTO> GetCodigosAmigaveisApostilas()
        {
            using (var ctx = new DesenvContext())
            {
                var codigosAmigaveis = ctx.tblProductCodes.Select(x => new ApostilaCodigoDTO
                                                                          {
                                                                            ProdutoId = x.intProductId,
                                                                            TemaId = x.intLessonTitleID,
                                                                            Nome = x.txtCode
                                                                          }).ToList();
                return codigosAmigaveis;
            }
        }

        public List<CronogramaPrateleiraDTO> GetCronogramaPrateleirasCPMEDTurmaConvidada(int ano, int menuId, int turmaId)
        {
            using (var ctx = new DesenvContext())
            {
               
                var query = (from cp in ctx.tblCronogramaPrateleira
                             join cplt in ctx.tblCronogramaPrateleira_LessonTitles on cp.intID equals cplt.intPrateleiraCronogramaID
                             join mc in ctx.mview_Cronograma on cplt.intLessonTitleID equals mc.intLessonTitleID
                             join c in ctx.tblCourses on mc.intCourseID equals c.intCourseID
                             join p in ctx.tblProducts on c.intCourseID equals p.intProductID
                             join lm in ctx.tblLesson_Material on mc.intLessonID equals lm.intLessonID
                             join b in ctx.tblBooks on lm.intMaterialID equals b.intBookID
                             join be in ctx.tblBooks_Entities on b.intBookEntityID equals be.intID
                             join lt in ctx.tblLessonTitles on mc.intLessonTitleID equals lt.intLessonTitleID
                             join p2 in ctx.tblProducts on b.intBookID equals p2.intProductID
                             where (c.intYear ?? 0) == ano
                             && (b.intYear ?? 0) == ano
                             && p2.intProductGroup2 == (int)Produto.Cursos.MED
                             && p2.intProductGroup3 == (int)Produto.Cursos.CPMED_MED
                             && cp.intMenuId == menuId
                             && mc.intCourseID == turmaId
                             select new CronogramaPrateleiraDTO
                             {
                                 ID = cp.intID,
                                 Descricao = cp.txtDescricao,
                                 Ordem = cp.intOrdem,
                                 EntidadeCodigo = p2.txtCode,
                                 EntidadeID = be.intID,
                                 Semana = lt.intSemana ?? 0,
                                 Ano = b.intYear ?? 0,
                                 Data = mc.dteDateTime,
                                 LessonTitleID = lt.intLessonTitleID,
                                 MaterialId = lm.intMaterialID,
                                 EspecialidadeId = p2.intProductGroup3 ?? 0,
                                 Nome = lt.txtLessonTitleName,
                                 ExibeConformeCronograma = cp.bitExibeConformeCronograma
                             }).ToList();

                return query;
            }

        }

        public List<MaterialChecklistDTO> GetChecklistsExtrasLiberados(int matricula, int idProduct)
        {
            using (var ctx = new DesenvContext())
            {
                var checklists = (from m in ctx.tblMaterialApostila
                                  join p in ctx.tblProducts
                                  on m.intProductId equals p.intProductID
                                  where
                                  p.intProductGroup2 == idProduct
                                  && p.intProductGroup3 == (int)Utilidades.EProductsGroup1.ApostilaCpmed
                                  select new MaterialChecklistDTO
                                  {
                                      ProductId = m.intProductId,
                                      ProductGroup3Id = p.intProductGroup3 ?? 0
                                  }).ToList();
                return checklists;
            }
        }


        public List<MaterialChecklistDTO> GetChecklistsPraticosLiberados(int matricula, int idProduct)
        {
            using (var ctx = new DesenvContext())
            {
                var checklists = (from m in ctx.tblMaterialApostila
                                  join p in ctx.tblProducts on m.intProductId equals p.intProductID
                                  join ma in ctx.tblMaterialApostilaAluno on m.intID equals ma.intMaterialApostilaID
                                  where
                                  ma.intClientID == matricula
                                  && p.intProductGroup2 == idProduct
                                  && p.intProductGroup3 == (int)Utilidades.EProductsGroup1.CursoPratico
                                  select new MaterialChecklistDTO
                                  {
                                      ProductId = m.intProductId,
                                      ProductGroup3Id = p.intProductGroup3 ?? 0
                                  }).ToList();
                return checklists;
            }
        }

        public List<AulaTema> GetTemasAnosAnteriores(List<string> nomes)
        {
            using (var ctx = new DesenvContext())
            {
                var temas = (from lt in ctx.tblLessonTitles
                             join rai in ctx.tblRevisaoAulaIndice on lt.intLessonTitleID equals rai.intLessonTitleId
                             join rav in ctx.tblRevisaoAulaVideo on rai.intRevisaoAulaIndiceId equals rav.intRevisaoAulaIndiceId
                             where nomes.Contains(lt.txtLessonTitleName)
                             select new AulaTema
                             {
                                 TemaID = lt.intLessonTitleID,
                                 Nome = lt.txtLessonTitleName
                             }
                             ).Distinct().ToList();
                return temas;
            }
        }

        public ResponseDTO<List<SemanaProgressoPermissao>> GetPermissoes(int idProduto, int matricula, int anoMaterial = 0)
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetPermissoes))
               return RedisCacheManager.GetItemObject<ResponseDTO<List<SemanaProgressoPermissao>>>(RedisCacheConstants.DadosFakes.KeyGetPermissoes);

            try
            {
                var semanaProgressoPermissao = new List<SemanaProgressoPermissao>();
                var aulaBusiness = new AulaBusiness(new AulaEntity());
                var anosProdutoAluno = new List<int?>();
                using(MiniProfiler.Current.Step("Obtendo mensagem de login"))
                {
                    anosProdutoAluno = AnosProdutoAluno(matricula, idProduto, anoMaterial);
                }
                var maiorAnoAluno = anosProdutoAluno.OrderByDescending(c => c).FirstOrDefault().GetValueOrDefault();

                if (anoMaterial == 0) anoMaterial = maiorAnoAluno;
                if (anoMaterial < maiorAnoAluno && anosProdutoAluno.Contains(anoMaterial)) maiorAnoAluno = anoMaterial; 

                using(MiniProfiler.Current.Step("Obtendo permissões da semana"))
                {
                    semanaProgressoPermissao.Add(new SemanaProgressoPermissao
                    {

                        PermissaoSemanas = aulaBusiness.GetPermissaoSemanas(maiorAnoAluno, matricula, idProduto, anoMaterial)
                    });
                }
                return new ResponseDTO<List<SemanaProgressoPermissao>> { Retorno = semanaProgressoPermissao, Sucesso = true };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<SemanaProgressoPermissao>> { Sucesso = false, Mensagem = ex.Message };
            }

        }

        public List<TurmaMatriculaBaseDTO> GetTurmaMatriculasBase(TurmaMatriculaBaseDTO filtro)
        {
            using (var ctx = new DesenvContext())
            {
                var matriculasBaseTurmas = ctx.tblTurmaExcecaoCpfBase
                    .Where(x=> (filtro.Ano == 0 || filtro.Ano == x.intYear || x.intYear == -1)
                        && (filtro.CourseId == 0 || filtro.CourseId == x.intCourseId || x.intCourseId == -1)
                        && (filtro.ProdutoId == 0 ||  filtro.ProdutoId == x.intProdutoID || x.intProdutoID == 0)
                    )
                    .Select(x => new TurmaMatriculaBaseDTO
                {
                    Id = x.intTurmaExcecaoCpfBaseId,
                    Ano = x.intYear,
                    CourseId = x.intCourseId,
                    MatriculaBase = x.intMatriculaBaseId,
                    DataCadastro = x.dteCadastro,
                    DiasLimite = x.intDiasLimite,
                    ProdutoId = x.intProdutoID
                }).ToList();

                return matriculasBaseTurmas;
            }
        }        

        public List<CronogramaPrateleiraDTO> GetConfiguracaoMateriaisEntidades(int menuId, int produtoId)
        {
            using (var ctx = new DesenvContext())
            {
                
                var cronogramaProduto = (from cp in ctx.tblCronogramaPrateleira
                                         join cplt in ctx.tblCronogramaPrateleira_LessonTitles on cp.intID equals cplt.intPrateleiraCronogramaID
                                         where cp.intMenuId == menuId && cp.intProductGroup1 == produtoId
                                         select new CronogramaPrateleiraDTO
                                         {
                                             ID = cp.intID,
                                             Descricao = cp.txtDescricao,
                                             Ordem = cp.intOrdem,
                                             LessonTitleID = cplt.intLessonTitleID,
                                             ExibeEspecialidade = cp.bitExibeEspecialidade,
                                             ExibeConformeCronograma = cp.bitExibeConformeCronograma,
                                             TipoLayout = cp.intTipoLayout != null ? (TipoLayoutMainMSPro) cp.intTipoLayout : TipoLayoutMainMSPro.WEEK_DOUBLE 
                                         }).ToList();

                return cronogramaProduto;
            }
        }

        public List<int?> AnosProdutoAluno(int matricula, int idProduto, int anoMaterial = 0)
        {
            var ctx = new DesenvContext();
            var anoAtual = Utilidades.GetYear();
            int? anoAnterior = anoAtual - 1;
            if (anoMaterial == 0) anoMaterial = anoAtual;

            var curso = (Produto.Cursos)idProduto;
            int idProductGroup1 = (int)curso.GetProductByCourse();
            var alunoMeioDeAno = Utilidades.IsCicloCompletoNoMeioDoAno(matricula);
            var alunoMeioDeAnoAnosAnteriores = Utilidades.CicloCompletoAnosAnterioresNoMeioDoAno(matricula);

            var idsExtensivos = new int[] { (int)Utilidades.ProductGroups.MED, (int)Utilidades.ProductGroups.MEDEAD, (int)Utilidades.ProductGroups.MEDCURSO, (int)Utilidades.ProductGroups.MEDCURSOEAD, (int)Utilidades.ProductGroups.MedMaster };

            var idsRmais = Utilidades.ProdutosR3();

            var produtos = new List<int?>();
            if (idProductGroup1 == (int)Utilidades.ProductGroups.MED)
            {
                produtos.Add((int)Utilidades.ProductGroups.MED);
                produtos.Add((int)Utilidades.ProductGroups.CPMED);
                produtos.Add((int)Utilidades.ProductGroups.MEDEAD);
                produtos.Add((int)Utilidades.ProductGroups.MedMaster);
            }
            else if (idProductGroup1 == (int)Utilidades.ProductGroups.MEDCURSO)
            {
                produtos.Add((int)Utilidades.ProductGroups.MEDCURSO);
                produtos.Add((int)Utilidades.ProductGroups.MEDCURSOEAD);
                produtos.Add((int)Utilidades.ProductGroups.MedMaster);
            }
            else
            {
                produtos.Add(idProductGroup1);
            }

            var query = (from so in ctx.tblSellOrders
                         join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                         join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                         join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                         where so.intClientID == matricula
                             && (so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                 || (anoAtual == c.intYear
                                     && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                                     && alunoMeioDeAno
                                 )
                                    || (so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                                        && (alunoMeioDeAnoAnosAnteriores.Contains(c.intYear.Value)
                                            || idsExtensivos.Contains((int)p.intProductGroup1)
                                            || idsRmais.Contains((int)p.intProductGroup1))
                                        )
                                     || (anoAtual == c.intYear
                                        && so.intStatus == (int)Utilidades.ESellOrderStatus.Pendente
                                        && p.intProductGroup1 == (int)Utilidades.ProductGroups.MedMaster
                                     )
                             )
                             && produtos.Contains(p.intProductGroup1)
                         orderby c.intYear descending
                         select new
                         {
                             Produto = p.intProductGroup1 ?? 0,
                             Ano = c.intYear,
                             Status = so.intStatus ?? 0
                         }
                         ).ToList();


            var extensivoCancelado = query.Any(x => Utilidades.ProdutosExtensivo().Contains(x.Produto) && x.Ano == anoAtual && x.Status == (int)Utilidades.ESellOrderStatus.Cancelada);
            var medmaster = query.Any(x => x.Produto == (int)Utilidades.ProductGroups.MedMaster && x.Ano == anoAtual);

            if (extensivoCancelado && medmaster)
                query.RemoveAll(r => Utilidades.ProdutosExtensivo().Contains(r.Produto) && r.Ano == anoAtual && r.Status == (int)Utilidades.ESellOrderStatus.Cancelada);

            if (anoAtual != anoMaterial)
            {
                query.RemoveAll(r => r.Produto == (int)Utilidades.ProductGroups.MedMaster);
                query.Add(new { Produto = (int)Utilidades.ProductGroups.MED, Ano = anoAnterior, Status = (int)Utilidades.ESellOrderStatus.Ativa});
                query.Add(new { Produto = (int)Utilidades.ProductGroups.MEDCURSO, Ano = anoAnterior, Status = (int)Utilidades.ESellOrderStatus.Ativa });
            }
            var anos = query.Select(a => a.Ano).ToList(); 

            return anos;
        }

    public enum ESubMenus
    {
        Nenhum = 0,
        Aulas = 92,
        Materiais = 93,
        Questoes = 94,
        Revalida = 119,
        Simulados = 96,
        ConcursoNaIntegra = 97,
        MontaProva = 99,
        MainSub = 162,
        MaterialFake = 167,
        DuvidasQuestaoMedCode = 228,
        FeedContribuir = 231,
        InserirContribuicao = 232,
        Financeiro = 18,
        Checklists = 306,
        Cronograma = 3,
        MultimidiaCPMED = 305,
        SimuladoR3Pediatria = 282,
        SimuladoR3Cirurgia = 283,
        SimuladoR3Clinica = 284,
        SimuladoR4GO = 285,
        NotasFiscais = 19,
        Provas = 323,
        BonusMs = 320
    }

        public enum EMenu
        {
            Main = 84,
            Medcode = 86,
            AreaTreinamento = 85,
            SlidesDeAula = 129,
            DuvidasAcademicas = 88,
            Contribuicoes = 301,
            Administrativo = 17,
            Academico = 1

        }

        internal class GroupSemana
        {
            public long ID { get; set; }
            public int? Semana { get; set; }
            public string DataInicio { get; set; }
            public string DataFim { get; set; }
        }


        public enum EPesquisaConteudoMenu
        {
            PesquisaAula = 317,
            PesquisaMaterial = 318,
            PesquisaQuestoes = 319
        }
    }
}