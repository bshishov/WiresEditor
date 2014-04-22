namespace WiresCommon
{
    /// <summary>
    /// Property definition, required for the component definition
    /// </summary>
    public class PropertyDefenition
    {
        public string Name;
        public string Importer;
        public string InitialValue;

        public PropertyDefenition()
        {
            Name = "DefaultProperty";
            Importer = "DefaultImporter";
            InitialValue = "0";
        }
    }
}
