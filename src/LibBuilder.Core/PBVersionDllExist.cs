// project=LibBuilder.Core, file=PBVersionDllExist.cs, creation=2021:6:11 Copyright (c)
// 2021 Timeline Financials GmbH & Co. KG. All rights reserved.
using PBDotNetLib.orca;

namespace LibBuilder.Core
{
    public class PBVersionDllExist
    {
        public bool DllExist { get; private set; }

        public int PBVersion { get; private set; }

        public PBVersionDllExist(PBDotNetLib.orca.Orca.Version? version)
        {
            if (version.HasValue)
            {
                this.PBVersion = (int)version.Value;
                this.DllExist = Utils.CheckLibrary(version.GetAttributeOfType<DllNameAttribute>().DllName);
            }
        }
    }
}