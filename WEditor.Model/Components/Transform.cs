#region

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media.Media3D;
using Newtonsoft.Json;
using OpenTK;
using WEditor.ComponentLibBase;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

#endregion

namespace Models.Components
{
    [Export(typeof (IGameComponent))]
    [ExportMetadata("Type", typeof (Transform))]
    public class Transform : IGameComponent
    {
        #region Constructors

        private Point3D _pos;
        private Point3D _rot;
        private Point3D _scale;
        public Transform()
        {
            Scale = new Point3D(1, 1, 1);
        }

        #endregion

        #region Properties

        public Point3D Position { get { return _pos; } set { _pos = value; } }

        public Point3D Rotation { get { return _rot; } set { _rot = value; } }

        public Point3D Scale { get { return _scale; } set { _scale = value; } }

        public void SetX(float val)
        {
            _pos.X = val;
        }
        public void SetY(float val)
        {
            _pos.Y = val;
        }
        public void SetZ(float val)
        {
            _pos.Z = val;
        }

        
        #endregion

        #region Methods

        public override string ToString()
        {
            return "Transform";
        }

        #endregion
    }
}