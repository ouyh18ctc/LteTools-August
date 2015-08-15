using Lte.Parameters.Kpi.Abstract;
using NUnit.Framework;

namespace Lte.Parameters.Test.Kpi
{
    public static class Drop2GInfoTestHelper
    {
        public static void AssertDistanceTest<TValue>(this IDrop2GDistanceInfo<TValue> info, TValue[] expectedValues)
        {
            Assert.IsNotNull(info);
            Assert.AreEqual(info.DistanceTo200Info, expectedValues[0]);
            Assert.AreEqual(info.DistanceTo400Info, expectedValues[1]);
            Assert.AreEqual(info.DistanceTo600Info, expectedValues[2]);
            Assert.AreEqual(info.DistanceTo800Info, expectedValues[3]);
            Assert.AreEqual(info.DistanceTo1000Info, expectedValues[4]);
            Assert.AreEqual(info.DistanceTo1200Info, expectedValues[5]);
            Assert.AreEqual(info.DistanceTo1400Info, expectedValues[6]);
            Assert.AreEqual(info.DistanceTo1600Info, expectedValues[7]);
            Assert.AreEqual(info.DistanceTo1800Info, expectedValues[8]);
            Assert.AreEqual(info.DistanceTo2000Info, expectedValues[9]);
            Assert.AreEqual(info.DistanceTo2200Info, expectedValues[10]);
            Assert.AreEqual(info.DistanceTo2400Info, expectedValues[11]);
            Assert.AreEqual(info.DistanceTo2600Info, expectedValues[12]);
            Assert.AreEqual(info.DistanceTo2800Info, expectedValues[13]);
            Assert.AreEqual(info.DistanceTo3000Info, expectedValues[14]);
            Assert.AreEqual(info.DistanceTo4000Info, expectedValues[15]);
            Assert.AreEqual(info.DistanceTo5000Info, expectedValues[16]);
            Assert.AreEqual(info.DistanceTo6000Info, expectedValues[17]);
            Assert.AreEqual(info.DistanceTo7000Info, expectedValues[18]);
            Assert.AreEqual(info.DistanceTo8000Info, expectedValues[19]);
            Assert.AreEqual(info.DistanceTo9000Info, expectedValues[20]);
            Assert.AreEqual(info.DistanceToInfInfo, expectedValues[21]);
        }

        public static void AssertHourTest<TValue>(this IDrop2GHourInfo<TValue> info, TValue[] expectedValues)
        {
            Assert.IsNotNull(info);
            Assert.AreEqual(info.Hour0Info, expectedValues[0]);
            Assert.AreEqual(info.Hour1Info, expectedValues[1]);
            Assert.AreEqual(info.Hour2Info, expectedValues[2]);
            Assert.AreEqual(info.Hour3Info, expectedValues[3]);
            Assert.AreEqual(info.Hour4Info, expectedValues[4]);
            Assert.AreEqual(info.Hour5Info, expectedValues[5]);
            Assert.AreEqual(info.Hour6Info, expectedValues[6]);
            Assert.AreEqual(info.Hour7Info, expectedValues[7]);
            Assert.AreEqual(info.Hour8Info, expectedValues[8]);
            Assert.AreEqual(info.Hour9Info, expectedValues[9]);
            Assert.AreEqual(info.Hour10Info, expectedValues[10]);
            Assert.AreEqual(info.Hour11Info, expectedValues[11]);
            Assert.AreEqual(info.Hour12Info, expectedValues[12]);
            Assert.AreEqual(info.Hour13Info, expectedValues[13]);
            Assert.AreEqual(info.Hour14Info, expectedValues[14]);
            Assert.AreEqual(info.Hour15Info, expectedValues[15]);
            Assert.AreEqual(info.Hour16Info, expectedValues[16]);
            Assert.AreEqual(info.Hour17Info, expectedValues[17]);
            Assert.AreEqual(info.Hour18Info, expectedValues[18]);
            Assert.AreEqual(info.Hour19Info, expectedValues[19]);
            Assert.AreEqual(info.Hour20Info, expectedValues[20]);
            Assert.AreEqual(info.Hour21Info, expectedValues[21]);
            Assert.AreEqual(info.Hour22Info, expectedValues[22]);
            Assert.AreEqual(info.Hour23Info, expectedValues[23]);
        }
    }
}
