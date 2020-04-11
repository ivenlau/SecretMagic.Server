using System;

namespace SecretMagic.Model
{
    public class UserInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public Guid RoleId { get; set; }

        public string Role { get; set; }
    }
}