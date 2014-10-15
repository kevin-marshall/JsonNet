namespace JsonNet
{
    public static class HashCodeHelper
    {
        public static int GetHashCode(object value)
        {
            System.Collections.IDictionary idictionary = value as System.Collections.IDictionary;
            if(!object.ReferenceEquals(null,idictionary))
            {
                int result = idictionary.Count;
                foreach(object key in idictionary.Keys)
                {
                    result = result ^ key.GetHashCode();
                    if(!object.ReferenceEquals(null,idictionary[key]))
                    {
                        result = result ^ idictionary[key].GetHashCode();
                    }
                }
                return result;
            }
            System.Collections.IEnumerable ienumerable = value as System.Collections.IEnumerable;
            if(!object.ReferenceEquals(null,ienumerable))
            {
                int result = 0;
                int count = 0;
                System.Collections.IEnumerator denum = ienumerable.GetEnumerator();
                while(denum.MoveNext())
                {
                    if(!object.ReferenceEquals(null,denum.Current))
                    {
                        result = result ^ denum.Current.GetHashCode();
                    }
                    ++count;
                }
                return count.GetHashCode() ^ result;
            }
            return value.GetHashCode();
        }
    }
}
