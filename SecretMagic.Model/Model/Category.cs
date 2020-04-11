using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecretMagic.Model
{
    public class Category : CoreEntity
    {
        [Required]
        public string Name { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}