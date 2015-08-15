using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;
using NUnit.Framework;

namespace Lte.Parameters.Test.Process
{
    [TestFixture]
    public class CoverageAdjustmentMergeListTest
    {
        private List<CoverageAdjustment> adjustments;

        private void Initialize()
        {
            adjustments = new List<CoverageAdjustment>
            {
                new CoverageAdjustment
                {
                    ENodebId = 1,
                    SectorId = 1,
                    Frequency = 100,
                    Factor15 = 3
                }
            };
        }

        private void AddOneSameCell()
        {
            adjustments.Add(
                new CoverageAdjustment
                {
                    ENodebId = 1,
                    SectorId = 1,
                    Frequency = 100,
                    Factor15 = 5
                }
            );
        }

        private void AddOneDifferentCell()
        {
            adjustments.Add(
                new CoverageAdjustment
                {
                    ENodebId = 2,
                    SectorId = 1,
                    Frequency = 100,
                    Factor15 = 5
                }
            );
        }

        [Test]
        public void TestCoverageAdjustmentMergeList_OnlyOneElement()
        {
            Initialize();
            IEnumerable<CoverageAdjustment> results = adjustments.MergeList();
            Assert.AreEqual(results.Count(), 1);
            Assert.AreEqual(results.ElementAt(0).Factor15, 3);
            Assert.AreEqual(results.ElementAt(0).ENodebId, 1);
            Assert.AreEqual(results.ElementAt(0).SectorId, 1);
            Assert.AreEqual(results.ElementAt(0).Frequency, 100);
        }

        [Test]
        public void TestCoverageAdjustmentMergeList_TwoSameCells()
        {
            Initialize();
            AddOneSameCell();
            IEnumerable<CoverageAdjustment> results = adjustments.MergeList();
            Assert.AreEqual(results.Count(), 1);
            Assert.AreEqual(results.ElementAt(0).Factor15, 4);
            Assert.AreEqual(results.ElementAt(0).ENodebId, 1);
            Assert.AreEqual(results.ElementAt(0).SectorId, 1);
            Assert.AreEqual(results.ElementAt(0).Frequency, 100);
        }

        [Test]
        public void TestCoverageAdjustmentMergeList_TwoDifferentCells()
        {
            Initialize();
            AddOneDifferentCell();
            IEnumerable<CoverageAdjustment> results = adjustments.MergeList();
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results.ElementAt(0).Factor15, 3);
            Assert.AreEqual(results.ElementAt(0).ENodebId, 1);
            Assert.AreEqual(results.ElementAt(0).SectorId, 1);
            Assert.AreEqual(results.ElementAt(0).Frequency, 100);
            Assert.AreEqual(results.ElementAt(1).Frequency, 100);
            Assert.AreEqual(results.ElementAt(1).ENodebId, 2);
            Assert.AreEqual(results.ElementAt(1).Factor15, 5);
        }

        [Test]
        public void TestCoverageAdjustmentMergeList_ThreeCells_WithOneDiffent()
        {
            Initialize();
            AddOneDifferentCell();
            AddOneSameCell();
            IEnumerable<CoverageAdjustment> results = adjustments.MergeList();
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results.ElementAt(0).Factor15, 4);
            Assert.AreEqual(results.ElementAt(0).ENodebId, 1);
            Assert.AreEqual(results.ElementAt(0).SectorId, 1);
            Assert.AreEqual(results.ElementAt(0).Frequency, 100);
            Assert.AreEqual(results.ElementAt(1).Frequency, 100);
            Assert.AreEqual(results.ElementAt(1).ENodebId, 2);
            Assert.AreEqual(results.ElementAt(1).Factor15, 5);
        }
    }
}
