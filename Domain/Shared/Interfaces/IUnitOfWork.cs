using System;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Commit();
    }
}