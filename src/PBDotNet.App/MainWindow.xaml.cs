// project=PBDotNet.App, file=MainWindow.xaml.cs, create=10:44 Copyright (c) 2021 Timeline
// Financials GmbH & Co. KG. All rights reserved.
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.Win32;
using PBDotNet.Core.orca;
using PBDotNet.Core.pbuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Powerscript = PBDotNet.Core.pbuilder.powerscript;

namespace PBDotNet.App
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private class LibEntryView : INotifyPropertyChanged
        {
            private ILibEntry libEntry;
            private IEnumerable<TypeView> types;

            public ILibEntry LibEntry
            {
                get
                {
                    return this.libEntry;
                }
            }

            public string Name
            {
                get
                {
                    return this.libEntry.Name;
                }
            }

            public TypeView Type { get; set; }

            public IEnumerable<TypeView> Types
            {
                get
                {
                    return this.types;
                }
                set
                {
                    this.types = value;

                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("Types"));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public LibEntryView(ILibEntry libEntry)
            {
                this.libEntry = libEntry;
            }
        }

        private class TypeView
        {
            public LibEntryView parent;
            private Powerscript.Type type;

            public string Name
            {
                get
                {
                    return this.type.Name;
                }
            }

            public LibEntryView Parent
            {
                get
                {
                    return this.parent;
                }
            }

            public Powerscript.Type Type
            {
                get
                {
                    return this.type;
                }
            }

            public TypeView(Powerscript.Type type, LibEntryView parent)
            {
                this.type = type;
                this.parent = parent;
            }
        }

        private IHighlightingDefinition highlightingDefinition = null;
        private Orca.Version pbVersion;
        private Workspace workspace;

        public MainWindow()
        {
            InitializeComponent();
            string pbVersion = Environment.GetEnvironmentVariable("PBVersion");
            if (String.IsNullOrEmpty(pbVersion))
            {
                this.pbVersion = Orca.Version.PB170;
            }
            else
            {
                switch (pbVersion)
                {
                    case "105":
                        this.pbVersion = Orca.Version.PB105;
                        break;

                    case "125":
                        this.pbVersion = Orca.Version.PB125;
                        break;

                    case "170":
                        this.pbVersion = Orca.Version.PB170;
                        break;
                }
            }

            if (Util.CmdlineArgs.Workspace != null)
            {
                this.openWorkspace(Util.CmdlineArgs.Workspace);
            }
            else if (Util.CmdlineArgs.Library != null)
            {
                this.openLibrary(Util.CmdlineArgs.Library);
            }

            using (Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("PBDotNet.App.PBSyntax.xshd"))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    highlightingDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }

            txtSource.SyntaxHighlighting = this.highlightingDefinition;
            txtUoSource.SyntaxHighlighting = this.highlightingDefinition;
            txtUoIVariables.SyntaxHighlighting = this.highlightingDefinition;
            txtUoSVariables.SyntaxHighlighting = this.highlightingDefinition;
            txtUoGVariables.SyntaxHighlighting = this.highlightingDefinition;
            txtUoExtFunctions.SyntaxHighlighting = this.highlightingDefinition;
        }

        private void gridUoEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridUoEvents.SelectedIndex < 0)
                return;

            txtUoSource.TextArea.Document.Text = ((Powerscript.Event[])gridUoEvents.ItemsSource)[gridUoEvents.SelectedIndex].Source;
        }

        private void gridUoMethods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridUoMethods.SelectedIndex < 0)
                return;

            txtMethodSource.TextArea.Document.Text = ((Powerscript.Method[])gridUoMethods.ItemsSource)[gridUoMethods.SelectedIndex].Source;
        }

        private void gridUoTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridUoTypes.SelectedIndex < 0)
                return;

            Powerscript.Type type = ((Powerscript.Type[])gridUoTypes.ItemsSource)[gridUoTypes.SelectedIndex];

            gridUoProps.ItemsSource = type.Properties;
            gridUoEvents.ItemsSource = type.Events;
        }

        private void listDatawindowObjs_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Powerscript.Datawindow.Element)
            {
                Powerscript.Datawindow.Element element = (Powerscript.Datawindow.Element)e.NewValue;
                txtDwValue.Text = element.Value;
                dgProperties.ItemsSource = element.Attributes;
            }
        }

        private void listPowerObjects_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            PBDotNet.Core.orca.ILibEntry lib;
            LibEntryView libView = null;
            TypeView typeView = null;
            Powerscript.Type[] types;
            Powerscript.Datawindow dw;

            if (e.NewValue is LibEntryView)
            {
                libView = (LibEntryView)e.NewValue;
                typeView = libView.Type;
            }
            else if (e.NewValue is TypeView)
            {
                typeView = (TypeView)e.NewValue;
                libView = (typeView).Parent;
            }

            if (libView != null)
            {
                lib = libView.LibEntry;

                txtSource.TextArea.Document.Text = lib.Source ?? "";

                switch (lib.Type)
                {
                    case Objecttype.Datawindow:
                        tabUserobject.Visibility = System.Windows.Visibility.Collapsed;
                        tabDatawindow.Visibility = System.Windows.Visibility.Visible;

                        dw = Powerscript.Datawindow.GetDatawindowFromSource(lib.Source);
                        txtDwRelease.Text = dw.Release;

                        if (dw.Object != null)
                        {
                            listDatawindowObjs.ItemsSource = dw.Object.Childs;
                        }

                        break;

                    case Objecttype.Structure:
                        tabUserobject.Header = "Structure";
                        goto case Objecttype.Window;
                    case Objecttype.Menu:
                        tabUserobject.Header = "Menu";
                        goto case Objecttype.Window;
                    case Objecttype.Function:
                        tabUserobject.Header = "Function";
                        goto case Objecttype.Window;
                    case Objecttype.Application:
                        tabUserobject.Header = "Application";
                        goto case Objecttype.Window;
                    case Objecttype.Userobject:
                        tabUserobject.Header = "UserObject";
                        goto case Objecttype.Window;
                    case Objecttype.Window:
                        tabUserobject.Visibility = System.Windows.Visibility.Visible;
                        tabDatawindow.Visibility = System.Windows.Visibility.Collapsed;

                        if (lib.Type == Objecttype.Window)
                            tabUserobject.Header = "Window";

                        types = Powerscript.Type.GetTypesFromSource(lib.Source);
                        if (types == null)
                        {
                            MessageBox.Show("keine Types");
                            return;
                        }

                        gridUoTypes.ItemsSource = types;

                        if (libView.Types == null)
                        {
                            List<TypeView> typeViews = new List<TypeView>();
                            foreach (Powerscript.Type t in types)
                            {
                                if (t.Name == libView.Name)
                                {
                                    libView.Type = new TypeView(t, libView);
                                    typeView = libView.Type;
                                }
                                else
                                {
                                    typeViews.Add(new TypeView(t, libView));
                                }
                            }
                            libView.Types = typeViews;
                        }

                        gridUoProps.ItemsSource = typeView.Type.Properties;

                        if (types.Length > 0)
                        {
                            txtUoIVariables.TextArea.Document.Text = types[0].InstanceVariables ?? "";
                            txtUoSVariables.TextArea.Document.Text = types[0].SharedVariables ?? "";
                            txtUoGVariables.TextArea.Document.Text = types[0].GlobalVariables ?? "";
                            txtUoExtFunctions.TextArea.Document.Text = types[0].ExternalFunctions ?? "";
                            gridUoMethods.ItemsSource = types[0].Methods;
                        }
                        break;
                }
            }
        }

        private void listWorkspace_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Library)
            {
                Library lib = (Library)e.NewValue;
                List<LibEntry> libs = new Orca(pbVersion).DirLibrary(lib.Dir + "\\" + lib.File);
                List<LibEntryView> libsViews = new List<LibEntryView>();

                foreach (LibEntry l in libs) libsViews.Add(new LibEntryView(l));

                listPowerObjects.ItemsSource = libsViews;
            }
        }

        private void menuOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.openLibrary(dialog.SelectedPath);
            }
        }

        private void menuOpenLibrary_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Powerbuilder Library (*.pbl)|*.pbl";

            if (!dialog.ShowDialog().Value)
                return;

            this.openLibrary(dialog.FileName);
        }

        private void menuOpenWorkspace_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Powerbuilder Workspace (*.pbw)|*.pbw";

            if (!dialog.ShowDialog().Value)
                return;

            this.openWorkspace(dialog.FileName);
        }

        private void openLibrary(string libraryPath)
        {
            ILibrary lib = null;
            if (libraryPath.EndsWith(".pbl"))
                lib = new Library(libraryPath, this.pbVersion);
            else
                lib = new VirtualLibrary(libraryPath);

            listPowerObjects.ItemsSource = lib.EntryList;

            listWorkspace.DataContext = null;
            listWorkspace.Visibility = System.Windows.Visibility.Collapsed;
            // TODO: Workspace ist jetzt ein Dock
            //gridWorkspace.Width = new GridLength(0);
        }

        private void openWorkspace(string workspacePath)
        {
            workspace = new Workspace(workspacePath, this.pbVersion);

            stWorkspace.Text = workspacePath;
            stVersion.Text = workspace.MajorVersion + "." + workspace.MinorVersion;

            listWorkspace.DataContext = workspace.Targets;
            listWorkspace.Visibility = System.Windows.Visibility.Visible;
            // TODO: Workspace ist jetzt ein Dock
            //gridWorkspace.Width = new GridLength(300);
        }
    }
}