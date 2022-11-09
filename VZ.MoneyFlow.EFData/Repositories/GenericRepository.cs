using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VZ.MoneyFlow.EFData.Data;
using VZ.MoneyFlow.IData.IRepositories;
using VZ.MoneyFlow.Models.Paging;

namespace VZ.MoneyFlow.EFData.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GenericRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await _context.Set<T>().AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            return result;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public async Task DeleteAsync(params object[] keys)
        {
            var entity = await _context.Set<T>().FindAsync(keys);
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public Task UpdateAsync(T entity)
        {
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task<PagedResult<TResult>> GetAllPagedAsync<TResult>(QueryParameters queryParameters, Expression<Func<T, bool>> filter = null)
        {
            var query = _context.Set<T>().AsQueryable();

            if (filter != null) query = query.Where(filter);

            var totalSize = await query.CountAsync();
            var items = await query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.RecordsPerPage)
                .Take(queryParameters.RecordsPerPage)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return new PagedResult<TResult>
            {
                Records = items,
                PageNumber = queryParameters.PageNumber,
                RecordsPerPage = queryParameters.RecordsPerPage,
                TotalCountOfRecords = totalSize,
            };
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            var accountCurrency = await _context.Set<T>().FirstOrDefaultAsync(filter);
            return accountCurrency;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
