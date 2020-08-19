// project=LibBuilder.WPFCore, file=Options.cs, creation=2020:8:17 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.
using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace LibBuilder.WPFCore.Business
{
    /// <summary>
    /// Options.
    /// </summary>
    public class Options
    {
        [Usage]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Rebuild aller Librarys", new Options { Workspace = "Kunden.pbw", Target = "tlfi_lokal.pbt", Build = true });
                yield return new Example("Rebuild einer Library", new Options { Workspace = "Kunden.pbw", Target = "tlfi_lokal.pbt", Librarys = new List<string>() { "client1.pbl" }, Build = true });
                yield return new Example("Rebuild einer Library und Regnerate dessen Objects", new Options { Workspace = "Kunden.pbw", Target = "tlfi_lokal.pbt", Librarys = new List<string>() { "client1.pbl" }, Build = true, Regenerate = true });
                yield return new Example("Regnerate der Objects von mehreren Librarys", new Options { Workspace = "Kunden.pbw", Target = "tlfi_lokal.pbt", Librarys = new List<string>() { "client1.pbl", "client2.pbl", "client3.pbl" }, Regenerate = true });
            }
        }

        [Option(shortName: 'a', longName: "Application", HelpText = "Applikation-Fenster anzeigen")]
        public bool? Application { get; set; }

        [Option(shortName: 'b', longName: "Build", HelpText = "Build einer Library")]
        public bool? Build { get; set; }

        [Option(shortName: 'l', longName: "Librarys", Separator = ';', HelpText = "Auswahl der Librays eines Targets")]
        public IEnumerable<string> Librarys { get; set; }

        [Option(shortName: 'x', longName: "RebuildType", HelpText = "Typ des Rebuild")]
        public PBDotNetLib.orca.Orca.PBORCA_REBLD_TYPE? RebuildType { get; set; }

        [Option(shortName: 'r', longName: "Regenerate", HelpText = "Regenerate aller Objects einer Library")]
        public bool? Regenerate { get; set; }

        [Option(shortName: 't', longName: "Target", HelpText = "Pfad bzw. Name des Targets")]
        public string Target { get; set; }

        [Option(shortName: 'v', longName: "Version", HelpText = "Powerbuilder Version des Workspace")]
        public PBDotNetLib.orca.Orca.Version? Version { get; set; }

        [Option(shortName: 'w', longName: "Workspace", Required = true, HelpText = "Pfad bzw. Name des Workspace")]
        public string Workspace { get; set; }
    }
}