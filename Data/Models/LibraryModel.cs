using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Data.Models
{
    public class LibraryModel : BaseEntity
    {
        public string Directory { get; set; }
        public string File { get; set; }

        [NotMapped]
        public string FilePath { get { return Path.Combine(Directory, File); } }

        public bool Build { get; set; }

        //public bool Regenerate { get; set; }
        public int? TargetId { get; set; }

        [ForeignKey("TargetId")]
        public virtual TargetModel Target { get; set; }

        public virtual IList<ObjectModel> Objects { get; set; }
    }
}