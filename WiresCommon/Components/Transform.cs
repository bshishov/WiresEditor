using System.ComponentModel;

namespace WiresCommon.Components
{
    public class Transform : IComponent
    {
        public string Name { get { return "Transform"; } }

        [Category("Position")]
        public int X { get; set; }

        [Category("Position")]
        public int Y { get; set; }

        [Category("Scale")]
        [DisplayName("Scale X")]
        public int ScaleX { get; set; }

        [Category("Scale")]
        [DisplayName("Scale Y")]
        public int ScaleY { get; set; }

        public Transform()
        {
            ScaleX = 1;
            ScaleY = 1;
        }
    }
}
