using SecretMagic.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SecretMagic.Repository
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(DataContext context) : base(context)
        {
        }

        public async Task<Permission> Create(Permission entity)
        {
            return await this.AddAsync(entity);
        }

        public async Task<int> Delete(Permission entity)
        {
            var current = this.Get(entity.Id);
            return await this.DeleteAsync(current);
        }

        public Permission GetById(Guid id)
        {
            return this.Get(id);
        }

        public IQueryable<Permission> GetAllByRoleId(Guid roldId)
        {
            return this.GetAll(p => p.RoleId == roldId);
        }

        public IQueryable<Permission> Read()
        {
            return this.GetAll();
        }

        public async Task<int> Update(Permission entity)
        {
            var current = this.Get(entity.Id);
            current.ResourceId = entity.ResourceId;
            current.RoleId = entity.RoleId;
            return await this.UpdateAsync(current);
        }

        public async Task<int> ReadCount()
        {
            return await this.CountAsync();
        }
    }
}
