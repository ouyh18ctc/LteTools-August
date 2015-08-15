using System;
using Lte.Domain.LinqToExcel.Entities;
using Lte.Domain.Regular;
using NUnit.Framework;

namespace Lte.Domain.Test.Regular
{
    [TestFixture]
    public class CloneAttributeTest
    {
        class A
        {
            public int M1 { get; set; }

            public int M2 { get; set; }
        }

        class B : IDisposable
        {
            public int M1 { get; set; }

            public int M2 { get; set; }

            public void Dispose()
            { }
        }

        class A1
        {
            public int M1 { get; set; }

            [CloneProtection]
            public int M2 { get; set; }
        }

        [Test]
        public void TestCloneSelf()
        {
            A a = new A { M1 = 11, M2 = 22 };
            A a2 = new A();
            a.CloneProperties<A>(a2);
            Assert.AreEqual(a2.M1, 11);
            Assert.AreEqual(a2.M2, 22);
        }

        [Test]
        public void TestClone_InterClass_NoProtection()
        {
            A a = new A { M1 = 11, M2 = 22 };
            B b = new B { M1 = 33, M2 = 44 };
            a.CloneProperties(b);
            Assert.AreEqual(b.M1, 11);
            Assert.AreEqual(b.M2, 22);

            a.CloneProperties(b, true);
            Assert.AreEqual(b.M1, 11);
            Assert.AreEqual(b.M2, 22);
        }

        [Test]
        public void TestClone_InterClass_Protection()
        {
            A1 a = new A1 { M1 = 11, M2 = 22 };

            using (B b = new B { M1 = 33, M2 = 44 })
            {
                a.CloneProperties(b);
                Assert.AreEqual(b.M1, 11);
                Assert.AreEqual(b.M2, 22);
            }

            using (B b = new B { M1 = 33, M2 = 44 })
            {
                a.CloneProperties(b, true);
                Assert.AreEqual(b.M1, 11);
                Assert.AreEqual(b.M2, 44);
            }
        }
    }
}
