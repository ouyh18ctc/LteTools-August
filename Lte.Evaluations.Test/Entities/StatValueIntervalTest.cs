using Lte.Evaluations.Entities;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Entities
{
    [TestFixture]
    public class StatValueIntervalTest
    {
        private StatValueInterval _statValueInterval;

        [SetUp]
        public void TestInitialize()
        {
            _statValueInterval = new StatValueInterval
            {
                Color = new Color
                {
                    ColorA = 122,
                    ColorB = 17,
                    ColorG = 201,
                    ColorR = 144
                },
                IntervalLowLevel = 10,
                IntervalUpLevel = 13
            };
        }

        [Test]
        public void TestStatValueInterval_ColorStringForKml()
        {
            Assert.AreEqual(_statValueInterval.Color.ColorStringForKml, "7A11C990");
        }

        [Test]
        public void TestStatValueInterval_XElement()
        {
            Assert.AreEqual(_statValueInterval.XElement.ToString().Replace("\r\n", "\n"), (@"<Interval>
  <LowLevel>10</LowLevel>
  <UpLevel>13</UpLevel>
  <A>122</A>
  <B>17</B>
  <R>144</R>
  <G>201</G>
</Interval>").Replace("\r\n", "\n"));
        }
    }
}
