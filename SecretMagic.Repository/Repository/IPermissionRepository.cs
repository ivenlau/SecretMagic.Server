using SecretMagic.Model;
using System;
using System.Linq;
namespace SecretMagic.Repository
{
    public interface IPermissionRepository : ICrudable<Permission>
    {
        IQueryable<Permission> GetAllByRoleId(Guid roldId);
    }
}