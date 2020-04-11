using Microsoft.AspNetCore.Authorization;

namespace SecretMagic.API.Authorization
{
    public class ProtectedAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "ProtectedAPI";

        public ProtectedAttribute(string resource) => Resource = resource;

        // Get or set the Age property by manipulating the underlying Policy property
        public string Resource
        {
            get
            {
                return Policy.Substring(POLICY_PREFIX.Length);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value.ToString()}";
            }
        }
    }
}