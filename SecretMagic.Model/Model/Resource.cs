using System.ComponentModel.DataAnnotations;

namespace SecretMagic.Model
{
    public class Resource : CoreEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }
    }
}