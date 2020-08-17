// project=PBDotNetLib, file=ILibEntry.cs, creation=2020:6:28 Copyright (c) 2020 Timeline
// Financials GmbH & Co. KG. All rights reserved.
using System;

namespace PBDotNetLib.orca
{
    public interface ILibEntry
    {
        string Comment { get; }

        DateTime Createtime { get; }

        string Library { get; }

        string Name { get; }

        int Size { get; }

        string Source { set; get; }

        Objecttype Type { get; }
    }
}