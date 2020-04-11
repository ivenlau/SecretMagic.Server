using System.ComponentModel.DataAnnotations;

namespace SecretMagic.Model
{
    public class Comments : CoreEntity
    {
        [Required]
        public string ReaderName { get; set; }

        [Required]
        public string Content { get; set; }
    }
}