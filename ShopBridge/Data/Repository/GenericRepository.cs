using Microsoft.EntityFrameworkCore;
using ShopBridge.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShopBridge.Data.Repository
{
    public class GenericRepository<T, C> : IRepository<T> where T : class where C : DbContext
    {
        /// <summary>
        /// DB Context
        /// </summary>
        private C _dbContext;

        /// <summary>
        /// Dataset Entity
        /// </summary>
        private DbSet<T> entity;

        public GenericRepository()
        {
            _dbContext = Activator.CreateInstance<C>();
            entity = _dbContext.Set<T>();
        }

        public async Task<int> AddEntityAsync(T obj)
        {
            entity.Add(obj);
            int result = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return result > 0 ? Convert.ToInt32(obj.GetType().GetProperty("Id").GetValue(obj, null)) : result;
        }

        public async Task<bool> DeleteEntityAsync(int id)
        {
            T obj = await entity.FindAsync(id).ConfigureAwait(false);
            if (obj == null)
                throw new NullReferenceException("Not found");
            entity.Remove(obj);
            int result = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return result > 0;
        }

        public Task<List<T>> FetchAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryableEntity = entity;

            foreach (Expression<Func<T, object>> include in includes)
                queryableEntity = queryableEntity.Include(include);

            if (filter != null)
            {
                queryableEntity = queryableEntity.Where(filter);
            }
            if (orderBy != null)
            {
                queryableEntity = orderBy(queryableEntity);
            }
            var results = queryableEntity.ToList();
            return Task.FromResult(results);
        }

        public async Task<T> UpdateEntityAsync(T obj)
        {
            entity.Attach(obj);
            _dbContext.Entry(obj).State = EntityState.Modified;
            int result = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return result > 0 ? obj : null;
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }

     
    }
}
