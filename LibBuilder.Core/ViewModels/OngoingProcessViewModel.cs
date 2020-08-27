using Data;
using Data.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LibBuilder.Core.ViewModels
{
    public class OngoingProcessViewModel : MvxNavigationViewModel<TargetModel>
    {
        protected object _lock = new object();

        private LibraryModel _library;
        private ObservableCollection<LibraryModel> _librarys;
        private bool _processError;

        private ObservableCollection<Data.Models.Process> _processes;

        private bool _processLoadingAnimation;

        private bool _processSucess;

        private TargetModel _target;

        //private string _title;
        private WorkspaceModel _workspace;

        private Task RunProcedurTask;

        public LibraryModel Library
        {
            get => _library;
            set
            {
                //bevor neu zugewiesen wird; altes Model in DB speichern
                //if (Library != null)
                //{
                //    using (var db = new DatabaseContext())
                //    {
                //        db.Library.Update(Library);
                //        db.SaveChanges();
                //    }
                //}

                SetProperty(ref _library, value);
            }
        }

        public ObservableCollection<LibraryModel> Librarys
        {
            get => _librarys;
            set => SetProperty(ref _librarys, value);
        }

        public bool ProcessError
        {
            get => _processError;
            set => SetProperty(ref _processError, value);
        }

        public ObservableCollection<Data.Models.Process> Processes
        {
            get => _processes;
            set => SetProperty(ref _processes, value);
        }

        public bool ProcessLoadingAnimation
        {
            get => _processLoadingAnimation;
            set => SetProperty(ref _processLoadingAnimation, value);
        }

        public bool ProcessSucess
        {
            get => _processSucess;
            set => SetProperty(ref _processSucess, value);
        }

        /// <summary>
        /// Gets or sets the run procedur command.
        /// </summary>
        /// <value>The run procedur command.</value>
        public MvxAsyncCommand RunProcedurCommand { get; set; }

        public TargetModel Target
        {
            get => _target;
            set => SetProperty(ref _target, value);
        }

        //public string Title
        //{
        //    get => _title;
        //    set => SetProperty(ref _title, value);
        //}

        public WorkspaceModel Workspace
        {
            get => _workspace;
            set => SetProperty(ref _workspace, value);
        }

        public OngoingProcessViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
                                                                                                    : base(logProvider, navigationService)
        {
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>Initilisierung.</returns>
        public override Task Initialize()
        {
            if (Target != null)
            {
                using (var data = new DatabaseContext())
                {
                    Workspace = data.Workspace.Find(Target.WorkspaceId);

                    var libraries = data.Library.Where(l => l.TargetId == Target.Id).ToList();
                    Librarys = new ObservableCollection<LibraryModel>(libraries);
                }

                RunProcedurCommand.Execute();
            }

            return base.Initialize();
        }

        /// <summary>
        /// Prepares the specified user.
        /// </summary>
        /// <param name="_user">The user.</param>
        public override void Prepare(TargetModel targetModel = null)
        {
            Target = targetModel;
            //Workspace = Target.Workspace;
            //Librarys = (ObservableCollection<LibraryModel>)Target.Librarys;

            base.Prepare();
        }

        /// <summary>
        /// Startet die Orca-Prozeduren in einem asynchronen Task
        /// </summary>
        protected virtual async Task RunProcedurAsync()
        {
            RunProcedurTask = Task.Run(async () =>
            {
                var session = new PBDotNetLib.orca.Orca(Workspace.PBVersion.Value);

                #region ApplicationLibrarys

                // Apllication Library Liste
                lock (_lock)
                {
                    Processes.Add(new Data.Models.Process
                    {
                        Target = this.Target.File,
                        Mode = "ApplicationLibrarys"
                    });
                }

                lock (_lock)
                {
                    Processes.Last().Result = session.SetLibraryList(Target.Librarys.Select(l => l.FilePath).ToArray(), Target.Librarys.Count);
                }

                #endregion ApplicationLibrarys

                #region CurrentApplication

                //Applikation setzen
                lock (_lock)
                {
                    Processes.Add(new Data.Models.Process
                    {
                        Target = this.Target.File,
                        Mode = "CurrentApplication"
                    });
                }

                // Applikationsname holen(bei TimeLine e2 immer main.pbd(Library) und
                // fakt3(Objekt))
                using (var db = new DatabaseContext())
                {
                    ObjectModel applObj = db.Object.Where(o => o.Library.Target == this.Target && o.ObjectType == PBDotNetLib.orca.Objecttype.Application).First();
                    await db.Entry(applObj).Reference(o => o.Library).LoadAsync();

                    lock (_lock)
                    {
                        Processes.Last().Result = session.SetCurrentAppl(applObj.Library.FilePath, applObj.Name);
                    }
                }

                #endregion CurrentApplication

                #region ApplicationRebuild

                if (Target.ApplicationRebuild.HasValue)
                {
                    lock (_lock)
                    {
                        Processes.Add(new Data.Models.Process
                        {
                            Target = this.Target.File,
                            Mode = "ApplicationRebuild"
                        });
                    }
                    lock (_lock)
                    {
                        Processes.Last().Result = session.ApplicationRebuild(Target.ApplicationRebuild.Value);
                    }

                    // Skip other processes
                    goto End;
                }

                #endregion ApplicationRebuild

                // für jede unkompilierte(.pbl) Library
                for (int l = 0; l < Librarys.Count; l++)
                {
                    #region Load&Check

                    //Laden des Datensatzes
                    using (var db = new DatabaseContext())
                    {
                        //Track Entitiy
                        Library = db.Library.Single(b => b.Id == Librarys[l].Id);

                        //Load Collections
                        await db.Entry(Library).Collection(t => t.Objects).LoadAsync();
                    }

                    //wenn nichts zum verarbeiten
                    if (Library.Build == false && Library.Objects.Where(lib => lib.Regenerate == true).ToList().Count == 0)
                        continue;

                    #endregion Load&Check

                    #region Regenerate

                    //für jedes Object
                    for (int i = 0; i < Library.Objects.Count; i++)
                    {
                        if (Library.Objects[i].Regenerate)
                        {
                            lock (_lock)
                            {
                                Processes.Add(new Data.Models.Process
                                {
                                    Target = this.Target.File,
                                    Library = this.Library.File,
                                    Object = this.Library.Objects[i].Name,
                                    Mode = "Regenerate"
                                });
                            }

                            lock (_lock)
                            {
                                Processes.Last().Result = session.RegenerateObject(Library.FilePath, Library.Objects[i].Name, Library.Objects[i].ObjectType.Value);
                            }
                        }
                    }

                    #endregion Regenerate

                    #region Build

                    if (Librarys[l].Build)
                    {
                        lock (_lock)
                        {
                            Processes.Add(new Data.Models.Process
                            {
                                Target = this.Target.File,
                                Library = this.Library.File,
                                Mode = "Rebuild"
                            });
                        }

                        lock (_lock)
                        {
                            Processes.Last().Result = session.CreateDynamicLibrary(Librarys[l].FilePath, "");
                        }
                    }

                    #endregion Build
                }

            End:
                session.SessionClose();

                #region Result

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

                    await db.SaveChangesAsync();
                }

                #endregion Result
            });

            await RunProcedurTask;
        }
    }
}