using PBDotNetLib.orca;

namespace PBDotNetLib.pbuilder
{
    public interface ILibrary
    {
        string Dir { get; }
        string File { get; }
        ILibEntry[] EntryList { get; }
    }
}