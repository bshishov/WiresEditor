using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WiresCommon
{
    /// <summary>
    /// The thing that contains everything needed for the editor and the game
    /// </summary>
    public class Project
    {
        /// <summary>
        /// All locations
        /// </summary>
        public ObservableCollection<Location> Locations { get; set; }
        
        /// <summary>
        /// Prefabs (templates) for the object instances
        /// </summary>
        public List<ObjectIntance> ObjectPrefabs;

        public Project()
        {
            Locations = new ObservableCollection<Location>();
            ObjectPrefabs = new List<ObjectIntance>();
        }
    }
}