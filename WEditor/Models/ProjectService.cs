#region

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Windows;
using Caliburn.Micro;
using Gemini.Framework.Results;
using Models;
using Newtonsoft.Json;

#endregion

namespace WEditor.Models
{
    [Export(typeof (IProjectService))]
    internal class ProjectService : PropertyChangedBase, IProjectService
    {
        #region Fields

        private string _filePath;
        private string _folder;
        private string _fileName;
        private Project _project;

        #endregion

        #region IProjectService Members

        public Project CurrentProject
        {
            get { return _project; }
            private set
            {
                _project = value;
                NotifyOfPropertyChange(() => CurrentProject);
                NotifyOfPropertyChange(() => CurrentProjectResourcesPath);
            }
        }

        public string CurrentProjectResourcesPath
        {
            get
            {
                if(CurrentProject != null)
                    return CurrentProjectFolder + CurrentProject.ResourcesPath;
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
            }
        }

        public IResult Open(string path)
        {
            try
            {
                CurrentProject = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
                CurrentProjectFilePath = path;
                CurrentProjectFileName = Path.GetFileName(path);
                CurrentProjectFolder = new FileInfo(path).DirectoryName;
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
            CurrentProjectFilePath = path;
            File.WriteAllText(path, JsonConvert.SerializeObject(_project, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            }));
            return new LambdaResult(delegate { Debug.WriteLine("Saved"); });
        }

        public IResult CreateNew()
        {
            var project = new Project();
            project.Locations.Add(new World());
            CurrentProject = project;
            return new LambdaResult(delegate { Debug.WriteLine("Created"); });
        }

        public string DefaultExt
        {
            get { return "json"; }
        }

        public string CurrentProjectFilePath
        {
            get { return _filePath; }
            private set
            {
                _filePath = value;
                NotifyOfPropertyChange(() => CurrentProjectFilePath);
            }
        }


        public string CurrentProjectFileName
        {
            get { return _fileName; }
            private set
            {
                _fileName = value;
                NotifyOfPropertyChange(() => CurrentProjectFileName);
            }
        }

        #endregion
    }
}