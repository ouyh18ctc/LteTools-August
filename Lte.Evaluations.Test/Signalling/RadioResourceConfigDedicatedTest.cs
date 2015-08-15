using Lte.Evaluations.Signalling;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Signalling
{
    [TestFixture]
    public class RadioResourceConfigDedicatedTest
    {
        private const string signalString = "13980a1dce0183c0ba007e131ffa211f0c288d980002e808000960";

        [Test]
        public void TestRadioResourceConfigDedicated_Switchs()
        {
            RadioResourceConfigDedicated signal = new RadioResourceConfigDedicated(signalString);
            Assert.IsTrue(signal.SrbToAddModListPresent);
            Assert.IsFalse(signal.DrbToAddModListPresent);
            Assert.IsFalse(signal.DrbToReleaseListPresent);
            Assert.IsTrue(signal.MacMainConfigPresent);
            Assert.IsTrue(signal.SpsConfigPresent);
            Assert.IsTrue(signal.PhysicalConfigDedicatedPresent);
            Assert.AreEqual(signal.SrbToAddModListLength, 1);
        }
    }
}
