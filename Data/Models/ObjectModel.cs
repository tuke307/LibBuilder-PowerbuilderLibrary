using PBDotNetLib.orca;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class ObjectModel : BaseEntity
    {
        public bool Regenerate { get; set; }
        public string Name { get; set; }
        public Objecttype? ObjectType { get; set; }
        public int? LibraryId { get; set; }

        [ForeignKey("LibraryId")]
        public virtual LibraryModel Library { get; set; }
    }
}