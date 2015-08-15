using System;
using System.Collections.Generic;
using Lte.Evaluations.Dingli;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Dingli
{
    [TestFixture]
    public class RateStatMergeTest
    {
        [SetUp]
        public void TestInitialize()
        {
            LogsOperations.RateEvaluationInterval = 1;
        }

        [TearDown]
        public void TestCleanup()
        {
            LogsOperations.RateEvaluationInterval = 0.5;
        }

        [Test]
        public void TestRateStatMerge_ContinuousTime()
        {
            List<RateStat> stats = new List<RateStat>{
                new RateStat{ Time=new DateTime(2013,1,1,10,10,24,233),
                    Rsrp=-101},
                new RateStat{ Time=new DateTime(2013,1,1,10,10,25,283),
                    Rsrp=-102},
                new RateStat{ Time=new DateTime(2013,1,1,10,10,25,338),
                    Rsrp=-102},
                new RateStat{ Time=new DateTime(2013,1,1,10,10,26,278),
                    Rsrp=-101}
            };
            List<BasicRateStat> basicStats = stats.Merge();
            Assert.AreEqual(basicStats.Count, 3);
            Assert.AreEqual(basicStats[0].Rsrp, -101);
            Assert.AreEqual(basicStats[1].Rsrp, -102);
            Assert.AreEqual(basicStats[2].Rsrp, -101);
        }

        [Test]
        public void TestRateStatMerge_DiscontinuousTime()
        {
            List<RateStat> stats = new List<RateStat>{
                new RateStat{ Time=new DateTime(2013,1,1,10,10,24,233),
                    Rsrp=-101},
                new RateStat{ Time=new DateTime(2013,1,1,10,10,26,333),
                    Rsrp=-102},
                new RateStat{ Time=new DateTime(2013,1,1,10,10,27,188),
                    Rsrp=-102},
                new RateStat{ Time=new DateTime(2013,1,1,10,10,28,278),
                    Rsrp=-101}
            };
            List<BasicRateStat> basicStats = stats.Merge();
            Assert.AreEqual(basicStats.Count, 3);
            Assert.AreEqual(basicStats[0].Rsrp, -101);
            Assert.AreEqual(basicStats[1].Rsrp, -102);
            Assert.AreEqual(basicStats[2].Rsrp, -101);
        }
    }
}
