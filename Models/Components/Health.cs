using System.ComponentModel;

namespace Models.Components
{
    [ExportComponent(typeof(Health))]
    class Health : IComponent
    {
        public int Value { get; set; }
        public int MaxValue { get; set; }

        public override string ToString()
        {
            return "Health";
        }

        public Health()
        {
            Value = 10;
            MaxValue = 10;
        }
    }
}
