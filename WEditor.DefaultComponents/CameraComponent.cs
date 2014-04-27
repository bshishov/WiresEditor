using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace WEditor.DefaultComponents
{
    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(CameraComponent))]
    class CameraComponent : IGameComponent
    {
        public float Zoom { get; set; }
        public float LeftBound { get; set; }
        public float RightBound { get; set; }
        public float TopBound { get; set; }
        public float BottomBound { get; set; }
    }
}
