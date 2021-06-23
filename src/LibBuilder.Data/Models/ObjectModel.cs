// project=LibBuilder.Data, file=ObjectModel.cs, create=09:16 Copyright (c) 2021 Timeline
// Financials GmbH & Co. KG. All rights reserved.
using PBDotNet.Core.orca;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibBuilder.Data.Models
{
    /// <summary>
    /// ObjectModel.
    /// </summary>
    /// <seealso cref="Data.Models.BaseEntity" />
    public class ObjectModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        /// <value>The library.</value>
        [ForeignKey("LibraryId")]
        public virtual LibraryModel Library { get; set; }

        /// <summary>
        /// Gets or sets the library identifier.
        /// </summary>
        /// <value>The library identifier.</value>
        public int? LibraryId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the object.
        /// </summary>
        /// <value>The type of the object.</value>
        public Objecttype? ObjectType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ObjectModel" /> is
        /// regenerate.
        /// </summary>
        /// <value><c>true</c> if regenerate; otherwise, <c>false</c>.</value>
        public bool Regenerate { get; set; }
    }
}