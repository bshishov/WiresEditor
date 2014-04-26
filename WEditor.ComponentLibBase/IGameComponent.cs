#region

using System;

#endregion

namespace WEditor.ComponentLibBase
{
    /// <summary>
    ///     Base interface for all game components
    /// </summary>
    public interface IGameComponent
    {
    }

    public interface IGameComponentMetadata
    {
        #region Properties

        Type Type { get; }

        #endregion
    }
}