using System;
using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace WEditor.DefaultComponents
{
    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(Script))]
    class Script : IGameComponent
    {
        public string Name { get; set; }
        public string Arg { get; set; }
    }
}
