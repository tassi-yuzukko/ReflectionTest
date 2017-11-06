using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ReflectionTest
{
    [TestClass]
    public class UnitTest1
    {
        public class CLASS
        {
            public int A { get; set; }
            public string B { get; set; }
            public string[] C { get; set; }

            public class Tashima
            {
                public int Int { get; set; }
                public byte Byte { get; set; }
            }

            public Tashima T { get; set; }

            public List<string> D { get; set; }
            public List<Tashima> E { get; set; }
            public int F { private get; set; } = 2;
            public int G => 4;
        }

        [TestMethod]
        public void TestMethod2()
        {
            var a = new CLASS();
            a.A = 1;
            a.B = "test";
            a.C = new string[] { "2", "3" };
            a.T = new CLASS.Tashima { Int = 1, Byte = 2 };
            a.D = new List<string>(new string[] { "3333", "aaaaaaaafefeef", "iayaaaaaaaaaaaaa" });
            a.E = new List<CLASS.Tashima>();
            a.E.Add(new CLASS.Tashima { Byte = 2, Int = 4 });
            a.E.Add(new CLASS.Tashima { Byte = 33, Int = 6 });
            a.E.Add(new CLASS.Tashima { Byte = 45, Int = 5 });

            NestMaaaaan(a);
        }

        public void NestMaaaaan(object obj)
        {
            PropertyInfo[] infoArray = obj.GetType().GetProperties();

            // プロパティ情報出力をループで回す
            foreach (PropertyInfo info in infoArray)
            {
                if (!info.PropertyType.IsPublic)
                {
                    continue;
                }

                if (info.GetGetMethod(false) == null)
                {
                    continue;
                }

                if (info.PropertyType.IsArray)
                {
                    var array = (Array)info.GetValue(obj);

                    Console.WriteLine($"●{info.Name}, length:{array.Length}");

                    foreach (var val in array)
                    {
                        if (val.GetType().IsNestedPublic)
                        {
                            NestMaaaaan(val);
                        }
                        else
                        {
                            PrimitiveOrString("array", val);
                        }
                    }
                }
                else if (info.PropertyType.IsNestedPublic)
                {
                    NestMaaaaan(info.GetValue(obj));
                }
                else if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    System.Collections.IList list = info.GetValue(obj) as System.Collections.IList;

                    Console.WriteLine($"●{info.Name}, length:{list.Count}");

                    foreach (var val in list)
                    {
                        if (val.GetType().IsNestedPublic)
                        {
                            NestMaaaaan(val);
                        }
                        else
                        {
                            PrimitiveOrString("list", val);
                        }
                    }
                }
                else
                {
                    PrimitiveOrString(info.Name, info.GetValue(obj));
                }
            }
        }

        public void PrimitiveOrString(string name, object obj)
        {
            if (obj.GetType().IsPrimitive)
            {
                Console.WriteLine($"●{name}, size:{Marshal.SizeOf(obj)}");
                Console.WriteLine(obj);
            }
            else if (obj.GetType() == typeof(string))
            {
                var str = obj as string;

                Console.WriteLine($"●{name}, size:{str.Length}");
                Console.WriteLine(str);
            }
        }
    }
}
