using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace WEditor.DefaultComponents
{

    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(Interactive))]
    class Sound : IGameComponent
    {
        public string SongName;
    }
}
