using System;
using System.Collections.Generic;
using Lte.Domain.Regular;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;

namespace Lte.Domain.Test.Regular
{
    public interface IMyClass
    {
        double MyDoubleAttribute { get; set; }

        List<int> MyListAttribute { get; set; }
    }
         
    [TestFixture]
    public class BindGetAndSetAttributesTest
    {
           
        private Mock<IMyClass> mock = new Mock<IMyClass>();

        [Test]
        public void TestBindGetAndSetAttributes_DoubleAttribute()
        {
            Expression<Func<IMyClass, double>> getter = x => x.MyDoubleAttribute;
            mock.SetupGet(getter).Returns(1.2);
            Assert.AreEqual(mock.Object.MyDoubleAttribute, 1.2);
            mock.BindGetAndSetAttributes<IMyClass, double>(x => x.MyDoubleAttribute, (x, v) => x.MyDoubleAttribute = v);
            mock.Object.MyDoubleAttribute = 1.1;
            Assert.AreEqual(mock.Object.MyDoubleAttribute, 1.1);
        }

        [Test]
        public void TestBindGetAndSetAttributes_ListAttribute()
        {
            mock.BindGetAndSetAttributes<IMyClass, List<int>>(x => x.MyListAttribute,
                (x, v) => x.MyListAttribute = v);
            mock.Object.MyListAttribute = new List<int> { 0, 1, 2, 3, 4 };
            Assert.AreEqual(mock.Object.MyListAttribute.Count, 5);
            Assert.AreEqual(mock.Object.MyListAttribute[3], 3);
        }
    }
}
