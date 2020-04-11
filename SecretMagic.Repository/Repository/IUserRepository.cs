using SecretMagic.Model;
using System.Threading.Tasks;

namespace SecretMagic.Repository
{
    public interface IUserRepository : ICrudable<User>
    {
        Task<User> ValidateUser(string account, string passWord);
    }
}