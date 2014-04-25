#region

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Media;
using WEditor.ComponentLibBase;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

#endregion

namespace Models.Components
{
    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(EditorInfo))]
    public class EditorInfo : IGameComponent, ICloneable
    {
        #region Constructors

        public EditorInfo()
        {
            ObjectColor = Colors.LightBlue;
            Width = 50;
            Height = 50;
            Visible = true;
            SnapToGrid = true;
        }

        #endregion

        #region Properties

        [PropertyOrder(0)]
        [Category("Main")]
        [DisplayName(@"Object Color")]
        [Description("Color of object in editor")]
        public Color ObjectColor { get; set; }

        [PropertyOrder(1)]
        [Category("Main")]
        [Description("Width of object in editor")]
        public float Width { get; set; }

        [PropertyOrder(2)]
        [Category("Main")]
        [Description("Height of object in editor")]
        public float Height { get; set; }

        [PropertyOrder(3)]
        [Category("Main")]
        [Description("Visible in editor or not")]
        public bool Visible { get; set; }

        [PropertyOrder(4)]
        [Category("Main")]
        [Description("Snaps to editors grid")]
        public bool SnapToGrid { get; set; }

        #endregion

        #region IGameComponent Members

        public override string ToString()
        {
            return "Editor Info";
        }

        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}