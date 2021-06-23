// project=LibBuilder.Data, file=LibraryModel.cs, create=09:16 Copyright (c) 2021 Timeline
// Financials GmbH & Co. KG. All rights reserved.
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace LibBuilder.Data.Models
{
    /// <summary>
    /// LibraryModel.
    /// </summary>
    /// <seealso cref="Data.Models.BaseEntity" />
    public class LibraryModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LibraryModel" /> is
        /// build.
        /// </summary>
        /// <value><c>true</c> if build; otherwise, <c>false</c>.</value>
        public bool Build { get; set; }

        /// <summary>
        /// Gets or sets the directory.
        /// </summary>
        /// <value>The directory.</value>
        public string Directory { get; set; }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>The file.</value>
        public string File { get; set; }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <value>The file path.</value>
        [NotMapped]
        public string FilePath { get { return Path.Combine(Directory, File); } }

        /// <summary>
        /// Gets or sets the objects.
        /// </summary>
        /// <value>The objects.</value>
        public virtual IList<ObjectModel> Objects { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        [ForeignKey("TargetId")]
        public virtual TargetModel Target { get; set; }

        /// <summary>
        /// Gets or sets the target identifier.
        /// </summary>
        /// <value>The target identifier.</value>
        public int? TargetId { get; set; }
    }
}