using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace WEditor.DefaultComponents
{
    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(Lethal))]
    class Lethal : IGameComponent
    {
    }
}