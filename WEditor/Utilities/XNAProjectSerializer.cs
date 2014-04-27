using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Models;

namespace WEditor.Utilities
{
    public static class XnaProjectSerializer
    {
        private static string Indent(int num)
        {
            var s = "";
            for (var i = 0; i < num; i++)
                s += "\t";
            return s;
        }

        private static string XmlValue(string param, string val)
        {
            if (val == "True") val = "true";
            if (val == "False") val = "false";

            float test;
            if(float.TryParse(val, out test))
                val = val.Replace(',', '.');

            return String.Format("<{0}>{1}</{0}>",param,val);
        }

        public static string Serialize(Project project)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n");
            sb.Append("<XnaContent>\n");
            sb.Append("\t<Asset Type=\"SharedContent.Project\">\n");
            sb.Append("\t\t<Worlds>\n");

            foreach (var world in project.Locations)
            {
                sb.Append("\t\t\t<Item>\n");
                sb.Append("\t\t\t\t" + XmlValue("Name", world.Name) + "\n");
                sb.Append("\t\t\t\t<Layers>\n");

                foreach (var layer in world.Layers)
                {
                    sb.Append(Indent(5) + "<Item>\n");
                    sb.Append(Indent(6) + XmlValue("Name", layer.Name) + "\n");
                    sb.Append(Indent(6) + XmlValue("Visible", layer.Visible.ToString()) + "\n");

                    sb.Append(Indent(6) + "<Objects>\n");

                    foreach (var o in layer.Objects)
                    {
                        sb.Append(Indent(7) + "<Item>\n");
                        sb.Append(Indent(8) + XmlValue("Name", o.Name) + "\n");

                        sb.Append(Indent(8) + "<Components>\n");

                        foreach (var component in o.Components)
                        {
                            sb.Append(Indent(9) + string.Format("<Item Type=\"SharedContent.{0}\">", component.GetType().Name) + "\n");

                            foreach (var prop in component.GetType().GetProperties())
                            {
                                var val = prop.GetValue(component, null) ?? "Null";
                                sb.Append(Indent(10) + XmlValue(prop.Name, val.ToString()) + "\n");
                            }

                            sb.Append(Indent(9) + "</Item>\n");
                        }

                        sb.Append(Indent(8) + "</Components>\n");
                        sb.Append(Indent(7) + "</Item>\n");    
                    }

                    sb.Append(Indent(6) + "</Objects>\n");
                    sb.Append(Indent(5) + "</Item>\n");    
                }

                sb.Append("\t\t\t\t</Layers>\n");
                sb.Append("\t\t\t</Item>\n");
            }

            sb.Append("\t\t</Worlds>\n");
            sb.Append("\t</Asset>\n");
            sb.Append("</XnaContent>");
            return sb.ToString();
        }
    }
}
