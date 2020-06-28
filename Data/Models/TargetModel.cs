using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Data.Models
{
    public class TargetModel : BaseEntity
    {
        public string Directory { get; set; }
        public string File { get; set; }

        [NotMapped]
        public string FilePath { get { return Path.Combine(Directory, File); } }

        public int? WorkspaceId { get; set; }

        [ForeignKey("WorkspaceId")]
        public virtual WorkspaceModel Workspace { get; set; }

        public virtual IList<LibraryModel> Librarys { get; set; }

        public virtual IList<ProcessModel> Process { get; set; }
    }
}