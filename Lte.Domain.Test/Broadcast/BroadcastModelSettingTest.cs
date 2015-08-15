using Lte.Domain.Measure;
using Lte.Domain.TypeDefs;
using NUnit.Framework;

namespace Lte.Domain.Test.Broadcast
{
    [TestFixture]
    public class BroadcastModelSettingTest
    {
        private IBroadcastModel model;

        [SetUp]
        public void TestInitialize()
        {
            model = new BroadcastModel();
        }

        [Test]
        public void Test_ModelIsNotNull()
        {
            Assert.IsNotNull(model);
            model = new BroadcastModel(FrequencyBandType.Downlink1800, UrbanType.Middle);
            Assert.IsNotNull(model);
        }

        [Test]
        public void Test_Contruct_Kvalue()
        {
            Assert.AreEqual(model.K1, 69.55);
            Assert.AreEqual(model.K4, 44.9);
            model = new BroadcastModel(utype: UrbanType.Middle);
            Assert.AreNotEqual(model.K1, 85.83);
            Assert.AreNotEqual(model.K4, 60);
            Assert.AreEqual(model.K1, 69.55);
            Assert.AreEqual(model.K4, 44.9);
            model = new BroadcastModel(utype: UrbanType.Dense);
            Assert.AreEqual(model.K1, 85.83);
            Assert.AreEqual(model.K4, 60);
        }

        [Test]
        public void Test_Contruct_Frequency()
        {
            Assert.AreEqual(model.Frequency, 2120);
            model = new BroadcastModel(FrequencyBandType.Downlink1800);
            Assert.AreEqual(model.Frequency, 1860);
            model = new BroadcastModel(FrequencyBandType.Uplink2100);
            Assert.AreEqual(model.Frequency, 1930);
            model = new BroadcastModel(FrequencyBandType.Uplink1800);
            Assert.AreEqual(model.Frequency, 1765);
            model = new BroadcastModel(FrequencyBandType.Tdd2600);
            Assert.AreEqual(model.Frequency, 2645);
        }

        [Test]
        public void Test_Construct_Earfcn()
        {
            model = new BroadcastModel(100);
            Assert.AreEqual(model.Frequency, 2120);
            model = new BroadcastModel(1825);
            Assert.AreEqual(model.Frequency, 1867.5);
            model = new BroadcastModel(1750);
            Assert.AreEqual(model.Frequency, 1860);
        }

        [Test]
        public void Test_SetKvalue()
        {
            Assert.AreEqual(model.UrbanType, UrbanType.Large);
            model.SetKvalue(UrbanType.Dense);
            Assert.AreEqual(model.UrbanType, UrbanType.Dense);
            Assert.AreEqual(model.K1, 85.83);
            Assert.AreEqual(model.K4, 60);
            model.SetKvalue(UrbanType.Middle);
            Assert.AreEqual(model.UrbanType, UrbanType.Middle);
            Assert.AreEqual(model.K1, 69.55);
            Assert.AreEqual(model.K4, 44.9);
        }

        [Test]
        public void Test_SetFrequency()
        {
            model.SetFrequencyBand(FrequencyBandType.Downlink1800);
            Assert.AreEqual(model.Frequency, 1860);
            model.SetFrequencyBand(FrequencyBandType.Uplink2100);
            Assert.AreEqual(model.Frequency, 1930);
            model.SetFrequencyBand(FrequencyBandType.Tdd2600);
            Assert.AreEqual(model.Frequency, 2645);
        }
    }
}
