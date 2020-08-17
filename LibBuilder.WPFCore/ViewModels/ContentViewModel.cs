// project=LibBuilder.WPFCore, file=ContentViewModel.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using Data.Models;
using Microsoft.Win32;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LibBuilder.WPFCore.ViewModels
{
    public class ContentViewModel : LibBuilder.Core.ViewModels.ContentViewModel
    {
        protected MainWindowViewModel mainWindowViewModel;
        protected Dictionary<string, string> parameter;

        public ContentViewModel(MainWindowViewModel mainWindowViewModel = null, Dictionary<string, string> parameter = null)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this.parameter = parameter;

            //Laden
            WorkspaceSelectedCommand = new MvxAsyncCommand(async () => await LoadWorkspace());
            TargetSelectedCommand = new MvxAsyncCommand(async () => await LoadTarget());
            LibrarySelectedCommand = new MvxAsyncCommand(async () => await LoadLibrary());

            //UI bezogen
            OpenWorkspaceCommand = new MvxAsyncCommand(OpenWorkspace);

            RunProcedurCommand = new MvxAsyncCommand(RunProcedur);

            if (parameter != null) { ParameterStartAsync(); }
            //letzten modifizierten Workspace laden, mit zuletzt ausgewähltem Target
            //if (Workspaces != null && Workspaces.Count > 0)
            //    Workspace = Workspaces.OrderByDescending(w => w.UpdatedDate).FirstOrDefault();
        }

        protected override async Task LoadLibrary()
        {
            ContentLoadingAnimation = true;
            await RaisePropertyChanged(() => ContentLoadingAnimation);

            await base.LoadLibrary();

            ContentLoadingAnimation = false;
            await RaisePropertyChanged(() => ContentLoadingAnimation);
        }

        protected override async Task LoadTarget()
        {
            ContentLoadingAnimation = true;
            await RaisePropertyChanged(() => ContentLoadingAnimation);

            await base.LoadTarget();

            ContentLoadingAnimation = false;
            await RaisePropertyChanged(() => ContentLoadingAnimation);
        }

        protected override async Task LoadWorkspace()
        {
            if (!CheckWorkspace())
                return;

            ContentLoadingAnimation = true;
            await RaisePropertyChanged(() => ContentLoadingAnimation);

            await base.LoadWorkspace();

            ContentLoadingAnimation = false;
            await RaisePropertyChanged(() => ContentLoadingAnimation);
        }

        protected async Task OpenWorkspace()
        {
            ContentLoadingAnimation = true;
            await RaisePropertyChanged(() => ContentLoadingAnimation);

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Powerbuilder Workspace (*.pbw)|*.pbw";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog().HasValue)
            {
                base.OpenWorkspace(openFileDialog.FileName);
            }

            ContentLoadingAnimation = false;
            await RaisePropertyChanged(() => ContentLoadingAnimation);
        }

        /// <summary>
        /// Parameters the start asynchronous.
        /// </summary>
        protected async Task ParameterStartAsync()
        {
            string value = "";

            if (parameter.ContainsKey("-w"))
            {
                if (parameter.TryGetValue("-w", out value))
                {
                    if (IsValidPath(value))
                    {
                        Workspace = Workspaces.Where(w => w.FilePath.ToLower() == value.ToLower()).First();
                    }
                    else
                    {
                        Workspace = Workspaces.Where(w => w.File.ToLower() == value.ToLower()).First();
                    }

                    await LoadWorkspace();
                }
                else
                {
                    // ohne Workspace geht nicht

                    return;
                }
            }

            if (parameter.ContainsKey("-t"))
            {
                if (parameter.TryGetValue("-t", out value))
                {
                    if (IsValidPath(value))
                    {
                        Target = Targets.Where(t => t.FilePath.ToLower() == value.ToLower()).First();
                    }
                    else
                    {
                        Target = Targets.Where(t => t.File.ToLower() == value.ToLower()).First();
                    }

                    await LoadTarget();
                }
                else
                {
                    // ohne Target geht nicht

                    return;
                }
            }

            if (parameter.ContainsKey("-v"))
            {
                if (parameter.TryGetValue("-v", out value))
                {
                    PBDotNetLib.orca.Orca.Version version = (PBDotNetLib.orca.Orca.Version)Enum.Parse(typeof(PBDotNetLib.orca.Orca.Version), "PB" + value);
                    Workspace.PBVersion = version;

                    await base.SaveWorkspace();
                    await LoadWorkspace();
                }
                else
                {
                    // in DB gespeicherte Version nehmen
                }
            }

            if (parameter.ContainsKey("-o"))
            {
                if (parameter.TryGetValue("-o", out value))
                {
                    //Console.WriteLine("For key = \"tif\", value = {0}.", value);
                }
                else
                {
                    //Console.WriteLine("Key = \"tif\" is not found.");
                }
            }

            if (parameter.ContainsKey("-l"))
            {
                if (parameter.TryGetValue("-l", out value))
                {
                    //Console.WriteLine("For key = \"tif\", value = {0}.", value);
                }
                else
                {
                    //Console.WriteLine("Key = \"tif\" is not found.");
                }
            }
        }

        protected async Task RunProcedur()
        {
            if (!CheckWorkspace())
                return;

            if (!CheckRunnable())
                return;

            Processes = new ObservableCollection<Process>();
            SecondTab = true; // auf zweiten tab switchen
            ProcessSucess = false;
            ProcessError = false;

            ProcessLoadingAnimation = true;
            await RaisePropertyChanged(() => ProcessLoadingAnimation);

            object _lock = new object();
            BindingOperations.EnableCollectionSynchronization(Processes, _lock);

            base.RunProcedur(_lock);
        }

        private bool CheckRunnable()
        {
            if (Target == null)
            {
                mainWindowViewModel.NotificationSnackbar.Enqueue("Bitte Target auswählen");
                return false;
            }

            return true;
        }

        private bool CheckWorkspace()
        {
            if (Workspace == null)
            {
                mainWindowViewModel.NotificationSnackbar.Enqueue("Bitte Workspace auswählen");
                return false;
            }
            else
            {
                if (Workspace.PBVersion == null)
                {
                    mainWindowViewModel.NotificationSnackbar.Enqueue("Bitte Powerbuilder-Version angeben");
                    return false;
                }
            }

            return true;
        }

        private bool IsValidPath(string path, bool allowRelativePaths = false)
        {
            bool isValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths)
                {
                    isValid = Path.IsPathRooted(path);
                }
                else
                {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}