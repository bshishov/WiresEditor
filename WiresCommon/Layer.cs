using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WiresCommon
{
    public class Layer
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public bool Visible { get; set; }
        public ObservableCollection<ObjectIntance> Objects { get; set; }

        public Layer()
        {
            Name = "DefaultLayer";
            Objects = new ObservableCollection<ObjectIntance>();
            Visible = true;
            Order = 0;
        }
    }
}