// project=PBDotNet.Core, file=ILibrary.cs, create=09:16 Copyright (c) 2021 tuke
// productions. All rights reserved.
using PBDotNet.Core.orca;

namespace PBDotNet.Core.pbuilder
{
    public interface ILibrary
    {
        string Dir { get; }

        ILibEntry[] EntryList { get; }

        string File { get; }
    }
}