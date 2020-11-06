using System.ComponentModel.DataAnnotations;

namespace SecretMagic.Model
{
    public class Resource : CoreEntity
    {
        [Required]
        public string Name { get; set; }


        public string Category { get; set; }

        public string Description { get; set; }
    }
}