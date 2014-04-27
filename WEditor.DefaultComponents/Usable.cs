using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace WEditor.DefaultComponents
{
    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(Usable))]
    class Usable : IGameComponent
    {
        public string ActionName { get; set; }
        public string ActionArg { get; set; }
    }
}