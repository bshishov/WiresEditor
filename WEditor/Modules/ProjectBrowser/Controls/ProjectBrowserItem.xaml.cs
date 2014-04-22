#region

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace WEditor.Modules.ProjectBrowser.Controls
{
    /// <summary>
    ///     Interaction logic for ProjectBrowserItem.xaml
    /// </summary>
    public partial class ProjectBrowserItem : UserControl
    {
        #region Constants

        public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register("IconColor", typeof (Brush), typeof (ProjectBrowserItem));

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof (string), typeof (ProjectBrowserItem));

        public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register("IconSource", typeof (ImageSource), typeof (ProjectBrowserItem));

        #endregion

        #region Constructors

        public ProjectBrowserItem()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public Brush IconColor
        {
            get { return (Brush) GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public string Caption
        {
            get { return (string) GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public ImageSource IconSource
        {
            get { return (ImageSource) GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        #endregion
    }
}