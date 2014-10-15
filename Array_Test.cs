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
            System.Collections.Generic.List<object> items = new System.Collections.Generic.List<object>();
            items.Add(null);
            items.Add("a");
            Array arr = new Array();
            arr.AddRange(items);
            arr.InsertRange(0, items);
            Assert.AreEqual(items.Count * 2, arr.Count);
            arr.Insert(0, "b");

            Array copy = new Array();
            CopyHelper.Copy(arr, copy);
            Assert.True(arr.Equals(copy));
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
