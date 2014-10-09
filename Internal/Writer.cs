namespace JsonNet.Internal
{
    class Writer
    {
        public static string ToString(Value value)
        {
            string result = "";
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(memory, System.Text.Encoding.UTF8, 1024))
                {
                    Writer w = new Writer();
                    w.Write(writer, value);
                }
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                using (System.IO.StreamReader reader = new System.IO.StreamReader(memory, System.Text.Encoding.UTF8, true, 1024))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        public void Write(System.IO.StreamWriter writer, Value value)
        {
            switch (value.ValueType)
            {
                case ValueType.Hash:
                    WriteHash(writer, value);
                    break;
                case ValueType.Array:
                    WriteArray(writer, value);
                    break;
                case ValueType.Null:
                    writer.Write("null");
                    break;
                case ValueType.Boolean:
                    if (value.Bool) writer.Write("true");
                    else writer.Write("false");
                    break;
                case ValueType.String:
                    string svalue = value.String.Replace("\"","\\\"");
                    writer.Write("\"" + svalue + "\"");
                    break;
                default:
                    writer.Write(value.Double.ToString());
                    break;
            }
        }
        public void WriteArray(System.IO.StreamWriter writer, Value value)
        {
            writer.Write("[");
            for (int i = 0; i < value.Count; ++i)
            {
                if (i > 0) writer.Write(",");
                Write(writer, value[i]);
            }
            writer.Write("]");
        }

        public void WriteHash(System.IO.StreamWriter writer, Value hash)
        {
            writer.Write("{");
            int index = 0;
            foreach (string key in hash.Keys)
            {
                if (index > 0) writer.Write(",");
                writer.Write("\"" + key + "\":");
                Write(writer, hash[key]);
                ++index;
            }
            writer.Write("}");
        }
    }
}
