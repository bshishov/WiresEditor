#region

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Caliburn.Micro;
using Models.Components;
using Newtonsoft.Json;
using WEditor.ComponentLibBase;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

#endregion

namespace Models
{
    /// <summary>
    ///     The game object instance in the World
    /// </summary>
    public class ObjectIntance : PropertyChangedBase
    {
        #region Fields

        private string _name = "NewObject";

        #endregion

        #region Constructors

        public ObjectIntance()
        {
            Components = new ObservableCollection<IGameComponent>();
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
        [Description("Collection of object components")]
        [NewItemTypes(typeof (Transform), typeof (EditorInfo))]
        public ObservableCollection<IGameComponent> Components { get; set; }

        [PropertyOrder(2)]
        [ExpandableObject]
        [JsonIgnore]
        public EditorInfo EditorInfo
        {
            get { return GetComponent<EditorInfo>(); }
        }

        [PropertyOrder(3)]
        [ExpandableObject]
        [JsonIgnore]
        public Transform Transform
        {
            get { return GetComponent<Transform>(); }
        }

        #endregion

        #region Methods

        public bool Contains(PointF point)
        {
            return point.X >= Transform.Position.X && (point.X <= Transform.Position.X + EditorInfo.Width) &&
                   point.Y >= Transform.Position.Y && (point.Y <= Transform.Position.Y + EditorInfo.Height);
        }

        public T GetComponent<T>()
            where T : IGameComponent
        {
            return Components.OfType<T>().FirstOrDefault();
        }

        public static ObjectIntance CreateEditorDefault(EditorInfo info = null)
        {
            var obj = new ObjectIntance();
            obj.Components.Add(new Transform());
            if (info == null)
                obj.Components.Add(new EditorInfo());
            else
                obj.Components.Add((EditorInfo) info.Clone());
            return obj;
        }

        public override string ToString()
        {
            return string.Format("Object: {0}", Name);
        }

        #endregion
    }
}