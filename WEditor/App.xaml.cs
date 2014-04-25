#region

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Windows;
using Caliburn.Micro;
using WEditor.ComponentLibBase;
using WEditor.Utilities;

#endregion

namespace WEditor
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Methods

        private void AppStartup(object sender, StartupEventArgs e)
        {
            IoC.Get<IStartupArgs>().Args = e.Args;
        }

        #endregion
    }
}