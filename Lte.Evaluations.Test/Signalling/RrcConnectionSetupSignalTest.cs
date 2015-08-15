using Lte.Evaluations.Signalling;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Signalling
{
    [TestFixture]
    public class RrcConnectionSetupSignalTest
    {
        private const string signalString = "6813980a1dce0183c0ba007e131ffa211f0c288d980002e808000960";

        [Test]
        public void TestRrcConnectionSetupSignal_BasicParameters()
        {
            RrcConnectionSetupSignal signal = new RrcConnectionSetupSignal(signalString);
            Assert.AreEqual(signal.RrcTransactionIdentifier, 1);
            Assert.IsNotNull(signal.RadioResourceConfigDedicated);
            Assert.AreEqual(signalString.Substring(2, signalString.Length - 2),
                "13980a1dce0183c0ba007e131ffa211f0c288d980002e808000960");
        }
    }
}
