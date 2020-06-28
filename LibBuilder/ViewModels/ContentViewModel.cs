using Data;
using Data.Models;
using LibBuilder.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace LibBuilder.ViewModels
{
    public class ContentViewModel : BaseViewModel
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly Orca libbuilder;

        private object _lock;

        private Task SaveWorkspaceTask; //für Targets
        private Task SaveTargetTask; //für Librarys
        private Task SaveLibraryTask; //für Librarys

        private Task LoadWorkspaceTask; //für Targets
        private Task LoadTargetTask; //für Librarys
        private Task LoadLibraryTask; //für Objects

        public ContentViewModel(MainWindowViewModel mainWindowViewModel)
        {
            //für Snackbar Message
            this.mainWindowViewModel = mainWindowViewModel;

            //UI bezogen
            OpenWorkspaceCommand = new ActionCommand(OpenWorkspace);
            SelectAllLibrarysCommand = new ActionCommand(SelectAllLibrarys);
            DeselectAllLibrarysCommand = new ActionCommand(DeselectAllLibrarys);
            SelectAllEntrysCommand = new ActionCommand(SelectAllEntrys);
            DeselectAllEntrysCommand = new ActionCommand(DeselectAllEntrys);

            //Speichern
            SaveWorkspaceCommand = new AsyncCommand(async () => await SaveWorkspace());

            //Laden
            WorkspaceSelectedCommand = new AsyncCommand(async () => await LoadWorkspace());
            TargetSelectedCommand = new AsyncCommand(async () => await LoadTarget());
            LibrarySelectedCommand = new AsyncCommand(async () => await LoadLibrary());

            RunCommand = new AsyncCommand(async () => await RunAsync());

            //Kommunikation zwischen Powerbuilder und .Net(Datenbank)
            libbuilder = new Orca();

            //VersionsListe
            PBVersions = Enum.GetValues(typeof(PBDotNetLib.orca.Orca.Version)).Cast<PBDotNetLib.orca.Orca.Version>().ToList();

            using (var db = new DatabaseContext())
            {
                //Workspace Liste laden
                Workspaces = new ObservableCollection<WorkspaceModel>(db.Workspace.ToList());
            }

            //letzten modifizierten Workspace laden, mit zuletzt ausgewähltem Target
            //if (Workspaces != null && Workspaces.Count > 0)
            //    Workspace = Workspaces.OrderByDescending(w => w.UpdatedDate).FirstOrDefault();
        }

        public ICommand SaveWorkspaceCommand { get; set; }
        public ICommand OpenWorkspaceCommand { get; set; }
        public ICommand WorkspaceSelectedCommand { get; set; }
        public ICommand TargetSelectedCommand { get; set; }
        public ICommand LibrarySelectedCommand { get; set; }
        public ICommand SelectAllLibrarysCommand { get; set; }
        public ICommand DeselectAllLibrarysCommand { get; set; }
        public ICommand SelectAllEntrysCommand { get; set; }
        public ICommand DeselectAllEntrysCommand { get; set; }
        public ICommand RunCommand { get; set; }

        private async Task SaveWorkspace()
        {
            if (SaveWorkspaceTask != null && SaveWorkspaceTask.Status == TaskStatus.Running)
                SaveWorkspaceTask.Wait();

            SaveWorkspaceTask = Task.Run(() =>
            {
                using (var db = new DatabaseContext())
                {
                    if (Workspace != null)
                        db.Workspace.Update(Workspace);

                    db.SaveChanges();
                }
            });
        }

        private async Task SaveTarget()
        {
            if (SaveTargetTask != null && SaveTargetTask.Status == TaskStatus.Running)
                SaveTargetTask.Wait();

            SaveTargetTask = Task.Run(() =>
            {
                using (var db = new DatabaseContext())
                {
                    if (Target != null)
                        db.Target.Update(Target);

                    db.SaveChanges();
                }
            });
        }

        private async Task SaveLibrary()
        {
            if (SaveLibraryTask != null && SaveLibraryTask.Status == TaskStatus.Running)
                SaveLibraryTask.Wait();

            SaveLibraryTask = Task.Run(() =>
            {
                using (var db = new DatabaseContext())
                {
                    if (Library != null)
                        db.Library.Update(Library);

                    db.SaveChanges();
                }
            });

            SaveLibraryTask.Wait();
        }

        private bool CheckWorkspace()
        {
            string message = string.Empty;

            if (Workspace == null)
            {
                message = "Bitte Workspace auswählen";
                mainWindowViewModel.NotificationSnackbar.Enqueue(message);
                return false;
            }
            else
            {
                if (Workspace.PBVersion == null)
                {
                    message = "Bitte Powerbuilder-Version angeben";
                    mainWindowViewModel.NotificationSnackbar.Enqueue(message);
                    return false;
                }
            }

            return true;
        }

        private bool CheckRunnable()
        {
            string message = string.Empty;

            if (Target == null)
            {
                message = "Bitte Target auswählen";
                mainWindowViewModel.NotificationSnackbar.Enqueue(message);
                return false;
            }

            return true;
        }

        private async Task LoadWorkspace()
        {
            if (!CheckWorkspace())
                return;

            if (LoadWorkspaceTask != null && LoadWorkspaceTask.Status == TaskStatus.Running)
                LoadWorkspaceTask.Wait();

            ContentLoadingAnimation = true;

            //Worksapce(Targets) updaten
            LoadWorkspaceTask = Task.Run(() =>
            {
                using (var db = new DatabaseContext())
                {
                    //Track Entitiy
                    Workspace = db.Workspace.Find(Workspace.Id);

                    //Load Targets
                    db.Entry(Workspace).Collection(t => t.Target).Load();

                    Workspace = libbuilder.UpdateWorkspaceTargets(Workspace);
                }
            });

            //warten bis geladen
            LoadWorkspaceTask.Wait();

            //asynchron Updaten
            await SaveWorkspace();

            //Einfügen der DB Targets
            await Task.Run(() => Targets = new ObservableCollection<TargetModel>(Workspace.Target.ToList()));

            //string message = "Workspace " + Workspace.File + " wurde eingelesen";
            //mainWindowViewModel.NotificationSnackbar.Enqueue(message);

            ContentLoadingAnimation = false;
        }

        private async Task LoadTarget()
        {
            if (LoadTargetTask != null && LoadTargetTask.Status == TaskStatus.Running)
                LoadTargetTask.Wait();

            ContentLoadingAnimation = true;

            LoadTargetTask = Task.Run(() =>
            {
                using (var db = new DatabaseContext())
                {
                    //Track Entitiy
                    Target = db.Target.Find(Target.Id);

                    //Load Collections
                    db.Entry(Target).Collection(t => t.Librarys).Load();

                    //Update Collections
                    Target = libbuilder.UpdateTargetLibraries(Target, Workspace.PBVersion.Value);
                }
            });

            LoadTargetTask.Wait();

            await SaveTarget();

            Librarys = new ObservableCollection<LibraryModel>(Target.Librarys.Where(l => l.File.EndsWith(".pbl")).ToList());

            //string message = "Target " + Target.File + " erfolgreich geladen";
            //mainWindowViewModel.NotificationSnackbar.Enqueue(message);

            ContentLoadingAnimation = false;
        }

        //Asynchrones Updaten/Laden der Library-Objects
        private async Task LoadLibrary()
        {
            if (LoadLibraryTask != null && LoadLibraryTask.Status == TaskStatus.Running)
                LoadLibraryTask.Wait();

            if (Library != null && Library.Objects == null)
            {
                ContentLoadingAnimation = true;

                LoadLibraryTask = Task.Run(() =>
                {
                    using (var db = new DatabaseContext())
                    {
                        //Track Entitiy
                        Library = db.Library.Find(Library.Id);

                        //Load Collections
                        db.Entry(Library).Collection(l => l.Objects).Load();

                        //Update Objects
                        Library = libbuilder.UpdateLibrayObjects(Library, Workspace.PBVersion.Value);
                    }
                });

                //warten
                LoadLibraryTask.Wait();

                //Updaten
                await SaveLibrary();

                Objects = new ObservableCollection<ObjectModel>(Library.Objects.ToList());

                string message = "Objects von " + Library.File + " erfolgreich geladen";
                mainWindowViewModel.NotificationSnackbar.Enqueue(message);

                ContentLoadingAnimation = false;
            }
        }

        public ObjectModel Object
        {
            get => Get<ObjectModel>();
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

                Set(value);
            }
        }

        public ObservableCollection<ObjectModel> Objects
        {
            get => Get<ObservableCollection<ObjectModel>>();
            set => Set(value);
        }

        private void DeselectAllLibrarys(object obj)
        {
            if (Librarys != null)
            {
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
        }

        private void SelectAllLibrarys(object obj)
        {
            if (Librarys != null)
            {
                List<LibraryModel> temp = new List<LibraryModel>();
                temp = Librarys.ToList();
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
        }

        private void DeselectAllEntrys(object obj)
        {
            if (Objects != null)
            {
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
        }

        private void SelectAllEntrys(object obj)
        {
            if (Objects != null)
            {
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
        }

        private async Task RunAsync()
        {
            if (!CheckWorkspace())
                return;

            if (!CheckRunnable())
                return;

            Processes = new ObservableCollection<Process>();
            SecondTab = true; // auf zweiten tab switchen
            ProcessLoadingAnimation = true;
            ProcessSucess = false;
            ProcessError = false;

            _lock = new object();
            BindingOperations.EnableCollectionSynchronization(Processes, _lock);

            await Task.Run(() =>
            {
                var session = new PBDotNetLib.orca.Orca(Workspace.PBVersion.Value);

                //Apllication Library Liste
                lock (_lock)
                    Processes.Add(new Process
                    {
                        Target = this.Target.File,
                        Mode = "ApplicationLibrarys"
                    });

                lock (_lock)
                    Processes.Last().Result = session.SetLibraryList(Target.Librarys.Select(l => l.FilePath).ToArray(), Target.Librarys.Count);

                //Applikation setzen
                lock (_lock)
                    Processes.Add(new Process
                    {
                        Target = this.Target.File,
                        Mode = "CurrentApplication"
                    });

                //Applikationsname holen(bei TimeLine e2 immer main.pbd(Library) und fakt3(Objekt))
                using (var db = new DatabaseContext())
                {
                    ObjectModel applObj = db.Object.Where(o => o.Library.Target == this.Target && o.ObjectType == PBDotNetLib.orca.Objecttype.Application).First();
                    db.Entry(applObj).Reference(o => o.Library).Load();
                  
                    lock (_lock)
                        Processes.Last().Result = session.SetCurrentAppl(applObj.Library.FilePath, applObj.Name);
                }

                //für jede unkompilierte(.pbl) Library
                for (int l = 0; l < Librarys.Count; l++)
                {
                    //Laden des Datensatzes
                    using (var db = new DatabaseContext())
                    {
                        //Track Entitiy
                        Library = db.Library.Single(b => b.Id == Librarys[l].Id);

                        //Load Collections
                        db.Entry(Library).Collection(t => t.Objects).Load();
                    }

                    //wenn nichts zum verarbeiten
                    if (Library.Build == false && Library.Objects.Where(l => l.Regenerate == true).ToList().Count == 0)
                        continue;

                    //Regenerate
                    //für jedes Object
                    for (int i = 0; i < Library.Objects.Count; i++)
                    {
                        if (Library.Objects[i].Regenerate)
                        {
                            lock (_lock)
                                Processes.Add(new Process
                                {
                                    Target = this.Target.File,
                                    Library = this.Library.File,
                                    Object = this.Library.Objects[i].Name,
                                    Mode = "Regenerate"
                                });

                            lock (_lock)
                                Processes.Last().Result = session.RegenerateObject(Library.FilePath, Library.Objects[i].Name, Library.Objects[i].ObjectType.Value);
                        }
                    }

                    //build
                    if (Librarys[l].Build)
                    {
                        lock (_lock)
                            Processes.Add(new Process
                            {
                                Target = this.Target.File,
                                Library = this.Library.File,
                                Mode = "Rebuild"
                            });

                        lock (_lock)
                            Processes.Last().Result = session.CreateDynamicLibrary(Librarys[l].FilePath, "");
                    }
                }

                var sucess = Processes.Where(r => r.Result.Equals(PBDotNetLib.orca.Orca.Result.PBORCA_OK)).Count();

                if (sucess == Processes.Count)
                    ProcessSucess = true;
                else
                    ProcessError = true;

                using (var db = new DatabaseContext())
                {
                    ProcessModel process = new ProcessModel()
                    {
                        Target = this.Target,
                        Sucess = sucess,
                        Error = Processes.Count - sucess
                    };

                    db.Attach(process);

                    db.SaveChanges();
                }

                session.SessionClose();
            });

            ProcessLoadingAnimation = false;
        }

        private void OpenWorkspace(object obj)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Powerbuilder Workspace (*.pbw)|*.pbw";

            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    var exist = Workspaces.Single(w => w.File == Path.GetFileName(dialog.FileName));
                    Workspace = exist;
                }
                catch
                {
                    WorkspaceModel workspace = new WorkspaceModel()
                    {
                        Directory = Path.GetDirectoryName(dialog.FileName),
                        File = Path.GetFileName(dialog.FileName),
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
        }

        public bool SecondTab
        {
            get => Get<bool>();
            set => Set(value);
        }

        public List<PBDotNetLib.orca.Orca.Version> PBVersions
        {
            get => Get<List<PBDotNetLib.orca.Orca.Version>>();
            set => Set(value);
        }

        public ObservableCollection<Process> Processes
        {
            get => Get<ObservableCollection<Process>>();
            set => Set(value);
        }

        public ObservableCollection<WorkspaceModel> Workspaces
        {
            get => Get<ObservableCollection<WorkspaceModel>>();
            set => Set(value);
        }

        public WorkspaceModel Workspace
        {
            get => Get<WorkspaceModel>();
            set => Set(value);
        }

        public ObservableCollection<TargetModel> Targets
        {
            get => Get<ObservableCollection<TargetModel>>();
            set => Set(value);
        }

        public TargetModel Target
        {
            get => Get<TargetModel>();
            set => Set(value);
        }

        public ObservableCollection<LibraryModel> Librarys
        {
            get => Get<ObservableCollection<LibraryModel>>();
            set => Set(value);
        }

        public LibraryModel Library
        {
            get => Get<LibraryModel>();
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

                Set(value);
            }
        }

        public bool ProcessLoadingAnimation
        {
            get => Get<bool>();
            set => Set(value);
        }

        public bool ContentLoadingAnimation
        {
            get => Get<bool>();
            set => Set(value);
        }

        public bool ProcessError
        {
            get => Get<bool>();
            set => Set(value);
        }

        public bool ProcessSucess
        {
            get => Get<bool>();
            set => Set(value);
        }
    }
}