using SecretMagic.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SecretMagic.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext context) : base(context)
        {
        }

        public async Task<Category> Create(Category entity)
        {
            return await this.AddAsync(entity);
        }

        public async Task<int> Delete(Category entity)
        {

            return await this.DeleteAsync(entity);
        }

        public Category GetById(Guid id)
        {
            return this.Get(id);
        }

        public IQueryable<Category> Read()
        {
            return this.GetAll();
        }

        public async Task<int> ReadCount()
        {
            return await this.CountAsync();
        }

        public Task<int> Update(Category entity)
        {
            return this.UpdateAsync(entity);
        }
    }
}
