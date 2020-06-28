using System;

namespace PBDotNetLib.orca
{
    public interface ILibEntry
    {
        DateTime Createtime { get; }
        string Comment { get; }
        string Name { get; }
        int Size { get; }
        Objecttype Type { get; }
        string Library { get; }
        string Source { set; get; }
    }
}