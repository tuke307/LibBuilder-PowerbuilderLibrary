using Data.Models;
using PBDotNetLib.orca;
using PBDotNetLib.pbuilder;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibBuilder.Core
{
    public static class Orca
    {
        /// <summary>
        /// Fügt neue, noch nicht vorhandene Targets für ein Workspace hinzu
        /// </summary>
        /// <param name="dbWorkspace">Der zu aktualisierende Workspace</param>
        /// <returns>Aktualisiert Workspace-Targets</returns>
        public static WorkspaceModel UpdateWorkspaceTargets(WorkspaceModel dbWorkspace)
        {
            //Orca Workspace öffnen
            Workspace pbWorkspace = new Workspace(dbWorkspace.FilePath, dbWorkspace.PBVersion.Value);

            //neue Targets werden gesucht
            var newTargets = pbWorkspace.Targets.Select(t => t.File).ToList().Except(dbWorkspace.Target.Select(t => t.File).ToList()).ToList();

            //neue Targets hinzufügen
            if (newTargets != null && newTargets.Count > 0)
            {
                for (int count = 0; count < newTargets.Count; count++)
                {
                    Target temp = pbWorkspace.Targets.Where(t => t.File == newTargets[count]).First();

                    dbWorkspace.Target.Add(new TargetModel()
                    {
                        File = temp.File,
                        Directory = temp.Dir
                    });
                }
            }

            return dbWorkspace;
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

            pbLibraryList = pbTarget.Libraries.Select(l => l?.FilePath).ToList();

            if (dbTarget.Librarys != null)
                dbLibraryList = dbTarget.Librarys.Select(l => l?.FilePath).ToList();

            //beide Listen vergleichen
            var difference = pbLibraryList.Except(dbLibraryList).ToList();

            //neue Librays hinzufügen
            for (int count = 0; count < difference.Count; count++)
                dbTarget.Librarys.Add(new LibraryModel()
                {
                    File = Path.GetFileName(difference[count]),
                    Directory = Path.GetDirectoryName(difference[count])
                });

            //Application Object des Targets muss geladen werden, da sie für Compile-Aufgaben gesetzt werden muss

            #region Application

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
        /// Fügt neue, noch nicht vorhandene Objects für eine Library hinzu
        /// </summary>
        /// <param name="dbLibrary">Die zu aktualisierende Library</param>
        /// <param name="version">Orca Version zum starten der Session</param>
        /// <returns>Aktualisiert Library-Objects</returns>
        public static LibraryModel UpdateLibrayObjects(LibraryModel dbLibrary, PBDotNetLib.orca.Orca.Version version)
        {
            //Powerbuilder-Objects für die selektierte Library holen
            List<LibEntry> pbObjects = new PBDotNetLib.orca.Orca(version).DirLibrary(dbLibrary.FilePath);

            //neue Objekte werden gesucht
            var newObjects = pbObjects.Select(l => l.Name).ToList().Except(dbLibrary.Objects.Select(l => l.Name).ToList()).ToList();

            //neue Objekte hinzufügen
            if (newObjects != null && newObjects.Count > 0)
            {
                for (int count = 0; count < newObjects.Count; count++)
                {
                    LibEntry temp = pbObjects.Where(o => o.Name == newObjects[count]).First();

                    dbLibrary.Objects.Add(new ObjectModel()
                    {
                        Name = temp.Name,
                        ObjectType = temp.Type
                    });
                }
            }

            return dbLibrary;
        }
    }
}