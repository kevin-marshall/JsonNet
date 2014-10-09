namespace JsonNet.Internal
{
    sealed class Hash : Base,Value
    {
        public static Hash ReadHash(string value)
        {
            return Internal.Reader.ReadHash(Internal.IO.MemoryStreamFromString(value));
        }
        public static Hash ReadHash(System.IO.Stream stream)
        {
            return Internal.Reader.ReadHash(stream);
        }
        public static string HashToString(Hash value)
        {
            return Internal.Writer.ToString(value);
        }

        public void Write(System.IO.Stream stream)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(stream, System.Text.Encoding.UTF8, 1024))
            {
                sw.Write(Internal.Writer.ToString(this));
            }
        }

        public void Read(System.IO.Stream stream)
        {
            Clear();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
            {
                Internal.Reader reader = new Reader();
                reader.ReadHash(sr, this);
            }
        }

        public Hash() { }
        public Hash(string value)
        {
            Internal.Reader reader = new Reader();
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(value)))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(memory, System.Text.Encoding.UTF8, true, 1024))
                {
                    reader.ReadHash(sr, this);
                }
            }
        }
        private System.Collections.Generic.Dictionary<string, Value> values = new System.Collections.Generic.Dictionary<string, Value>();

        public Value this[string key]
        {
            get 
            { 
                if(!values.ContainsKey(key))
                {
                    values[key] = new Internal.Dynamic();
                }
                return values[key];  
            }
            set{
                values[key] = value;
                Base bvalue = value as Base;
                if (!object.ReferenceEquals(null, bvalue)) bvalue.Key = key;
            }
        }

        public ValueType ValueType { get { return ValueType.Hash; } }

        public bool ContainsKey(string key) { return values.ContainsKey(key); }
        public System.Collections.Generic.IList<string> Keys
        {
            get {
                System.Collections.Generic.List<string> keys = new System.Collections.Generic.List<string>(values.Keys);
                return keys; 
            }
        }

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
            Hash clone = new Hash();
            foreach(string key in Keys)
            {
                clone[key] = this[key].Clone();
            }
            return clone;
        }
        public int Count { get { return values.Count; } }
        public int DeepCount
        {
            get
            {
                int total = Count;
                System.Collections.Generic.Dictionary<string, Value>.Enumerator denum = values.GetEnumerator();
                while(denum.MoveNext())
                {
                    Value v = denum.Current.Value;
                    if (v.ValueType == ValueType.Hash)
                    {
                        Hash h = v as Hash;
                        total = total + h.DeepCount;
                    }
                    if (v.ValueType == ValueType.Array)
                    {
                        Internal.Array a = v as Internal.Array;
                        total = total + a.DeepCount;
                    }
                }
                return total;
            }
        }

        public bool IsNull
        {
            get { return false; }
            set {  }
        }
        public string String
        {
            get
            {
                throw new System.InvalidOperationException("Hash is not of ValueType.String.");
            }
            set
            {
                throw new System.InvalidOperationException("Hash is not of ValueType.String.");
            }
        }

        public double Double
        {
            get
            {
                throw new System.InvalidOperationException("Hash is not of ValueType.Number.");
            }
            set
            {
                throw new System.InvalidOperationException("Hash is not of ValueType.Number.");
            }
        }

        public bool Bool
        {
            get
            {
                throw new System.InvalidOperationException("Hash is not of ValueType.Bool.");
            }
            set
            {
                throw new System.InvalidOperationException("Hash is not of ValueType.Bool.");
            }
        }

        public override string ToString()
        {
            return Hash.HashToString(this);
        }

        public Value this[int index]
        {
            get { return this[Keys[index]]; }
            set { this[Keys[index]] = value;}//throw new System.InvalidOperationException("Hash does not support int indexer"); }
        }
        public void Clear() { values.Clear(); }

        public void Remove(string key) { values.Remove(key); }
        public System.Collections.IEnumerator GetEnumerator()
        {
            return new ValueEnumerator(this);
        }
    }
}
