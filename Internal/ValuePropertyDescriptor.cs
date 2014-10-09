namespace JsonNet.Internal
{
    class ValuePropertyDescriptor :
                  System.ComponentModel.PropertyDescriptor
    {
        private Value value = new Internal.Hash();

        public ValuePropertyDescriptor(string keyValue, System.Attribute[] attributes)
            : base(keyValue, attributes) { }
        public override bool CanResetValue(object component) { return true; }
        public override void ResetValue(object component)
        {
            value = component as Value;
        }
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
        public override void SetValue(object component, object value)
        {
            Value tmpvalue = component as Value;
            if (!object.ReferenceEquals(null, tmpvalue))
            {
                switch (tmpvalue[Name].ValueType)
                {
                    case ValueType.String:
                        tmpvalue[Name].String = (string)value;
                        break;
                    case ValueType.Double:
                        tmpvalue[Name].Double = (double)value;
                        break;
                    case ValueType.Boolean:
                        tmpvalue[Name].Bool = (bool)value;
                        break;
                }
            }
        }
        public override System.Type PropertyType
        {
            get
            {
                switch (value[Name].ValueType)
                {
                    case ValueType.String:
                        return typeof(string);
                    case ValueType.Boolean:
                        return typeof(bool);
                    case ValueType.Double:
                        return typeof(bool);
                }
                return typeof(string);
            }
        }

        public override object GetValue(object component)
        {
            Value node = component as Value;
            if (!object.ReferenceEquals(null, node))
            {
                switch (node[Name].ValueType)
                {
                    case ValueType.String:
                        return node[Name].String;
                    case ValueType.Boolean:
                        return node[Name].Bool;
                    case ValueType.Double:
                        return node[Name].Double;
                }
            }
            return null;
        }

        public override System.Type ComponentType { get { return null; } }
        public override bool IsReadOnly
        {
            get { return false; }
        }
    }
}
