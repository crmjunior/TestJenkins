using System;
using System.Threading.Tasks;
using CAContext.Infra.Data.Context;
using CAContext.Domain.Entities;
using CAContext.Domain.Interfaces.Repositories;
using Shared.Repositories;

namespace CAContext.Infra.Data.Repository
{
    public class ContribuicaoArquivoRepository : BaseRepository<ContribuicaoArquivo>, IContribuicaoArquivoRepository
    {
        public ContribuicaoArquivoRepository(ContribuicoesContext context)
            : base(context)
        {

        }

        public override Task Adicionar(ContribuicaoArquivo e)
        {
            e.DataCriacao = DateTime.Now;
            return base.Adicionar(e);
        }
    }
}