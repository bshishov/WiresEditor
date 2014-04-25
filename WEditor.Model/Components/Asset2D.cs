using System.ComponentModel.Composition;
using WEditor.ComponentLibBase;

namespace Models.Components
{
    public enum Orientation
    {
        North,
        East,
        South,
        West
    }

    public enum ImageMode
    {
        Wrap,
        Tile,
    }


    [Export(typeof(IGameComponent))]
    [ExportMetadata("Type", typeof(Asset2D))]
    public class Asset2D : IGameComponent
    {
        public string Resource { get; set; }

        public Orientation Orientation { get; set; }

        public ImageMode ImageMode { get; set; }

        public float ScaleX { get; set; }

        public float ScaleY { get; set; }

        public Asset2D()
        {
            ScaleX = 50 / 8f;
            ScaleY = 50 / 8f;
            ImageMode = ImageMode.Tile;
            Orientation = Orientation.North;
        }

        public override string ToString()
        {
            return "Asset";
        }
    }
}
