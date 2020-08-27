// project=LibBuilder.Core, file=Orca.cs, creation=2020:7:21 Copyright (c) 2020 Timeline
// Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.Core
{
    using Data.Models;
    using PBDotNetLib.orca;
    using PBDotNetLib.pbuilder;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// PB zu Datenbank.
    /// TODO: Überprüfen ob remove funktioniert
    /// </summary>
    public static class Orca
    {
        /// <summary>
        /// Fügt neue, noch nicht vorhandene Objects für eine Library hinzu
        /// TODO: vergleichen des Object namens/typ, nicht nur des Namens
        /// </summary>
        /// <param name="dbLibrary">Die zu aktualisierende Library</param>
        /// <param name="version">Orca Version zum starten der Session</param>
        /// <returns>Aktualisiert Library-Objects</returns>
        public static LibraryModel UpdateLibrayObjects(LibraryModel dbLibrary, PBDotNetLib.orca.Orca.Version version)
        {
            //Powerbuilder-Objects für die selektierte Library holen
            List<LibEntry> pbObjects = new PBDotNetLib.orca.Orca(version).DirLibrary(dbLibrary.FilePath);

            List<string> pbObjectList = new List<string>();
            List<string> dbObjectList = new List<string>();

            // einlesen
            pbObjectList = pbObjects?.Select(l => l.Name).ToList();
            dbObjectList = dbLibrary?.Objects?.Select(t => t.Name).ToList();

            //beide Listen vergleichen
            var differenceToAdd = pbObjectList.Except(dbObjectList).ToList();
            var differenceToRemove = dbObjectList.Except(pbObjectList).ToList();

            // neue Targets hinzufügen
            foreach (var item in differenceToAdd)
            {
                LibEntry temp = pbObjects.Where(o => o.Name.Equals(item)).First();

                dbLibrary.Objects.Add(new ObjectModel()
                {
                    Name = temp.Name,
                    ObjectType = temp.Type
                });
            }

            // alte Targets löschen
            foreach (var item in differenceToRemove)
            {
                var _object = dbLibrary.Objects.Single(l => l.Name.ToLower().Equals(item.ToLower()));
                dbLibrary.Objects.Remove(_object);
            }

            return dbLibrary;
        }

        /// <summary>
        /// Fügt neue, noch nicht vorhandene Librarys für ein Targets hinzu
        /// TODO: Remove einfügen
        /// </summary>
        /// <param name="dbTarget">Das zu aktualisierende Target</param>
        /// <param name="version">Orca Version zum starten der Session</param>
        /// <returns>Aktualisiert Target-Librarys</returns>
        public static TargetModel UpdateTargetLibraries(TargetModel dbTarget, PBDotNetLib.orca.Orca.Version version)
        {
            Target pbTarget = new Target(dbTarget.FilePath, version);

            List<string> pbLibraryList = new List<string>();
            List<string> dbLibraryList = new List<string>();

            // einlesen
            pbLibraryList = pbTarget.Libraries.Select(l => l?.FilePath).ToList();
            dbLibraryList = dbTarget?.Librarys?.Select(l => l?.FilePath).ToList();

            //beide Listen vergleichen
            var differenceToAdd = pbLibraryList.Except(dbLibraryList).ToList();
            var differenceToRemove = dbLibraryList.Except(pbLibraryList).ToList();

            // neue Librays hinzufügen
            foreach (var item in differenceToAdd)
            {
                dbTarget.Librarys.Add(new LibraryModel()
                {
                    File = Path.GetFileName(item),
                    Directory = Path.GetDirectoryName(item)
                });
            }

            // alte Libraries löschen
            foreach (var item in differenceToRemove)
            {
                var library = dbTarget.Librarys.Single(l => l.FilePath.ToLower().Equals(item.ToLower()));
                dbTarget.Librarys.Remove(library);
            }

            #region Application

            // Application Object des Targets muss geladen werden, da sie für
            // Compile-Aufgaben gesetzt werden muss

            var applLibrary = pbTarget.Libraries.Where(l => l.EntryList.Where(o => o.Type == Objecttype.Application).Any()).First();
            var dbLibrary = dbTarget.Librarys.Where(l => l.File == applLibrary.File).First();

            //löschen in target-Library liste
            dbTarget.Librarys.Remove(dbLibrary);

            //mit objects wieder hinzufügen
            dbLibrary.Objects = new List<ObjectModel>();
            dbTarget.Librarys.Add(UpdateLibrayObjects(dbLibrary, version));

            #endregion Application

            return dbTarget;
        }

        /// <summary>
        /// Fügt neue, noch nicht vorhandene Targets für ein Workspace hinzu
        /// </summary>
        /// <param name="dbWorkspace">Der zu aktualisierende Workspace</param>
        /// <returns>Aktualisiert Workspace-Targets</returns>
        public static WorkspaceModel UpdateWorkspaceTargets(WorkspaceModel dbWorkspace)
        {
            //Orca Workspace öffnen
            Workspace pbWorkspace = new Workspace(dbWorkspace.FilePath, dbWorkspace.PBVersion.Value);

            List<string> pbTargetList = new List<string>();
            List<string> dbTargetList = new List<string>();

            // einlesen
            pbTargetList = pbWorkspace?.Targets?.Select(t => Path.Combine(t.Dir, t.File)).ToList();
            dbTargetList = dbWorkspace?.Target?.Select(t => t.FilePath).ToList();

            //beide Listen vergleichen
            var differenceToAdd = pbTargetList.Except(dbTargetList).ToList();
            var differenceToRemove = dbTargetList.Except(pbTargetList).ToList();

            // neue Targets hinzufügen
            foreach (var item in differenceToAdd)
            {
                dbWorkspace.Target.Add(new TargetModel()
                {
                    File = Path.GetFileName(item),
                    Directory = Path.GetDirectoryName(item)
                });
            }

            // alte Targets löschen
            foreach (var item in differenceToRemove)
            {
                var target = dbWorkspace.Target.Single(l => l.FilePath.ToLower().Equals(item.ToLower()));
                dbWorkspace.Target.Remove(target);
            }

            return dbWorkspace;
        }
    }
}