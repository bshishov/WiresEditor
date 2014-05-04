#region

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Media;
using Caliburn.Micro;
using WEditor.ComponentLibBase;
using WEditor.CustomEditors;
using Color = System.Windows.Media.Color;

#endregion

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


    [Export(typeof (IGameComponent))]
    [ExportMetadata("Type", typeof (Asset2D))]
    public class Asset2D : PropertyChangedBase, IGameComponent
    {
        #region Fields

        private string _resource = "";

        #endregion

        #region Constructors

        public Asset2D()
        {
            ScaleX = 1;
            ScaleY = 1;
            ImageMode = ImageMode.Tile;
            Orientation = Orientation.North;
            ZIndex = 0;
            Color = Color.FromArgb(255, 255, 255, 255);
        }

        #endregion

        #region Properties

        [Editor(typeof (FileEditor), typeof (FileEditor))]
        [Description("Relative path to the resource")]
        public string Resource
        {
            get { return _resource; }
            set
            {
                _resource = GetRelativePath(value);
                NotifyOfPropertyChange(() => Resource);
            }
        }

        public Orientation Orientation { get; set; }

        public ImageMode ImageMode { get; set; }

        public float ScaleX { get; set; }

        public float ScaleY { get; set; }

        public float ZIndex { get; set; }

        public Color Color { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Asset";
        }

        private static string GetRelativePath(string raw)
        {
            var index = raw.LastIndexOf('\\');
            return index > 0 ? raw.Substring(index) : raw;
        }

        #endregion
    }
}