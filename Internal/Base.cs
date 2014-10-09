namespace JsonNet.Internal
{
    abstract class Base : System.ComponentModel.INotifyPropertyChanged
    {
        private string key = "";
        public string Key
        {
            get { return key; }
            set 
            {
                SetProperty<string>(ref key, value,"Key");
            }
        }

        public string ToJson(bool indented)
        {
            string json = "";
            Value value = this as Value;
            if (!object.ReferenceEquals(null, value))
            {
                using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
                {
                    value.Write(memory);//, indented);
                    memory.Seek(0, System.IO.SeekOrigin.Begin);
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(
                        memory, System.Text.Encoding.UTF8, true, 1024))
                    {
                        json = sr.ReadToEnd();
                    }
                }
            }
            return json;
        }

        #region INotifyPropertyChanged
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T value, global::System.String name)
        {
            if (!global::System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(name);
                return true;
            }
            return false;
        }
        public void OnPropertyChanged(string name)
        {
            if (!object.ReferenceEquals(null, PropertyChanged))
            {
                PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(name));
            }
        }
        #endregion

        #region ICustomTypeDescriptor interface
        public System.ComponentModel.PropertyDescriptorCollection GetProperties(System.Attribute[] attributes)
        {
            System.Collections.Generic.List<System.ComponentModel.PropertyDescriptor> descriptors =
                new System.Collections.Generic.List<System.ComponentModel.PropertyDescriptor>();
            Value value = this as Value;
            if (!object.ReferenceEquals(null, value))
            {
                foreach (string key in value.Keys)
                {
                    if (value[key].ValueType == ValueType.Boolean ||
                        value[key].ValueType == ValueType.Double ||
                        value[key].ValueType == ValueType.String)
                    {
                        descriptors.Add(new ValuePropertyDescriptor(key, attributes));
                    }
                }
                descriptors.Add(new ValuePropertyDescriptor("Count", attributes));
            }
            return new System.ComponentModel.PropertyDescriptorCollection(descriptors.ToArray());
        }
        public object GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd) { return this; }
        public string GetClassName() { return System.ComponentModel.TypeDescriptor.GetClassName(this, true); }
        public System.ComponentModel.AttributeCollection GetAttributes() { return System.ComponentModel.TypeDescriptor.GetAttributes(this, true); }
        public System.ComponentModel.PropertyDescriptorCollection GetProperties() { return GetProperties(null); }
        public System.ComponentModel.EventDescriptorCollection GetEvents() { return System.ComponentModel.TypeDescriptor.GetEvents(this, true); }
        public System.ComponentModel.EventDescriptorCollection GetEvents(System.Attribute[] attributes) { return System.ComponentModel.TypeDescriptor.GetEvents(this, attributes, true); }
        public object GetEditor(System.Type editorBaseType) { return null; }
        public System.ComponentModel.PropertyDescriptor GetDefaultProperty() { return System.ComponentModel.TypeDescriptor.GetDefaultProperty(this, true); }
        public System.ComponentModel.EventDescriptor GetDefaultEvent() { return System.ComponentModel.TypeDescriptor.GetDefaultEvent(this, true); }
        public System.ComponentModel.TypeConverter GetConverter() { return System.ComponentModel.TypeDescriptor.GetConverter(this, true); }
        public string GetComponentName() { return System.ComponentModel.TypeDescriptor.GetComponentName(this, true); }
        #endregion
    }
}
