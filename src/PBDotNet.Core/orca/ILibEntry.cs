// project=PBDotNet.Core, file=ILibEntry.cs, create=09:16 Copyright (c) 2021 tuke
// productions. All rights reserved.
using System;

namespace PBDotNet.Core.orca
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