using PBDotNetLib.common;
using PBDotNetLib.orca;
using System.IO;

namespace PBDotNetLib.pbuilder
{
    /// <summary>
    /// pb library
    /// </summary>
    public class Library : PBSrcFile, ILibrary
    {
        #region private

        private string dir;
        private string file;
        private Orca orca = null;

        #endregion private

        #region properties

        public string Dir
        {
            get
            {
                return dir;
            }
        }

        public string File
        {
            get
            {
                return file;
            }
        }

        public string FilePath
        {
            get
            {
                return Path.Combine(Dir, File);
            }
        }

        #endregion properties

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="file">path to lib</param>
        /// <param name="version">PB version</param>
        public Library(string file, Orca.Version version)
        {
            this.orca = new Orca(version);
            this.dir = Path.GetDirectoryName(file);
            this.file = Path.GetFileName(file);
        }

        public ILibEntry[] EntryList
        {
            get
            {
                return orca.DirLibrary(FilePath).ToArray();
            }
        }
    }
}