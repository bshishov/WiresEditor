using System.ComponentModel;
using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace WEditor.DefaultComponents
{
    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(Collidable))]
    class Collidable : IGameComponent
    {
        public override string ToString()
        {
            return "Collidable";
        }

        [Category("Offset")]
        [Description("Offset to object position along X axis relative to object size")]
        public float OffsetX { get; set; }

        [Category("Offset")]
        [Description("Offset to object position along Y axis relative to object size")]
        public float OffsetY { get; set; }
        
        [Category("Scale")]
        [Description("Scale along X axis relative to object size")]
        public float ScaleX { get; set; }
        
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
