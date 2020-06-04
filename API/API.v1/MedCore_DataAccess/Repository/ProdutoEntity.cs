using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedCore_DataAccess.Repository
{
    public class ProdutoEntity : IProdutoData
    {

        public int GetByIdApostila(int idApostila)
        {
            var ctx = new DesenvContext();
            var consulta = (from p in ctx.tblProducts
                            where p.intProductID == idApostila && p.intProductGroup1 == 4
                            select p.intProductGroup2).FirstOrDefault() ?? 0;
            return (int)consulta;
        }

        public Produtos GetComboProdutos(int intComboID)
        {
            using (var ctx = new DesenvContext())
            {
                var lstProduto = new Produtos();
                var query = (from a in ctx.mview_ProductCombos
                             join b in ctx.tblProducts on a.intProductID equals b.intProductID
                             where a.intComboID == intComboID
                             select new Produto()
                             {
                                 ID = a.intProductID,
                                 Nome = b.txtShortName.Trim()
                             }).ToList();

                lstProduto.AddRange(query);

                return lstProduto;
            }
        }

        public static int GetProductByCourse(int course)
        {
            switch ((Produto.Cursos)course)
            {
                case Produto.Cursos.MEDCURSO:
                    return (int)Produto.Produtos.MEDCURSO;
                case Produto.Cursos.MED:
                    return (int)Produto.Produtos.MED;
                case Produto.Cursos.CPMED:
                    return (int)Produto.Produtos.CPMED;
                case Produto.Cursos.INTENSIVAO:
                    return (int)Produto.Produtos.INTENSIVAO;
                case Produto.Cursos.MEDELETRO:
                    return (int)Produto.Produtos.MEDELETRO;
                case Produto.Cursos.RAC:
                    return (int)Produto.Produtos.RAC;
                case Produto.Cursos.RACIPE:
                    return (int)Produto.Produtos.RACIPE;
                case Produto.Cursos.RAC_IMED:
                    return (int)Produto.Produtos.RAC_IMED;
                case Produto.Cursos.RACIPE_IMED:
                    return (int)Produto.Produtos.RACIPE_IMED;
                case Produto.Cursos.REVALIDA:
                    return (int)Produto.Produtos.REVALIDA;
                case Produto.Cursos.ADAPTAMED:
                    return (int)Produto.Produtos.ADAPTAMED;
                default:
                    return course;
            }
        }

        public List<Produto> GetProdutosContratadosPorAnoMatricula(int intClientID)
        {
            return GetProdutosContratadosPorAnoComCache(intClientID);
        }

        public static List<Produto> GetProdutosContratadosPorAnoComCache(int intClientID)
        {
            try
            {
                if (RedisCacheManager.CannotCache(RedisCacheConstants.Permissao.GetProdutosContratadosPorAno))
                    return GetProdutosContratadosPorAno(intClientID);

                var key = String.Format("{0}:{1}", RedisCacheConstants.Permissao.GetProdutosContratadosPorAno, intClientID);
                var produtos = RedisCacheManager.GetItemObject<List<Produto>>(key);

                if (produtos == null)
                {
                    produtos = GetProdutosContratadosPorAno(intClientID);
                    if (produtos != null)
                    {
                        RedisCacheManager.SetItemObject(key, produtos, TimeSpan.FromMinutes(RedisCacheConstants.Permissao.ValidadeGetProdutosContratadosPorAno));
                    }
                }

                return produtos;
            }
            catch
            {
                return GetProdutosContratadosPorAno(intClientID);
            }
        }

        public List<Produto> GetAll()
        {
            var ctx = new DesenvContext();
            var consulta = ctx.tblProductGroups1.Where(it =>
                                                           it.intProductGroup1ID == (int)Produto.Cursos.MEDCURSO ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.MED ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.INTENSIVAO ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.CPMED ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.MEDELETRO ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.RAC ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.RACIPE ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.RA ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.PRATICO ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.REVALIDA ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.ADAPTAMED ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.R3Cirurgia ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.R3Clinica ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.R3Pediatria ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.R4GO ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.MEDELETRO_IMED ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.MASTO ||
                                                           it.intProductGroup1ID == (int)Produto.Cursos.TEGO
            );

            var produtos = new Produtos();
            foreach (var it in consulta)
            {
                var p = new Produto
                {
                    ID = it.intProductGroup1ID,
                    Nome = it.txtDescription
                };

                if (p.ID == (int)Produto.Cursos.RA)
                    p.Nome = "R.A";
                else
                    p.Nome = (p.ID != (int)Produto.Cursos.INTENSIVAO) ? p.Nome : "INTENSIVÃO";

                p.GrandesAreas = new GrandeAreaEntity().GetAll(p.ID);

                produtos.Add(p);
            }

            return produtos;
        }        

        public static List<int> GetProdutosContratados(int intClientID, int[] anos, int produto = 0, int adimplentes = 0)
        {
            var MEDCURSO = new[] { (int)Produto.Produtos.MEDCURSO, (int)Produto.Produtos.MEDCURSOEAD }.ToList();
            var MED = new[] { (int)Produto.Produtos.MED, (int)Produto.Produtos.MEDEAD }.ToList();
            var CPMED = (int)Produto.Produtos.CPMED;
            var ECG = (int)Produto.Produtos.MEDELETRO;
            var ADAPTAMED = (int)Produto.Produtos.ADAPTAMED;
            var result = new TurmaEntity().GetTurmasContratadas(intClientID, anos, produto, adimplentes).Select(i => i.IdProduto);
            var lst = new List<int>();
            foreach (var valor in result)
            {
                var v = valor;
                if (produto == 0)
                {

                    lst.Add(MED.Contains(v) ? (int)Produto.Cursos.MED :
                        MEDCURSO.Contains(v) ? (int)Produto.Cursos.MEDCURSO :
                            CPMED.Equals(v) ? (int)Produto.Cursos.CPMED :
                            ECG.Equals(v) ? (int)Produto.Cursos.MEDELETRO :
                            ADAPTAMED.Equals(v) ? (int)Produto.Cursos.ADAPTAMED : v);
                }
                else
                {
                    lst.Add(
                        MED.Contains(v) && MED.Contains(produto) ? (int)Produto.Cursos.MED :
                        MEDCURSO.Contains(v) && MEDCURSO.Contains(produto) ? (int)Produto.Cursos.MEDCURSO :
                        CPMED.Equals(v) && v == produto ? (int)Produto.Cursos.CPMED :
                        ECG.Equals(v) && v == produto ? (int)Produto.Cursos.MEDELETRO :
                        ADAPTAMED.Equals(v) && v == produto ? (int)Produto.Cursos.ADAPTAMED :
                        v == produto ? v : 0);
                }
            }


            return lst.Distinct().ToList();

        }

        public static List<Produto> GetProdutosContratadosPorAno(int intClientID, bool IncluiPendentes = false, int SomentePendentes = 0, bool IncluirCancelados = false, int aplicacaoId = 0)
        {
            using (var ctx = new DesenvContext())
            {

                var meioDeano = ctx.tblAlunosAnoAtualMaisAnterior.Any(x => x.intClientID == intClientID);
                var produtosR3 = Utilidades.ProdutosR3();
                var produtoMedEletroIMed = Produto.Produtos.MEDELETRO_IMED.GetHashCode();
                var isMedSoftPro = (new int[] { (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, (int)Aplicacoes.MsProMobile }).Contains(aplicacaoId);

                var produtos = (from so in ctx.tblSellOrders
                                join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                                join c in ctx.tblCourses on p.intProductID equals c.intCourseID
                                where so.intClientID == intClientID && ((so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                                              || (so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada && new[] { (int)Utilidades.ESellOrderStatus.Suspensa, (int)Utilidades.ESellOrderStatus.Carencia, (int)Utilidades.ESellOrderStatus.Cancelada, (int)Utilidades.ESellOrderStatus.InadimplenteMesesAnteriores }.Contains(p.intProductGroup1.Value) && meioDeano)) && SomentePendentes == (int)Utilidades.ESellOrderStatus.Pendente
                                                              || (IncluiPendentes && so.intStatus == (int)Utilidades.ESellOrderStatus.Pendente)
                                                              || (SomentePendentes == (int)Utilidades.ESellOrderStatus.Suspensa && so.intStatus == (int)Utilidades.ESellOrderStatus.Pendente)
                                                              || (IncluirCancelados && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada)
                                                              || (produtosR3.Contains(p.intProductGroup1.Value) && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada)
                                                              || (p.intProductGroup1.Value == produtoMedEletroIMed && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada)
                                                              || (isMedSoftPro && so.intStatus == (int)Utilidades.ESellOrderStatus.Pendente && (p.intProductGroup1 == (int)Produto.Produtos.MED_MASTER || p.intProductGroup1 == (int)Produto.Produtos.CPMED_EXTENSIVO))
                                                              )
                                
                                select new Produto
                                {
                                    Ano = c.intYear,
                                    IDProduto = p.intProductGroup1.Value,
                                    GrupoProduto2 = p.intProductGroup2 ?? 0,
                                    OrdemVenda = so.intOrderID,
                                    txtComment = so.txtComment,
                                    intStatus = so.intStatus
                                }).OrderBy(x => x.Ano).Distinct().ToList();


                return produtos;
            }
        }

        public static List<Produto> GetProdutosContratadosPorAnoCache(int intClientID, bool IncluiPendentes = false, int SomentePendentes = 0, bool IncluirCancelados = false, int aplicacaoId = 0)
        {
            var key = String.Format("{0}:{1}:{2}:{3}:{4}:{5}", RedisCacheConstants.Produtos.KeyProdutosContratadosPorAno, intClientID, IncluiPendentes, SomentePendentes, IncluirCancelados, aplicacaoId);

            if (RedisCacheManager.CannotCache(RedisCacheConstants.Produtos.KeyProdutosContratadosPorAno))
            {
                return GetProdutosContratadosPorAno(intClientID, IncluiPendentes, SomentePendentes, IncluirCancelados, aplicacaoId);
            }
            else
            {

                if (!RedisCacheManager.HasAny(key))
                {
                    var produtos = GetProdutosContratadosPorAno(intClientID, IncluiPendentes, SomentePendentes, IncluirCancelados, aplicacaoId);
                    RedisCacheManager.SetItemObject(key, produtos, TimeSpan.FromHours(1));
                    return produtos;
                }
                else
                {
                    return RedisCacheManager.GetItemObject<List<Produto>>(key);
                }
            }

        }

          public List<Produto> GetProdutosCombo(Produto combo)
        {
            using (var ctx = new DesenvContext())
            {
                var produtoscombo = (from ct in ctx.tblProductCombos_Products
                                     join p in ctx.tblProducts on ct.intProductID equals p.intProductID
                                     join c in ctx.tblCourses on p.intProductID equals c.intCourseID
                                     join sp in ctx.tblSitePage_AsTurmas_parametros on c.intCourseID equals sp.intCourseID
                                     where ct.intComboID == combo.ID
                                     orderby p.intProductID
                                     select new Produto
                                     {
                                         ID = p.intProductID,
                                         TurmaConfirmada = sp.bitConfirmado ?? true
                                     }).ToList();



                return produtoscombo;

            }

        }
    }
}