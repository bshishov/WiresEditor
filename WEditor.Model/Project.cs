#region

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Caliburn.Micro;
using WEditor.CustomEditors;

#endregion

namespace Models
{
    /// <summary>
    ///     The thing that contains everything needed for the editor and the game
    /// </summary>
    public class Project : PropertyChangedBase
    {
        #region Fields

        /// <summary>
        ///     Prefabs (templates) for the object instances
        /// </summary>
        public List<ObjectIntance> ObjectPrefabs;

        private string _resourcesPath;

        #endregion

        #region Constructors

        public Project()
        {
            Locations = new ObservableCollection<World>();
            ObjectPrefabs = new List<ObjectIntance>();
            ResourcesPath = "Resources";
        }

        #endregion

        #region Properties

        /// <summary>
        ///     All locations
        /// </summary>
        public ObservableCollection<World> Locations { get; set; }

        [Editor(typeof (PathEditor), typeof (PathEditor))]
        [Description("Relative path to the resources folder")]
        public string ResourcesPath
        {
            get { return _resourcesPath; }
            set
            {
                _resourcesPath = value;
                NotifyOfPropertyChange(() => ResourcesPath);
            }
        }

        #endregion
    }
}