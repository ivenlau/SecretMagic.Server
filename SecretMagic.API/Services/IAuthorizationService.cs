using System.Threading.Tasks;

namespace SecretMagic.API.Services
{
    public interface IAuthorizationService
    {
        Task<bool> IsAuthorized(string userId, string[] resources);
    }
}