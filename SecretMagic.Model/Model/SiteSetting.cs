using System.ComponentModel.DataAnnotations;

namespace SecretMagic.Model
{
    public class SiteSetting : CoreEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Language Language { get; set; }

        [Required]
        public Color Color { get; set; }

        public string UiSetting { get; set; }
    }

    public enum Language
    {
        Zh,
        En
    }

    public enum Color
    {
        Light,
        Dark
    }
}