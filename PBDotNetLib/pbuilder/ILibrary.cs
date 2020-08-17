// project=PBDotNetLib, file=ILibrary.cs, creation=2020:6:28 Copyright (c) 2020 Timeline
// Financials GmbH & Co. KG. All rights reserved.
using PBDotNetLib.orca;

namespace PBDotNetLib.pbuilder
{
    public interface ILibrary
    {
        string Dir { get; }

        ILibEntry[] EntryList { get; }

        string File { get; }
    }
}