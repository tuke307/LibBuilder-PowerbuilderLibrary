// project=LibBuilder.Core, file=PBVersionDllExist.cs, create=08:41 Copyright (c) 2021
// Timeline Financials GmbH & Co. KG. All rights reserved.
using PBDotNet.Core.orca;

namespace LibBuilder.Core
{
    public class PBVersionDllExist
    {
        public bool DllExist { get; private set; }

        public int PBVersion { get; private set; }

        public PBVersionDllExist(PBDotNet.Core.orca.Orca.Version? version)
        {
            if (version.HasValue)
            {
                this.PBVersion = (int)version.Value;
                this.DllExist = Utils.CheckLibrary(version.Value);
            }
        }
    }
}