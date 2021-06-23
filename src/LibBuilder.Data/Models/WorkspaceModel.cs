// project=LibBuilder.Data, file=WorkspaceModel.cs, create=09:16 Copyright (c) 2021
// Timeline Financials GmbH & Co. KG. All rights reserved.
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace LibBuilder.Data.Models
{
    /// <summary>
    /// WorkspaceModel.
    /// </summary>
    /// <seealso cref="Data.Models.BaseEntity" />
    public class WorkspaceModel : BaseEntity
    {
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
        /// Gets or sets the pb version.
        /// </summary>
        /// <value>The pb version.</value>
        public PBDotNet.Core.orca.Orca.Version? PBVersion { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public virtual IList<TargetModel> Target { get; set; }
    }
}