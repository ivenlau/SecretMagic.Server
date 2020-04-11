using System;
using SecretMagic.Model;

namespace SecretMagic.API.Services
{
    public interface IOssService
    {
        OssStsCredentials GetOssStsToken();
        Uri GetAssignedUri(string key);
    }
}