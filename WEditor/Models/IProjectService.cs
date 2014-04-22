#region

using Caliburn.Micro;
using Models;

#endregion

namespace WEditor.Models
{
    internal interface IProjectService : INotifyPropertyChangedEx
    {
        #region Properties

        Project CurrentProject { get; }
        string DefaultExt { get; }

        string CurrentProjectFilePath { get; }

        string CurrentProjectFolder { get; }

        string CurrentProjectFileName { get; }

        string CurrentProjectResourcesPath { get; }

        #endregion

        #region Methods

        IResult Open(string path);

        IResult Save(string path);

        IResult CreateNew();

        #endregion
    }
}