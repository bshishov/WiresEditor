#region

using System.ComponentModel.Composition;

#endregion

namespace WEditor.Utilities
{
    [Export(typeof (IStartupArgs))]
    internal class StartupArgs : IStartupArgs
    {
        #region IStartupArgs Members

        public string[] Args { get; set; }

        #endregion
    }
}