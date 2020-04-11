using System.ComponentModel.DataAnnotations;

namespace SecretMagic.Model
{
    public class Role: CoreEntity
    {
        [Required]
        public string Name { get; set; }
    }
}