using SecretMagic.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SecretMagic.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DataContext context) : base(context)
        {
        }

        public async Task<Role> Create(Role entity)
        {
            return await this.AddAsync(entity);
        }

        public async Task<int> Delete(Role entity)
        {
            var current = this.Get(entity.Id);
            return await this.DeleteAsync(current);
        }

        public Role GetById(Guid id)
        {
            return this.Get(id);
        }

        public IQueryable<Role> Read()
        {
            return this.GetAll();
        }

        public async Task<int> ReadCount()
        {
            return await this.CountAsync();
        }

        public async Task<int> Update(Role entity)
        {
            var current = this.Get(entity.Id);
            current.Name = entity.Name;
            return await this.UpdateAsync(current);
        }
    }
}
