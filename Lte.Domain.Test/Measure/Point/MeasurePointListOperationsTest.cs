using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Measure;
using Lte.Domain.TypeDefs;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Point
{
    [TestFixture]
    public class MeasurePointListOperationsTest
    {
        private readonly IList<MeasurePoint> mesurePointList = new List<MeasurePoint>();

        [SetUp]
        public void TestInitialize()
        {
            MeasurePoint point = new MeasurePoint();
            point.MockMeasurePointProperties(-33, -105, -110, -78);
            mesurePointList.Add(point);
            point = new MeasurePoint();
            point.MockMeasurePointProperties(-28, -3000, -110, -98);
            mesurePointList.Add(point);
            point = new MeasurePoint();
            point.MockMeasurePointProperties(-26, -2000, -300, -88);
            mesurePointList.Add(point);
            point = new MeasurePoint();
            point.MockMeasurePointProperties(-20, -199, -400, -301);
            mesurePointList.Add(point);
        }

        [Test]
        public void TestMeasurePointListOperations_IntializeProperties()
        {
            Assert.AreEqual(4, mesurePointList.Count);
            Assert.AreEqual(-33, mesurePointList[0].Result.NominalSinr);
            Assert.AreEqual(-3000, mesurePointList[1].Result.StrongestCell.ReceivedRsrp);
            Assert.AreEqual(-300, mesurePointList[2].Result.StrongestInterference.ReceivedRsrp);
            Assert.AreEqual(-301, mesurePointList[3].Result.TotalInterferencePower);
        }

        [Test]
        public void TestMeasurePointListOperations_FilterNormalPoints_DefaultFilter_NominalSinr()
        {
            IEnumerable<MeasurePoint> resultList = 
                mesurePointList.FilterNormalPoints(MeasurePointKpiSelection.NominalSinr);
            Assert.AreEqual(resultList.Count(), 3);
            Assert.AreEqual(resultList.ElementAt(0).Result.StrongestCell.ReceivedRsrp, -3000);
        }

        [Test]
        public void TestMeasurePointListOperations_FilterNormalPoints_DefaultFilter_StrongestCellRsrp()
        {
            IEnumerable<MeasurePoint> resultList =
                mesurePointList.FilterNormalPoints(MeasurePointKpiSelection.StrongestCellRsrp);
            Assert.AreEqual(resultList.Count(), 2);
            Assert.AreEqual(resultList.ElementAt(1).Result.NominalSinr, -20);
        }

        [Test]
        public void TestMeasurePointListOperations_FilterNormalPoints_DefaultFilter_StrongestInterferenceRsrp()
        {
            IEnumerable<MeasurePoint> resultList =
                mesurePointList.FilterNormalPoints(MeasurePointKpiSelection.StrongestInterferenceRsrp);
            Assert.AreEqual(resultList.Count(), 2);
            Assert.AreEqual(resultList.ElementAt(1).Result.TotalInterferencePower, -98);
        }

        [Test]
        public void TestMeasurePointListOperations_FilterNormalPoints_CustomFilter_StrongestInterferenceRsrp()
        {
            double[] filterThreshold = { -30, -200, -301, -200 };
            IEnumerable<MeasurePoint> resultList =
                mesurePointList.FilterNormalPoints(MeasurePointKpiSelection.StrongestInterferenceRsrp,
                filterThreshold);
            Assert.AreEqual(resultList.Count(), 3);
            Assert.AreEqual(resultList.ElementAt(1).Result.TotalInterferencePower, -98);
        }

        [Test]
        public void TestMeasurePointListOperations_FilterNormalPoints_CustomFilter_TotalInterferencePower()
        {
            double[] filterThreshold = { -30, -200, -301, -200 };
            IEnumerable<MeasurePoint> resultList =
                mesurePointList.FilterNormalPoints(MeasurePointKpiSelection.TotalInterferencePower,
                filterThreshold);
            Assert.AreEqual(resultList.Count(), 3);
            Assert.AreEqual(resultList.ElementAt(2).Result.StrongestInterference.ReceivedRsrp, -300);
        }
    }
}
