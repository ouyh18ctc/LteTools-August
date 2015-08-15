using System.Xml.Linq;
using Lte.Evaluations.Entities;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Entities
{
    [TestFixture]
    public class StatValueIntervalConstructorTest
    {
        [Test]
        public void TestStatValueInterval_Constructor()
        {
            XElement element = new XElement("Interval",
                    new XElement("LowLevel", "11"),
                    new XElement("UpLevel", "15"),
                    new XElement("A", "98"),
                    new XElement("B", "114"),
                    new XElement("R", "198"),
                    new XElement("G", "221"));
            StatValueInterval interval = new StatValueInterval(element);
            Assert.AreEqual(interval.IntervalLowLevel, 11);
            Assert.AreEqual(interval.IntervalUpLevel, 15);
            Assert.AreEqual(interval.Color.ColorA, 98);
            Assert.AreEqual(interval.Color.ColorB, 114);
            Assert.AreEqual(interval.Color.ColorG, 221);
            Assert.AreEqual(interval.Color.ColorR, 198);
        }
    }
}
