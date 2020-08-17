// project=PBDotNetLib, file=VirtualLibrary.cs, creation=2020:6:28 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.
using PBDotNetLib.orca;
using System.IO;
using System.Linq;

namespace PBDotNetLib.pbuilder
{
    public class VirtualLibrary : ILibrary
    {
        private DirectoryInfo dirInfo = null;

        public string Dir
        {
            get { return dirInfo.FullName; }
        }

        public orca.ILibEntry[] EntryList
        {
            get
            {
                var files = Directory
                    .GetFiles(this.Dir, "*.*")
                    .Where(f => f.ToLower().EndsWith(".psr") || f.Substring(f.Length - 3, 2).ToLower() == "sr")
                    .ToList();

                var entries = new VirtualLibEntry[files.Count];

                for (int i = 0; i < files.Count; i++)
                {
                    entries[i] = new VirtualLibEntry(files[i]);
                }

                return entries;
            }
        }

        public string File
        {
            get { return dirInfo.Name; }
        }

        public VirtualLibrary(string folder)
        {
            this.dirInfo = new DirectoryInfo(folder);
        }
    }
}