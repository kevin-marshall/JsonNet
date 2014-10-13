using NUnit.Framework;

namespace JsonNet
{
    [TestFixture,Category("Factory")]
    public class Factory_Test
    {
        private System.IO.StreamWriter streamWriter = null;
        [TestCase]
        public void Factory_Create()
        {
            Factory.Create(ValueType.Hash,CreateMemoryStream("{}"));
            Factory.Create(ValueType.Double, CreateMemoryStream("1.23"));
            Factory.Create(ValueType.Double, "1.23");
        }
        private System.IO.MemoryStream CreateMemoryStream(string data)
        {
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            streamWriter = new System.IO.StreamWriter(memory);
            streamWriter.Write(data);
            streamWriter.Flush();
            memory.Seek(0, System.IO.SeekOrigin.Begin);
            return memory;
        }
    }

    

}
