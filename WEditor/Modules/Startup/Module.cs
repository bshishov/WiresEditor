#region

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Results;
using Gemini.Framework.Services;
using Gemini.Modules.MainMenu.Models;
using Gemini.Modules.PropertyGrid;
using Microsoft.Win32;
using Models;
using WEditor.Models;
using WEditor.Modules.Editor.ViewModels;
using WEditor.Modules.ProjectBrowser.ViewModels;
using WEditor.Utilities;

#endregion

namespace WEditor.Modules.Startup
{
    [Export(typeof (IModule))]
    public class Module : ModuleBase
    {
        #region Fields

        [Import] private ProjectBrowserViewModel _projectBrowser;
        [Import] private IProjectService _projectService;
        [Import] private IPropertyGrid _propertyGrid;
        [Import] private IResourceManager _resourceManager;
        [Import] private IStartupArgs _startupArgs;

        #endregion

        #region Properties

        public override IEnumerable<Type> DefaultTools
        {
            get
            {
                yield return typeof (IPropertyGrid);
                yield return typeof (ProjectBrowserViewModel);
            }
        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            var fileMenu = MainMenu.All.First(x => x.Name == "File");
            var standartOpen = fileMenu.Children.FirstOrDefault(c => c.Name.Contains("Open"));
            if (standartOpen != null)
                fileMenu.Children.Remove(standartOpen);
            fileMenu.Children.Insert(0, new MenuItem("_New", NewFile)
                .WithIcon("/Resources/NewDocumentHS.png")
                .WithGlobalShortcut(ModifierKeys.Control, Key.N));
            fileMenu.Children.Insert(1, new MenuItem("_Open", OpenFile)
                .WithIcon("/Resources/openfolderHS.png")
                .WithGlobalShortcut(ModifierKeys.Control, Key.O));
            fileMenu.Children.Insert(2, new MenuItem("_Save", SaveFile)
                .WithIcon("/Resources/saveHS.png")
                .WithGlobalShortcut(ModifierKeys.Control, Key.S));
            fileMenu.Children.Insert(3, new MenuItem("_Save As", SaveAsFile)
                .WithIcon("/Resources/saveHS.png"));

            MainWindow.WindowState = WindowState.Maximized;
            MainWindow.Title = "Wires Editor";
            MainWindow.Icon = _resourceManager.GetBitmap("Resources/Icon.png",
                Assembly.GetExecutingAssembly().GetAssemblyName());

            _projectService.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "CurrentProjectFilePath")
                    MainWindow.Title = "Wires Editor - " + _projectService.CurrentProjectFilePath;
            };

            if (_startupArgs.Args.Length > 0)
                _projectService.Open(_startupArgs.Args[0]);
        }

        public override void PostInitialize()
        {
            while (Shell.Documents.Count > 0)
                Shell.CloseDocument(Shell.Documents.First());    
        }

        private IEnumerable<IResult> NewFile()
        {
            yield return _projectService.CreateNew();
        }

        private IEnumerable<IResult> OpenFile()
        {
            var dialog = new OpenFileDialog {DefaultExt = "json", AddExtension = true, Filter = "Json Files | *.json"};
            yield return Show.Dialog(dialog);
            yield return _projectService.Open(dialog.FileName);
        }

        private IEnumerable<IResult> SaveFile()
        {
            if (_projectService.CurrentProject == null || string.IsNullOrEmpty(_projectService.CurrentProjectFilePath))
            {
                yield return new LambdaResult(delegate { Debug.WriteLine("Error saving"); });
                yield break;
            }

            yield return _projectService.Save(_projectService.CurrentProjectFilePath);
        }

        private IEnumerable<IResult> SaveAsFile()
        {
            var dialog = new SaveFileDialog {DefaultExt = _projectService.DefaultExt, AddExtension = true, Filter = "Json Files | *.json"};
            yield return Show.Dialog(dialog);
            yield return _projectService.Save(dialog.FileName);
        }

        #endregion
    }
}