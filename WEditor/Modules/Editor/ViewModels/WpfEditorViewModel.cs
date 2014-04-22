#region

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Modules.PropertyGrid;
using Models;

#endregion

namespace WEditor.Modules.Editor.ViewModels
{
    [Export(typeof (WpfEditorViewModel))]
    internal class WpfEditorViewModel : Document
    {
        #region Fields

        private bool _isMovingObject;
        private bool _isPanning;
        private double _offsetX;
        private double _offsetY;
        private double _scaleFactor = 1;
        private ObjectIntance _selectedObject;
        private double _size = 2000;
        private World _world;

        #endregion

        #region Constructors

        public WpfEditorViewModel()
        {
            DisplayName = "Editor";
        }

        public WpfEditorViewModel(World world)
        {
            CurrentWorld = world;
        }

        #endregion

        #region Properties

        public World CurrentWorld
        {
            get { return _world; }
            set
            {
                _world = value;
                DisplayName = _world.Name;
                NotifyOfPropertyChange(() => CurrentWorld);
                NotifyOfPropertyChange(() => Layers);
            }
        }

        public ObjectIntance SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                _selectedObject = value;
                IoC.Get<IPropertyGrid>().SelectedObject = _selectedObject;
                NotifyOfPropertyChange(() => SelectedObject);
            }
        }

        public ObservableCollection<Layer> Layers
        {
            get
            {
                if (_world == null) return null;
                return _world.Layers;
            }
            set
            {
                _world.Layers = value;
                NotifyOfPropertyChange(() => Layers);
            }
        }

        public double OffsetX
        {
            get { return _offsetX; }
            set
            {
                _offsetX = value;
                NotifyOfPropertyChange(() => OffsetX);
            }
        }

        public double OffsetY
        {
            get { return _offsetY; }
            set
            {
                _offsetY = value;
                NotifyOfPropertyChange(() => OffsetY);
            }
        }

        public double ScaleFactor
        {
            get { return _scaleFactor; }
            set
            {
                _scaleFactor = value;
                NotifyOfPropertyChange(() => ScaleFactor);
            }
        }

        public double Size
        {
            get { return _size; }
            set
            {
                _size = value;
                NotifyOfPropertyChange(() => Size);
            }
        }

        public bool IsPanning
        {
            get { return _isPanning; }
            set
            {
                _isPanning = value;
                NotifyOfPropertyChange(() => IsPanning);
            }
        }

        #endregion

        #region Methods

        public void Test1()
        {
            Debug.WriteLine("Scrolling!!!!!!!!!!!!!");
        }

        public void Test2()
        {
            Debug.WriteLine("PANNING!!!!!!!!!!!!!");
        }

        private void DragDelta(EventArgs e)
        {
            var ea = e as DragDeltaEventArgs;
            IsPanning = true;
            OffsetX = (OffsetX + (((ea.HorizontalChange/10)*-1)*ScaleFactor));
            OffsetY = (OffsetY + (((ea.VerticalChange/10)*-1)*ScaleFactor));


            IsPanning = false;

            Debug.WriteLine("PANNING!!!!!!!!!!!!!");
        }

        public void OnMouseDown(object sender, EventArgs raw)
        {
            if (_selectedObject != null)
                _isMovingObject = true;
        }

        public void OnMouseUp(object sender, EventArgs raw)
        {
            if (_selectedObject != null)
                _isMovingObject = false;
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (sender != null && _selectedObject != null && _isMovingObject)
            {
                var pos = e.GetPosition((IInputElement) sender);
                SelectedObject.Transform.X = (int) pos.X;
                SelectedObject.Transform.Y = (int) pos.Y;
            }
        }

        public void OnDoubleClick(object sender, MouseEventArgs e)
        {
            if (CurrentWorld == null) return;
            var pos = e.GetPosition((IInputElement) sender);
            var newobj = new ObjectIntance();
            newobj.Transform.X = (int) pos.X;
            newobj.Transform.Y = (int) pos.Y;
            CurrentWorld.Layers[0].Objects.Add(newobj);
            SelectedObject = newobj;
        }

        #endregion
    }
}