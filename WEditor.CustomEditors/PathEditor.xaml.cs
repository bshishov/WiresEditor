﻿#region

using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Binding = System.Windows.Data.Binding;
using UserControl = System.Windows.Controls.UserControl;

#endregion

namespace WEditor.CustomEditors
{
    /// <summary>
    ///     Interaction logic for PathEditor.xaml
    /// </summary>
    public partial class PathEditor : UserControl, ITypeEditor
    {
        #region Constants

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof (string),
            typeof (PathEditor),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion

        #region Constructors

        public PathEditor()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public string Value
        {
            get { return (string) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #region ITypeEditor Members

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            var binding = new Binding("Value")
            {
                Source = propertyItem,
                Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
            };
            BindingOperations.SetBinding(this, ValueProperty, binding);
            return this;
        }

        #endregion

        #region Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
                Value = dialog.SelectedPath;
        }

        #endregion
    }
}