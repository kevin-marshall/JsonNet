namespace JsonNet.Internal
{
    class IO
    {
        public static void EatWhiteSpace(System.IO.StreamReader stream)
        {
            while (System.Char.IsWhiteSpace((char)(stream.Peek()))) { stream.Read(); }
        }

        public static void Eat(System.IO.StreamReader stream, char value)
        {
            while (value == (char)(stream.Peek())) { stream.Read(); }
        }
        public static string Seek(System.IO.StreamReader stream, char value)
        {
            string result = "";
            while (stream.Peek() != (int)value)
            {
                int i = stream.Read();
                result += (char)i;
            }
            return result;
        }

        public static string Seek(System.IO.StreamReader stream, char[] values)
        {
            string result = "";
            bool done = false;
            while (!done)
            {
                foreach (char c in values)
                {
                    if (stream.Peek() == c)
                    {
                        done = true;
                        return result;
                    }

                }
                int ch = stream.Read();
                if (ch == -1) done = true;
                else result += (char)(ch);
            }
            return result;
        }

        public static System.IO.MemoryStream MemoryStreamFromString(string value)
        {
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(memory, System.Text.Encoding.UTF8, 1024);
            writer.Write(value);
            writer.Flush();
            memory.Seek(0, System.IO.SeekOrigin.Begin);
            return memory;
        }
    }
}
