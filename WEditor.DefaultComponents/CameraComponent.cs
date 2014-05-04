using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace WEditor.DefaultComponents
{
    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(CameraComponent))]
    class CameraComponent : IGameComponent
    {
        public float Zoom { get; set; }
        public CameraComponent()
        {
            Zoom = 1f;
        }
    }
}
