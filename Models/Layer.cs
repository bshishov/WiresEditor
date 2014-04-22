#region

using System.Collections.ObjectModel;
using Models.Components;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

#endregion

namespace Models
{
    public class Layer
    {
        #region Constructors

        public Layer()
        {
            Name = "DefaultLayer";
            Objects = new ObservableCollection<ObjectIntance>();
            Visible = true;
            DefaultStyle = new EditorInfo();
            Order = 0;
        }

        #endregion

        #region Properties

        [PropertyOrder(0)]
        public string Name { get; set; }

        [PropertyOrder(1)]
        public ObservableCollection<ObjectIntance> Objects { get; set; }

        [PropertyOrder(2)]
        public int Order { get; set; }

        [PropertyOrder(3)]
        public bool Visible { get; set; }

        [PropertyOrder(4)]
        [ExpandableObject]
        public EditorInfo DefaultStyle { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format("Layer: {0}", Name);
        }

        #endregion
    }
}