// project=PBDotNet.Core, file=Target.cs, create=09:16 Copyright (c) 2021 tuke
// productions. All rights reserved.
using PBDotNet.Core.common;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace PBDotNet.Core.pbuilder
{
    /// <summary>
    /// pb target
    /// </summary>
    public class Target : PBProjFile
    {
        #region private

        private bool defaultRemoteTarget;
        private bool defaultTarget;

        //private string applLibName;
        //private string applName;
        private List<string> libs = new List<string>();

        private int order;

        #endregion private

        #region properties

        public Library[] Libraries
        {
            get
            {
                List<Library> libList = new List<Library>();

                foreach (string lib in libs)
                {
                    libList.Add(new Library(lib, this.version));
                }

                return libList.ToArray();
            }
        }

        #endregion properties

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="order">order number in worpspace</param>
        /// <param name="file">target file path</param>
        /// <param name="defaultTarget">fag if it is default target</param>
        /// <param name="defaultRemoteTarget">flag if it is default remote target</param>
        /// <param name="version">PB version</param>
        public Target(string file, orca.Orca.Version version, int order = 0, bool defaultTarget = false, bool defaultRemoteTarget = false)
            : base(file, version)
        {
            this.order = order;
            this.defaultTarget = defaultTarget;
            this.defaultRemoteTarget = defaultRemoteTarget;
        }

        /// <summary>
        /// parse source
        /// </summary>
        /// <param name="source">source of pbt</param>
        protected override void Parse(string source)
        {
            base.Parse(source);

            //ParseApplication(source);

            ParseLibList(source);
        }

        //private void ParseApplication(string source)
        //{
        //    MatchCollection matches1 = Regex.Matches(source, "appname", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        //    if (matches1.Count == 0) applName = null;
        //    else applName = matches1[0].Value;

        //    MatchCollection matches2 = Regex.Matches(source, "applib", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        //    if (matches1.Count == 0) applLibName = null;
        //    else applLibName = matches2[0].Value;
        //}

        /// <summary>
        /// complete path to get a absolute path of pbls in pbt
        /// </summary>
        /// <param name="list">list of pbls (relativ)</param>
        /// <returns>list of pbls (absolute)</returns>
        private List<string> CompletePath(string[] list)
        {
            List<string> resList = new List<string>();

            foreach (string lib in list)
            {
                if (!System.IO.File.Exists(lib))
                {
                    resList.Add(Path.GetFullPath(Path.Combine(Dir, lib)));
                }
                else
                {
                    resList.Add(Path.GetFullPath(lib));
                }
            }

            return resList;
        }

        /// <summary>
        /// parse liblist from source
        /// </summary>
        /// <param name="source">source of pbt</param>
        private void ParseLibList(string source)
        {
            string liblist;
            MatchCollection matches = null;

            matches = Regex.Matches(source, "LibList \"(?<liblist>[^\"]*)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (matches.Count == 0) return;

            liblist = matches[0].Groups["liblist"].Value;
            libs = new List<string>();

            libs = CompletePath(liblist.Split(new char[] { ';' }));
        }
    }
}