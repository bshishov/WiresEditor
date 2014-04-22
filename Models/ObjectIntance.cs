#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using Models.Components;
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

        public static IList<Type> AvailComponents
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetTypes().Select(t =>
                {
                    var attr = t.GetCustomAttributes(typeof (ExportComponent), true).OfType<ExportComponent>().FirstOrDefault();
                    return attr != null ? attr.ComponenType : null;
                }).Where(i => i != null).ToList();
            }
        }

        #endregion

        #region Constructors

        public ObjectIntance()
        {
            Components = new ObservableCollection<IComponent>();
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
        [Browsable(true)]
        [Description("Collection of object components")]
        [NewItemTypes(typeof (Transform), typeof (EditorInfo), typeof(Health))]
        public ObservableCollection<IComponent> Components { get; set; }

        [PropertyOrder(2)]
        [ExpandableObject]
        public EditorInfo EditorInfo
        {
            get { return GetComponent<EditorInfo>(); }
        }

        [PropertyOrder(3)]
        [ExpandableObject]
        public Transform Transform
        {
            get { return GetComponent<Transform>(); }
        }

        #endregion

        #region Methods

        public bool Contains(PointF point)
        {
            return point.X >= Transform.X && (point.X <= Transform.X + EditorInfo.Width) &&
                   point.Y >= Transform.Y && (point.Y <= Transform.Y + EditorInfo.Height);
        }

        public T GetComponent<T>()
            where T : IComponent
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
                obj.Components.Add((EditorInfo)info.Clone());
            return obj;
        }

        public override string ToString()
        {
            return string.Format("Object: {0}", Name);
        }

        #endregion
    }
}