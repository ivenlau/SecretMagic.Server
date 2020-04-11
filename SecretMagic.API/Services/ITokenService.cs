namespace SecretMagic.API.Services
{
    public interface ITokenService
    {
        string CreateToken(string userId, string user, string role, string[] resources);
    }
}