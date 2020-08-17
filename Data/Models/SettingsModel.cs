// project=Data, file=SettingsModel.cs, create=20:12 Copyright (c) 2020 tuke productions.
// All rights reserved.
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    /// <summary>
    /// SettingsModel.
    /// </summary>
    /// <seealso cref="Data.Models.BaseEntity" />
    public class SettingsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether [dark mode].
        /// </summary>
        /// <value><c>true</c> if [dark mode]; otherwise, <c>false</c>.</value>
        [Required]
        public bool DarkMode { get; set; }

        /// <summary>
        /// Gets or sets the color of the primary.
        /// </summary>
        /// <value>The color of the primary.</value>
        [Required]
        public string PrimaryColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the secondary.
        /// </summary>
        /// <value>The color of the secondary.</value>
        [Required]
        public string SecondaryColor { get; set; }
    }
}