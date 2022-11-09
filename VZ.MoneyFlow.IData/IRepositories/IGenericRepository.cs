using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VZ.MoneyFlow.Models.Paging;

namespace VZ.MoneyFlow.IData.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<PagedResult<TResult>> GetAllPagedAsync<TResult>(QueryParameters queryParameters, Expression<Func<T, bool>> filter = null);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task DeleteAsync(string id);
        Task DeleteAsync(params object[] keys);
        Task SaveChangesAsync();
    }
}
