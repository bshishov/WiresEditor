using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace WEditor.DefaultComponents
{
    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(CameraComponent))]
    class CameraComponent : IGameComponent
    {
        public float Zoom;
        public float LeftBound;
        public float RightBound;
        public float TopBound;
        public float BottomBound;
    }
}
