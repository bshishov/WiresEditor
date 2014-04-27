#region

using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Documents;
using System.Windows.Media;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Modules.PropertyGrid;
using Models;
using WEditor.ComponentLibBase;
using WEditor.Models;
using WEditor.Modules.Editor.Controls;
using WEditor.Modules.ProjectBrowser.ViewModels;
using WEditor.Utilities;
using Xceed.Wpf.Toolkit;

#endregion

namespace WEditor.Modules.Editor.ViewModels
{
    [Export(typeof (GlViewModel))]
    public class GlViewModel : Document
    {
        #region Fields

        private Color _background = Colors.White;

        private Color _gridColor = Colors.LightGray;

        private float _gridSize;
        private ObjectIntance _selectedObject;

        private World _world;

        private bool _parallaxMode = false;
        private bool _showBorders = true;

        #endregion

        #region Constructors

        public GlViewModel()
        {
            DisplayName = "[CLOSE ME]";
            GridSize = 50;
        }

        public GlViewModel(World world)
        {
            CurrentWorld = world;
            DisplayName = CurrentWorld.Name;
            world.PropertyChanged += (s, e) => { if (e.PropertyName == "Name") DisplayName = CurrentWorld.Name; };
            GridSize = 50;
        }

        #endregion

        #region Properties

        public Color Background
        {
            get { return _background; }
            set
            {
                _background = value;
                NotifyOfPropertyChange(() => Background);
            }
        }

        public Color GridColor
        {
            get { return _gridColor; }
            set
            {
                _gridColor = value;
                NotifyOfPropertyChange(() => GridColor);
            }
        }

        public float GridSize
        {
            get { return _gridSize; }
            set
            {
                _gridSize = value;
                NotifyOfPropertyChange(() => GridSize);
            }
        }

        public World CurrentWorld
        {
            get { return _world; }
            set
            {
                _world = value;
                NotifyOfPropertyChange(() => CurrentWorld);
            }
        }

        public bool ShowBorders
        {
            get { return _showBorders; }
            set
            {
                _showBorders = value;
                NotifyOfPropertyChange(()=>ShowBorders);
            }
        }
        public bool ParallaxMode
        {
            get { return _parallaxMode; }
            set
            {
                _parallaxMode = value;
                NotifyOfPropertyChange(() => ParallaxMode);
            }
        }

        public ObjectIntance SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                _selectedObject = value;
                NotifyOfPropertyChange(() => SelectedObject);
            }
        }

        public string ResourcesPath
        {
            get
            {
                var ps = IoC.Get<IProjectService>();
                if (ps == null) return "";
                return ps.CurrentProjectResourcesPath ?? "";
            }
        }

        #endregion

        #region Methods

        public void OnSelect(EventArgs args)
        {
            var a = args as SelectionChangedEventArgs;
            if (a == null)
                return;
            if (a.Selected == null)
                IoC.Get<IPropertyGrid>().SelectedObject = this;
            else
                IoC.Get<IPropertyGrid>().SelectedObject = a.Selected;
        }

        public void OnDoubleClick(EventArgs args)
        {
            if (CurrentWorld == null) return;
            if (SelectedObject == null)
            {
                var a = args as ClickEventArgs;
                Layer layer;
                if (IoC.Get<ProjectBrowserViewModel>().SelectedObject is Layer)
                    layer = (Layer) IoC.Get<ProjectBrowserViewModel>().SelectedObject;
                else
                    return;
                if (layer == null || a == null)
                    return;
                var newObject = ObjectIntance.CreateEditorDefault(layer.DefaultStyle);
                if (newObject.EditorInfo.SnapToGrid)
                {
                    newObject.Transform.X = MathUtilities.Snap(a.Position.X, GridSize);
                    newObject.Transform.Y = MathUtilities.Snap(a.Position.Y, GridSize);
                }
                else
                {
                    newObject.Transform.X = a.Position.X;
                    newObject.Transform.Y = a.Position.Y;
                }
                layer.Objects.Add(newObject);
                SelectedObject = newObject;
            }
            else
            {
                var projectService = IoC.Get<IProjectService>();
                var dialog = new CollectionControlDialog(typeof (IGameComponent))
                {
                    Title = "Components",
                    ItemsSource = SelectedObject.Components,
                    NewItemTypes = projectService.GetAvailComponentTypes()
                };
                dialog.Show();
            }
        }

        public override bool ShouldSerializeIsNotifying()
        {
            return false;
        }

        #endregion
    }
}