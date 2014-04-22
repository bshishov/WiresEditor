#region

using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

#endregion

namespace Models.Components
{
    [ExportComponent(typeof(Transform))]
    public class Transform : IComponent
    {
        #region Constructors

        public Transform()
        {
            ScaleX = 1;
            ScaleY = 1;
        }

        #endregion

        #region Properties

        [PropertyOrder(0)]
        [Category("Position")]
        public float X { get; set; }

        [PropertyOrder(1)]
        [Category("Position")]
        public float Y { get; set; }

        [PropertyOrder(2)]
        [Category("Scale")]
        [DisplayName("Scale X")]
        public float ScaleX { get; set; }

        [PropertyOrder(3)]
        [Category("Scale")]
        [DisplayName("Scale Y")]
        public float ScaleY { get; set; }

        #endregion

        public override string ToString()
        {
            return "Transform";
        }
    }
}