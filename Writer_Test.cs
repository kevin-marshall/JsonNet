using NUnit.Framework;

namespace JsonNet
{
    [TestFixture,Category("Writer")]
    public class Writer_Test
    {
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
        }
    }
}
