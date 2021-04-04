// project=Data, file=Process.cs, create=20:12 Copyright (c) 2020 tuke productions. All
// rights reserved.
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
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
        public PBDotNetLib.orca.Orca.Result Result { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public string Target { get; set; }
    }
}