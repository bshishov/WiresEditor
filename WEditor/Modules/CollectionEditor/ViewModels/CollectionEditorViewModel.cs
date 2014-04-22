#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Gemini.Framework;
using Gemini.Framework.Services;

#endregion

namespace WEditor.Modules.CollectionEditor.ViewModels
{
    [Export(typeof (ICollectionEditor))]
    internal class CollectionEditorViewModel : Tool, ICollectionEditor
    {
        #region Fields

        private IList _itemsSource;
        private Type _itemsSourceType;
        private IList<Type> _newItemTypes;

        #endregion

        #region Constructors

        public CollectionEditorViewModel()
        {
            DisplayName = "Collection Editor";
        }

        #endregion

        #region ICollectionEditor Members

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Right; }
        }

        public Type ItemsSourceType
        {
            get { return _itemsSourceType; }
            set
            {
                _itemsSourceType = value;
                NotifyOfPropertyChange(() => ItemsSourceType);
            }
        }

        public IList ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                _itemsSource = value;
                NotifyOfPropertyChange(() => ItemsSource);
            }
        }

        public IList<Type> NewItemTypes
        {
            get { return _newItemTypes; }
            set
            {
                _newItemTypes = value;
                NotifyOfPropertyChange(() => NewItemTypes);
            }
        }

        #endregion
    }
}