namespace JsonNet.Internal
{
    sealed class Array : Base,Value 
    {
        private System.Collections.ObjectModel.ObservableCollection<Value> values = new System.Collections.ObjectModel.ObservableCollection<Value>();
        public Array() { }

        /*
        public Array(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                Add(new Hash());
            }
        }
        public Array(string value)
        {
            Internal.Reader reader = new Reader();
            using(System.IO.MemoryStream memory = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(value)))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(memory, System.Text.Encoding.UTF8, true, 1024))
                {
                    reader.ReadArray(sr, this);
                }
            }
            
        }*/

        public void Clear() { values.Clear(); }
        public void Remove(string key) { }
        public int Count { get { return values.Count; } }
        public Value this[int index]
        {
            get
            {
                while (values.Count < index + 1) values.Add(new Dynamic());
                return values[index];
            }
            set
            {
                while (values.Count < index + 1) values.Add(new Dynamic());
                values[index] = value;
            }
        }

        public void Add(Value value)
        {
            values.Add(value);
        }
        public static Value ReadArray(string value)
        {
            return Internal.Reader.ReadArray(Internal.IO.MemoryStreamFromString(value));
        }

        public static Value ReadArray(System.IO.Stream stream)
        {
            return Internal.Reader.ReadArray(stream);
        }

        public void Read(System.IO.Stream stream)
        {
            Clear();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
            {
                Internal.Reader reader = new Reader();
                reader.ReadArray(sr, this);
            }
        }

        public void Write(System.IO.Stream stream)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(stream, System.Text.Encoding.UTF8, 1024);
            sw.Write(ToString());
            sw.Flush();
        }
        public static string ArrayToString(Array value)
        {
            return Internal.Writer.ToString(value);
        }

        public void Add(double value) { values.Add(new Internal.Primitive(value)); }
        public void Add(string value) { values.Add(new Internal.Primitive(value)); }

        public override int GetHashCode()
        {
            return ValueHashCode.GetHashCode(this);
        }

        public override bool Equals(object obj)
        {
            if (CompareTo(obj) == 0) return true;
            return false;
        }


        public int CompareTo(object value)
        {
            return ValueCompare.Compare(this, value);
        }

        public Value Clone()
        {
            Array clone = new Array();
            for(int i = 0; i < this.Count; ++i)
            {
                clone.Add(this[i].Clone());
            }
            return clone;
        }

        public ValueType ValueType { get { return ValueType.Array; } }

        public int DeepCount
        {
            get
            {
                int total = Count;
                for(int i = 0; i < Count; ++i)
                {
                    total = total + values[i].DeepCount;
                    /*
                    if (values[i].ValueType == ValueType.Hash)
                    {
                        Hash h = values[i] as Hash;
                        total = total + h.DeepCount;
                    }
                    if (values[i].ValueType == ValueType.Array)
                    {
                        Array a = values[i] as Array;
                        total = total + a.DeepCount;
                    }*/
                }
                return total;
            }
        }

        public Value this[string key]
        {
            get { throw new System.InvalidOperationException("string indexing not supported for Array."); }
            set
            {
                throw new System.InvalidOperationException("string indexing not supported for Array.");
            }
        }

        public bool ContainsKey(string key) { return false; }
        public System.Collections.Generic.IList<string> Keys { get { return new System.Collections.Generic.List<string>(); } }

        public bool IsNull
        {
            get { return false; }
            set { }
        }
        public string String
        {
            get
            {
                throw new System.InvalidOperationException("Array is not of ValueType.String.");
            }
            set
            {
                throw new System.InvalidOperationException("Array is not of ValueType.String.");
            }
        }

        public double Double
        {
            get
            {
                throw new System.InvalidOperationException("Array is not of ValueType.Number.");
            }
            set
            {
                throw new System.InvalidOperationException("Array is not of ValueType.Number.");
            }
        }

        public bool Bool
        {
            get
            {
                throw new System.InvalidOperationException("Array is not of ValueType.Bool.");
            }
            set
            {
                throw new System.InvalidOperationException("Array is not of ValueType.Bool.");
            }
        }

        public override string ToString()
        {
            return Array.ArrayToString(this);
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return new ValueEnumerator(this);
        }
    }
}
