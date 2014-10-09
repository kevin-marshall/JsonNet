namespace JsonNet.Internal
{
    class ValueCompare
    {
        public static int Compare(Value a,object b)
        {
            if (object.ReferenceEquals(a, b)) return 0;
            if (object.ReferenceEquals(null, b)) return 1;
            if (object.ReferenceEquals(null, a)) return -1;

            Value valueB = b as Value;
            if (!object.ReferenceEquals(null, valueB)) return Compare(a, valueB);

            return a.GetType().FullName.CompareTo(b.GetType().FullName);
        }
        public static int Compare(Value a,Value b)
        {
            if (object.ReferenceEquals(a, b)) return 0;
            if (object.ReferenceEquals(null, b)) return 1;
            if (object.ReferenceEquals(null, a)) return -1;

            if(a.ValueType == b.ValueType)
            {
                switch(a.ValueType)
                {
                    case ValueType.Hash:
                        return CompareHash(a, b);
                    case ValueType.Array:
                        return CompareArray(a, b);
                    default:
                        return ComparePrimitive(a, b);
                }
            }

            return a.ValueType.CompareTo(b.ValueType);
        }

        private static int ComparePrimitive(Value a,Value b)
        {
            // a and b are of Same ValueType
            switch (a.ValueType)
            {
                case ValueType.Boolean:
                    return a.Bool.CompareTo(b.Bool);
                case ValueType.Double:
                    return a.Double.CompareTo(b.Double);
                case ValueType.String:
                    return a.String.CompareTo(b.String);
            }
            return 0;
        }

        private static int CompareArray(Value a,Value b)
        {
            // a and b are of ValueType.Array
            if (a.Count != b.Count) return a.Count.CompareTo(b.Count);
            for (int i = 0; i < a.Count; ++i )
            {
                int c = a[i].CompareTo(b[i]);
                if (c != 0) return c;
            }
            return 0;
        }

        private static int CompareHash(Value a,Value b)
        {
            // a and b are of ValueType.Hash
            int result = 0;
            if (a.Count != b.Count) return a.Count.CompareTo(b.Count);
            System.Collections.Generic.IEnumerator<string> enum1 = a.Keys.GetEnumerator();
            System.Collections.Generic.IEnumerator<string> enum2 = b.Keys.GetEnumerator();

            while (enum1.MoveNext())
            {
                enum2.MoveNext();
                result = enum1.Current.CompareTo(enum2.Current);
                if (result != 0) return result;

                result = a[enum1.Current].CompareTo(b[enum2.Current]);
                if (result != 0) return result;
            }

            return 0;
        }
    }
}
