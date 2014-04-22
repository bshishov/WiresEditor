#region

using System;
using System.Collections;
using System.Collections.Generic;
using Gemini.Framework;

#endregion

namespace WEditor.Modules.CollectionEditor
{
    internal interface ICollectionEditor : ITool
    {
        #region Properties

        Type ItemsSourceType { get; set; }
        IList ItemsSource { get; set; }
        IList<Type> NewItemTypes { get; set; }

        #endregion
    }
}