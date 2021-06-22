// project=LibBuilder.Data, file=ProcessModel.cs, create=09:16 Copyright (c) 2021 tuke
// productions. All rights reserved.
using System.ComponentModel.DataAnnotations.Schema;

namespace LibBuilder.Data.Models
{
    /// <summary>
    /// ProcessModel.
    /// </summary>
    /// <seealso cref="Data.Models.BaseEntity" />
    public class ProcessModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public int? Error { get; set; }

        /// <summary>
        /// Gets or sets the sucess.
        /// </summary>
        /// <value>The sucess.</value>
        public int? Sucess { get; set; }

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