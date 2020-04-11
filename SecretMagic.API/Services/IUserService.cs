using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretMagic.Model;

namespace SecretMagic.API.Services
{
    public interface IUserService
    {
        IEnumerable<UserInfo> GetAllUsers();
        Task UpdateUser(UserInfo userInfo);
        Task AddUser(UserInfo userInfo);
        Task<int> DeleteUser(Guid id);
    }
}