using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace MedCore_DataAccess.Repository
{
    public class PagamentosClienteEntity
    {
                public static List<PagamentosCliente> GetPagamentosCliente(int intClientID, int[] intYear, int intOrderid = 0)
        {

            using (var ctx = new DesenvContext())
            {

                List<csp_CustomClient_PagamentosProdutosGeral_Result> final = new List<csp_CustomClient_PagamentosProdutosGeral_Result>();

                foreach (int ano in intYear)
                {
                    
                    List<csp_CustomClient_PagamentosProdutosGeral_Result> parcial = ctx.Set<csp_CustomClient_PagamentosProdutosGeral_Result>().FromSqlRaw("csp_CustomClient_PagamentosProdutosGeral @ClientId = {0}, @intYear = {1}, @intOrderId = {2}",intClientID, ano, intOrderid).ToList();
                    final.AddRange(parcial);
                }

                var lst = new List<PagamentosCliente>();
                //var count = final.ToList();
                foreach (var r in final)
                {
                    var pc = new PagamentosCliente
                    {
                        IDAluno = (int)r.intClientID,
                        Aluno = r.txtName,
                        intOrderID = (int)r.intOrderID,
                        CPF = r.txtRegister,
                        Ano = (int)r.intYear,
                        Mes = (int)r.intMonth,
                        Ref = r.txtComment,
                        DblSumOfDebits = (double)(r.dblSumOfPaymt < 0 ? r.dblSumOfPaymt : 0),
                        DblSumOfPaymt = r.dblSumOfPaymt > 0 ? r.dblSumOfPaymt : 0,
                        DblValue = (double)r.dblValue
                    };


                    lst.Add(pc);
                };
                return lst;
            }
        }
    }
}