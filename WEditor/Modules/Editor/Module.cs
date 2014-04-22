#region

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Results;
using Gemini.Modules.Inspector;
using Gemini.Modules.MainMenu.Models;
using Gemini.Modules.PropertyGrid;
using WEditor.Modules.Editor.ViewModels;

#endregion

namespace WEditor.Modules.Editor
{
    [Export(typeof (IModule))]
    internal class Module : ModuleBase
    {
        #region Fields

        [Import] private IInspectorTool _inspector;
        [Import] private IPropertyGrid _propertyGrid;

        #endregion

        #region Methods

        public override void Initialize()
        {
            MainMenu.All.First(x => x.Name == "View").Add(new MenuItem("Editor", OpenGlEditor));
        }

        private IEnumerable<IResult> OpenGlEditor()
        {
            yield return Show.Document<GlViewModel>();
        }

        #endregion
    }
}