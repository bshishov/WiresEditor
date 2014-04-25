#region

using System.Collections.ObjectModel;
using Caliburn.Micro;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

#endregion

namespace Models
{
    public class World : PropertyChangedBase
    {
        #region Fields

        private ObservableCollection<Layer> _layers;
        private string _name;

        #endregion

        #region Constructors

        public World()
        {
            Name = "New World";
            Layers = new ObservableCollection<Layer>();
        }

        #endregion

        #region Properties

        [PropertyOrder(0)]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        [PropertyOrder(1)]
        public ObservableCollection<Layer> Layers
        {
            get { return _layers; }
            set
            {
                _layers = value;
                NotifyOfPropertyChange(() => Layers);
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format("World: {0}", _name);
        }

        #endregion
    }
}