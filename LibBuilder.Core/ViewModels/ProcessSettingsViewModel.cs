using Data;
using Data.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibBuilder.Core.ViewModels
{
    public class ProcessSettingsViewModel : MvxNavigationViewModel
    {
        public ProcessSettingsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            // UI bezogen
            SelectAllLibrarysCommand = new MvxCommand(SelectAllLibrarys);
            DeselectAllLibrarysCommand = new MvxCommand(DeselectAllLibrarys);
            SelectAllEntrysCommand = new MvxCommand(SelectAllEntrys);
            DeselectAllEntrysCommand = new MvxCommand(DeselectAllEntrys);

            //Speichern
            SaveWorkspaceCommand = new MvxAsyncCommand(async () => await SaveWorkspace());

            //VersionsListe
            PBVersions = Enum.GetValues(typeof(PBDotNetLib.orca.Orca.Version)).Cast<PBDotNetLib.orca.Orca.Version>().ToList();

            //RebuildType
            ApplicationRebuild = Enum.GetValues(typeof(PBDotNetLib.orca.Orca.PBORCA_REBLD_TYPE)).Cast<PBDotNetLib.orca.Orca.PBORCA_REBLD_TYPE>().ToList();

            using (var db = new DatabaseContext())
            {
                //Workspace Liste laden
                Workspaces = new ObservableCollection<WorkspaceModel>(db.Workspace.ToList());
            }
        }

        /// <summary>
        /// Deselektiert alle Objects(Entrys)
        /// </summary>
        protected void DeselectAllEntrys()
        {
            if (Objects == null)
                return;

            List<ObjectModel> temp = new List<ObjectModel>();
            temp = Objects.ToList();
            Objects.Clear();

            for (int i = 0; i < temp.Count; i++)
            {
                temp[i].Regenerate = false;
            }

            Objects = new ObservableCollection<ObjectModel>(temp.ToList());

            using (var db = new DatabaseContext())
            {
                if (Objects != null)
                    db.Object.UpdateRange(Objects);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deselektiert alle Librarys
        /// </summary>
        protected void DeselectAllLibrarys()
        {
            if (Librarys == null)
                return;

            List<LibraryModel> temp = new List<LibraryModel>();
            temp = Librarys.ToList();
            Librarys.Clear();

            for (int i = 0; i < temp.Count; i++)
            {
                //temp[i].Regenerate = false;
                temp[i].Build = false;
            }

            Librarys = new ObservableCollection<LibraryModel>(temp.ToList());

            using (var db = new DatabaseContext())
            {
                if (Librarys != null)
                    db.Library.UpdateRange(Librarys);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Lädt Library aus Datenbank Vergleich mit aktueller Powerbuilder Library
        /// Gegebenfalls löschen oder hinzufügen neuer Objects(Entrys)
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadLibrary()
        {
            if (LoadLibraryTask != null && LoadLibraryTask.Status == TaskStatus.Running)
                await LoadLibraryTask;

            if (Library != null && Library.Objects == null)
            {
                LoadLibraryTask = Task.Run(async () =>
                {
                    using (var db = new DatabaseContext())
                    {
                        //Track Entitiy
                        Library = await db.Library.FindAsync(Library.Id);

                        //Load Collections
                        await db.Entry(Library).Collection(l => l.Objects).LoadAsync();

                        //Update Objects
                        Library = LibBuilder.Core.Orca.UpdateLibrayObjects(Library, Workspace.PBVersion.Value);
                    }
                });

                //warten
                await LoadLibraryTask;

                //Updaten
                await SaveLibrary();

                Objects = new ObservableCollection<ObjectModel>(Library.Objects.ToList());

                //string message = "Objects von " + Library.File + " erfolgreich geladen";
                //mainWindowViewModel.NotificationSnackbar.Enqueue(message);
            }
        }

        /// <summary>
        /// Lädt Target aus Datenbank Vergleich mit aktuellem Powerbuilder Target
        /// gegenbenfalls werden neue Librarys hinzugefügt oder alte gelöscht
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadTarget()
        {
            if (LoadTargetTask != null && LoadTargetTask.Status == TaskStatus.Running)
                await LoadTargetTask;

            LoadTargetTask = Task.Run(async () =>
            {
                using (var db = new DatabaseContext())
                {
                    //Track Entitiy
                    Target = await db.Target.FindAsync(Target.Id);

                    //Load Collections
                    await db.Entry(Target).Collection(t => t.Librarys).LoadAsync();

                    //Update Collections
                    Target = LibBuilder.Core.Orca.UpdateTargetLibraries(Target, Workspace.PBVersion.Value);
                }
            });

            await LoadTargetTask;

            await SaveTarget();

            Librarys = new ObservableCollection<LibraryModel>(Target.Librarys.Where(l => l.File.EndsWith(".pbl")).ToList());

            //string message = "Target " + Target.File + " erfolgreich geladen";
            //mainWindowViewModel.NotificationSnackbar.Enqueue(message);
        }

        /// <summary>
        /// Lädt Workspace aus Datenbank Vergleich mit aktuellem Powerbuilder Workspace.
        /// Gegenbenfalls werden neue Targets hinzugefügt oder alte gelöscht
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadWorkspace()
        {
            if (LoadWorkspaceTask != null && LoadWorkspaceTask.Status == TaskStatus.Running)
            {
                await LoadWorkspaceTask;
            }

            //Worksapce(Targets) updaten
            LoadWorkspaceTask = Task.Run(async () =>
            {
                using (var db = new DatabaseContext())
                {
                    //Track Entitiy
                    Workspace = await db.Workspace.FindAsync(Workspace.Id);

                    //Load Targets
                    await db.Entry(Workspace).Collection(t => t.Target).LoadAsync();

                    Workspace = LibBuilder.Core.Orca.UpdateWorkspaceTargets(Workspace);
                }
            });

            //warten bis geladen
            await LoadWorkspaceTask;

            //asynchron Updaten
            await SaveWorkspace();

            //Einfügen der DB Targets
            await Task.Run(() => Targets = new ObservableCollection<TargetModel>(Workspace.Target.ToList()));

            //string message = "Workspace " + Workspace.File + " wurde eingelesen";
            //mainWindowViewModel.NotificationSnackbar.Enqueue(message);
        }

        /// <summary>
        /// Verarbeitet gewählten Workspace aus dem OpenFileDialog Fügt Workspace in
        /// Datenbank hinzu
        /// </summary>
        /// <param name="filePath"></param>
        protected virtual void OpenWorkspace(string filePath)
        {
            try
            {
                var exist = Workspaces.Single(w => w.FilePath.ToLower() == filePath.ToLower());
                Workspace = exist;
            }
            catch
            {
                WorkspaceModel workspace = new WorkspaceModel()
                {
                    Directory = Path.GetDirectoryName(filePath),
                    File = Path.GetFileName(filePath),
                };

                using (var db = new DatabaseContext())
                {
                    db.Workspace.Add(workspace);
                    db.SaveChanges();
                }
                Workspaces.Add(workspace);
                Workspace = Workspaces.Last();
            }
        }

        /// <summary>
        /// Speichert Library(Library-Objects) asynchron in Datenbank
        /// </summary>
        /// <returns></returns>
        protected async Task SaveLibrary()
        {
            if (SaveLibraryTask != null && SaveLibraryTask.Status == TaskStatus.Running)
                await SaveLibraryTask;

            SaveLibraryTask = Task.Run(async () =>
            {
                using (var db = new DatabaseContext())
                {
                    if (Library != null)
                        db.Library.Update(Library);

                    await db.SaveChangesAsync();
                }
            });

            await SaveLibraryTask;
        }

        /// <summary>
        /// Speichert Target(Target-Librarys) asynchron in Datenbank
        /// </summary>
        /// <returns></returns>
        protected async Task SaveTarget()
        {
            if (SaveTargetTask != null && SaveTargetTask.Status == TaskStatus.Running)
                await SaveTargetTask;

            SaveTargetTask = Task.Run(async () =>
            {
                using (var db = new DatabaseContext())
                {
                    if (Target != null)
                        db.Target.Update(Target);

                    await db.SaveChangesAsync();
                }
            });

            await SaveTargetTask;
        }

        /// <summary>
        /// Speichert Workspace(Workspace-Targets) asynchron in Datenbank
        /// </summary>
        /// <returns></returns>
        protected async Task SaveWorkspace()
        {
            if (SaveWorkspaceTask != null && SaveWorkspaceTask.Status == TaskStatus.Running)
                await SaveWorkspaceTask;

            SaveWorkspaceTask = Task.Run(async () =>
            {
                using (var db = new DatabaseContext())
                {
                    if (Workspace != null)
                        db.Workspace.Update(Workspace);

                    await db.SaveChangesAsync();
                }
            });

            await SaveWorkspaceTask;
        }

        /// <summary>
        /// Selektiert alle Objects(Entrys)
        /// </summary>
        protected void SelectAllEntrys()
        {
            if (Objects == null)
                return;

            List<ObjectModel> temp = new List<ObjectModel>();
            temp = Objects.ToList();
            Objects.Clear();

            for (int i = 0; i < temp.Count; i++)
            {
                temp[i].Regenerate = true;
            }

            Objects = new ObservableCollection<ObjectModel>(temp.ToList());

            using (var db = new DatabaseContext())
            {
                if (Objects != null)
                    db.Object.UpdateRange(Objects);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Selektiert alle Librarys
        /// </summary>
        protected void SelectAllLibrarys()
        {
            if (Librarys == null)
                return;

            List<LibraryModel> temp = Librarys.ToList();
            Librarys.Clear();

            for (int i = 0; i < temp.Count; i++)
            {
                //temp[i].Regenerate = true;
                temp[i].Build = true;
            }

            Librarys = new ObservableCollection<LibraryModel>(temp.ToList());

            using (var db = new DatabaseContext())
            {
                if (Librarys != null)
                    db.Library.UpdateRange(Librarys);

                db.SaveChanges();
            }
        }

        #region Properties

        #region Commands

        /// <summary>
        /// Gets or sets the deselect all entrys command.
        /// </summary>
        /// <value>The deselect all entrys command.</value>
        public IMvxCommand DeselectAllEntrysCommand { get; set; }

        /// <summary>
        /// Gets or sets the deselect all librarys command.
        /// </summary>
        /// <value>The deselect all librarys command.</value>
        public IMvxCommand DeselectAllLibrarysCommand { get; set; }

        /// <summary>
        /// Gets or sets the library selected command.
        /// </summary>
        /// <value>The library selected command.</value>
        public IMvxAsyncCommand LibrarySelectedCommand { get; set; }

        /// <summary>
        /// Gets or sets the open workspace command.
        /// </summary>
        /// <value>The open workspace command.</value>
        public IMvxAsyncCommand OpenWorkspaceCommand { get; set; }

        /// <summary>
        /// Gets or sets the run procedur command.
        /// </summary>
        /// <value>The run procedur command.</value>
        public MvxAsyncCommand RunProcedurCommand { get; set; }

        /// <summary>
        /// Gets or sets the save workspace command.
        /// </summary>
        /// <value>The save workspace command.</value>
        public IMvxAsyncCommand SaveWorkspaceCommand { get; set; }

        /// <summary>
        /// Gets or sets the select all entrys command.
        /// </summary>
        /// <value>The select all entrys command.</value>
        public IMvxCommand SelectAllEntrysCommand { get; set; }

        /// <summary>
        /// Gets or sets the select all librarys command.
        /// </summary>
        /// <value>The select all librarys command.</value>
        public IMvxCommand SelectAllLibrarysCommand { get; set; }

        /// <summary>
        /// Gets or sets the target selected command.
        /// </summary>
        /// <value>The target selected command.</value>
        public IMvxAsyncCommand TargetSelectedCommand { get; set; }

        /// <summary>
        /// Gets or sets the workspace selected command.
        /// </summary>
        /// <value>The workspace selected command.</value>
        public IMvxAsyncCommand WorkspaceSelectedCommand { get; set; }

        #endregion Commands

        private List<PBDotNetLib.orca.Orca.PBORCA_REBLD_TYPE> _applicationRebuild;
        private bool _contentLoadingAnimation;
        private LibraryModel _library;
        private ObservableCollection<LibraryModel> _librarys;
        private ObjectModel _object;
        private ObservableCollection<ObjectModel> _objects;
        private List<PBDotNetLib.orca.Orca.Version> _pBVersions;
        private TargetModel _target;
        private ObservableCollection<TargetModel> _targets;
        private WorkspaceModel _workspace;
        private ObservableCollection<WorkspaceModel> _workspaces;
        private Task LoadLibraryTask;
        private Task LoadTargetTask;
        private Task LoadWorkspaceTask;

        private Task SaveLibraryTask;
        private Task SaveTargetTask;
        private Task SaveWorkspaceTask;

        public List<PBDotNetLib.orca.Orca.PBORCA_REBLD_TYPE> ApplicationRebuild
        {
            get => _applicationRebuild;
            set => SetProperty(ref _applicationRebuild, value);
        }

        public bool ContentLoadingAnimation
        {
            get => _contentLoadingAnimation;
            set => SetProperty(ref _contentLoadingAnimation, value);
        }

        public LibraryModel Library
        {
            get => _library;
            set
            {
                //bevor neu zugewiesen wird; altes Model in DB speichern
                if (Library != null)
                {
                    using (var db = new DatabaseContext())
                    {
                        db.Library.Update(Library);
                        db.SaveChanges();
                    }
                }

                SetProperty(ref _library, value);
            }
        }

        public ObservableCollection<LibraryModel> Librarys
        {
            get => _librarys;
            set => SetProperty(ref _librarys, value);
        }

        public ObjectModel Object
        {
            get => _object;
            set
            {
                //bevor neu zugewiesen wird; altes Model in DB speichern
                if (Object != null)
                {
                    using (var db = new DatabaseContext())
                    {
                        //if(db.Object.Find(Object.Id).Regenerate != Object.Regenerate)
                        db.Object.Update(Object);
                        db.SaveChanges();
                    }
                }

                SetProperty(ref _object, value);
            }
        }

        public ObservableCollection<ObjectModel> Objects
        {
            get => _objects;
            set => SetProperty(ref _objects, value);
        }

        public List<PBDotNetLib.orca.Orca.Version> PBVersions
        {
            get => _pBVersions;
            set => SetProperty(ref _pBVersions, value);
        }

        public TargetModel Target
        {
            get => _target;
            set => SetProperty(ref _target, value);
        }

        public ObservableCollection<TargetModel> Targets
        {
            get => _targets;
            set => SetProperty(ref _targets, value);
        }

        public WorkspaceModel Workspace
        {
            get => _workspace;
            set => SetProperty(ref _workspace, value);
        }

        public ObservableCollection<WorkspaceModel> Workspaces
        {
            get => _workspaces;
            set => SetProperty(ref _workspaces, value);
        }

        #endregion Properties
    }
}