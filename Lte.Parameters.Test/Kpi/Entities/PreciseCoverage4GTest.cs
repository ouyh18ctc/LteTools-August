using System;
using Lte.Parameters.Kpi.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Kpi.Entities
{
    [TestFixture]
    public class PreciseCoverage4GTest
    {
        [TestCase(1, 2, 100, 10, 20, 30, 10, 20, 30)]
        [TestCase(32, 21, 1000, 1, 2, 30, 10, 20, 300)]
        [TestCase(655, 4, 1000, 43, 2.1, 3.5, 430, 21, 35)]
        public void Test_Construction(int cellId, byte sectorId, int totalMrs,
            double thirdRate, double secondRate, double firstRate,
            int thirdCount, int secondCount, int firstCount)
        {
            PreciseCoverage4GCsv info = new PreciseCoverage4GCsv
            {
                CellId = cellId,
                SectorId = sectorId,
                TotalMrs = totalMrs,
                ThirdNeighborRate = thirdRate,
                SecondNeighborRate = secondRate,
                FirstNeighborRate = firstRate,
                StatTime = DateTime.Today
            };
            PreciseCoverage4G resultCoverage4G = new PreciseCoverage4G();
            resultCoverage4G.Import(info);
            Assert.AreEqual(resultCoverage4G.CellId, cellId);
            Assert.AreEqual(resultCoverage4G.SectorId, sectorId);
            Assert.AreEqual(resultCoverage4G.StatTime, DateTime.Today);
            Assert.AreEqual(resultCoverage4G.ThirdNeighbors, thirdCount);
            Assert.AreEqual(resultCoverage4G.SecondNeighbors, secondCount);
            Assert.AreEqual(resultCoverage4G.FirstNeighbors, firstCount);
        }
    }
}
