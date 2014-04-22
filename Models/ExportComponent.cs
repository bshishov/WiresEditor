using System;

namespace Models
{
    [AttributeUsage(AttributeTargets.All)]
    class ExportComponent : Attribute
    {
        public readonly Type ComponenType;

        public ExportComponent(Type type)
        {
            ComponenType = type;
        }
    }
}
