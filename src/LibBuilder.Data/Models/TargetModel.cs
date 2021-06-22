// project=LibBuilder.Data, file=TargetModel.cs, create=09:16 Copyright (c) 2021 tuke
// productions. All rights reserved.
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace LibBuilder.Data.Models
{
    /// <summary>
    /// TargetModel.
    /// </summary>
    /// <seealso cref="Data.Models.BaseEntity" />
    public class TargetModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the application rebuild.
        /// </summary>
        /// <value>The application rebuild.</value>
        public PBDotNet.Core.orca.Orca.PBORCA_REBLD_TYPE? ApplicationRebuild { get; set; }

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
        /// Gets or sets the librarys.
        /// </summary>
        /// <value>The librarys.</value>
        public virtual IList<LibraryModel> Librarys { get; set; }

        /// <summary>
        /// Gets or sets the process.
        /// </summary>
        /// <value>The process.</value>
        public virtual IList<ProcessModel> Process { get; set; }

        /// <summary>
        /// Gets or sets the workspace.
        /// </summary>
        /// <value>The workspace.</value>
        [ForeignKey("WorkspaceId")]
        public virtual WorkspaceModel Workspace { get; set; }

        /// <summary>
        /// Gets or sets the workspace identifier.
        /// </summary>
        /// <value>The workspace identifier.</value>
        public int? WorkspaceId { get; set; }
    }
}