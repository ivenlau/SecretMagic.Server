using SecretMagic.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SecretMagic.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public async Task<User> Create(User entity)
        {
            return await this.AddAsync(entity);
        }

        public IQueryable<User> Read()
        {
            return this.GetAll();
        }

        public async Task<int> Update(User entity)
        {
            var currentUser= this.Get(entity.Id);
            currentUser.Name = entity.Name;
            currentUser.Email = entity.Email;
            return await this.UpdateAsync(currentUser);
        }

        public async Task<int> Delete(User entity)
        {
            var currentUser= this.Get(entity.Id);
            return await this.DeleteAsync(currentUser);
        }

        public async Task<User> ValidateUser(string account, string passWord)
        {
            var user = await this.GetAsync(u=>u.Name == account && u.Password == passWord);
            if(user != null)
            {
                user.Password = string.Empty;
            }
            return user;
        }

        public User GetById(Guid id)
        {
            return this.Get(id);
        }

        public async Task<int> ReadCount()
        {
            return await this.CountAsync();
        }
    }
}
