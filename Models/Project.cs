#region

using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace Models
{
    /// <summary>
    ///     The thing that contains everything needed for the editor and the game
    /// </summary>
    public class Project
    {
        #region Fields

        /// <summary>
        ///     Prefabs (templates) for the object instances
        /// </summary>
        public List<ObjectIntance> ObjectPrefabs;

        #endregion

        #region Constructors

        public Project()
        {
            Locations = new ObservableCollection<World>();
            ObjectPrefabs = new List<ObjectIntance>();
            ResourcesPath = "\\Resources\\";
        }

        #endregion

        #region Properties

        /// <summary>
        ///     All locations
        /// </summary>
        public ObservableCollection<World> Locations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ResourcesPath { get; set; }

        #endregion
    }
}