using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class SettingsModel : BaseEntity
    {
        [Required]
        public bool DarkMode { get; set; }

        [Required]
        public string PrimaryColor { get; set; }

        [Required]
        public string SecondaryColor { get; set; }
    }
}