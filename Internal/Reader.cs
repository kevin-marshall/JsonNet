namespace JsonNet.Internal
{
    class Reader
    {
        public static Hash ReadHash(string value) { return ReadHash(Internal.IO.MemoryStreamFromString(value)); }
        public static Primitive ReadPrimitive(System.IO.Stream stream)
        {
            Reader r = new Reader();
            return r.Read(stream) as Primitive;
        }

        public static Value ReadArray(string value) { return ReadArray(Internal.IO.MemoryStreamFromString(value)); }
        public static Value ReadArray(System.IO.Stream stream)
        {
            Reader r = new Reader();
            return r.Read(stream) as Value;
        }

        public static Hash ReadHash(System.IO.Stream stream)
        {
            Reader r = new Reader();
            return r.Read(stream) as Hash;
        }

        private IO io = new IO();
        

        public ValueType PeekValueType(System.IO.StreamReader reader)
        {
            int ipeek = reader.Peek();
            char cpeek = (char)ipeek;
            ValueType vtype = ValueType.Double;
            switch (ipeek)
            {
                case (int)('{'):
                    vtype = ValueType.Hash;
                    break;
                case (int)('['):
                    vtype = ValueType.Array;
                    break;
                case (int)('t'):
                case (int)('f'):
                    vtype = ValueType.Boolean;
                    break;
                case (int)('"'):
                    vtype = ValueType.String;
                    break;
                case (int)('n'):
                    vtype = ValueType.Null;
                    break;
            }
            return vtype;
        }
        public object Read(System.IO.Stream stream)
        {
            Value v = null;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                io.EatWhiteSpace(reader);
                ValueType vt = PeekValueType(reader);
                switch (vt)
                {
                    case ValueType.Hash:
                        Hash hash = new Hash();
                        ReadHash(reader, hash);
                        v = hash;
                        break;
                    case ValueType.Array:
                        //Array array = new Array();
                        Value array = new Array();//ValueType.Array);//Array();//Dynamic();
                        ReadArray(reader, array);
                        v = array;
                        break;
                    default:
                        Primitive primitive = new Primitive();
                        ReadPrimitive(reader, primitive);
                        v = primitive;
                        break;
                }
            }
            return v;
        }

        public void ReadHash(System.IO.StreamReader reader, Value hash)
        {
            io.Seek(reader, '{');
            io.Read(reader);
            //reader.Read();
            bool done = false;
            while (!done)
            {
                io.EatWhiteSpace(reader);
                int ipeek = reader.Peek();
                char cpeek = (char)ipeek;
                if (ipeek == (int)('}'))
                {
                    io.Read(reader);
                    //reader.Read();
                    done = true;
                }
                else
                {
                    // should be on a double quote character (or a single quote character)
                    Primitive primitive = new Primitive();
                    ReadPrimitive(reader, primitive);
                    string key = primitive.String;

                    io.Seek(reader, ':');
                    io.Read(reader);// reader.Read();
                    io.EatWhiteSpace(reader);

                    ValueType vtype = PeekValueType(reader);
                    switch (vtype)
                    {
                        case ValueType.Hash:
                            hash[key] = new Hash();
                            ReadHash(reader, hash[key] as Hash);
                            break;
                        case ValueType.Array:
                            hash[key] = new Array();
                            ReadArray(reader, hash[key] as Array);
                            break;
                        default:
                            hash[key] = new Primitive();
                            ReadPrimitive(reader, hash[key] as Primitive);
                            break;
                    }

                    // , OR }, whichever comes first
                    char[] chars = { ',', '}' };
                    io.Seek(reader, chars);
                    //if (reader.Peek() == (int)('"')) IO.Seek(reader, '"');
                    if (reader.Peek() == (int)('}')) { io.Read(reader); done = true; }// reader.Read();
                    if (reader.EndOfStream) done = true;
                }

            }
            //if (reader.Peek() == (int)('}')) reader.Read();
        }

        public void ReadArray(System.IO.StreamReader reader, Value array)
        {
            io.Seek(reader, '[');
            io.Read(reader);// reader.Read();

            bool done = false;
            while (!done)
            {
                io.EatWhiteSpace(reader);
                int peek = reader.Peek();
                char peekChar = (char)peek;
                switch (peek)
                {
                    case (int)(']'):
                        {
                            io.Read(reader);
                            done = true;
                            break;
                        }
                    case -1:
                        done = true;
                        break;
                    default:
                        ValueType vtype = PeekValueType(reader);
                        switch (vtype)
                        {
                            case ValueType.Hash:
                                //array.Add(new Hash());
                                array[array.Count] = new Hash();
                                ReadHash(reader, array[array.Count - 1] as Hash);
                                break;
                            case ValueType.Array:
                                array[array.Count] = new Array();
                                ReadArray(reader, array[array.Count - 1] as Array);
                                break;
                            default:
                                Primitive primitive = new Primitive();
                                ReadPrimitive(reader, primitive);
                                array[array.Count] = primitive;
                                break;
                        }
                        // , OR ], whichever comes first
                        char[] chars = { ',', ']' };
                        io.Seek(reader, chars);
                        if (reader.Peek() == ']') { io.Read(reader); done = true; }
                        if (reader.Peek() == (int)(',')) io.Eat(reader, ',');
                        break;
                }
            }
            //if (reader.Peek() == (int)(']')) reader.Read();
        }

        public void ReadPrimitive(System.IO.StreamReader reader, Value value)
        {
            io.EatWhiteSpace(reader);
            char[] chars = { 't', 'f', 'n', '"', '\'', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string result;
            io.Seek(reader, chars);
            int peek = reader.Peek();
            switch (peek)
            {
                case (int)('t'):
                    io.Seek(reader, 'e');
                    value.Bool = true;
                    break;
                case (int)('f'):
                    io.Seek(reader, 'e');
                    value.Bool = false;
                    break;
                case (int)('n'):
                    io.Seek(reader, 'l');
                    io.Seek(reader, 'l');
                    value.IsNull = true;
                    break;
                case (int)('"'):
                    int c = io.Read(reader);//reader.Read();
                    result = io.Seek(reader, '"');
                    while (result.Length > 0 && result[result.Length - 1] == '\\')
                    {
                        c = io.Read(reader);// reader.Read();
                        result = result.Substring(0, result.Length - 1) + "\"";
                        result = result + io.Seek(reader, '"');
                    }
                    //primitive.Data = result;
                    value.String = result;
                    break;
                case (int)('\''):
                    int c2 = io.Read(reader);//reader.Read();
                    result = io.Seek(reader, '\'');
                    while (result.Length > 0 && result[result.Length - 1] == '\\')
                    {
                        c2 = io.Read(reader);// reader.Read();
                        result = result.Substring(0, result.Length - 1) + "'";
                        result = result + io.Seek(reader, '\'');
                    }
                    //primitive.Data = result;
                    value.String = result;
                    break;
                default:
                    //char[] endchars = { '.','0','1','2','3','4','5','6','7','8','9' };
                    char[] endchars = { '}', ']', ',', ' ' };
                    string dstring = io.Seek(reader, endchars);
                    //primitive.Data = System.Convert.ToDouble(dstring);
                    value.Double = System.Convert.ToDouble(dstring);
                    break;

            }
        }
    }
}
