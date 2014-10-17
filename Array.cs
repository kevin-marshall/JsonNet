namespace JsonNet
{
    public sealed class Array : System.Collections.Generic.List<dynamic>,
                                System.IComparable
    {
        public Array() { }
        public Array(string json) 
        {
            Array array = Reader.Parse(json) as Array;
            for(int i = 0; i < array.Count; ++i){Add(array[i]);}
        }

        public object Clone() { return new Array(Writer.ToJson(this)); }
        
        public int CompareTo(object value){return CompareHelper.Compare(this, value);}

        public override bool Equals(object obj){return CompareTo(obj) == 0 ? true : false;}

        public override int GetHashCode(){return HashCodeHelper.GetHashCode(this);}

        private object GetValidItem(object item)
        {
            if (object.ReferenceEquals(null, item)) return item;
            if(item.GetType()==typeof(System.Int32)) {double value = (int)item; return value;}
            return item;
        }
        
        public new void Add(object item)
        {
            object vitem = GetValidItem(item);
            if (IsValidItem(vitem)) base.Add(vitem);
            else throw new System.ArgumentOutOfRangeException("item", item,
                        "only null,string,double,bool,Array,Hash instances are value for Array.Add");
        }
        public new void AddRange(System.Collections.Generic.IEnumerable<object> items)
        {
            bool hasInvalidItem = false;
            System.Collections.Generic.List<object> validItems = new System.Collections.Generic.List<object>();
            System.Collections.IEnumerator denum = items.GetEnumerator();
            while(denum.MoveNext() && !hasInvalidItem)
            {
                object vitem = GetValidItem(denum.Current);
                if (!IsValidItem(vitem)) hasInvalidItem = true;
                else validItems.Add(vitem);
            }
            if (!hasInvalidItem) base.AddRange(validItems);
            else throw new System.ArgumentOutOfRangeException("items", items,
                        "only null,string,double,bool,Array,Hash instances are value for Array.AddRange");
        }

        public new void Insert(int index,object item)
        {
            object vitem = GetValidItem(item);
            if(IsValidItem(vitem)) base.Insert(index,vitem);
            else throw new System.ArgumentOutOfRangeException("item", item,
                        "only null,string,double,bool,Array,Hash instances are value for Array.Insert");
        }

        public new void InsertRange(int index,System.Collections.Generic.IEnumerable<object> items)
        {
            bool hasInvalidItem = false;
            System.Collections.Generic.List<object> validItems = new System.Collections.Generic.List<object>();
            System.Collections.IEnumerator denum = items.GetEnumerator();
            while (denum.MoveNext() && !hasInvalidItem)
            {
                object vitem = GetValidItem(denum.Current);
                if (!IsValidItem(vitem)) hasInvalidItem = true;
                else validItems.Add(vitem);
            }
            if (!hasInvalidItem) base.InsertRange(index, validItems);
            else throw new System.ArgumentOutOfRangeException("items", items,
                        "only null,string,double,bool,Array,Hash instances are value for Array.InsertRange");
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

        public void Write(System.IO.Stream stream) { Writer.WriteToStream(this, stream); }
        public void Read(System.IO.Stream stream) 
        { 
            Clear(); 
            Array that = Reader.Parse(stream) as Array;
            for (int i = 0; i < that.Count; ++i) { Add(that[i]);}
        }
    }
}
