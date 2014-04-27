using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace WEditor.DefaultComponents
{
    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(State))]
    class State : IGameComponent
    {
        public string Value { get; set; }
    }
}