#region

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Windows;
using Caliburn.Micro;
using Gemini.Framework.Results;
using Models;
using Newtonsoft.Json;
using WEditor.ComponentLibBase;

#endregion

namespace WEditor.Models
{
    [Export(typeof (IProjectService))]
    internal class ProjectService : PropertyChangedBase, IProjectService
    {
        #region Fields

        private string _fileName;
        private string _folder;
        private Project _project;

        #endregion

        #region Properties

        [ImportMany]
        public Lazy<IGameComponent, IGameComponentMetadata>[] GameComponents { get; private set; }

        #endregion

        #region IProjectService Members

        public Project CurrentProject
        {
            get { return _project; }
            private set
            {
                _project = value;
                _project.PropertyChanged += (sender, args) =>
                {
                    Uri uri;
                    if (
                        args.PropertyName == "ResourcesPath" &&
                        CurrentProjectFolder != null &&
                        Uri.TryCreate(_project.ResourcesPath, UriKind.Absolute, out uri))
                    {
                        _project.ResourcesPath = GetRelativeUri(uri).ToString().Replace("/", "\\");
                    }
                };
                NotifyOfPropertyChange(() => CurrentProject);
                NotifyOfPropertyChange(() => CurrentProjectResourcesPath);
            }
        }

        public string CurrentProjectResourcesPath
        {
            get
            {
                if (CurrentProject != null)
                    return CurrentProjectFolder + CurrentProject.ResourcesPath + "\\";
                return null;
            }
        }

        public string CurrentProjectFolder
        {
            get { return _folder; }
            private set
            {
                _folder = value;
                NotifyOfPropertyChange(() => CurrentProjectFolder);
                NotifyOfPropertyChange(() => CurrentProjectResourcesPath);
                NotifyOfPropertyChange(() => CurrentProjectFilePath);
            }
        }

        public IResult Open(string path)
        {
            try
            {
                CurrentProject = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
                });
                CurrentProjectFileName = Path.GetFileName(path);
                CurrentProjectFolder = new FileInfo(path).DirectoryName + "\\";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error while opening", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            return new LambdaResult(delegate { Debug.WriteLine("Loaded"); });
        }

        public IResult Save(string path)
        {
            var fi = new FileInfo(path);
            CurrentProjectFileName = fi.Name;
            CurrentProjectFolder = fi.DirectoryName + "\\";

            File.WriteAllText(path, JsonConvert.SerializeObject(_project, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
            }));
            return new LambdaResult(delegate { Debug.WriteLine("Saved"); });
        }

        public IResult CreateNew()
        {
            var project = new Project();
            project.Locations.Add(new World());
            CurrentProject = project;
            CurrentProjectFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";
            CurrentProjectFileName = Path.GetRandomFileName();
            return new LambdaResult(delegate { Debug.WriteLine("Created"); });
        }

        public string DefaultExt
        {
            get { return "json"; }
        }

        public string CurrentProjectFilePath
        {
            get { return CurrentProjectFolder + CurrentProjectFileName; }
        }


        public string CurrentProjectFileName
        {
            get { return _fileName; }
            private set
            {
                _fileName = value;
                NotifyOfPropertyChange(() => CurrentProjectFileName);
                NotifyOfPropertyChange(() => CurrentProjectFilePath);
            }
        }

        public List<Type> GetAvailComponentTypes()
        {
            return GameComponents.Select(gameComponent => gameComponent.Metadata.Type).ToList();
        }

        #endregion

        #region Methods

        public Uri GetRelativeUri(Uri uri)
        {
            var relRoot = new Uri(CurrentProjectFolder, UriKind.Absolute);
            return relRoot.MakeRelativeUri(uri);
        }

        #endregion
    }
}