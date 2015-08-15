using Lte.Evaluations.Signalling;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Signalling
{
    [TestFixture]
    public class RrcConnectionRequestSignalTest
    {
        [Test]
        public void TestRrcConnectionRequestSignal_MoData()
        {
            RrcConnectionRequestSignal signal = new RrcConnectionRequestSignal("401cf0b88d58");
            Assert.IsTrue(signal.STmsiIncluded);
            Assert.AreEqual(signal.MmeCode, 1);
            Assert.AreEqual(signal.STmsi, "cf0b88d5");
            Assert.AreEqual(signal.EstablishmentCause, RrcConnectionEstablishmentCause.MoData);
        }

        [Test]
        public void TestRrcConnectionRequestSignal_MtAccess()
        {
            RrcConnectionRequestSignal signal = new RrcConnectionRequestSignal("401cf0f90254");
            Assert.IsTrue(signal.STmsiIncluded);
            Assert.AreEqual(signal.MmeCode, 1);
            Assert.AreEqual(signal.STmsi, "cf0f9025");
            Assert.AreEqual(signal.EstablishmentCause, RrcConnectionEstablishmentCause.MtAccess);
        }

        [Test]
        public void TestRrcConnectionRequestSignal_MtAccess_2()
        {
            RrcConnectionRequestSignal signal = new RrcConnectionRequestSignal("401e20386e94");
            Assert.IsTrue(signal.STmsiIncluded);
            Assert.AreEqual(signal.MmeCode, 1);
            Assert.AreEqual(signal.STmsi, "e20386e9");
            Assert.AreEqual(signal.EstablishmentCause, RrcConnectionEstablishmentCause.MtAccess);
        }
    }
}
