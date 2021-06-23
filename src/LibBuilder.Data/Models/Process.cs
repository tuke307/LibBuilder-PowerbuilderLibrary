// project=LibBuilder.Data, file=Process.cs, create=09:16 Copyright (c) 2021 Timeline
// Financials GmbH & Co. KG. All rights reserved.
using System;
using System.Collections.Generic;
using System.Text;

namespace LibBuilder.Data.Models
{
    /// <summary>
    /// Process.
    /// </summary>
    public class Process
    {
        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        /// <value>The library.</value>
        public string Library { get; set; }

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public string Mode { get; set; }

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public string Object { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public PBDotNet.Core.orca.Orca.Result Result { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public string Target { get; set; }
    }
}