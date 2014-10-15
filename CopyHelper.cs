namespace JsonNet
{
    public static class CopyHelper
    {
        public static void Copy(object source,object destination)
        {
            System.Collections.IEnumerable sourceEnumerable = source as System.Collections.IEnumerable;
            System.Collections.IList destinationList = destination as System.Collections.IList;
            if(!object.ReferenceEquals(null,sourceEnumerable) &&
               !object.ReferenceEquals(null,destinationList))
            {
                System.Collections.IEnumerator denum = sourceEnumerable.GetEnumerator();
                while(denum.MoveNext())
                {

                }
            }
        }
    }
}
