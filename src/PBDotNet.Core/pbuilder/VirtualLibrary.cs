// project=PBDotNet.Core, file=VirtualLibrary.cs, create=09:16 Copyright (c) 2021 tuke
// productions. All rights reserved.
using PBDotNet.Core.orca;
using System.IO;
using System.Linq;

namespace PBDotNet.Core.pbuilder
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