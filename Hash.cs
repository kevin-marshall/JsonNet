namespace JsonNet
{
    public sealed class Hash : System.Collections.Generic.Dictionary<string,dynamic>,
                               System.IComparable
    {
        public Hash() { }
        public Hash(string json) 
        {
            Hash hash = Reader.Parse(json) as Hash;
            foreach(string key in hash.Keys){Add(key, hash[key]);}
        }

        public object Clone() { return new Hash(Writer.ToJson(this)); }
        public int CompareTo(object value) { return CompareHelper.Compare(this, value); }

        public override bool Equals(object obj) { return CompareTo(obj) == 0 ? true : false; }

        public override int GetHashCode() { return HashCodeHelper.GetHashCode(this); }

        private object GetValidItem(object item)
        {
            if (object.ReferenceEquals(null, item)) return item;
            if (item.GetType() == typeof(System.Int32)) { double value = (int)item; return value; }
            return item;
        }

        public new dynamic this[string key]
        {
            get { return base[key]; }
            set
            {
                object vitem = GetValidItem(value);
                if (IsValidItem(vitem)) base[key] = vitem;
                else throw new System.ArgumentOutOfRangeException(
                        "only null,string,double,bool,Array,Hash instances are value for Hash.this[string key]");
            }
        }

        public new void Add(string key,object value)
        {
            if (IsValidItem(value)) base.Add(key, value);
            else throw new System.ArgumentOutOfRangeException(
                        "only null,string,double,bool,Array,Hash instances are value for Hash.this[string key]");
        }

        private bool IsValidItem(object item)
        {
            if (object.ReferenceEquals(null, item)) return true;
            if (item.GetType() == typeof(System.String)) return true;
            if (item.GetType() == typeof(Hash)) return true;
            if (item.GetType() == typeof(System.Double)) return true;
            if (item.GetType() == typeof(Array)) return true;
            if (item.GetType() == typeof(System.Boolean)) return true;
            return false;
        }

        public override string ToString()
        {
            Writer writer = new Writer();
            return writer.ToString(this);
        }
    }
}
