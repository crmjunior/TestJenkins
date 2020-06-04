using System.Threading.Tasks;
using Shared.Interfaces;

namespace Shared
{
    public class BaseService
    {
        private readonly IUnitOfWork _uow;

        public BaseService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        protected async Task<int> Commit()
        {
            return await _uow.Commit();
        }

    }
}