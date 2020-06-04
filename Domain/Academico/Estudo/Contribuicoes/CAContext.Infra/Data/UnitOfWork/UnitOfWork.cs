using System.Threading.Tasks;
using CAContext.Infra.Data.Context;
using Shared.Interfaces;

namespace CAContext.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContribuicoesContext _context;

        public UnitOfWork(ContribuicoesContext context)
        {
            _context = context;
        }

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}