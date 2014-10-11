namespace JsonNet.Internal
{
    sealed class Primitive : Base,Value, System.ComponentModel.INotifyPropertyChanged
    {
        public static explicit operator Primitive(bool value) { return new Primitive(value); }
        public static explicit operator Primitive(double value) { return new Primitive(value); }
        public static explicit operator Primitive(string value) { return new Primitive(value); }

        public static Primitive ReadPrimitive(string value)
        {
            return Internal.Reader.ReadPrimitive(Internal.IO.MemoryStreamFromString(value));
        }

        public static Primitive ReadPrimitive(System.IO.Stream stream)
        {
            return Internal.Reader.ReadPrimitive(stream);
        }

        public static string PrimitiveToString(Primitive value)
        {
            return Internal.Writer.ToString(value);
        }

        private object data = null;
        public Primitive() { }
        public Primitive(double value) { data = value; }
        public Primitive(string value) { data = value; }
        public Primitive(bool value) { data = value; }

        public object Data
        {
            get { return data; }
            set
            {
                SetProperty<object>(ref data, value, "Data");
            }
        }

        public ValueType ValueType
        {
            get
            {
                if (object.ReferenceEquals(null, data)) return ValueType.Null;
                if (data.GetType() == typeof(bool)) return ValueType.Boolean;
                if (data.GetType() == typeof(string)) return ValueType.String;
                return ValueType.Double;
            }
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

        public override int GetHashCode()
        {
            return ValueHashCode.GetHashCode(this);
        }

        public Value Clone()
        {
            if (ValueType == ValueType.Boolean) return new Primitive(this.Bool);
            if (ValueType == ValueType.Double) return new Primitive(this.Double);
            if (ValueType == ValueType.String) return new Primitive(this.String);
            return new Primitive();
        }

        public bool IsNull
        {
            get { return ValueType == JsonNet.ValueType.Null; }
            set 
            {
                if (ValueType != JsonNet.ValueType.Null)
                {
                    data = null;
                    OnPropertyChanged("IsNull");
                }
            }
        }
        public double Double
        {
            get
            {
                if (!object.ReferenceEquals(null, data) && data.GetType() == typeof(double)) return (double)data;
                throw new System.InvalidOperationException("Primitive is not of ValueType.Number.");
            }
            set
            {
                if (ValueType != ValueType.Double || Double != value)
                {
                    data = value;
                    OnPropertyChanged("Double");
                }
            }
        }

        public string String
        {
            get
            {
                if (!object.ReferenceEquals(null, data) && data.GetType() == typeof(string)) return (string)data;
                throw new System.InvalidOperationException("Primitive is not of ValueType.String.");
            }
            set
            {
                SetProperty<object>(ref data, value, "String");
            }
        }

        public bool Bool
        {
            get
            {
                if (!object.ReferenceEquals(null, data) && data.GetType() == typeof(bool)) return (bool)data;
                return true;
            }
            set
            {
                bool changed = true;
                if (!object.ReferenceEquals(null,data) &&(bool)data != value) changed = true;
                data = value;
                if (changed )
                {
                    OnPropertyChanged("Bool");
                }
            }
        }
        public Value this[string key]
        {
            get { throw new System.InvalidOperationException("string indexing not supported for Primitive."); }
            set
            {
                throw new System.InvalidOperationException("string indexing not supported for Primitive.");
            }
        }
        public bool ContainsKey(string key) { return false; }
        public System.Collections.Generic.IList<string> Keys { get { return new System.Collections.Generic.List<string>(); } }

        public int DeepCount { get { return 0; } }
        public int Count { get { return 0; } }
        public void Clear() { }
        public void Remove(string key) { }
        public void Add(string value) { throw new System.InvalidOperationException("Add not supported for Primitive."); }
        public void Add(double value) { throw new System.InvalidOperationException("Add not supported for Primitive."); }
        public Value this[int index]
        {
            get { throw new System.InvalidOperationException("int indexing not supported for Primitive."); }
            set
            {
                throw new System.InvalidOperationException("int indexing not supported for Primitive.");
            }
        }

        public override string ToString()
        {
            return Primitive.PrimitiveToString(this);
        }

        public void Write(System.IO.Stream stream)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(stream, System.Text.Encoding.UTF8, 1024);
            sw.Write(Primitive.PrimitiveToString(this));
            sw.Flush();
        }

        public void Read(System.IO.Stream stream)
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
            {
                Internal.Reader reader = new Reader();
                reader.ReadPrimitive(sr, this);
            }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return new ValueEnumerator(this);
        }
    }
}
