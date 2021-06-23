// project=PBDotNet.Core, file=Workspace.cs, create=09:16 Copyright (c) 2021 Timeline
// Financials GmbH & Co. KG. All rights reserved.
using PBDotNet.Core.common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace PBDotNet.Core.pbuilder
{
    /// <summary>
    /// pb workspace
    /// </summary>
    public class Workspace : PBProjFile
    {
        #region private

        private string defaultRemoteTarget;
        private string defaultTarget;
        private List<Tuple<string, int, bool, bool>> targets = new List<Tuple<string, int, bool, bool>>();

        #endregion private

        #region properties

        public Target[] Targets
        {
            get
            {
                List<Target> targList = new List<Target>();

                foreach (Tuple<string, int, bool, bool> targ in targets)
                {
                    targList.Add(new Target(targ.Item1, this.version, targ.Item2, targ.Item3, targ.Item4));
                }

                return targList.ToArray();
            }
        }

        #endregion properties

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="file">path to workspace</param>
        /// <param name="version">PB Version</param>
        public Workspace(string file, orca.Orca.Version version) : base(file, version)
        {
        }

        /// <summary>
        /// parse source of workspace
        /// </summary>
        /// <param name="source">source of workspace</param>
        protected override void Parse(string source)
        {
            base.Parse(source);

            ParseDefaultTargets(source);
            ParseTargets(source);
        }

        /// <summary>
        /// find the default targets in workspace
        /// </summary>
        /// <param name="source">source of workspace</param>
        private void ParseDefaultTargets(string source)
        {
            MatchCollection matches = null;

            matches = Regex.Matches(source, "DefaultTarget \"(?<defaulttarget>.*)\";", RegexOptions.IgnoreCase);
            if (matches.Count > 0)
                defaultTarget = matches[0].Groups["defaulttarget"].Value;

            matches = Regex.Matches(source, "DefaultRemoteTarget \"(?<defaultremtarget>.*)\";", RegexOptions.IgnoreCase);
            if (matches.Count > 0)
                defaultRemoteTarget = matches[0].Groups["defaultremtarget"].Value;
        }

        /// <summary>
        /// parse the target list in workspace
        /// </summary>
        /// <param name="source"></param>
        private void ParseTargets(string source)
        {
            MatchCollection matches = null;

            matches = Regex.Matches(source, "@begin Targets\r\n(?<target>.*\r\n)*?@end", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            if (matches.Count == 0) return;

            foreach (Capture cap in matches[0].Groups["target"].Captures)
            {
                string capture, path;
                bool defTargetFlag = false, defRemTarFlag = false;
                MatchCollection matchesTarget = null;

                capture = cap.Value;
                matchesTarget = Regex.Matches(capture, "(?<order>[0-9]+).*\"(?<path>.*)\"", RegexOptions.IgnoreCase);

                if (matchesTarget.Count == 0) continue;

                path = matchesTarget[0].Groups["path"].Value;

                if (path.Equals(defaultTarget))
                    defTargetFlag = true;

                if (path.Equals(defaultRemoteTarget))
                    defRemTarFlag = true;

                // TODO: verbessern!!!
                if (path.IndexOf(':') < 0)
                    path = Path.Combine(Dir, path).Replace(@"\\", @"\");

                //DirectoryInfo directoryInfo = new DirectoryInfo(@"\\tlfi-sql2");
                //DirectoryInfo directoryInfo2 = new DirectoryInfo(@"\\tlfi-sql2\TL_kunden");
                path = Path.GetFullPath(path);
                if (path.StartsWith(@"C:\tlfi-sql2"))
                {
                    path = path.Replace(@"C:\tlfi-sql2", @"\\tlfi-sql2");
                }

                targets.Add(new Tuple<string, int, bool, bool>(path, Int32.Parse(matchesTarget[0].Groups["order"].Value), defTargetFlag, defRemTarFlag));
            }
        }
    }
}