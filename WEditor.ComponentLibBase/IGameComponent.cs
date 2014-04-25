using System;

namespace WEditor.ComponentLibBase
{
    /// <summary>
    /// Base interface for all game components
    /// </summary>
    public interface IGameComponent
    {
    }

    public interface IGameComponentMetadata
    {
        Type Type { get; }
    }
}