using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WiresCommon.Components;

namespace WiresCommon
{
    /// <summary>
    /// The game object instance in the location
    /// </summary>
    public class ObjectIntance 
    {
        public string Name { get; set; }
        public ObservableCollection<IComponent> Components { get; set; }

        public int X
        {
            get { return GetComponent<Transform>().X; }
            set
            {
                GetComponent<Transform>().X = value;
                NotifyOfPropertyChange(() => X);
            }
        }

        public int Y
        {
            get { return GetComponent<Transform>().Y; }
            set { GetComponent<Transform>().Y = value; }
        }

        public int Width
        {
            get { return GetComponent<Transform>().ScaleX; }
            set { GetComponent<Transform>().ScaleX = value; }
        }

        public int Height
        {
            get { return GetComponent<Transform>().ScaleY; }
            set { GetComponent<Transform>().ScaleY = value; }
        }

        public T GetComponent<T>()
            where T : IComponent
        {
            return Components.OfType<T>().First();
        }

        public ObjectIntance()
        {
            Name = "NewObject";
            Components = new ObservableCollection<IComponent> { new Transform() };
        }

        public ObjectIntance(ObjectIntance prefab)
        {
            // TODO: Add deepclone from prefab here
        }
    }
}