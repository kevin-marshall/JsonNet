using NUnit.Framework;

namespace JsonNet
{
    [TestFixture,Category("Writer")]
    public class Writer_Test : QcNet.TestFixture<Writer>
    {
        [SetUp]
        public void SetUp()
        {
            Clear();
            Add("default", new Writer());
            Add("indented", new Writer(JsonFormat.Indented));
        }

        [TestCase]
        public void Writer_TestInstance()
        {
            base.TestInstances();

            Hash_Test hashes = new Hash_Test();
            hashes.SetUp();

            Array_Test arrays = new Array_Test();
            arrays.SetUp();
            foreach(string key in Keys)
            {

            }
        }
        [TestCase]
        public void Writer_Usage()
        {
            System.Collections.Generic.Dictionary<string, string> jsonMap
                = new System.Collections.Generic.Dictionary<string, string>();

            using (Writer writer = new Writer())
            {
                Hash_Test hashes = new Hash_Test();
                hashes.SetUp();
                foreach (string key in hashes.Keys)
                {
                    using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
                    {
                        writer.Write(hashes[key], memory);
                        memory.Seek(0, System.IO.SeekOrigin.Begin);
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(memory))
                        {
                            string json = sr.ReadToEnd();
                            jsonMap.Add(key, sr.ReadToEnd());
                            Assert.AreEqual(json, writer.ToString(hashes[key]));
                        }
                    }

                }
            }
            using (Writer writer = new Writer(JsonFormat.Indented))
            {
                Hash_Test hashes = new Hash_Test();
                hashes.SetUp();
                foreach (string key in hashes.Keys)
                {
                    using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
                    {
                        writer.Write(hashes[key], memory);
                        memory.Seek(0, System.IO.SeekOrigin.Begin);
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(memory))
                        {
                            string json = sr.ReadToEnd();
                            Assert.AreEqual(json, writer.ToString(hashes[key]));
                            //using(System.IO.FileStream fs = new System.IO.FileStream(GetUnit))
                        }
                    }

                }
            }
        }
    }
}
