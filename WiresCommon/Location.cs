using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WiresCommon
{
    public class Location
    {
        public string Name { get; set; }
        public ObservableCollection<Layer> Layers { get; set; }

        public Location()
        {
            Name = "DefaultLocation";
            Layers = new ObservableCollection<Layer>();
        }
    }
}