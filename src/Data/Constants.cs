// project=Data, file=Constants.cs, create=20:12 Copyright (c) 2020 tuke productions. All
// rights reserved.
using System;
using System.IO;

namespace Data
{
    /// <summary>
    /// Constants.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// The database name.
        /// </summary>
        public const string DatabaseName = "libbuilder.db";

        /// <summary>
        /// The log name.
        /// </summary>
        public const string LogName = "libbuilder_log_{Date}.txt";

        /// <summary>
        /// Gets or sets the database path.
        /// </summary>
        /// <value>The database path.</value>
        public static string DatabasePath
        {
            get => Path.Combine(Constants.FileDirectory, Data.Constants.DatabaseName);
        }

        /// <summary>
        /// Gets the file directory.
        /// </summary>
        /// <value>The file directory.</value>
        public static string FileDirectory
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LibBuilder");
            }
        }

        /// <summary>
        /// Gets the log path.
        /// </summary>
        /// <value>The log path.</value>
        public static string LogPath
        {
            get => Path.Combine(Constants.FileDirectory, "logs", Data.Constants.LogName);
        }
    }
}