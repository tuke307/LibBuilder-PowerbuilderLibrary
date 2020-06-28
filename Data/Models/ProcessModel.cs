using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class ProcessModel : BaseEntity
    {
        public int? TargetId { get; set; }

        [ForeignKey("TargetId")]
        public virtual TargetModel Target { get; set; }

        public int? Error { get; set; }
        public int? Sucess { get; set; }
    }
}