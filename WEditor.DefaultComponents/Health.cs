using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace WEditor.DefaultComponents
{
    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(Health))]
    class Health : IGameComponent
    {
        public int Value { get; set; }
        public int MaxValue { get; set; }

        public override string ToString()
        {
            return "Health";
        }

        public Health()
        {
            Value = 10;
            MaxValue = 10;
        }
    }
}
