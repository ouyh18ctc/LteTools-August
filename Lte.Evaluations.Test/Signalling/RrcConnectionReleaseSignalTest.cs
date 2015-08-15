using Lte.Evaluations.Signalling;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Signalling
{
    [TestFixture]
    public class RrcConnectionReleaseSignalTest
    {
        [Test]
        public void TestRrcConnectionReleaseSignal_2a02()
        {
            RrcConnectionReleaseSignal signal = new RrcConnectionReleaseSignal("2a02");
            Assert.AreEqual(signal.RrcTransactionIdentifier, 1);
            Assert.AreEqual(signal.ReleaseCause, RrcConnectionReleaseCause.Other);
        }

        [Test]
        public void TestRrcConnectionReleaseSignal_2c02()
        {
            RrcConnectionReleaseSignal signal = new RrcConnectionReleaseSignal("2c02");
            Assert.AreEqual(signal.RrcTransactionIdentifier, 2);
            Assert.AreEqual(signal.ReleaseCause, RrcConnectionReleaseCause.Other);
        }
    }
}
