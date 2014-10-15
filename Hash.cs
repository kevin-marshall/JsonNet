namespace JsonNet
{
    public sealed class Hash : System.Collections.Generic.Dictionary<string,object>,
                               System.IComparable
    {
        public Hash() { }
        public Hash(string json) 
        {
            Reader reader = new Reader();
            Hash hash = reader.Read(json) as Hash;
            foreach(string key in hash.Keys)
            {
                Add(key, hash[key]);
            }
        }

        public int CompareTo(object value) { return CompareHelper.Compare(this, value); }

        public override bool Equals(object obj) { return CompareTo(obj) == 0 ? true : false; }

        public override int GetHashCode() { return HashCodeHelper.GetHashCode(this); }
    }
}
