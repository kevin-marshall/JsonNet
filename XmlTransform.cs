namespace JsonNet
{
    public class XmlTransform
    {
        public static object FromXml(System.Xml.XmlDocument xdoc)
        {
            if (object.ReferenceEquals(null, xdoc)) return null;
            return new Hash();
        }

        public static System.Xml.XmlDocument ToXml(Hash hash)
        {
            if (object.ReferenceEquals(null, hash)) return null;
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.AppendChild(ToXml(xdoc,hash));
            return xdoc;
        }

        public static System.Xml.XmlElement ToXml(System.Xml.XmlDocument xdoc, object value)
        {
            if (object.ReferenceEquals(null, value))
            {
                return xdoc.CreateElement("Null");
            }
            if (value.GetType() == typeof(Hash)) return ToXml(xdoc, value as Hash);
            if (value.GetType() == typeof(Array)) return ToXml(xdoc, value as Array);
            if (value.GetType() == typeof(System.String)) return ToXml(xdoc, value.ToString());
            if (value.GetType() == typeof(System.Double)) return ToXml(xdoc, (double)value);
            if (value.GetType() == typeof(System.Boolean)) return ToXml(xdoc, (bool)value);
            return null;
        }

        private static System.Xml.XmlElement ToXml(System.Xml.XmlDocument xdoc,Hash hash)
        {
            System.Xml.XmlElement element = xdoc.CreateElement("Hash");
            foreach(string key in hash.Keys)
            {
                System.Xml.XmlElement keyElement = xdoc.CreateElement("Key");
                element.AppendChild(keyElement);

                System.Xml.XmlAttribute keyNameAttribute = xdoc.CreateAttribute("name");
                keyNameAttribute.Value = key;
                keyElement.Attributes.Append(keyNameAttribute);

                keyElement.AppendChild(ToXml(xdoc,(object)hash[key]));
            }
            return element;
        }

        private static System.Xml.XmlElement ToXml(System.Xml.XmlDocument xdoc,Array array)
        {
            System.Xml.XmlElement element = xdoc.CreateElement("Array");
            for (int i = 0; i < array.Count; ++i)
            {
                System.Xml.XmlElement indexElement = xdoc.CreateElement("Index");
                element.AppendChild(indexElement);

                System.Xml.XmlAttribute indexAttribute = xdoc.CreateAttribute("index");
                indexAttribute.Value = i.ToString();
                indexElement.Attributes.Append(indexAttribute);

                indexElement.AppendChild(ToXml(xdoc,array[i]));
            }
            return element;
        }
        private static System.Xml.XmlElement ToXml(System.Xml.XmlDocument xdoc, string value)
        {
            System.Xml.XmlElement element = xdoc.CreateElement("String");
            element.InnerText = value;
            return element;
        }
        private static System.Xml.XmlElement ToXml(System.Xml.XmlDocument xdoc, double value)
        {
            System.Xml.XmlElement element = xdoc.CreateElement("Double");
            element.InnerText = value.ToString();
            return element;
        }
        private static System.Xml.XmlElement ToXml(System.Xml.XmlDocument xdoc, bool value)
        {
            System.Xml.XmlElement element = xdoc.CreateElement("Boolean");
            element.InnerText = value.ToString();
            return element;
        }

        public static string ToString(System.Xml.XmlDocument xdoc)
        {
            string xml = "";
            using(System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                xdoc.Save(memory);
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                using(System.IO.StreamReader sr = new System.IO.StreamReader(memory))
                {
                    xml = sr.ReadToEnd();
                }
            }
            return xml;
        }
    }
}
