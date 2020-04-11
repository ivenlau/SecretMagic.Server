using SecretMagic.Model;
using System;
using System.Threading.Tasks;
namespace SecretMagic.Repository
{
    public interface IUrmRepository : ICrudable<UserRoleMapping>
    {
        Task<UserRoleMapping> GetUrmByUserId(Guid userId);
    }
}