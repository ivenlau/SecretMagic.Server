using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecretMagic.Model
{
    public class Blog : CoreEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public User Author { get; set; }

        [Required]
        public Category Category { get; set; }

        public string ImgUrl { get; set; }

        public ICollection<Comments> Commnents { get; set; }
    }
}