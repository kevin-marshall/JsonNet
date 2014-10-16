namespace JsonNet
{
    public static class CopyHelper
    {
        public static void Copy(object source,object destination)
        {
            System.Collections.IDictionary sourceDictionary = source as System.Collections.IDictionary;
            System.Collections.IDictionary destinationDictionary = destination as System.Collections.IDictionary;
            if(!object.ReferenceEquals(null,sourceDictionary) &&
               !object.ReferenceEquals(null,destinationDictionary))
            {
                foreach(object key in sourceDictionary.Keys)
                {
                    destinationDictionary.Add(key,sourceDictionary[key]);
                }
                return;
            }
            System.Collections.IEnumerable sourceEnumerable = source as System.Collections.IEnumerable;
            System.Collections.IList destinationList = destination as System.Collections.IList;
            if(!object.ReferenceEquals(null,sourceEnumerable) &&
               !object.ReferenceEquals(null,destinationList))
            {
                System.Collections.IEnumerator denum = sourceEnumerable.GetEnumerator();
                while(denum.MoveNext())
                {
                    destinationList.Add(denum.Current);
                }
                return;
            }
        }
    }
}
