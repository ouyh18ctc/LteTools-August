using Lte.Domain.TypeDefs;
using NUnit.Framework;

namespace Lte.Domain.Test.TypeDefs
{
    [TestFixture]
    public class FrequencyBandTest
    {
        [Test]
        public void TestFrequencyBand_2100()
        {
            double frequency = 2120;
            Assert.AreEqual(frequency.GetBandFromFrequency(), FrequencyBandType.Downlink2100);
            int earfcn = frequency.GetEarfcn();
            Assert.AreEqual(earfcn, 100);
            Assert.AreEqual(earfcn.GetBandFromFcn(), FrequencyBandType.Downlink2100);
            frequency = earfcn.GetFrequency();
            Assert.AreEqual(frequency, 2120);
        }

        [Test]
        public void TestFrequencyBand_1800()
        {
            double frequency = 1867.5;
            Assert.AreEqual(frequency.GetBandFromFrequency(), FrequencyBandType.Downlink1800);
            int earfcn = frequency.GetEarfcn();
            Assert.AreEqual(earfcn, 1825);
            Assert.AreEqual(earfcn.GetBandFromFcn(), FrequencyBandType.Downlink1800);
            frequency = earfcn.GetFrequency();
            Assert.AreEqual(frequency, 1867.5);
        }
    }
}
