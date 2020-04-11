using SecretMagic.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SecretMagic.Repository
{
    public class ResourceRepository : Repository<Resource>, IResourceRepository
    {
        public ResourceRepository(DataContext context) : base(context)
        {
        }

        public async Task<Resource> Create(Resource entity)
        {
            return await this.AddAsync(entity);
        }

        public async Task<int> Delete(Resource entity)
        {
            var current = this.Get(entity.Id);
            return await this.DeleteAsync(current);
        }

        public Resource GetById(Guid id)
        {
            return this.Get(id);
        }

        public IQueryable<Resource> Read()
        {
            return this.GetAll();
        }

        public async Task<int> ReadCount()
        {
            return await this.CountAsync();
        }

        public async Task<int> Update(Resource entity)
        {
            var current = this.Get(entity.Id);
            current.Name = entity.Name;
            current.Category = entity.Category;
            current.Description = entity.Description;
            return await this.UpdateAsync(current);
        }
    }
}
