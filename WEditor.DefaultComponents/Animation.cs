using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace WEditor.DefaultComponents
{
    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(Animation))]
    class Animation : IGameComponent
    {
        public float FrameTime { get; set; }
    }
}
