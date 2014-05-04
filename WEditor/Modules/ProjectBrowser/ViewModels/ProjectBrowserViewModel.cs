#region

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.PropertyGrid;
using Models;
using WEditor.ComponentLibBase;
using WEditor.Models;
using WEditor.Modules.Editor.ViewModels;
using Xceed.Wpf.Toolkit;

#endregion

namespace WEditor.Modules.ProjectBrowser.ViewModels
{
    [Export(typeof (ProjectBrowserViewModel))]
    internal class ProjectBrowserViewModel : Tool
    {
        #region Fields

        private object _selected;
        private ObjectIntance _clipBoard;

        #endregion

        #region Constructors

        public ProjectBrowserViewModel()
        {
            DisplayName = "Project Explorer";
            IoC.Get<IProjectService>().PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "CurrentProject")
                    NotifyOfPropertyChange(() => Projects);
            };
        }

        #endregion

        #region Properties

        public IEnumerable<Project> Projects
        {
            get
            {
                var project = IoC.Get<IProjectService>().CurrentProject;
                return project != null ? new List<Project> {IoC.Get<IProjectService>().CurrentProject} : null;
            }
        }

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        public override double PreferredWidth
        {
            get { return 200; }
        }

        public Object SelectedObject
        {
            get { return _selected; }
            set
            {
                _selected = value;
                NotifyOfPropertyChange(() => SelectedObject);
            }
        }

        #endregion

        #region Methods

        private Project GetParentFor(World world, Project project)
        {
            return project.Locations.Contains(world) ? project : null;
        }

        private World GetParentFor(Layer layer, Project project)
        {
            return project.Locations
                .FirstOrDefault(world => world.Layers.Contains(layer));
        }

        private Layer GetParentFor(ObjectIntance obj, Project project)
        {
            return project.Locations
                .SelectMany(location => location.Layers)
                .FirstOrDefault(layer => layer.Objects.Contains(obj));
        }

        private ObjectIntance GetParentFor(IGameComponent gameComponent, Project project)
        {
            return project.Locations
                .SelectMany(location => location.Layers)
                .SelectMany(layer => layer.Objects)
                .FirstOrDefault(obj => obj.Components.Contains(gameComponent));
        }

        public void OnSelectionChanged(object sender, EventArgs e)
        {
            var args = e as RoutedPropertyChangedEventArgs<object>;
            if (args == null || args.NewValue == null)
            {
                SelectedObject = null;
                return;
            }
            var obj = args.NewValue;

            if (obj is ObjectIntance)
                OnSelectObject(obj);

            if (obj is IGameComponent)
                OnSelectComponent(obj);

            if (obj is World)
                OnSelectWorld(obj);

            if (obj is Layer)
                OnSelectLayer(obj);

            if (obj is Project)
                OnSelectProject(obj);

            SelectedObject = obj;
        }

        #endregion

        #region Project ContextMenu

        public void OnAddWorldToProject(object item)
        {
            var project = item as Project;
            if (project == null)
                return;
            project.Locations.Add(new World());
        }

        public void OnSelectProject(object item)
        {
            var project = item as Project;
            if (project == null)
                return;
            IoC.Get<IPropertyGrid>().SelectedObject = item;
        }

        #endregion

        #region World ContextMenu

        public void OnSelectWorld(object item)
        {
            var world = item as World;
            if (world == null)
                return;
            OnShowWorldInEditor(item);
            OnShowWorldProperties(item);
        }

        public void OnAddLayerToWorld(object item)
        {
            var world = item as World;
            if (world == null)
                return;
            world.Layers.Insert(0, new Layer());
        }

        public void OnDeleteWorld(object item)
        {
            var world = item as World;
            if (world == null)
                return;

            var doc = IoC.Get<IShell>().Documents
                .OfType<GlViewModel>()
                .FirstOrDefault(d => d.CurrentWorld.Equals(world));

            if (doc != null)
                doc.TryClose();

            var project = GetParentFor(world, Projects.First());
            if (project != null)
                project.Locations.Remove(world);
        }

        public void OnShowWorldInEditor(object item)
        {
            var world = item as World;
            if (world == null)
                return;

            var doc = IoC.Get<IShell>().Documents.FirstOrDefault(
                l => (l is GlViewModel) &&
                     ((GlViewModel) l).CurrentWorld != null &&
                     ((GlViewModel) l).CurrentWorld.Equals(world));

            // If no opened document with this World - create new window
            // else reopen existing
            IoC.Get<IShell>().OpenDocument(doc ?? new GlViewModel(world));
        }

        public void OnShowWorldProperties(object item)
        {
            var world = item as World;
            if (world == null)
                return;
            IoC.Get<IPropertyGrid>().SelectedObject = item;
        }

        #endregion

        #region Layer ContextMenu

        public void OnSelectLayer(object item)
        {
            var layer = item as Layer;
            if (layer == null)
                return;
            IoC.Get<IPropertyGrid>().SelectedObject = layer;
        }

        public void OnDeleteLayer(object item)
        {
            var layer = item as Layer;
            if (layer == null)
                return;
            var world = GetParentFor(layer, Projects.First());
            if (world != null)
                world.Layers.Remove(layer);
        }

        public void OnAddEmptyObjectToLayer(object item)
        {
            var layer = item as Layer;
            if (layer == null)
                return;
            layer.Objects.Add(new ObjectIntance());
        }

        public void OnAddObjectToLayer(object item)
        {
            var layer = item as Layer;
            if (layer == null)
                return;
            layer.Objects.Add(ObjectIntance.CreateEditorDefault(layer.DefaultStyle));
        }

        public void OnMoveLayerUp(object item)
        {
            var layer = item as Layer;
            if (layer == null) return;
            var world = GetParentFor(layer, Projects.First());
            if (world == null) return;

            var last = world.Layers.IndexOf(layer);
            world.Layers.Move(last, Math.Max(0, last - 1));
            Console.WriteLine(world.Layers.Select(w => w.Name));
        }

        public void OnMoveLayerDown(object item)
        {
            var layer = item as Layer;
            if (layer == null) return;
            var world = GetParentFor(layer, Projects.First());
            if (world == null) return;

            var last = world.Layers.IndexOf(layer);
            world.Layers.Move(last, Math.Min(last + 1, world.Layers.Count - 1));
            Console.WriteLine(world.Layers.Select(w => w.Name));
        }

        public void OnPasteObject(object item)
        {
            var layer = item as Layer;
            if (layer == null) return;
            var world = GetParentFor(layer, Projects.First());
            if (world == null) return;

            if (_clipBoard != null)
            {
                var obj = _clipBoard.Clone() as ObjectIntance;
                if(obj != null)
                    layer.Objects.Add(obj);
            }
        }

        #endregion

        #region Object ContextMenu

        public void OnSelectObject(object item)
        {
            var obj = item as ObjectIntance;
            if (obj == null)
                return;
            IoC.Get<IPropertyGrid>().SelectedObject = obj;
            var doc = IoC.Get<IShell>().Documents.Where(d => d.IsActive).OfType<GlViewModel>().FirstOrDefault();
            if (doc != null)
                doc.SelectedObject = obj;
        }

        public void OnCopyObject(object item)
        {
            var obj = item as ObjectIntance;
            if (obj == null)
                return;

            _clipBoard = obj.Clone() as ObjectIntance;
        }

        public void OnDeleteObject(object item)
        {
            var obj = item as ObjectIntance;
            if (obj == null)
                return;
            var layer = GetParentFor(obj, Projects.First());
            if (layer != null)
                layer.Objects.Remove(obj);
        }

        public void OnMoveObjectUp(object item)
        {
            var obj = item as ObjectIntance;
            if (obj == null) return;
            var layer = GetParentFor(obj, Projects.First());
            if (layer == null) return;

            var last = layer.Objects.IndexOf(obj);
            layer.Objects.Move(last, Math.Max(0, last - 1));
            Console.WriteLine(layer.Objects.Select(w => w.Name));
        }

        public void OnMoveObjectDown(object item)
        {
            var obj = item as ObjectIntance;
            if (obj == null) return;
            var layer = GetParentFor(obj, Projects.First());
            if (layer == null) return;

            var last = layer.Objects.IndexOf(obj);
            layer.Objects.Move(last, Math.Min(last + 1, layer.Objects.Count - 1));
            Console.WriteLine(layer.Objects.Select(w => w.Name));
        }

        public void ShowComponents(object item)
        {
            var obj = item as ObjectIntance;
            if (obj == null)
                return;

            var projectService = IoC.Get<IProjectService>();
            var dialog = new CollectionControlDialog(typeof (IGameComponent))
            {
                Title = "Components",
                ItemsSource = obj.Components,
                NewItemTypes = projectService.GetAvailComponentTypes()
            };
            dialog.ShowDialog();
        }

        #endregion

        #region Component ContextMenu

        public void OnSelectComponent(object item)
        {
            var component = item as IGameComponent;
            if (component == null)
                return;
            IoC.Get<IPropertyGrid>().SelectedObject = component;
        }

        public void OnDeleteComponent(object item)
        {
            var component = item as IGameComponent;
            if (component == null)
                return;
            var obj = GetParentFor(component, Projects.First());
            if (obj != null)
                obj.Components.Remove(component);
        }

        #endregion
    }
}