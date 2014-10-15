using NUnit.Framework;

namespace JsonNet
{
    [TestFixture,Category("Reader")]
    public class Reader_Test
    {
        [TestCase]
        public void Reader_Usage()
        {
            using (Writer writer = new Writer())
            {
                Hash_Test hashes = new Hash_Test();
                hashes.SetUp();
                foreach (string key in hashes.Keys)
                {
                    string json = writer.ToString(hashes[key]);
                    using(Reader reader = new Reader())
                    {
                        Hash clone = reader.Read(json) as Hash;
                        Assert.NotNull(clone);
                        Assert.True(clone.Equals(hashes[key]));
                    }
                }

                Array_Test arrays = new Array_Test();
                arrays.SetUp();
                foreach(string key in arrays.Keys)
                {
                    string json = writer.ToString(arrays[key]);
                    using (Reader reader = new Reader())
                    {
                        Array clone = reader.Read(json) as Array;
                        Assert.NotNull(clone);
                        Assert.True(clone.Equals(arrays[key]));
                    }
                }
            }
        }
    }
}
