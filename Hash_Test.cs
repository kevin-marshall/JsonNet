using NUnit.Framework;

namespace JsonNet
{
    [TestFixture,Category("Hash")]
    public class Hash_Test : QcNet.TestFixture<Hash>
    {
        [SetUp]
        public void SetUp()
        {
            Clear();
            Add("default", new Hash());
            Add("default2", new Hash());
            Add("hash_null", new Hash("{'null':null}"));
            Add("hash_false", new Hash("{'false':false}"));
            Add("hash_true", new Hash("{'true':true}"));
            Add("hash_a",new Hash("{'a':'a'}"));
            Add("hash_b", new Hash("{'b':'b'}"));
            Add("hash_0", new Hash("{'0':0}"));
            Add("hash_1",new Hash("{'1':1}"));
            Add("hash_nested", new Hash("{'a':{'0':0,'1':1}}"));
            Add("hash_nested2", new Hash("{'a':{'0':[0,1,2],'1':[3,4,5]}}"));
        }

        [TestCase]
        public void Hash_TestInstances() { base.TestInstances(); }

        [TestCase]
        public void Hash_Usage()
        {
            Hash h = new Hash();
            h["string"] = "a";
            h["bool"] = false;
            h["null"] = null;
            h["double"] = 1.5;
            h["Array"] = new Array("[0,1,2,3]");
            h["Hash"] = new Hash("{'a':0}");
            Assert.AreEqual(6, h.Count);
        }
        [TestCase]
        public void Hash_TestConstraints()
        {
            Hash hash = new Hash();
            Assert.Throws<System.ArgumentOutOfRangeException>(() => hash["bad"]=System.DateTime.Now);
            Assert.Throws<System.ArgumentOutOfRangeException>(() => hash.Add("date",System.DateTime.Now));
        }
    }
}
