using Lte.Evaluations.Entities;
using System.Xml.Linq;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Entities
{
    [TestFixture]
    public class StatValueFieldElementTest
    {
        private StatValueField _field;
        private XElement _fieldElement;

        [SetUp]
        public void TestInitialize()
        {
            _fieldElement = new XElement("Field", new XAttribute("ID", "myFieldName"));
            _fieldElement.Add(new XElement("Interval",
                    new XElement("LowLevel", "11"),
                    new XElement("UpLevel", "15"),
                    new XElement("A", "98"),
                    new XElement("B", "114"),
                    new XElement("R", "198"),
                    new XElement("G", "221")));
            _fieldElement.Add(new XElement("Interval",
                    new XElement("LowLevel", "15"),
                    new XElement("UpLevel", "17"),
                    new XElement("A", "6"),
                    new XElement("B", "14"),
                    new XElement("R", "108"),
                    new XElement("G", "171")));
            _field = new StatValueField(_fieldElement);
        }

        [Test]
        public void TestStatValueField_InputElement()
        {
            Assert.AreEqual(_fieldElement.ToString().Replace("\r\n","\n"), (@"<Field ID=""myFieldName"">
  <Interval>
    <LowLevel>11</LowLevel>
    <UpLevel>15</UpLevel>
    <A>98</A>
    <B>114</B>
    <R>198</R>
    <G>221</G>
  </Interval>
  <Interval>
    <LowLevel>15</LowLevel>
    <UpLevel>17</UpLevel>
    <A>6</A>
    <B>14</B>
    <R>108</R>
    <G>171</G>
  </Interval>
</Field>").Replace("\r\n", "\n"));
        }

        [Test]
        public void TestStatValueField_InputElement_AssertParameters()
        {
            Assert.AreEqual(_field.FieldName, "myFieldName");
            Assert.AreEqual(_field.IntervalList.Count, 2);
            Assert.AreEqual(_field.IntervalList[0].IntervalLowLevel, 11);
            Assert.AreEqual(_field.IntervalList[0].Color.ColorA, 98);
            Assert.AreEqual(_field.IntervalList[0].Color.ColorG, 221);
            Assert.AreEqual(_field.IntervalList[1].Color.ColorB, 14);
            Assert.AreEqual(_field.IntervalList[1].IntervalUpLevel, 17);
        }
    }
}
