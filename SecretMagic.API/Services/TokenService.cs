using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecretMagic.Model;

namespace SecretMagic.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenConfig config;
        private readonly ILogger<TokenService> logger;

        public TokenService(ILogger<TokenService> logger, IOptions<TokenConfig> config)
        {
            this.logger = logger;
            this.config = config.Value;
        }

        public string CreateToken(string userId, string user, string role, string[] resources = null)
        {
            var token = string.Empty;

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user));
            claims.Add(new Claim("UID", userId));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                config.Issuer,
                config.Audience,
                claims, expires: DateTime.Now.AddMinutes(config.AccessExpiration),
                signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return token;
        }
    }
}