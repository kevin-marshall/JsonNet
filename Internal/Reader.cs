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


        public ValueType PeekValueType(System.IO.StreamReader reader)
        {
            ValueType vtype = ValueType.Double;
            switch (reader.Peek())
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
                Internal.IO.EatWhiteSpace(reader);
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
                        Value array = new Dynamic(ValueType.Array);//Array();//Dynamic();
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
            Internal.IO.Seek(reader, '{');
            reader.Read();
            bool done = false;
            while (!done)
            {
                Internal.IO.EatWhiteSpace(reader);
                int ipeek = reader.Peek();
                char cpeek = (char)ipeek;
                if (ipeek == (int)('}'))
                {
                    reader.Read();
                    done = true;
                }
                else
                {
                    // should be on a double quote character
                    Primitive primitive = new Primitive();
                    ReadPrimitive(reader, primitive);
                    string key = primitive.String;

                    Internal.IO.Seek(reader, ':');
                    reader.Read();
                    Internal.IO.EatWhiteSpace(reader);

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
                    Internal.IO.Seek(reader, chars);
                    //if (reader.Peek() == (int)('"')) IO.Seek(reader, '"');
                    if (reader.EndOfStream) done = true;
                }

            }
            if (reader.Peek() == (int)('}')) reader.Read();
        }

        public void ReadArray(System.IO.StreamReader reader, Value array)
        {
            Internal.IO.Seek(reader, '[');
            reader.Read();

            bool done = false;
            while (!done)
            {
                char peekChar = (char)reader.Peek();
                switch (reader.Peek())
                {
                    case (int)(']'):
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
                                //array.Add(new Array());
                                array[array.Count] = new Array();
                                ReadArray(reader, array[array.Count - 1] as Array);
                                break;
                            default:
                                Primitive primitive = new Primitive();
                                ReadPrimitive(reader, primitive);
                                //array.Add(primitive);
                                array[array.Count] = primitive;
                                break;
                        }
                        // , OR ], whichever comes first
                        char[] chars = { ',', ']' };
                        Internal.IO.Seek(reader, chars);
                        if (reader.Peek() == (int)(',')) Internal.IO.Eat(reader, ',');
                        break;
                }
            }
            if (reader.Peek() == (int)(']')) reader.Read();
        }

        public void ReadPrimitive(System.IO.StreamReader reader, Value value)
        {
            Internal.IO.EatWhiteSpace(reader);
            char[] chars = { 't', 'f', 'n', '"', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            Internal.IO.Seek(reader, chars);
            switch (reader.Peek())
            {
                case (int)('t'):
                    Internal.IO.Seek(reader, 'e');
                    value.Bool = true;
                    //primitive.Data = true;
                    break;
                case (int)('f'):
                    Internal.IO.Seek(reader, 'e');
                    //primitive.Data = false;
                    value.Bool = false;
                    break;
                case (int)('n'):
                    Internal.IO.Seek(reader, 'l');
                    Internal.IO.Seek(reader, 'l');
                    //primitive.Data = null;
                    value.IsNull = true;
                    break;
                case (int)('"'):
                    int c = reader.Read();
                    string result = Internal.IO.Seek(reader, '"');
                    while (result.Length > 0 && result[result.Length - 1] == '\\')
                    {
                        c = reader.Read();
                        result = result.Substring(0, result.Length - 1) + "\"";
                        result = result + Internal.IO.Seek(reader, '"');
                    }
                    //primitive.Data = result;
                    value.String = result;
                    break;
                default:
                    //char[] endchars = { '.','0','1','2','3','4','5','6','7','8','9' };
                    char[] endchars = { '}', ']', ',', ' ' };
                    string dstring = Internal.IO.Seek(reader, endchars);
                    //primitive.Data = System.Convert.ToDouble(dstring);
                    value.Double = System.Convert.ToDouble(dstring);
                    break;

            }
        }
    }
}
