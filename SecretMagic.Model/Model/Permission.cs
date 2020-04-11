using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecretMagic.Model
{
    public class Permission: CoreEntity
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public Guid ResourceId { get; set; }
    }
}