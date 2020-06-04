using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using System.Linq;

namespace MedCore_DataAccess.Repository
{
    public class ApostilaEntidadeEntity
    {
        public List<ApostilaEntidade> GetAll()
        {
            return GetAll(0);
        }

        public List<ApostilaEntidade> GetAll(int ano)
        {
            using (var ctx = new DesenvContext())
            {
                return (from bookEntity in ctx.tblBooks_Entities
                        join book in ctx.tblBooks on bookEntity.intID equals book.intBookEntityID
                        join product in ctx.tblProducts on book.intBookID equals product.intProductID
                        where product.intProductGroup1 == 4 && product.intProductGroup3 != null
                        && product.intProductGroup3.Value != -1
                        && (book.intYear == ano || (ano == 0))
                        select new ApostilaEntidade
                        {
                            ID = bookEntity.intID,
                            Nome = product.txtCode.Substring(5),
                            Descricao = bookEntity.txtDescription.Trim(),
                            IdProduto = (product.intProductGroup2 ?? 0)
                        })
                        .Distinct()
                        .ToList();
            }
        }        

        public List<Apostila> GetApostilasComUnificacaoPorAno(int entidadeId, int ano)
        {
            var listaEntidades = ObterEntidadesPorUnificadasPorAno(entidadeId, ano);
            using (var ctx = new DesenvContext())
            {
                var apostilas = (from b in ctx.tblBooks
                                 where listaEntidades.Contains(b.intBookEntityID ?? 0)
                                   && (b.intYear <= ano || ano == 0 || ano == -1)
                                 select new Apostila
                                 {
                                     ID = b.intBookID
                                 }).ToList();
                return apostilas;
            }
        }

        public List<long> ObterEntidadesPorUnificadasPorAno(int entidadeId, int ano)
        {
            List<long> lista = new List<long>();
            lista.Add(entidadeId);

            var apostilas = GetUnificadas(ano);
            lista.AddRange(apostilas.Where(a => a.EntityAtualId == entidadeId).Select(a => a.EntityAnteriorId));

            return lista;

        }

        public List<ApostilaUnificada> GetUnificadas(int ano)
        {
            using (var ctx = new DesenvContext())
            {
                var unificadas = (from u in ctx.tblApostilasUnificadas
                                  join b in ctx.tblBooks on u.intBookID equals b.intBookID
                                  join e in ctx.tblBooks_Entities on b.intBookEntityID equals e.intID
                                  join b1 in ctx.tblBooks on u.intBookIDAnterior equals b1.intBookID
                                  join e1 in ctx.tblBooks_Entities on b1.intBookEntityID equals e1.intID
                                  where (b.intYear == ano || ano == 0)
                                  select new ApostilaUnificada
                                  {
                                      BookAtualId = u.intBookID,
                                      BookAnteriorId = u.intBookIDAnterior,
                                      AnoUnificacao = b.intYear ?? 0,
                                      EntityAtualId = b.intBookEntityID ?? 0,
                                      EntityAtual = e.txtName.Trim(),
                                      EntityAnteriorId = b1.intBookEntityID ?? 0,
                                      EntityAnterior = e1.txtName.Trim()
                                  });
                return unificadas.ToList();
            }
        }
    }
}