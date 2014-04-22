#region

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Results;
using Gemini.Modules.MainMenu.Models;
using WEditor.Modules.ProjectBrowser.ViewModels;

#endregion

namespace WEditor.Modules.ProjectBrowser
{
    [Export(typeof (IModule))]
    internal class Module : ModuleBase
    {
        #region Methods

        public override void Initialize()
        {
            MainMenu.All
                .First(x => x.Name == "View")
                .Add(new MenuItem("Project Explorer", OpenProjectBrowser));
        }

        private static IEnumerable<IResult> OpenProjectBrowser()
        {
            yield return Show.Tool<ProjectBrowserViewModel>();
        }

        #endregion
    }
}