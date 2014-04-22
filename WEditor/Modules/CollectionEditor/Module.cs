#region

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Results;
using Gemini.Modules.MainMenu.Models;

#endregion

namespace WEditor.Modules.CollectionEditor
{
    //[Export(typeof (IModule))]
    internal class Module : ModuleBase
    {
        #region Methods

        public override void Initialize()
        {
            MainMenu.All.First(x => x.Name == "View")
                .Add(new MenuItem("Collection Editor", Open));
        }

        private static IEnumerable<IResult> Open()
        {
            yield return Show.Tool<ICollectionEditor>();
        }

        #endregion
    }
}