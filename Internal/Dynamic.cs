namespace JsonNet.Internal
{
    sealed class Dynamic : Base,Value
    {
        public Dynamic() { }
        public Dynamic(Value src) { data = src; }
        public Dynamic(ValueType valueType)
        {
            if (valueType == ValueType.Hash) data = new Hash();
            else if (valueType == ValueType.Array) data = new Array();
            else data = new Primitive();
        }
        public Dynamic(double value) { data = new Primitive(value); }
        public Dynamic(string value) { data = new Primitive(value); }
        public Dynamic(bool value) { data = new Primitive(value); }

        private Value data = null;
        public Value Value 
        { 
            get 
            { 
                if(object.ReferenceEquals(null,data))
                {
                    data = new Hash();
                    data.PropertyChanged += data_PropertyChanged;
                }
                return data; 
            }
            set
            {
                if(!object.ReferenceEquals(data,value) && !object.ReferenceEquals(null,value))
                {
                    data = value;
                    data.PropertyChanged +=data_PropertyChanged;
                    OnPropertyChanged("Value");
                }
            }
        }

        void data_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public ValueType ValueType { get { return Value.ValueType; } }

        public override int GetHashCode(){return ValueHashCode.GetHashCode(this);}

        public bool IsNull
        {
            get { return Value.IsNull; }
            set 
            {
                if(!Value.IsNull && true)
                {
                    data = new Primitive();
                    OnPropertyChanged("IsNull");
                }
            }
        }

        public string String
        {
            get
            {
                if (ValueType != ValueType.String) Value = new Primitive("");
                return Value.String;
            }
            set
            {
                if (ValueType != ValueType.String)
                {
                    Value = new Primitive(value);
                    OnPropertyChanged("String");
                }
                else
                {
                    if (Value.String != value)
                    {
                        Value.String = value;
                        OnPropertyChanged("String");
                    }
                    OnPropertyChanged("String");
                }
            }
        }

        public double Double 
        { 
            get 
            {
                if (ValueType != ValueType.Double) Value = new Primitive(0);
                return Value.Double; 
            } 
            set 
            {
                if (ValueType == ValueType.Double) data.Double = value;
                else
                {
                    Value = new Primitive(value);
                    OnPropertyChanged("Double");
                }
            } 
        }

        public bool Bool
        {
            get
            {
                if (ValueType != ValueType.Boolean) return true;
                return Value.Bool;
            }
            set
            {
                if (ValueType == ValueType.Boolean) data.Bool = value;
                else
                {
                    Value = new Primitive(value);
                    OnPropertyChanged("Bool");
                }
            }
        }

        public int CompareTo(object valueIn){return ValueCompare.Compare(this, valueIn);}
        public override bool Equals(object obj)
        {
            if (CompareTo(obj) == 0) return true;
            return false;
        }
        
        public Value this[int index]
        {
            get 
            {
                if (ValueType != ValueType.Array) Value = new Array();
                return Value[index]; 
            }
            set 
            {
                if (ValueType != ValueType.Array) Value = new Array();
                Value[index] = value; 
            }
        }

        public Value Clone()
        {
            if (object.ReferenceEquals(null, data)) return new Dynamic();
            return new Dynamic(data.Clone());
        }

        public int DeepCount { get { return Value.DeepCount; } }
        public int Count { get { return Value.Count; } }
        public void Clear() { Value.Clear(); }

        public void Remove(string key) { Value.Remove(key); }
        public Value this[string key]
        {
            get 
            {
                if (ValueType != ValueType.Hash) Value = new Hash();
                return Value[key]; 
            }
            set 
            { 
                Value[key] = value; 
            }
        }

        public bool ContainsKey(string key) { return Value.ContainsKey(key); }
        public System.Collections.Generic.IList<string> Keys { get { return Value.Keys; } }

        public override string ToString(){return data.ToString();}

        public void Write(System.IO.Stream stream)
        {
            if(object.ReferenceEquals(null,data))
            {
                data = new Primitive();
            }
            data.Write(stream);
        }

        public void Read(System.IO.Stream stream)
        {
            Internal.Reader reader = new Reader();
            Value = reader.Read(stream) as Value;
            if(object.ReferenceEquals(null,data)) data = Factory.Create("");
        }
        public System.Collections.IEnumerator GetEnumerator()
        {
            return new ValueEnumerator(this);
        }

    }
}
