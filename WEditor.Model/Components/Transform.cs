#region

using System.ComponentModel;
using System.ComponentModel.Composition;
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

        #region Methods

        public override string ToString()
        {
            return "Transform";
        }

        #endregion
    }
}