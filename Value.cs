namespace JsonNet
{
    public interface Value : System.IComparable, 
                             System.ComponentModel.INotifyPropertyChanged, 
                             System.Collections.IEnumerable,
                             System.ComponentModel.ICustomTypeDescriptor
    {
        ValueType ValueType { get; }

        string Key { get; }
        bool IsNull { get; set; }
        string String { get; set; }
        double Double { get; set; }
        bool Bool { get; set; }

        Value Clone();

        bool ContainsKey(string key);
        System.Collections.Generic.IList<string> Keys { get; }
        Value this[string key] { get; set; }

        int Count { get; }
        void Clear();
        Value this[int index] { get; set; }
        void Remove(string key);

        int DeepCount { get; }

        void Write(System.IO.Stream stream);
        void Read(System.IO.Stream stream);
        string ToJson(bool indented);
    }
}
