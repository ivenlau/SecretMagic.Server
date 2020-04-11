using SecretMagic.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SecretMagic.Repository
{
    public class UrmRepository : Repository<UserRoleMapping>, IUrmRepository
    {
        public UrmRepository(DataContext context) : base(context)
        {
        }

        public async Task<UserRoleMapping> Create(UserRoleMapping entity)
        {
            return await this.AddAsync(entity);
        }

        public async Task<int> Delete(UserRoleMapping entity)
        {
            return await this.DeleteAsync(entity);
        }

        public IQueryable<UserRoleMapping> Read()
        {
           return this.GetAll();
        }

        public async Task<int> Update(UserRoleMapping entity)
        {
            var current = this.Get(entity.Id);
            current.RoleId = entity.RoleId;
            current.UserId = entity.UserId;
            return await this.UpdateAsync(current);
        }

        public async Task<UserRoleMapping> GetUrmByUserId(Guid userId)
        {
            return await this.GetAsync( x=> x.UserId == userId);
        }

        public UserRoleMapping GetById(Guid id)
        {
            return this.Get(id);
        }

        public async Task<int> ReadCount()
        {
            return await this.CountAsync();
        }
    }
}
