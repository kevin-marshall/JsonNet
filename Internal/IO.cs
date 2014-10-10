namespace JsonNet.Internal
{
    class IO
    {
        private bool ignoreWhiteSpace = true;
        public System.Collections.Generic.Queue<char> diagnosticBuffer =
            new System.Collections.Generic.Queue<char>();
        public int Read(System.IO.StreamReader reader)
        {
            int ichar = reader.Read();
            if (diagnosticBuffer.Count > 9) diagnosticBuffer.Dequeue();
            if (ichar >= 0)
            {
                char ch = (char)ichar;
                if(ignoreWhiteSpace)
                {
                    if(!System.Char.IsWhiteSpace(ch)) diagnosticBuffer.Enqueue(ch);
                }
                else{diagnosticBuffer.Enqueue(ch);}
            }
            while(diagnosticBuffer.Count > 10) diagnosticBuffer.Dequeue();
            return ichar;
        }

        public void EatWhiteSpace(System.IO.StreamReader stream)
        {
            while (System.Char.IsWhiteSpace((char)(stream.Peek()))) { Read(stream); }// stream.Read(); }
        }

        public void Eat(System.IO.StreamReader stream, char value)
        {
            while (value == (char)(stream.Peek())) { Read(stream);}// stream.Read(); }
        }
        public string Seek(System.IO.StreamReader stream, char value)
        {
            string result = "";
            while (stream.Peek() != (int)value)
            {
                int i = Read(stream);// stream.Read();
                result += (char)i;
            }
            return result;
        }

        public string Seek(System.IO.StreamReader stream, char[] values)
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
                int ch = Read(stream);// stream.Read();
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
