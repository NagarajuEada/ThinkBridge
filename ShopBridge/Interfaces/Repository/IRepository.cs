using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShopBridge.Interfaces.Repository
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Task<List<T>> FetchAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes);

        Task<int> AddEntityAsync(T obj);


        Task<T> UpdateEntityAsync(T obj);


        Task<bool> DeleteEntityAsync( int id);
    }
}
