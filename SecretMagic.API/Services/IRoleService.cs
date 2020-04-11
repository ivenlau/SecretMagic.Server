using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretMagic.Model;

namespace SecretMagic.API.Services
{
    public interface IRoleService
    {
        IEnumerable<RoleInfo> GetAllRoles();
        Task AddRole(RoleInfo roleInfo);
        void UpdateRole(RoleInfo roleInfo);
        Task<Role> AddRole(Role role);
        Task<Permission> AddPermission(Permission permission);
        Task<int> DeleteRole(Guid id);
        Task<int> DeletePermission(Permission permission);
        IEnumerable<Resource> GetAllResources();
    }
}