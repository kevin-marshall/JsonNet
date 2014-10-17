namespace JsonNet
{
    public enum JsonFormat { Compressed, Indented };

    public class Writer : System.IDisposable
    {
        public Writer() { }
        public Writer(JsonFormat value) { format = value; }
        private JsonFormat format = JsonFormat.Compressed;

        public JsonFormat Format
        {
            get { return format; }
            set { format = value; }
        }

        private int level = 0;

        public static string ToJson(object value)
        {
            using(Writer writer = new Writer())
            {
                return writer.ToString(value);
            }
        }

        public static void WriteToStream(object value,System.IO.Stream stream)
        {
            using(Writer writer = new Writer())
            {
                writer.Write(value, stream);
            }
        }
        private System.IO.StreamWriter streamWriter = null;

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!object.ReferenceEquals(null, streamWriter)) streamWriter.Dispose();
            }
        }

        public string ToString(object value)
        {
            string result = "";
            using(System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                Write(value, memory);
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                using(System.IO.StreamReader sr = new System.IO.StreamReader(memory))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        public void Write(object value,System.IO.Stream stream)
        {
            System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(stream);
            Write(value,streamWriter);
            streamWriter.Flush();

        }

        private string GetIndent()
        {
            if (Format == JsonFormat.Compressed) return "";
            return "".PadRight(level * 2);
        }
        private void Write(object value,System.IO.StreamWriter streamWriter)
        {
            if(object.ReferenceEquals(null,value)) {streamWriter.Write("null");return;}

            System.Collections.IDictionary idictionary = value as System.Collections.IDictionary;
            if(!object.ReferenceEquals(null,idictionary))
            {
                streamWriter.Write(GetIndent() + "{");
                ++level;
                if (Format == JsonFormat.Indented) streamWriter.Write(System.Environment.NewLine);
                int index = 0;
                foreach(object key in idictionary.Keys)
                {
                    if (index > 0) streamWriter.Write(",");
                    streamWriter.Write("\"" + key.ToString() + "\":");
                    Write(idictionary[key], streamWriter);
                    ++index;
                }
                streamWriter.Write(GetIndent() + "}");
                --level;
                if (Format == JsonFormat.Indented) streamWriter.Write(System.Environment.NewLine);
                return;
            }
            if(value.GetType()==typeof(System.String))
            {
                string svalue = value.ToString().Replace("\"","\\\"");
                streamWriter.Write("\"" + svalue + "\"");
                return;
            }
            System.Collections.IEnumerable ienumerable = value as System.Collections.IEnumerable;
            if(!object.ReferenceEquals(null,ienumerable))
            {
                streamWriter.Write("[");
                System.Collections.IEnumerator denum = ienumerable.GetEnumerator();
                int index = 0;
                while(denum.MoveNext())
                {
                    if (index > 0) streamWriter.Write(",");
                    Write(denum.Current, streamWriter);
                    ++index;
                }
                streamWriter.Write("]");
                return;
            }
            if (value.GetType() == typeof(System.Double)) { streamWriter.Write(value.ToString()); return; }
            if (value.GetType() == typeof(System.Boolean))
            {
                if (((bool)value)) { streamWriter.Write("true"); return; }
                else { streamWriter.Write("false"); return; }
            }
        }
    }
}
