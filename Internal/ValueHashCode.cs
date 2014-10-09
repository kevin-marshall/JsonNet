namespace JsonNet.Internal
{
    class ValueHashCode
    {
        public static int GetHashCode(Value value)
        {
            switch (value.ValueType)
            {
                case ValueType.Hash:
                    {
                        int hashCode = value.Count;
                        foreach (string key in value.Keys)
                        {
                            hashCode = hashCode ^ key.GetHashCode() ^ value[key].GetHashCode();
                        }
                        return hashCode;
                    }
                case ValueType.Array:
                    {
                        int hashCode = value.Count;
                        for (int i = 0; i < value.Count; ++i)
                        {
                            hashCode = hashCode ^ value[i].GetHashCode();
                        }
                        return hashCode;
                    }
                case ValueType.Boolean:
                    return value.Bool.GetHashCode();
                case ValueType.Double:
                    return value.Double.GetHashCode();
                case ValueType.String:
                    return value.String.GetHashCode();
            }

            return 0;
        }
    }
}
