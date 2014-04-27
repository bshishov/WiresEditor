using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace WEditor.DefaultComponents
{
    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(ActionOnCollide))]
    class ActionOnCollide : IGameComponent
    {
        public string ActionName { get; set; }
        public string ActionArg { get; set; }
    }
}
