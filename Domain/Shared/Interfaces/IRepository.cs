using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> ObterPorId(int id);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task Adicionar(TEntity entity);
        TEntity Atualizar(TEntity entity);
        Task Deletar(int id);
        Task<List<TEntity>> Listar(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> Listar();
        Task<int> CountWhere(Expression<Func<TEntity, bool>> predicate);
    }
}