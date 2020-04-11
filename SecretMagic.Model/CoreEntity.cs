using System;
using System.ComponentModel.DataAnnotations;

namespace SecretMagic.Model
{
    public class CoreEntity : ICoreEntity
    {
        [Key]
        public Guid Id { set; get; } = Guid.NewGuid();

        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }
}
