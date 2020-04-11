using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecretMagic.Model
{
    public class User : CoreEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}