using System;
using System.Linq;
using System.Threading.Tasks;
using CAContext.Infra.Data.Context;
using CAContext.Domain.Entities;
using CAContext.Domain.Interfaces.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace CAContext.Infra.Data.Repository
{
    public class ContribuicaoRepository : BaseRepository<Contribuicao>, IContribuicaoRepository
    {
        public ContribuicaoRepository(ContribuicoesContext context)
            : base(context)
        {
        }

        public override Task Adicionar(Contribuicao e)
        {
            e.DataCriacao = DateTime.Now;
            return base.Adicionar(e);
        }

        public bool TemDescricaoRepetida(string descricao)
        {
            var cn = DB.Database.GetDbConnection().ConnectionString;

            string sql = String.Format("SELECT TOP 1 * FROM tblContribuicao WHERE txtDescricao = '{0}'", descricao);
            using(var con = new SqlConnection(cn))
            {
                return con.Query<Contribuicao>(sql).Any();
            }
        }
    }
}