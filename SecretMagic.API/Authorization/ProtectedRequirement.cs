using Microsoft.AspNetCore.Authorization;

namespace SecretMagic.API.Authorization
{
    public class ProtectedRequirement : IAuthorizationRequirement
    {
        public string Resource { get; private set; }

        public ProtectedRequirement(string resource) { Resource = resource; }
    }
}