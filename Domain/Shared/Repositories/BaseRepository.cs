using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;

namespace Shared.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext DB;

        public BaseRepository(DbContext context)
        {
            DB = context;
        }

        public virtual Task<TEntity> ObterPorId(int id) 
        {
            return DB.Set<TEntity>().FindAsync(id).AsTask();
        } 

        public virtual Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DB.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            await DB.Set<TEntity>().AddAsync(entity);
        }

        public virtual TEntity Atualizar(TEntity entity)
        {
            DB.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual async Task Deletar(int id)
        {
            var entity = await ObterPorId(id);
            DB.Set<TEntity>().Remove(entity);
        }

        public virtual Task<List<TEntity>> Listar(Expression<Func<TEntity, bool>> predicate)
        {
            return DB.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual Task<List<TEntity>> Listar()
        {
            return DB.Set<TEntity>().ToListAsync();
        }

        public virtual Task<int> CountWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return DB.Set<TEntity>().CountAsync(predicate);
        }

        public void Dispose()
        {
            DB.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}