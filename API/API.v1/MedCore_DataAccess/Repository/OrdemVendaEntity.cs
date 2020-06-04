using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Repository
{
    public class OrdemVendaEntity
    {
        public static List<int> AnosValidos(int idAplicacao, DateTime data)
        {

            var anoAtual = Utilidades.GetYear();
            var anos = new[] { anoAtual }.ToList();
            var ctx = new DesenvContext();

            anos = anos.Concat((from l in ctx.tblAccess_DataLimite
                                where l.dteDataLimite >= data
                                   && l.intAplicationID == idAplicacao
                                select l.intAlunoYear)).ToList();
            return anos;
        }

        public OrdensVenda GetResumed(int idCliente, List<int> ano, int group1 = 0, int group2 = 0, List<int> lgroup = null)
        {
            var ctx = new DesenvContext();
            var status = new int[] { 0, 1, 2, 5 }.ToList();

            List<int> produtos = new List<int>();
            if (group1 > 0) produtos.Add(group1);
            if (group2 > 0) produtos.Add(group2);
            if (lgroup != null && lgroup.Count() > 0)
            {
                produtos.AddRange(lgroup);
            }
            List<OrdemVenda> consulta =
                (from so in ctx.tblSellOrders
                 join d in ctx.tblSellOrderDetails on so.intOrderID equals d.intOrderID
                 join p in ctx.tblProducts on d.intProductID equals p.intProductID
                 join c in ctx.tblCourses on d.intProductID equals c.intCourseID
                 where
                 so.intClientID == idCliente
                 && so.intStatus != 1
                 && ano.Contains(c.intYear.Value)
                 && (
                    (produtos.Count() == 0)
                    ||
                    (produtos.Count() > 0 && produtos.Contains(p.intProductGroup1.Value))
                 )
                 orderby so.intOrderID descending
                 select new OrdemVenda
                 {
                     ID = so.intOrderID,
                     IdCliente = so.intClientID,
                     IdFilial = so.intStoreID,
                     Status = (OrdemVenda.StatusOv)so.intStatus,
                     Status2 = (OrdemVenda.StatusOv)so.intStatus2,
                     /////Data = it.dteDate ?? DateTime.MinValue, 
                     Descricao = so.txtComment,
                     IdCondicaoPagamento = so.intConditionTypeID ?? -1,
                     IdMethodoEnvio = so.intShippingMethodID ?? -1,
                     IdVendedor = so.intSellerID ?? -1,
                     IdTermo = so.intAgreementID ?? -1,
                     IsParcelado = false,
                     TxtRegister = "",
                     IdProduto = ctx.tblSellOrderDetails.Where(sd => sd.intOrderID == so.intOrderID).Count() == 1 ? p.intProductID : 0,
                     Year = c.intYear ?? 0,
                     GroupID = p.intProductGroup1 ?? 0,
                    //  IdCombo = ctx.tblSellOrderDetails.Where(sd => sd.intOrderID == so.intOrderID).Count() > 1 ? //sÓ FAZ SE FOR COMBO 

                    //     ctx.tblProductCombos_Products
                    //     .Where(combo => ctx.tblSellOrderDetails
                    //                         .Any(sd => sd.intOrderID == so.intOrderID && sd.intProductID.Equals(combo.intProductID))) //PEGO TODA A RELAÇÃO PRODUTO X COMBOS QUE POSSUEM O ALGUM ID DE PRODUTO DA OV 
                    //     .GroupBy(x => x.intComboID) //AGRUPO POR ID DE COMBO 

                    //     .Select(x => new
                    //     {
                    //         qtdProdutosCombo = ctx.tblProductCombos_Products
                    //                  .Where(cp => cp.intComboID == x.Key)
                    //                  .Select(j => j.intProductID).Count(),
                    //         qtdProdutosComboNaOV = x.Distinct().Count(),
                    //         IdCombo = x.Key
                    //     }) // VEJO A QUANTIDADE DE PRODUTOS POR COMBO e a quantidade de produtos do combo presentes na OV 

                    //     .Where(y => y.qtdProdutosCombo.Equals(y.qtdProdutosComboNaOV) && y.qtdProdutosCombo.Equals(ctx.tblProducts
                    //                                 .Where(produto => ctx.tblSellOrderDetails
                    //                                 .Any(sd => sd.intOrderID == so.intOrderID && sd.intProductID.Equals(produto.intProductID)))
                    //                                 .Count())) //Filtra os combos com todos os produtos do combo na OV e  a mesma qtdade de produtos que a OV  

                    //     .Select(x => x.IdCombo).DefaultIfEmpty(0).FirstOrDefault() : 0
                 }
            ).ToList();

            OrdensVenda ov = new OrdensVenda();
            ov.AddRange(consulta.Select(x => new OrdemVenda
            {
                ID = x.ID,
                IdCliente = x.IdCliente,
                IdFilial = x.IdFilial,
                Status = x.Status,
                Status2 = x.Status2,
                /////Data = it.dteDate ?? DateTime.MinValue, 
                Descricao = x.Descricao,
                IdCondicaoPagamento = x.IdCondicaoPagamento,
                IdMethodoEnvio = x.IdMethodoEnvio,
                IdVendedor = x.IdVendedor,
                IdTermo = x.IdTermo,
                IsParcelado = x.IsParcelado,
                TxtRegister = x.TxtRegister,
                GroupID = x.GroupID,
                Year = x.Year,
                IdProduto = x.IdCombo == 0 ? x.IdProduto : 0,
                IdCombo = x.IdCombo
            }).Distinct());

            foreach (var o in ov)
            {
                if (o.IdCombo > 0)
                {
                    var produtosCombo = new ProdutoEntity().GetComboProdutos(o.IdCombo);
                    o.ProductIDs = produtosCombo.Select(p => p.ID).ToList();
                }
                else
                {
                    o.ProductIDs = consulta.Where(c => c.ID == o.ID).Select(z => z.IdProduto).Distinct().ToList();
                }
            }
            return ov;
        }

        public OrdensVenda GetOrdensVenda(int idCliente, List<int> ano, int group1, int group2)
        {
            var ctx = new DesenvContext();
            var status = new int[] { 0, 2, 5 }.ToList();//Somente status de OV que consideramos 

            List<int> produtos = new List<int>();
            if (group1 > 0) produtos.Add(group1);
            if (group2 > 0) produtos.Add(group2);

            List<OrdemVenda> consulta =
                (from so in ctx.tblSellOrders
                 //join s in status on so.intStatus equals s
                 join d in ctx.tblSellOrderDetails on so.intOrderID equals d.intOrderID
                 join p in ctx.tblProducts on d.intProductID equals p.intProductID
                 join c in ctx.tblCourses on d.intProductID equals c.intCourseID
                 where
                 so.intClientID == idCliente
                 && ano.Contains(c.intYear.Value)
                 && (
                    (produtos.Count() == 0)
                    ||
                    (produtos.Count() > 0 && produtos.Contains(p.intProductGroup1.Value))
                 )
                 orderby so.intOrderID descending
                 select new OrdemVenda
                 {
                     ID = so.intOrderID,
                     IdCliente = so.intClientID,
                     IdFilial = so.intStoreID,
                     Status = (OrdemVenda.StatusOv)so.intStatus,
                     Status2 = (OrdemVenda.StatusOv)so.intStatus2,
                     /////Data = it.dteDate ?? DateTime.MinValue, 
                     Descricao = so.txtComment,
                     IdCondicaoPagamento = so.intConditionTypeID ?? -1,
                     IdMethodoEnvio = so.intShippingMethodID ?? -1,
                     IdVendedor = so.intSellerID ?? -1,
                     IdTermo = so.intAgreementID ?? -1,
                     IsParcelado = false,
                     TxtRegister = "",
                     IdProduto = ctx.tblSellOrderDetails.Where(sd => sd.intOrderID == so.intOrderID).Count() == 1 ? p.intProductID : 0,
                     GroupID = p.intProductGroup1 ?? 0,
                     Year = c.intYear ?? 0,
                     IdCombo = ctx.tblSellOrderDetails.Where(sd => sd.intOrderID == so.intOrderID).Count() > 1 ?
                        ctx.tblProductCombos_Products
                        .Where(combo => ctx.tblSellOrderDetails
                                            .Any(sd => sd.intOrderID == so.intOrderID && sd.intProductID.Equals(combo.intProductID)))
                        .GroupBy(x => x.intComboID)
                        .Select(x => new { qtd = x.Count(), IdCombo = x.Key })
                        .Where(y => y.qtd.Equals(ctx.tblProducts
                                                    .Where(produto => ctx.tblSellOrderDetails
                                                    .Any(sd => sd.intOrderID == so.intOrderID && sd.intProductID.Equals(produto.intProductID)))
                                                    .Count()))
                        .Select(x => x.IdCombo).FirstOrDefault() : 0

                 }
            ).ToList();

            OrdensVenda ov = new OrdensVenda();
            ov.AddRange(consulta.Select(x => new OrdemVenda
            {
                ID = x.ID,
                IdCliente = x.IdCliente,
                IdFilial = x.IdFilial,
                Status = x.Status,
                Status2 = x.Status2,
                /////Data = it.dteDate ?? DateTime.MinValue, 
                Descricao = x.Descricao,
                IdCondicaoPagamento = x.IdCondicaoPagamento,
                IdMethodoEnvio = x.IdMethodoEnvio,
                IdVendedor = x.IdVendedor,
                IdTermo = x.IdTermo,
                IsParcelado = x.IsParcelado,
                TxtRegister = x.TxtRegister,
                GroupID = x.GroupID,
                Year = x.Year,
                IdProduto = x.IdCombo == 0 ? x.IdProduto : 0,
                IdCombo = x.IdCombo
            }).Distinct());
            ov.ForEach(y => y.ProductIDs = consulta.Where(c => c.ID == y.ID).Select(z => z.IdProduto).Distinct().ToList());

            return ov;
        }

        public List<OrdemVenda> GetOrdensVenda(string txtRegister, int ProductGroup)
        {
            using (var ctx = new DesenvContext())
            {

                var ordens =
                    (from so in ctx.tblSellOrders
                     join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                     join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                     join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                     join person in ctx.tblPersons on so.intClientID equals person.intContactID
                     where
                     p.intProductGroup1 == ProductGroup
                     && person.txtRegister == txtRegister && (new[] { 0, 2, 5 }).Contains(so.intStatus ?? 0)
                     select new OrdemVenda
                     {
                         ID = so.intOrderID,
                         Year = c.intYear ?? 0,
                         IdCliente = so.intClientID,
                         Status = (OrdemVenda.StatusOv)so.intStatus,
                         IdFilial = so.intStoreID
                     }).Distinct().ToList();

                var lOv = new List<OrdemVenda>();
                foreach (var o in ordens)
                {
                    if (o.Status == OrdemVenda.StatusOv.Ativa || (PagamentosClienteEntity.GetPagamentosCliente(o.IdCliente, (new[] { o.Year }), o.ID).Any(pg => (pg.DblSumOfDebits >= 0 || pg.Status == PagamentosCliente.StatusPagamento.OK) && pg.DblValue > 0.0)))
                    {
                        lOv.Add(o);
                    }
                }
                return lOv;

            }

        }        

        public List<OrdemVenda> GetOrdensVenda(string txtRegister, List<int> ProductGroup)
        {
            using (var ctx = new DesenvContext())
            {
                var query = from so in ctx.tblSellOrders
                            join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                            join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                            join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                            join person in ctx.tblPersons on so.intClientID equals person.intContactID
                            where
                            ProductGroup.Contains(p.intProductGroup1.Value)
                            && person.txtRegister.Trim() == txtRegister && (new[] { 0, 2, 5 }).Contains(so.intStatus ?? 0)
                            orderby so.dteDate descending
                            select new OrdemVenda
                            {
                                ID = so.intOrderID,
                                GroupID = p.intProductGroup1.Value,
                                Year = c.intYear ?? 0,
                                IdCliente = so.intClientID,
                                Status = (OrdemVenda.StatusOv)so.intStatus,
                                IdFilial = so.intStoreID,
                                Descricao = so.txtComment,
                            };

                var ordens =
                    query.Distinct().OrderByDescending(x => x.ID).ToList();

                return ordens;
            }
        }

        public static OrdemVenda.StatusOv GetRealStatusOv(OrdemVenda.StatusOv statusOv, RegraCondicao.tipoAno tipoAno, int matricula, bool isAntesDataLimite)
        {
            using (var ctx = new DesenvContext())
            {
                var returnOv = statusOv;
                var isOvMeioDeAno = statusOv.Equals(OrdemVenda.StatusOv.Cancelada)
                    && (tipoAno.Equals(RegraCondicao.tipoAno.Atual)
                    || (tipoAno.Equals(RegraCondicao.tipoAno.Anterior) && isAntesDataLimite))
                    && ClienteEntity.IsCicloCompletoNoMeioDoAno(matricula);

                if (isOvMeioDeAno)
                    returnOv = OrdemVenda.StatusOv.Ativa;

                return returnOv;

            }
        }        
    

        public static List<OrdemVenda> GetOvsAluno(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                List<OrdemVenda> ovsAluno;
                ovsAluno = (from so in ctx.tblSellOrders
                            join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                            join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                            join c in ctx.tblCourses on p.intProductID equals c.intCourseID
                            where so.intClientID == matricula && (new[] { 0, 2, 5 }).Contains(so.intStatus ?? 0)

                            select new OrdemVenda
                            {
                                ID = so.intOrderID,
                                Status = (OrdemVenda.StatusOv)(so.intStatus ?? 1),
                                Year = c.intYear ?? 0,
                                IdProduto = c.intCourseID,
                                GroupID = p.intProductGroup1 ?? 0,
                                IdFilial = so.intStoreID
                            }
                                     ).Distinct().ToList().OrderByDescending(y => y.ID).ToList();
                return ovsAluno;

            }
        }

        public static bool TemOvsAnoAtual(List<OrdemVenda> ovsAluno, List<int> anosConsideradosComoAtuais, bool isApostilaMed, bool isMeioDeAno)
        {

            var produtosExtensivo = Utilidades.ProdutosExtensivo();

            var retorno = ovsAluno.Where(x => anosConsideradosComoAtuais.Contains(x.Year)
                                            && (produtosExtensivo.Contains(x.GroupID) || (isApostilaMed && x.GroupID == (int)Produto.Produtos.CPMED)
                                                 || (int)Produto.Produtos.R3CLINICA == x.GroupID
                                                 || (int)Produto.Produtos.R3CIRURGIA == x.GroupID
                                                 || (int)Produto.Produtos.R3PEDIATRIA == x.GroupID
                                                 || (int)Produto.Produtos.R4GO == x.GroupID
                                                 || (int)Produto.Produtos.TEGO == x.GroupID
                                                 || (int)Produto.Produtos.MASTO == x.GroupID
                                            )
                                            && ((isMeioDeAno) || (new[] { 0, 2 }).Contains((int)x.Status))).Any();
            return retorno;


        }

         public static List<OrdemVenda> GetOvsAposAnoLancamentoAulaRevisao(List<OrdemVenda> ovsAluno, int groupApostila, bool isApostilaMED, bool isApostilaCPMED, List<int> anosConsideradosComoAtuais)
        {
            var extensivoMedcurso = new int[] { (int)Produto.Produtos.MEDCURSO, (int)Produto.Produtos.MEDCURSOEAD, (int)Produto.Produtos.MED_MASTER };
            var extensivoMed = new int[] { (int)Produto.Produtos.MED, (int)Produto.Produtos.MEDEAD, (int)Produto.Produtos.MED_MASTER };

            var groupApostilasMedcurso = new int[] { (int)Produto.Cursos.MEDCURSO, (int)Produto.Cursos.MEDMEDCURSO };
            var groupApostilasMed = new int[] { (int)Produto.Cursos.MED, (int)Produto.Cursos.MEDMEDCURSO };

            var grupoTurmasR3Clm = (int)Produto.Produtos.R3CLINICA;
            var grupoApostilasR3Clm = (int)Produto.Cursos.R3Clinica;


            var grupoTurmasR3Cir = (int)Produto.Produtos.R3CIRURGIA;
            var grupoApostilasR3Cir = (int)Produto.Cursos.R3Cirurgia;

            var grupoTurmasR3Ped = (int)Produto.Produtos.R3PEDIATRIA;
            var grupoApostilasR3Ped = (int)Produto.Cursos.R3Pediatria;


            var grupoTurmasR4Go = (int)Produto.Produtos.R4GO;
            var grupoApostilasR4Go = (int)Produto.Cursos.R4GO;


            var grupoTurmasTego = (int)Produto.Produtos.TEGO;
            var grupoApostilasTego = (int)Produto.Cursos.TEGO;

            var grupoTurmasMasto = (int)Produto.Produtos.MASTO;
            var grupoApostilasMasto = (int)Produto.Cursos.MASTO;

            List<OrdemVenda> retorno = ovsAluno.Where(x => x.Year >= Constants.AULAREVISAO_PRIMEIROANO && (
                (extensivoMedcurso.Contains(x.GroupID) && groupApostilasMedcurso.Contains(groupApostila))
                                 || (extensivoMed.Contains(x.GroupID) && groupApostilasMed.Contains(groupApostila)
                                 || ((x.GroupID == (int)Produto.Produtos.CPMED)

                                                        && isApostilaMED
                                                        && isApostilaCPMED
                                                        && anosConsideradosComoAtuais.Contains(x.Year))
                                 )
                                 || (grupoTurmasR3Clm == x.GroupID && grupoApostilasR3Clm == groupApostila)
                                 || (grupoTurmasR3Cir == x.GroupID && grupoApostilasR3Cir == groupApostila)
                                 || (grupoTurmasR3Ped == x.GroupID && grupoApostilasR3Ped == groupApostila)
                                 || (grupoTurmasR4Go == x.GroupID && grupoApostilasR4Go == groupApostila)
                                 || (grupoTurmasTego == x.GroupID && grupoApostilasTego == groupApostila)
                                 || (grupoTurmasMasto == x.GroupID && grupoApostilasMasto == groupApostila)

                )).ToList();
            return retorno;

        }
        
        public List<OrdemVenda> GetOvsAtivaAluno(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                List<OrdemVenda> ovsAluno;
                ovsAluno = (from so in ctx.tblSellOrders
                            join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                            join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                            join c in ctx.tblCourses on p.intProductID equals c.intCourseID
                            join pg in ctx.tblProductGroups1 on p.intProductGroup1 equals pg.intProductGroup1ID
                            where so.intClientID == matricula && 
                                 (new[] { (int)OrdemVenda.StatusOv.Ativa }).Contains(so.intStatus ?? 0)

                            select new OrdemVenda
                            {
                                ID = so.intOrderID,
                                Year = c.intYear ?? 0,
                                GroupID = p.intProductGroup1 ?? 0,
                                Descricao = pg.txtDescription
                            }
                            ).Distinct().ToList().OrderByDescending(y => y.ID).ToList();

                return ovsAluno;
            }

        }
        
    }
}