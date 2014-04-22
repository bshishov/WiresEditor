using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Models.Components
{
    [ExportComponent(typeof(Collidable))]
    class Collidable : IComponent
    {
        public override string ToString()
        {
            return "Collidable";
        }

        [PropertyOrder(0)]
        [Category("Offset")]
        [Description("Offset to object position along X axis relative to object size")]
        public float OffsetX { get; set; }

        [PropertyOrder(1)]
        [Category("Offset")]
        [Description("Offset to object position along Y axis relative to object size")]
        public float OffsetY { get; set; }

        [PropertyOrder(2)]
        [Category("Scale")]
        [Description("Scale along X axis relative to object size")]
        public float ScaleX { get; set; }

        [PropertyOrder(3)]
        [Category("Scale")]
        [Description("Scale along Y axis relative to object size")]
        public float ScaleY { get; set; }

        public Collidable()
        {
            OffsetX = 0;
            OffsetY = 0;
            ScaleX = 1;
            ScaleY = 1;
        }
    }
}
