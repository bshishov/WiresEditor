#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Models;
using Models.Components;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using WEditor.Utilities;
using Color = System.Windows.Media.Color;
using Orientation = Models.Components.Orientation;
using Transform = Models.Components.Transform;
using UserControl = System.Windows.Controls.UserControl;

#endregion

namespace WEditor.Modules.Editor.Controls
{
    public class SelectionChangedEventArgs : EventArgs
    {
        #region Constructors

        public SelectionChangedEventArgs(ObjectIntance lastobj, ObjectIntance newobj)
        {
            Last = lastobj;
            Selected = newobj;
        }

        #endregion

        #region Properties

        public ObjectIntance Last { get; private set; }
        public ObjectIntance Selected { get; private set; }

        #endregion
    }

    public class ClickEventArgs : EventArgs
    {
        #region Constructors

        public ClickEventArgs(PointF pos)
        {
            Position = pos;
        }

        #endregion

        #region Properties

        public PointF Position { get; private set; }

        #endregion
    }


    /// <summary>
    ///     Interaction logic for GlViewer.xaml
    /// </summary>
    public partial class GlViewer : UserControl
    {
        #region Delegates

        public delegate void DoubleClickEventHandler(object sender, ClickEventArgs e);

        public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);

        #endregion

        #region Constants

        public static readonly DependencyProperty GlBackgroundProperty = DependencyProperty.Register("GlBackground", typeof (Color), typeof (GlViewer));

        public static readonly DependencyProperty GridColorProperty = DependencyProperty.Register("GridColor", typeof (Color), typeof (GlViewer));

        public static readonly DependencyProperty GridSizeProperty = DependencyProperty.Register("GridSize", typeof (float), typeof (GlViewer));

        public static readonly DependencyProperty WorldProperty = DependencyProperty.Register("World", typeof (World), typeof (GlViewer));

        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof (ObjectIntance), typeof (GlViewer));

        public static readonly DependencyProperty ResourcesPathProperty = DependencyProperty.Register("ResourcesPath", typeof (string), typeof (GlViewer));

        #endregion

        #region Fields

        private readonly Color _selectionColor = Colors.LimeGreen;
        private readonly Dictionary<string, TextureInfo> _textures = new Dictionary<string, TextureInfo>();

        private Camera _camera;

        private PointF _dragOffset;
        private bool _dragging;
        private SpriteFont _font;
        private GLControl _glControl;
        private PointF _lastPanningPos;
        private bool _panning;

        private bool _scaling;

        #endregion

        #region Constructors

        public GlViewer()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        private float ScalerSize
        {
            get { return 5/_camera.Zoom; }
        }

        public Color GlBackground
        {
            get { return (Color) GetValue(GlBackgroundProperty); }
            set { SetValue(GlBackgroundProperty, value); }
        }

        public Color GridColor
        {
            get { return (Color) GetValue(GridColorProperty); }
            set { SetValue(GridColorProperty, value); }
        }

        public float GridSize
        {
            get { return (float) GetValue(GridSizeProperty); }
            set { SetValue(GridSizeProperty, value); }
        }

        public World World
        {
            get { return (World) GetValue(WorldProperty); }
            set { SetValue(WorldProperty, value); }
        }

        public string ResourcesPath
        {
            get { return (string) GetValue(ResourcesPathProperty); }
            set { SetValue(ResourcesPathProperty, value); }
        }

        public ObjectIntance SelectedObject
        {
            get { return (ObjectIntance) GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        #endregion

        #region Methods

        private void SetupViewport()
        {
            if (_glControl == null)
                return;
            GL.MatrixMode(MatrixMode.Projection);
            GL.Viewport(0, 0, _glControl.Width, _glControl.Height);
        }

        private void DrawGrid(float left, float right, float bottom, float top)
        {
            var l = (int) (left/GridSize);
            var r = (int) (right/GridSize + 1);
            var t = (int) (top/GridSize);
            var b = (int) (bottom/GridSize + 1);
            var axisColor = GridColor;
            var lineColor = GridColor;
            lineColor.ScR *= 1.3f;
            lineColor.ScG *= 1.3f;
            lineColor.ScB *= 1.3f;

            for (var i = l; i < r; i++)
                DrawingUtilities.DrawLine(new PointF(i*GridSize, top), new PointF(i*GridSize, bottom), i == 0 ? axisColor : lineColor);

            for (var i = t; i < b; i++)
                DrawingUtilities.DrawLine(new PointF(left, i*GridSize), new PointF(right, i*GridSize), i == 0 ? axisColor : lineColor);
        }

        private void DrawLayers()
        {
            if (World == null)
                return;
            GL.LineWidth(2f);
            foreach (var layer in World.Layers.Where(l => l.Visible).Reverse())
            {
                foreach (var obj in layer.Objects.Reverse())
                {
                    var transform = obj.GetComponent<Transform>();
                    var ed = obj.GetComponent<EditorInfo>();

                    if (transform != null && ed != null && ed.Visible)
                    {
                        var rectcolor = obj == SelectedObject ? _selectionColor : ed.ObjectColor;
                        var texColor = obj == SelectedObject ? _selectionColor : Colors.White;
                        var top = transform.Y;
                        var left = transform.X;
                        var right = transform.X + ed.Width;
                        var bottom = transform.Y + ed.Height;

                        rectcolor.ScA = 0.5f;
                        DrawingUtilities.DrawRect(left, right, top, bottom, rectcolor, PolygonMode.Fill);

                        rectcolor.ScA = 1f;
                        DrawingUtilities.DrawRect(left, right, top, bottom, rectcolor, PolygonMode.Line);

                        var asset = obj.GetComponent<Asset2D>();
                        if (asset != null)
                        {
                            var rotation = 0f;
                            if (asset.Orientation == Orientation.East)
                                rotation = -(float) Math.PI/2;
                            if (asset.Orientation == Orientation.West)
                                rotation = (float) Math.PI/2;
                            if (asset.Orientation == Orientation.South)
                                rotation = (float) Math.PI;

                            var tex = GetTexture(asset.Resource);
                            if (asset.ImageMode == ImageMode.Wrap)
                                DrawingUtilities.DrawTexture(
                                    new PointF(left, top),
                                    tex,
                                    ed.Width, ed.Height,
                                    rotation,
                                    texColor);
                            else
                            {
                                DrawingUtilities.DrawTiledTexture(
                                    new PointF(left, top),
                                    tex,
                                    ed.Width, ed.Height,
                                    ed.Width/(tex.Width*asset.ScaleX),
                                    ed.Height/(tex.Height*asset.ScaleY),
                                    rotation,
                                    texColor);
                            }
                        }

                        if (obj == SelectedObject)
                        {
                            DrawingUtilities.DrawRect(right - ScalerSize, right + ScalerSize, bottom - ScalerSize, bottom + ScalerSize, _selectionColor, PolygonMode.Fill);
                            DrawingUtilities.DrawString(_font, new PointF(left + 1/_camera.Zoom, top + 1/_camera.Zoom), 0.5f/_camera.Zoom, obj.Name, Colors.Black);
                            DrawingUtilities.DrawString(_font, new PointF(left, top), 0.5f/_camera.Zoom, obj.Name, Colors.White);
                        }
                    }
                }
            }
            GL.LineWidth(1f);
        }

        private PointF ToWorld(int x, int y)
        {
            var xf = (x - _glControl.Width/2)/_camera.Zoom + _camera.X;
            var yf = (y - _glControl.Height/2)/_camera.Zoom + _camera.Y;
            return new PointF(xf, yf);
        }

        private TextureInfo GetTexture(string resource)
        {
            if (resource == null || ResourcesPath == null)
                return _textures["Default"];

            if (_textures.ContainsKey(resource))
                return _textures[resource];

            var path = ResourcesPath + resource;
            if (!File.Exists(path))
                return _textures["Default"];
            var id = DrawingUtilities.LoadTexture(path, false);
            _textures.Add(resource, id);
            return id;
        }

        private void GlPaint(object sender, PaintEventArgs e)
        {
            _glControl.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.LoadIdentity();

            var w = _glControl.Width/2f;
            var h = _glControl.Height/2f;
            var left = _camera.X - w/_camera.Zoom;
            var right = _camera.X + w/_camera.Zoom;
            var top = _camera.Y - h/_camera.Zoom;
            var bottom = _camera.Y + h/_camera.Zoom;

            GL.Ortho(left, right, bottom, top, -1, 1);
            DrawGrid(left, right, bottom, top);
            DrawLayers();

            GL.Flush();
            GL.Finish();
            _glControl.SwapBuffers();
        }

        private void WinFormsHost_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_glControl == null)
                return;
            if (!_glControl.Context.IsCurrent)
            {
                _glControl.MakeCurrent();
                SetupViewport();
                _glControl.Context.MakeCurrent(null);
            }
            else
            {
                SetupViewport();
                _glControl.Invalidate();
            }
        }

        private void WinFormsHost_Loaded(object sender, RoutedEventArgs e)
        {
            var prop = DependencyPropertyDescriptor.FromProperty(GlBackgroundProperty, typeof (GlViewer));
            prop.AddValueChanged(this, delegate
            {
                if (_glControl != null)
                {
                    GL.ClearColor(GlBackground.ScR, GlBackground.ScG, GlBackground.ScB, GlBackground.ScA);
                    GlPaint(this, null);
                }
            });

            prop = DependencyPropertyDescriptor.FromProperty(GridSizeProperty, typeof (GlViewer));
            prop.AddValueChanged(this, delegate { if (_glControl != null) GlPaint(this, null); });

            prop = DependencyPropertyDescriptor.FromProperty(GridColorProperty, typeof (GlViewer));
            prop.AddValueChanged(this, delegate { if (_glControl != null) GlPaint(this, null); });

            prop = DependencyPropertyDescriptor.FromProperty(SelectedObjectProperty, typeof (GlViewer));
            prop.AddValueChanged(this, delegate { if (_glControl != null) GlPaint(this, null); });

            if (_glControl == null)
            {
                _glControl = new GLControl();
                _camera = new Camera();
            }
            if (WinFormsHost.Child == null && _glControl != null)
            {
                WinFormsHost.Child = _glControl;
                _glControl.Paint += GlPaint;
                _glControl.MouseUp += GlControlMouseUp;
                _glControl.MouseDown += GlControlMouseDown;
                _glControl.MouseMove += GlControlMouseMove;
                _glControl.MouseWheel += OnMouseWheelHandler;
                _glControl.MouseDoubleClick += GlControlDoubleClick;

                GL.Enable(EnableCap.Texture2D);
                GL.Enable(EnableCap.Normalize);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                GL.ClearColor(GlBackground.ScR, GlBackground.ScG, GlBackground.ScB, GlBackground.ScA);

                _textures.Add("Default", DrawingUtilities.LoadTexture("Resources/no-asset.png", false));
                _font = new SpriteFont(new Font("Consolas", 28));
            }
        }

        private void GlControlMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
                _panning = false;

            if (e.Button == MouseButtons.Left && (_dragging || _scaling))
            {
                _dragging = false;
                _scaling = false;
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                var point = ToWorld(e.X, e.Y);
                Debug.WriteLine(point);
                if (World == null) return;
                ObjectIntance last;
                foreach (var layer in World.Layers.Where(l => l.Visible))
                {
                    var res = layer.Objects.FirstOrDefault(o => o.EditorInfo != null && o.EditorInfo.Visible && o.Contains(point));
                    if (res != null)
                    {
                        last = SelectedObject;
                        SelectedObject = res;
                        if (SelectionChanged != null)
                            SelectionChanged.Invoke(this, new SelectionChangedEventArgs(last, res));
                        return;
                    }
                }

                // Deselect
                last = SelectedObject;
                SelectedObject = null;
                if (SelectionChanged != null)
                    SelectionChanged.Invoke(this, new SelectionChangedEventArgs(last, null));
            }
        }

        private void GlControlMouseMove(object sender, MouseEventArgs e)
        {
            if (_panning)
            {
                _camera.X -= (e.X - _lastPanningPos.X)/_camera.Zoom;
                _camera.Y -= (e.Y - _lastPanningPos.Y)/_camera.Zoom;

                _lastPanningPos = e.Location;
            }
            if (_scaling)
            {
                var pos = ToWorld(e.X, e.Y);
                if (SelectedObject.EditorInfo.SnapToGrid)
                {
                    SelectedObject.EditorInfo.Width = Math.Max(0, MathUtilities.Snap(pos.X - SelectedObject.Transform.X, GridSize));
                    SelectedObject.EditorInfo.Height = Math.Max(0, MathUtilities.Snap(pos.Y - SelectedObject.Transform.Y, GridSize));
                }
                else
                {
                    SelectedObject.EditorInfo.Width = Math.Max(0, pos.X - SelectedObject.Transform.X);
                    SelectedObject.EditorInfo.Height = Math.Max(0, pos.Y - SelectedObject.Transform.Y);
                }
            }
            else if (_dragging)
            {
                var pos = ToWorld(e.X, e.Y);

                if (SelectedObject.EditorInfo.SnapToGrid)
                {
                    SelectedObject.Transform.X = MathUtilities.Snap(pos.X + _dragOffset.X, GridSize);
                    SelectedObject.Transform.Y = MathUtilities.Snap(pos.Y + _dragOffset.Y, GridSize);
                }
                else
                {
                    SelectedObject.Transform.X = pos.X + _dragOffset.X;
                    SelectedObject.Transform.Y = pos.Y + _dragOffset.Y;
                }
            }
            GlPaint(this, null);
        }

        private void GlControlMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                _glControl.Focus();
                _panning = true;
                _lastPanningPos = e.Location;
            }
            if (e.Button == MouseButtons.Left &&
                SelectedObject != null &&
                SelectedObject.Transform != null &&
                SelectedObject.EditorInfo != null)
            {
                var pos = ToWorld(e.X, e.Y);
                var right = SelectedObject.Transform.X + SelectedObject.EditorInfo.Width;
                var bottom = SelectedObject.Transform.Y + SelectedObject.EditorInfo.Height;
                if (pos.X >= right - ScalerSize &&
                    pos.Y >= bottom - ScalerSize &&
                    pos.X <= right + ScalerSize &&
                    pos.Y <= bottom + ScalerSize)
                    _scaling = true;
                else if (SelectedObject.Contains(pos))
                {
                    _dragOffset = new PointF(SelectedObject.Transform.X - pos.X, SelectedObject.Transform.Y - pos.Y);
                    _dragging = true;
                }
            }
        }

        private void OnMouseWheelHandler(object sender, MouseEventArgs e)
        {
            if (!_glControl.Bounds.Contains(e.X, e.Y))
                return;

            var w = _glControl.Width/2f;
            var h = _glControl.Height/2f;
            var lastZoom = _camera.Zoom;
            _camera.Zoom *= (float) Math.Pow(_camera.ZoomRatio, Math.Sign(e.Delta)); // определяет уменьшать или увеличивать масштаб
            _camera.Zoom = Math.Max(_camera.MinZoom, _camera.Zoom);
            _camera.Zoom = Math.Min(_camera.MaxZoom, _camera.Zoom);

            if (Math.Abs(lastZoom - _camera.Zoom) < 0.001f)
                return;

            var mx = _camera.X + (e.X - w)/_camera.Zoom;
            var my = _camera.Y + (h - (2*h - e.Y))/_camera.Zoom;

            if (e.Delta > 0) // приближение
            {
                _camera.X += (mx - _camera.X)*(_camera.ZoomRatio - 1);
                _camera.Y += (my - _camera.Y)*(_camera.ZoomRatio - 1);
            }
            else // отдаление
            {
                _camera.X -= (mx - _camera.X)*(_camera.ZoomRatio - 1)/_camera.ZoomRatio;
                _camera.Y -= (my - _camera.Y)*(_camera.ZoomRatio - 1)/_camera.ZoomRatio;
            }
            GlPaint(this, null);
        }

        private void GlControlDoubleClick(object sender, MouseEventArgs e)
        {
            if (DoubleClick != null)
                DoubleClick.Invoke(this, new ClickEventArgs(ToWorld(e.X, e.Y)));
        }

        #endregion

        public event SelectionChangedEventHandler SelectionChanged;
        public event DoubleClickEventHandler DoubleClick;
    }
}