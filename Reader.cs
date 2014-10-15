namespace JsonNet
{
    public class Reader : System.IDisposable
    {
        private System.IO.StreamReader streamReader = null;
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!object.ReferenceEquals(null, streamReader)) streamReader.Dispose();
            }
        }

        public object Read(string value)
        {
            object result = null;
            using(System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                using(System.IO.StreamWriter sw = new System.IO.StreamWriter(memory))
                {
                    sw.Write(value);
                    sw.Flush();
                    memory.Seek(0, System.IO.SeekOrigin.Begin);
                    result = Read(memory);
                }
            } 
            return result;
        }
        public object Read(System.IO.Stream stream)
        {
            streamReader = new System.IO.StreamReader(stream);
            return ReadNext();
        }

        private object ReadNext()
        {
            EatWhiteSpace();
            char ch = (char)Peek();
            if (ch == '{') { return ReadHash(); }
            if (ch == '[') return ReadArray();
            if (ch == '\'' || ch == '"') return ReadString();
            if (ch == 't' || ch == 'f') return ReadBool();
            if (System.Char.IsDigit(ch) || ch == '-') return ReadDouble();
            if (ch == 'n') { Read(); Read(); Read(); Read(); }// consume n,u,l,l
            return null;
        }

        private Hash ReadHash()
        {
            Hash hash = new Hash();
            EatWhiteSpace();
            Read(); // consume the '{'
            EatWhiteSpace();
            bool done = false;
            if((char)(Peek())=='}')
            {
                done = true;
                Read(); // consume the '}'
            }
            while(!done)
            {
                EatWhiteSpace();
                string key = ReadString();
                EatWhiteSpace();
                char ch = (char)Peek();
                if(ch != ':') throw new System.Exception("found character " + ch + " when a semicolon was expected");
                Read(); //consume ':'
                hash[key] = ReadNext();

                EatWhiteSpace();
                ch = (char)Peek();
                if (ch == ',') Read(); // consume ','

                EatWhiteSpace();
                ch = (char)Peek();
                if(ch == '}')
                {
                    Read();
                    done = true;
                }
            }
            return hash;
        }

        private Array ReadArray()
        {
            char ch = ' ';
            Array array = new Array();
            EatWhiteSpace();
            Read(); // consume the '['
            EatWhiteSpace();
            bool done = false;
            ch = (char)Peek();
            if (ch == ']')
            {
                done = true;
                Read(); // consume the ']'
            }
            while(!done)
            {
                EatWhiteSpace();
                array.Add(ReadNext());
                EatWhiteSpace();

                ch = (char)Peek();
                if (ch == ',') Read(); // consume ','

                EatWhiteSpace();
                ch = (char)Peek();
                if (ch == ']')
                {
                    Read(); // consume ']'
                    done = true;
                }
            }
            return array;
        }

        private string ReadString()
        {
            EatWhiteSpace();
            char ch = (char)Peek();
            if (ch != '\'' && ch != '"') throw new System.Exception("found character " + ch + " when a single or double quote was expected");
            Read(); // consume single or double quote
            string result = Seek(ch);
            Read(); // consume escaped character
            while(result.Length > 0 && result[result.Length-1] =='\\')
            {
                // have escaped character
                result = result.Substring(0, result.Length - 1) + ch;
                result = result + Seek(ch);
                Read(); // consume escaped character
            }
            return result;
        }

        private bool ReadBool()
        {
            EatWhiteSpace();
            char ch = (char)Peek();
            if(ch == 't')
            {
                Read(); Read(); Read(); Read(); // read chars t,r,u,e
                return true;
            }
            Read(); Read(); Read(); Read(); Read(); // read char f,a,l,s,e
            return false;
        }

        private double ReadDouble()
        {
            EatWhiteSpace();
            char[] endchars = { '}', ']', ',', ' ' };
            return System.Convert.ToDouble(Seek(endchars));
        }

        private int Peek() { return streamReader.Peek(); }
        private int Read() 
        {
            int ichar = streamReader.Read();
            if(ichar == -1)
            {
                throw new System.Exception("end of file reached before json read was finished.");
            }
            return ichar;
        }
        private int EatWhiteSpace()
        {
            int ichar = -1;
            while (System.Char.IsWhiteSpace((char)(Peek())))
            {
                int tchar = Read();
            }
            return ichar;
        }
        private string Seek(char value)
        {
            string result = "";
            while(Peek() != (int)value)
            {
                int ichar = Read();
                result += (char)ichar;
            }
            return result;
        }

        private string Seek(char[] values)
        {
            string result = "";
            bool done = false;
            foreach (char ch in values) { if ((char)Peek() == ch) { done = true; } }
            while (!done)
            {
                int ichar = Read();
                result += (char)ichar;
                foreach (char ch in values) { if ((char)Peek() == ch) { done = true; } }
            }
            return result;
        }
    }
}
