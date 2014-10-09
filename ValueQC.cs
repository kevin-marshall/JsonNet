namespace JsonNet
{
    public class ValueQC
    {
        public static void Test(JsonNet.Value value)
        {
            JsonNet.Value clone = value.Clone();
            if(clone.CompareTo(value) != 0)
            {
                throw new System.Exception("CompareTo did not evalute to zero for cloned Value."
                    + System.Environment.NewLine + "Original Value ToString:" 
                    + System.Environment.NewLine + value.ToString()
                    + System.Environment.NewLine + "Cloned Value ToString:"
                    + System.Environment.NewLine + clone.ToString() 
                    + System.Environment.NewLine + "value GetType() = " + value.GetType().FullName);
            }

            switch(value.ValueType)
            {
                case ValueType.String:
                    clone.String = value.String;
                    break;
                case ValueType.Boolean:
                    clone.Bool = value.Bool;
                    break;
                case ValueType.Double:
                    clone.Double = value.Double;
                    break;
                case ValueType.Array:
                    if(clone.Count !=value.Count)
                    {
                        throw new System.Exception("clone.Count != value.Count");
                    }
                    break;
            }
        }
    }
}
