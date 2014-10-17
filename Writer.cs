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

        public static string ToJson(object value,JsonFormat format = JsonFormat.Compressed)
        {
            using(Writer writer = new Writer(format))
            {
                return writer.ToString(value);
            }
        }

        public static void WriteToStream(object value,System.IO.Stream stream,JsonFormat format = JsonFormat.Compressed)
        {
            using(Writer writer = new Writer(format))
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
            if (level == 0) return "";
            return System.Environment.NewLine + "".PadRight(level * 2);
        }
        private void Write(object value,System.IO.StreamWriter streamWriter)
        {
            if(object.ReferenceEquals(null,value)) {streamWriter.Write("null");return;}

            System.Collections.IDictionary idictionary = value as System.Collections.IDictionary;
            if(!object.ReferenceEquals(null,idictionary))
            {
                streamWriter.Write(GetIndent() + "{");
                ++level;
                int index = 0;
                foreach(object key in idictionary.Keys)
                {
                    if (index > 0) streamWriter.Write(",");
                    streamWriter.Write(GetIndent() + "\"" + key.ToString() + "\":");
                    Write(idictionary[key], streamWriter);
                    ++index;
                }
                --level;
                if (level == 0 && Format == JsonFormat.Indented) streamWriter.Write(System.Environment.NewLine);
                streamWriter.Write(GetIndent() + "}");
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
                streamWriter.Write(GetIndent() + "[");
                ++level;
                System.Collections.IEnumerator denum = ienumerable.GetEnumerator();
                int index = 0;
                while(denum.MoveNext())
                {
                    if (index > 0) streamWriter.Write(","+GetIndent());
                    else streamWriter.Write(GetIndent());
                    Write(denum.Current, streamWriter);
                    ++index;
                }
                --level;
                if (level == 0 && Format == JsonFormat.Indented) streamWriter.Write(System.Environment.NewLine);
                streamWriter.Write(GetIndent() + "]");
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
