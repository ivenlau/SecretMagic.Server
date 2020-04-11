using System.Threading.Tasks;

namespace SecretMagic.Repository
{
    public interface IAuthorizationRepository
    {
        Task<string[]> GetUserResource(string userId);
    }
}