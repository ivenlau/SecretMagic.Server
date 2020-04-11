using SecretMagic.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace SecretMagic.Repository
{
    public class Repository<T> : IRepository<T> where T : CoreEntity
    {
        protected readonly DataContext Context;

        public Repository(DataContext context)
        {
            Context = context;
        }

        public int Count()
        {
            return Context.Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await Context.Set<T>().CountAsync();
        }

        public int Delete(params T[] entities)
        {
            Context.RemoveRange(entities);
            return Context.SaveChanges();
        }
        public async Task<int> DeleteAsync(params T[] entities)
        {
            Context.RemoveRange(entities);
            return await Context.SaveChangesAsync();
        }
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().FirstOrDefault(predicate);
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public T Get(Guid id)
        {
            return Context.Set<T>().SingleOrDefault(a => a.Id == id);
        }
        public async Task<T> GetAsync(Guid id)
        {
            return await Context.Set<T>().SingleOrDefaultAsync(a => a.Id == id);
        }
        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? Context.Set<T>() : Context.Set<T>().Where(predicate);
        }
        public T Add(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
            return entity;
        }
        public async Task<T> AddAsync(T entity)
        {
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity)
        {
            await Context.AddRangeAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        public int Update(T entity, IEnumerable<string> fieldNames = null, bool commit = true)
        {
            if (fieldNames == null || fieldNames.Count() == 0)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                Context.Attach(entity);
                foreach (var field in fieldNames)
                {
                    Context.Entry(entity).Property(field).IsModified = true;
                }
            }
            if (commit) return Context.SaveChanges();
            return 0;
        }
        public async Task<int> UpdateAsync(T entity, IEnumerable<string> fieldNames = null, bool commit = true)
        {
            if (fieldNames == null || fieldNames.Count() == 0)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                Context.Attach(entity);
                foreach (var field in fieldNames)
                {
                    Context.Entry(entity).Property(field).IsModified = true;
                }
            }
            if (commit) return await Context.SaveChangesAsync();
            return 0;
        }
    }
}
