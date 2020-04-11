using SecretMagic.Model;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace SecretMagic.Repository
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        public BlogRepository(DataContext context) : base(context)
        {
        }

        public async Task<Blog> Create(Blog entity)
        {
            if(entity.Category == null)
            {
                entity.Category = Context.Set<Category>().FirstOrDefault( c => c.Name == "Default");
            }
            else
            {
                entity.Category = Context.Set<Category>().FirstOrDefault( c => c.Id == entity.Category.Id);
            }
            return await this.AddAsync(entity);
        }

        public async Task<int> Delete(Blog entity)
        {
            return await this.DeleteAsync(entity);
        }

        public IEnumerable<Blog> GetBlogByCategory(Guid categoryId)
        {
            return Context.Set<Category>().Include("Blogs").FirstOrDefault( c => c.Id == categoryId).Blogs;
        }

        public Blog GetById(Guid id)
        {
            return this.GetAll().Include("Category").Include("Author").FirstOrDefault(b => b.Id == id);
        }

        public IQueryable<Blog> Read()
        {
            return this.GetAll().Include("Category").Include("Author");
        }

        public async Task<int> ReadCount()
        {
            return await this.CountAsync();
        }

        public Task<int> Update(Blog entity)
        {
            return this.UpdateAsync(entity);
        }
    }
}
