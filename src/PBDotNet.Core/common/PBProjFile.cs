// project=PBDotNet.Core, file=PBProjFile.cs, create=09:16 Copyright (c) 2021 Timeline
// Financials GmbH & Co. KG. All rights reserved.
using System.IO;
using System.Text.RegularExpressions;

namespace PBDotNet.Core.common
{
    /// <summary>
    /// projectfile like a workspace or a target
    /// </summary>
	abstract public class PBProjFile
    {
        #region private

        protected orca.Orca.Version version;
        private string dir;
        private bool exists;
        private string file;
        private string majorVersion;
        private string minorVersion;

        #endregion private

        #region properties

        public string Dir
        {
            get
            {
                return dir;
            }
        }

        public bool Exists
        {
            get
            {
                return exists;
            }
        }

        public string File
        {
            get
            {
                return file;
            }
        }

        public string MajorVersion
        {
            get
            {
                return majorVersion;
            }
        }

        public string MinorVersion
        {
            get
            {
                return minorVersion;
            }
        }

        #endregion properties

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="file">the file to read</param>
        public PBProjFile(string file, orca.Orca.Version version)
        {
            string source;
            StreamReader reader = null;

            this.version = version;

            this.dir = Path.GetDirectoryName(file);
            this.file = Path.GetFileName(file);

            reader = new StreamReader(new FileStream(file, FileMode.Open));

            exists = true;

            source = reader.ReadToEnd();
            reader.Close();

            Parse(source);
        }

        /// <summary>
        /// Parse method can be extened in ancestor for reading liblist or containing
        /// targets etc.
        /// </summary>
        /// <param name="source">source to parse</param>
        protected virtual void Parse(string source)
        {
            ParseVersion(source);
        }

        /// <summary>
        /// method to parse version from file
        /// </summary>
        /// <param name="source">source to parse</param>
        private void ParseVersion(string source)
        {
            MatchCollection matches = null;

            matches = Regex.Matches(source, @"Save Format v(?<majorversion>[0-9]*\.[0-9])\((?<minorversion>[0-9]*)\)", RegexOptions.IgnoreCase);

            if (matches.Count == 0) return;

            majorVersion = matches[0].Groups["majorversion"].Value;
            minorVersion = matches[0].Groups["minorversion"].Value;
        }
    }
}