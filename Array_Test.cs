using NUnit.Framework;

namespace JsonNet
{
    [TestFixture,Category("Array")]
    public class Array_Test : QcNet.TestFixture<Array>
    {
        [SetUp]
        public void SetUp()
        {
            Clear();
            Add("default", new Array());
            Add("default2", new Array());
            Add("null", new Array("[null]"));
            Add("null2", new Array("[null]"));
            Add("strings_a",new Array("['a']"));
            Add("strings_a2", new Array("['a']"));
            Add("strings_b", new Array("['b']"));
            Add("doubles_0", new Array("[0]"));
            Add("doubles_1", new Array("[1]"));
            Add("doubles_negatives", new Array("[1,-2,-3,4]"));
            Add("bool_true", new Array("[true]"));
            Add("bool_true2", new Array("[true]"));
            Add("bool_false", new Array("[false]"));
            Add("nested", new Array("[[0,1,2],['a','b','c'],[false,true,false],[null,null,null]]"));
            Add("nested2", new Array("[[{'a':0,'b':1},1,2],['a','b','c'],[false,true,false],[null,null,null]]"));
        }

        [TestCase]
        public void Array_TestInstances() { base.TestInstances(); }

        [TestCase]
        public void Array_Usage()
        {
            Array a = new Array();
            a.Add(0);

            System.Collections.Generic.List<object> items = new System.Collections.Generic.List<object>();
            items.Add(null);
            items.Add("a");
            items.Add(0);
            items.Add(false);
            items.Add(new Array("[0,1,2]"));
            items.Add(new Hash("{'a':'a'}"));
            Array arr = new Array();
            arr.AddRange(items);
            arr.InsertRange(0, items);
            Assert.AreEqual(items.Count * 2, arr.Count);

            arr.Insert(0, 3);
            arr.Add(new Array("[0,1,2,3]"));
            arr[arr.Count-1].Add(4);
            arr.Insert(0, 5);

            Array negatives = this["doubles_negatives"] as Array;
            Assert.AreEqual(-2, (double)negatives[1]);
        }
        [TestCase]
        public void Array_TestConstraints()
        {
            Array arr = new Array();
            Assert.Throws<System.ArgumentOutOfRangeException>(() => arr.Add(System.DateTime.Now));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => arr.Insert(0,System.DateTime.Now));

            System.Collections.Generic.List<object> dates = new System.Collections.Generic.List<object>();
            dates.Add(System.DateTime.Now);
            Assert.Throws<System.ArgumentOutOfRangeException>(() => arr.AddRange(dates));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => arr.InsertRange(0,dates));
        }
    }
}
