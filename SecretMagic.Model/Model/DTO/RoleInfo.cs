using System;

namespace SecretMagic.Model
{
    public class RoleInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public Resource[] Resources { get;set; }
    }
}