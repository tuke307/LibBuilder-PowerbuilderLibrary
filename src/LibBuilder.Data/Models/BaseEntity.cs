// project=LibBuilder.Data, file=BaseEntity.cs, create=09:16 Copyright (c) 2021 Timeline
// Financials GmbH & Co. KG. All rights reserved.
using System;
using System.ComponentModel.DataAnnotations;

namespace LibBuilder.Data.Models
{
    /// <summary>
    /// BaseEntity.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        /// <value>The updated date.</value>
        public DateTime UpdatedDate { get; set; }
    }
}