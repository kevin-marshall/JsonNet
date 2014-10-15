namespace JsonNet
{
    public static class CompareHelper
    {
        public static int Compare(object a,object b)
        {
            if (object.ReferenceEquals(null, a)) return object.ReferenceEquals(null, b) ? 0 : -1;
            if (object.ReferenceEquals(null, b)) return 1;
            if (a.GetType().IsAssignableFrom(b.GetType()) ||
               b.GetType().IsAssignableFrom(a.GetType()))
            {
                return a.GetHashCode().CompareTo(b.GetHashCode());
            }
            return a.GetType().FullName.CompareTo(b.GetType().FullName);
        }
    }
}
