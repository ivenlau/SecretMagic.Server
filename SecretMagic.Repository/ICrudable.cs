using SecretMagic.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SecretMagic.Repository
{
    public interface ICrudable<T> where T: ICoreEntity
    {
        Task<T> Create(T entity);
        IQueryable<T> Read();
        Task<int> ReadCount();
        Task<int> Update(T entity);
        Task<int> Delete(T entity);
        T GetById(Guid id);
    }
}