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
        public void Hash_Xml_TransformationsA()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Hash_Test));
            foreach(string name in assembly.GetManifestResourceNames())
            {
                if(name.Contains(".xml"))
                {
                    System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
                    xdoc.Load(assembly.GetManifestResourceStream(name));
                    string xml = XmlTransform.ToString(xdoc);
                    Hash hash = XmlTransform.FromXml(xdoc) as Hash;
                    Assert.NotNull(hash);
                    System.Xml.XmlDocument xdoc2 = XmlTransform.ToXml(hash);
                    string xml2 = XmlTransform.ToString(xdoc2);
                    //Assert.AreEqual(xml, xml2);
                    //Hash hash2 = XmlTransform.FromXml(xdoc) as Hash;
                   // Assert.True(hash.Equals(hash2));
                }
            }
        }
        [TestCase]
        public void Hash_Xml_Transformations()
        {
            Assert.IsNull(XmlTransform.ToXml(null));
            Assert.IsNull(XmlTransform.FromXml(null));

            foreach(string key in Keys)
            {
                Hash hash = this[key];
                string json = hash.ToString();
                System.Xml.XmlDocument xdoc = XmlTransform.ToXml(hash);
                string xml = XmlTransform.ToString(xdoc);
                Assert.NotNull(xdoc);
                Hash hash2 = XmlTransform.FromXml(xdoc) as Hash;
                Assert.NotNull(hash2);
                //Assert.True(hash.Equals(hash2));
            }
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
