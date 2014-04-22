#region

using System.ComponentModel.Composition;
using System.IO;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;

#endregion

namespace WEditor.Models
{
    [Export(typeof (IEditorProvider))]
    internal class EditorProvider : IEditorProvider
    {
        #region IEditorProvider Members

        public bool Handles(string path)
        {
            var extension = Path.GetExtension(path);
            return extension == ".json";
        }

        public IDocument Create(string path)
        {
            IoC.Get<IProjectService>().Open(path);
            return null;
        }

        #endregion
    }
}