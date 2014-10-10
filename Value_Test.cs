using NUnit.Framework;

namespace JsonNet
{
    [TestFixture]
    public class Value_Test : System.Collections.Generic.Dictionary<string,Value>
    {
        public void Set(string name,Value value)
        {
            if(!ContainsKey(name))Add(name, value);
        }
        [SetUp]
        public void SetUp()
        {
            Clear();
            
            Set("null", Factory.Create(ValueType.Null));
            Set("null2", Factory.Create(ValueType.Null));
            Set("stringA", Factory.Create("a"));
            Set("stringC", Factory.Create("c"));
            Set("stringSingleQuote", Factory.Create("c'c"));
            Set("stringDoubleQuote", Factory.Create("b\"a"));
            Set("int1", Factory.Create(1));
            Set("double2_5", Factory.Create(2.5));
            Set("false", Factory.Create(false));
            Set("true", Factory.Create(true));

            Set("null", Factory.Create());
             
            Set("numbers_4a", Factory.Create(ValueType.Array, "[0,1,2,3]"));
            
            Set("numbers_4b", Factory.Create(ValueType.Array, "[0,1,2,4]"));
            Set("numbers_4c", Factory.Create(ValueType.Array, "[3,2,1,0]"));
            Set("numbers_4d", Factory.Create(ValueType.Array, "[3,2,1,0]"));
            Set("strings_2", Factory.Create(ValueType.Array, "[\"A\",\"B\"]"));
            Set("numbers_5a", Factory.Create(ValueType.Array, "[0,1,2,3,4]"));

            Set("empty", Factory.Create(ValueType.Hash, "{}"));
            Set("hashA", Factory.Create(ValueType.Hash, "{\"A\":0}"));
            Set("hashB", Factory.Create(ValueType.Hash, "{\"A\":0}"));
            Set("hashC", Factory.Create(ValueType.Hash, "{\"B\":0}"));
            Set("hashD", Factory.Create(ValueType.Hash, "{\"Items\":[1,\"a\",false]}"));
        }

        [TestCase]
        public void Value_Usage()
        {
            Value value = Factory.Create();
            value[0].Double = 0;
            value["A"].String = "B";
            value.Bool = false;

            Value value2 = value.Clone();
            value2.Clear();
            Assert.AreEqual(0, value2.Count);
            value2["A"].String = "A";
            value2["B"].String = "B";
            Assert.AreEqual(2, value2.Count);
            value2.Remove("A");
            Assert.AreEqual(1, value2.Count);
        }

        [TestCase]
        public void Value_Factory_Create_From_Stream()
        {
            using(System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(memory);
                sw.Write("[0,1,2,4]");
                sw.Flush();
                memory.Seek(0,System.IO.SeekOrigin.Begin);
                Value value = Factory.Create(ValueType.Array, memory);
                Assert.AreEqual(4, value.Count);
            }
        }

        [TestCase]
        public void Value_Write_Read()
        {
            Value value = Factory.Create();
            value["a"].String = "A";
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                value.Write(memory);
                string smem = memory.ToString();
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                Value v3 = Factory.Create();
                v3.Read(memory);
                Assert.True(value.Equals(v3));
            }
        }
        [TestCase]
        public void Value_ValueSemantics()
        {
            Value double2_5 = this["double2_5"];
            Assert.AreEqual(2.5, double2_5.Double);
            Value double2_5Clone = double2_5.Clone();
            Assert.AreEqual(2.5, double2_5.Double);
            Assert.AreEqual(double2_5.Double, double2_5Clone.Double);
            double2_5Clone.Double = 3.5;
            Assert.AreNotEqual(double2_5.Double, double2_5Clone.Double);

            Value stringA = this["stringA"];
            Value stringAClone = stringA.Clone();
            Assert.AreEqual(stringA.String, stringAClone.String);
            stringAClone.String = "b";
            Assert.AreNotEqual(stringA.String, stringAClone.String);

            Value boolA = Factory.Create(true);
            Value boolAClone = boolA.Clone();
            Assert.AreEqual(boolA.Bool, boolAClone.Bool);
            boolAClone.Bool = false;
            Assert.AreNotEqual(boolA.Bool, boolAClone.Bool);

            Value arrayA = Factory.Create(ValueType.Array);
            arrayA[0].String = "a";
            arrayA[1].String = "b";

            Value arrayB = arrayA.Clone();
            Assert.AreEqual(arrayA.Count, arrayB.Count);
            arrayB[0].String = "x";
            Assert.AreNotEqual(arrayA[0].String, arrayB[0].String);
            arrayB[arrayB.Count].Double = 3;
            Assert.AreNotEqual(arrayA.Count, arrayB.Count);

            Value hashA = Factory.Create(ValueType.Hash);
            hashA["a"].String = "A";
            hashA["b"].String = "B";

            Value hashAClone = hashA.Clone();
            Assert.AreEqual(hashA.Count, hashAClone.Count);
            hashAClone["a"].String = "X";
            Assert.AreNotEqual(hashA["a"].String, hashAClone["a"].String);
        }

        [TestCase]
        public void Value_TestInstance_Cloning()
        {
            Value value = Factory.Create(ValueType.Hash);
            value["Name"].String = "A";

            foreach(string key in Keys)
            {
                Value v = this[key];
                ValueQC.Test(v);
            }
        }

        [TestCase]
        public void Value_ICustomTypeDescriptor()
        {
            Value v = Factory.Create(ValueType.Hash);
            Assert.AreEqual(1, v.GetProperties().Count);
            v["a"].String = "a";
            v["b"].String = "b";
            Assert.AreEqual(3, v.GetProperties().Count);
        }

        [TestCase]
        public void Value_Stress_Test()
        {
            Value v = CreateHash(8, 5);//8,5 488280     10,5 12207030
            int deepCount = v.DeepCount;
            Value v2 = v.Clone();
            Assert.AreEqual(v, v2);
            v2 = null;

            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                v.Write(memory);
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                Value v3 = Factory.Create();
                v3.Read(memory);
                Assert.AreEqual(v, v3);
            }
        }

        private Value CreateHash(int levels, int items_per_level)
        {
            Value value = Factory.Create();
            if (levels == 0) return value;
            for (int i = 0; i < items_per_level; ++i)
            {
                value["L" + levels.ToString() + "i" + i.ToString()] = CreateHash(levels - 1, items_per_level);
            }
            return value;
        }
    }
}
