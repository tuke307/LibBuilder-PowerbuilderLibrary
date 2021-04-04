namespace LibBuilder.Core.ViewModels
{
    using Data;
    using Data.Models;
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// OngoingProcessViewModel.
    /// </summary>
    /// <seealso cref="MvvmCross.ViewModels.MvxNavigationViewModel{Data.Models.TargetModel}" />
    public class OngoingProcessViewModel : MvxNavigationViewModel<TargetModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OngoingProcessViewModel" />
        /// class.
        /// </summary>
        /// <param name="logProvider">The log provider.</param>
        /// <param name="navigationService">The navigation service.</param>
        public OngoingProcessViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
                                                                                                    : base(logProvider, navigationService)
        {
        }

        #region Methods

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
                    // Für PB-Version, Workspace laden
                    Workspace = data.Workspace.Find(Target.WorkspaceId);

                    //Load Librarys
                    Target = data.Target.Find(Target.Id);
                    data.Entry(Target).Collection(t => t.Librarys).LoadAsync();
                    //var libraries = data.Library.Where(l => l.TargetId == Target.Id).ToList();
                    //Librarys = new ObservableCollection<LibraryModel>(libraries);
                }
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
                var resultApplicationLibrarys = session.SetLibraryList(Target.Librarys.Select(l => l.FilePath).ToArray(), Target.Librarys.Count);

                lock (_lock)
                {
                    Processes.Add(new Data.Models.Process
                    {
                        Target = this.Target.File,
                        Mode = "ApplicationLibrarys",
                        Result = resultApplicationLibrarys
                    });
                }

                #endregion ApplicationLibrarys

                #region CurrentApplication

                //Applikation setzen
                // Applikationsname holen(bei TimeLine e2 immer main.pbd(Library) und
                // fakt3(Objekt))
                using (var db = new DatabaseContext())
                {
                    ObjectModel applObj = db.Object.Where(o => o.Library.Target == this.Target && o.ObjectType == PBDotNetLib.orca.Objecttype.Application).First();
                    await db.Entry(applObj).Reference(o => o.Library).LoadAsync();

                    var resultCurrentApplication = session.SetCurrentAppl(applObj.Library.FilePath, applObj.Name);

                    lock (_lock)
                    {
                        Processes.Add(new Data.Models.Process
                        {
                            Target = this.Target.File,
                            Mode = "CurrentApplication",
                            Result = resultCurrentApplication
                        });
                    }
                }

                #endregion CurrentApplication

                #region ApplicationRebuild

                if (Target.ApplicationRebuild.HasValue)
                {
                    var resultApplicationRebuild = session.ApplicationRebuild(Target.ApplicationRebuild.Value);

                    lock (_lock)
                    {
                        Processes.Add(new Data.Models.Process
                        {
                            Target = this.Target.File,
                            Mode = "ApplicationRebuild",
                            Result = resultApplicationRebuild
                        });
                    }

                    // Skip other processes
                    goto End;
                }

                #endregion ApplicationRebuild

                #region Rebuild&Regenerate

                // für jede unkompilierte(.pbl) Library
                foreach (var lib in Librarys)
                {
                    #region Load&Check

                    //Laden des Datensatzes
                    using (var db = new DatabaseContext())
                    {
                        //Track Entitiy
                        Library = await db.Library.FindAsync(lib.Id);

                        //Load Collections
                        await db.Entry(Library).Collection(t => t.Objects).LoadAsync();
                    }

                    //wenn nichts zum verarbeiten
                    if (Library.Build == false && Library.Objects.Where(l => l.Regenerate == true).ToList().Count == 0)
                        continue;

                    #endregion Load&Check

                    #region Regenerate

                    //für jedes Object
                    foreach (var obj in Library.Objects)
                    {
                        if (obj.Regenerate)
                        {
                            var resultRegenerate = session.RegenerateObject(Library.FilePath, obj.Name, obj.ObjectType.Value);

                            lock (_lock)
                            {
                                Processes.Add(new Data.Models.Process
                                {
                                    Target = this.Target.File,
                                    Library = this.Library.File,
                                    Object = obj.Name,
                                    Mode = "Regenerate",
                                    Result = resultRegenerate,
                                });
                            }
                        }
                    }

                    #endregion Regenerate

                    #region Build

                    if (lib.Build)
                    {
                        var resultBuild = session.CreateDynamicLibrary(lib.FilePath, "");

                        lock (_lock)
                        {
                            Processes.Add(new Data.Models.Process
                            {
                                Target = this.Target.File,
                                Library = this.Library.File,
                                Mode = "Rebuild",
                                Result = resultBuild,
                            });
                        }
                    }

                    #endregion Build
                }

            #endregion Rebuild&Regenerate

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

        #endregion Methods

        #region Properties

        protected object _lock = new object();

        private LibraryModel _library;

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
            get => new ObservableCollection<LibraryModel>(Target?.Librarys);
            //set => SetProperty(ref _librarys, value);
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

        public WorkspaceModel Workspace
        {
            get => _workspace;
            set => SetProperty(ref _workspace, value);
        }

        #endregion Properties
    }
}