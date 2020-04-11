using System.Threading.Tasks;
using SecretMagic.Model;

namespace SecretMagic.API.Services
{
    public interface ILoginService
    {
        Task<LoginResult> Login(string userName, string password);
    }
}