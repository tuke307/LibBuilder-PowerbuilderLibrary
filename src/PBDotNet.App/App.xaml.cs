// project=PBDotNet.App2, file=App.xaml.cs, create=10:44 Copyright (c) 2021 tuke
// productions. All rights reserved.
using System.Windows;

namespace PBDotNet.App
{
    /// <summary>
    /// App.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                string path = e.Args[0];

                if (System.IO.File.Exists(path))
                {
                    if (path.EndsWith(".pbw"))
                    {
                        Util.CmdlineArgs.Workspace = path;
                    }
                    else if (path.EndsWith(".pbl"))
                    {
                        Util.CmdlineArgs.Library = path;
                    }
                }
            }
            base.OnStartup(e);
        }
    }
}