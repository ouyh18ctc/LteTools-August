using System.Collections.Generic;
using Lte.Parameters.Kpi.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Kpi
{
    [TestFixture]
    public class DistanceDistributionTest
    {
        private List<DistanceDistribution> result;

        [Test]
        public void TestImport_AllInfos_Null()
        {
            result = new List<DistanceDistribution>();
            result.Import(null, null, null, null);
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestImport_CdrCalls()
        {
            CdrCallsDistanceInfo info = new CdrCallsDistanceInfo
            {
                DistanceTo1000Info = 1233,
                DistanceTo1600Info = 2344,
                DistanceTo2200Info = 3765
            };
            result = new List<DistanceDistribution>();
            result.Import(info, null, null, null);
            Assert.AreEqual(result.Count, 22);
            Assert.AreEqual(result[4].CdrCalls, 1233);
            Assert.AreEqual(result[7].CdrCalls, 2344);
            Assert.AreEqual(result[10].CdrCalls, 3765);
        }
    }
}
