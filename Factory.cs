namespace JsonNet
{
    public sealed class Factory
    {
        public static Value Create() { return new Internal.Dynamic(); }
        public static Value Create(ValueType type)
        {
            switch(type)
            {
                case ValueType.Array:
                    return new Internal.Array();
                case ValueType.Hash:
                    return new Internal.Hash();
            }
            return new Internal.Primitive();
        }

        public static Value Create(string value)
        {
            return new Internal.Primitive(value);
        }

        public static Value Create(double value)
        {
            return new Internal.Primitive(value);
        }

        public static Value Create(bool value)
        {
            return new Internal.Primitive(value);
        }

        public static Value Create(ValueType type,string value)
        {
            switch(type)
            {
                case ValueType.Array:
                    return Internal.Array.ReadArray(value);
                case ValueType.Hash:
                    return Internal.Hash.ReadHash(value);
            }
            return Internal.Primitive.ReadPrimitive(value);
        }

        public static Value Create(ValueType type, System.IO.Stream stream)
        {
            switch(type)
            {
                case ValueType.Array:
                    return Internal.Array.ReadArray(stream);
                case ValueType.Hash:
                    return Internal.Hash.ReadHash(stream);
            }
            return Internal.Primitive.ReadPrimitive(stream);
        }
    }
}
