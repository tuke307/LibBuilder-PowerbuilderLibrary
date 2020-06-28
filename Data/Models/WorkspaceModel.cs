using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Data.Models
{
    public class WorkspaceModel : BaseEntity
    {
        public string Directory { get; set; }
        public string File { get; set; }

        [NotMapped]
        public string FilePath { get { return Path.Combine(Directory, File); } }

        public PBDotNetLib.orca.Orca.Version? PBVersion { get; set; }
        public virtual IList<TargetModel> Target { get; set; }
    }
}