using SecretMagic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SecretMagic.Repository
{
    public interface IRepository<T> where T: ICoreEntity
    {
        int Count();
        Task<int> CountAsync();
        int Delete(params T[] entities);
        Task<int> DeleteAsync(params T[] entities);
        T Get(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        T Get(Guid id);
        Task<T> GetAsync(Guid id);
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null);
        T Add(T entity);
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity);
        int Update(T entity, IEnumerable<string> fieldNames = null, bool commit = true);
        Task<int> UpdateAsync(T entity, IEnumerable<string> fieldNames = null, bool commit = true);
    }
}
