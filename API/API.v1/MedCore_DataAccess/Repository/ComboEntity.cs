

using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Repository
{
     public class ComboEntity : IComboData
    {

        public bool IsBeforeDataLimite(int applicationId, int anoAtual)
        {
            return Utilidades.IsBeforeDataLimite(applicationId, anoAtual);
        }

        public List<Combo> GetCombosPermitidos(List<AccessObject> lstCombo, int applicationId, string versaoApp)
        {
            using (var ctx = new DesenvContext())
            {
                int valorOrdemComMenorPrioridade = 50;

                var comboPermitido = (from a in lstCombo
                                      join c in ctx.tblAccess_Combo on a.Id equals c.intObjectId
                                      join ca in ctx.tblAccess_Object_Application on c.intObjectId equals ca.intObjectId
                                      where ca.intApplicationId == applicationId                                    
                                      select new Combo()
                                      {
                                          ComboId = c.intProductGroup2Id.Value,
                                          Nome = c.txtNome,
                                          Id = a.Id,
                                          IntOrdem = c.intOrdem ?? valorOrdemComMenorPrioridade,
                                          tipoLayoutMain = (int)TipoLayoutMainMSPro.WEEK_DOUBLE,
                                          txtMinVersion = ca.txtMinVersion
                                      }).ToList();

                if (string.IsNullOrEmpty(versaoApp)) versaoApp = "0.0.0";
                Version version = Version.Parse(versaoApp);

                return comboPermitido.Where(x => Version.Parse(x.txtMinVersion) <= version).ToList();
            }
        }



        public List<Combo> GetComboAplicacao(int applicationId)
        {

            using (var ctx = new DesenvContext())
            {
                var combosApplicacao = (from c in ctx.tblAccess_Combo
                                        join a in ctx.tblAccess_Object on c.intObjectId equals a.intObjectId
                                        where a.intApplicationId == applicationId
                                        select new Combo()
                                        {
                                            ComboId = c.intProductGroup2Id.Value,
                                            Nome = c.txtNome,
                                            Id = a.intObjectId
                                        }).ToList();


                return combosApplicacao;
            }
        }

        public List<Combo> GetAcessoEspecial(int matricula)
        {
            // ------------ usage
            //var acessoEspecial = GetAcessoEspecial(idClient);
            //if (acessoEspecial.Any())
            //    comboPermitido.AddRange(acessoEspecial);

            using (var ctx = new DesenvContext())
            {

                return (from a in ctx.tblAccess_Combo
                        join c in ctx.tblMedSoft_AcessoEspecial on a.intObjectId equals c.intObjectId
                        where c.intClientID == matricula
                        select new Combo()
                        {
                            ComboId = a.intProductGroup2Id.Value,
                            Nome = a.txtNome,
                            Id = a.intObjectId
                        }).ToList();
            }
        }

        public List<ProdutoComboLiberadoDTO> GetProdutoComboLiberado(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                return (from cl in ctx.tblProdutoComboLiberado
                             join ac in ctx.tblAccess_Combo
                             on cl.intCurso equals ac.intProductGroup2Id
                        where cl.intClientId == matricula
                        select new ProdutoComboLiberadoDTO
                        {
                            intCurso = cl.intCurso,
                            Nome = ac.txtNome,
                            intYear = cl.intYear,
                            Id = ac.intObjectId,
                            Ordem = ac.intOrdem ?? 0,
                            ProdutoFake = cl.bitFake
                        }).ToList();
            }
            
        }


        public Dictionary<int, int[]> GetAnosPorProduto(int idClient)
        {
            using (var ctx = new DesenvContext())
            {
                var anoAtual = Utilidades.GetYear();
                var anoSeguinte = anoAtual + 1;
                var alunoMeioDeAno = Utilidades.IsCicloCompletoNoMeioDoAno(idClient);
                var alunoMeioDeAnoAnosAnteriores = Utilidades.CicloCompletoAnosAnterioresNoMeioDoAno(idClient);

                var produtosR3 = Utilidades.ProdutosR3();
                var produtoMedEletroIMed = Produto.Produtos.MEDELETRO_IMED.GetHashCode();
                //Listar os anos por produto onde o aluno tem OV Ativa e Adimplente.
                var lstAnoPorProduto = (from so in ctx.tblSellOrders
                                        join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                        join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                        join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                                        where so.intClientID == idClient
                                            && ((so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                                    || (anoAtual == c.intYear
                                                        && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                                                        && alunoMeioDeAno
                                                    )
                                                    || (so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                                                        && alunoMeioDeAnoAnosAnteriores.Contains(c.intYear.Value)
                                                    )
                                                  || (so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada)
                                                ) || (produtosR3.Contains(p.intProductGroup1.Value) && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada)
                                                || (p.intProductGroup1.Value == produtoMedEletroIMed && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada)
                                                || (p.intProductGroup1.Value == (int)Produto.Produtos.MED_MASTER && so.intStatus == (int)Utilidades.ESellOrderStatus.Pendente))
                                                && c.intYear <= anoSeguinte
                                        orderby c.intYear descending
                                        select new
                                        {
                                            id = p.intProductGroup1.Value,
                                            ano = c.intYear.Value
                                        }).AsEnumerable().Distinct()
                                        .GroupBy(x => x.id)
                                        .ToDictionary(x => x.Key, x => x.Select(y => y.ano).ToArray());

                return lstAnoPorProduto;
            }
        }


    }
}