#region

using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

#endregion

namespace WEditor.DefaultComponents
{
    [Export(typeof (IGameComponent))]
    [ExportMetadata("Type", typeof (Health))]
    internal class Health : IGameComponent
    {
        #region Constructors

        public Health()
        {
            Value = 10;
            MaxValue = 10;
        }

        #endregion

        #region Properties

        public int Value { get; set; }
        public int MaxValue { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Health";
        }

        #endregion
    }
}