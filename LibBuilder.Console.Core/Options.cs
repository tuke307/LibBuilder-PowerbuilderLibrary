// project=LibBuilder.WPFCore, file=Options.cs, creation=2020:8:17 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.
using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace LibBuilder.Console.Core
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
                yield return new Example("Hinzufügen eines Workspace und Build aller Librarys eines Targets", new Options
                {
                    Workspace = @"C:\tl_kunden\Kunden.pbw",
                    Target = "tlfi_lokal.pbt",
                    Build = true
                });

                yield return new Example("Rebuild einer Library", new Options
                {
                    Workspace = "Kunden.pbw",
                    Target = "tlfi_lokal.pbt",
                    Librarys = new List<string>()
                    {
                        "client1.pbl"
                    },
                    Build = true
                });

                yield return new Example("Rebuild einer Library und Regnerate dessen Objects", new Options
                {
                    Workspace = "Kunden.pbw",
                    Target = "tlfi_lokal.pbt",
                    Librarys = new List<string>()
                    {
                        "client1.pbl"
                    },
                    Build = true,
                    Regenerate = true
                });

                yield return new Example("Regnerate der Objects von mehreren Librarys", new Options
                {
                    Workspace = "Kunden.pbw",
                    Target = "tlfi_lokal.pbt",
                    Librarys = new List<string>()
                    {
                        "client1.pbl",
                        "client2.pbl",
                        "client3.pbl"
                    },
                    Regenerate = true
                });
            }
        }

        [Option(shortName: 'b', longName: nameof(Build), HelpText = "Build einer Library; true, false")]
        public bool? Build { get; set; }

        [Option(shortName: 'l', longName: nameof(Librarys), Separator = ';', HelpText = "Auswahl der Librays eines Targets")]
        public IEnumerable<string> Librarys { get; set; }

        [Option(shortName: 'x', longName: nameof(RebuildType), HelpText = "Typ des Rebuild; PBORCA_FULL_REBUILD(1), PBORCA_INCREMENTAL_REBUILD(2), PBORCA_MIGRATE(3), PBORCA_3PASS(4)")]
        public PBDotNetLib.orca.Orca.PBORCA_REBLD_TYPE? RebuildType { get; set; }

        [Option(shortName: 'r', longName: nameof(Regenerate), HelpText = "Regenerate aller Objects einer Library; true, false")]
        public bool? Regenerate { get; set; }

        [Option(shortName: 't', longName: nameof(Target), HelpText = "Pfad bzw. Name des Targets")]
        public string Target { get; set; }

        [Option(shortName: 'v', longName: nameof(Version), HelpText = "Powerbuilder Version des Workspace; PB105(105), PB125(125), PB170(170), PB190(190)")]
        public PBDotNetLib.orca.Orca.Version? Version { get; set; }

        [Option(shortName: 'w', longName: nameof(Workspace), Required = true, HelpText = "Pfad bzw. Name des Workspace")]
        public string Workspace { get; set; }
    }
}