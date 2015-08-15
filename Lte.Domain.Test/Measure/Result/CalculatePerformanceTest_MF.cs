using System.Collections.Generic;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Result
{
    [TestFixture]
    public class CalculatePerformanceTestMf
    {
        private List<MeasurableCell> _cellList;

        [SetUp]
        public void TestInitialize()
        {
            _cellList = new List<MeasurableCell>{
                new MeasurableCell{ ReceivedRsrp=-70,Cell=new ComparableCell{
                    PciModx=0,Cell=new StubOutdoorCell(1,2){Frequency=100}}},
                new MeasurableCell{ ReceivedRsrp=-80,Cell=new ComparableCell{
                    PciModx=0,Cell=new StubOutdoorCell(1,2){Frequency=100}}},
                new MeasurableCell{ ReceivedRsrp=-80,Cell=new ComparableCell{
                    PciModx=1,Cell=new StubOutdoorCell(1,2){Frequency=100}}},
                new MeasurableCell{ ReceivedRsrp=-80,Cell=new ComparableCell{
                    PciModx=0,Cell=new StubOutdoorCell(1,2){Frequency=1825}}}
            };
        }

        [Test]
        public void TestCalculatePerformance_MFMeasurePointResult()
        {
            MeasurableCellRepository repository = new MeasurableCellRepository
            {
                CellList = _cellList
            };
            MeasurePointResult result = new MfMeasurePointResult();
            result.StrongestCell = repository.CalculateStrongestCell();
            result.CalculateInterference(repository.CellList, 0.1);
            result.NominalSinr = result.StrongestCell.ReceivedRsrp - result.TotalInterferencePower;
            Assert.AreEqual(result.StrongestCell, _cellList[0]);
            Assert.AreEqual(result.StrongestInterference, _cellList[1]);
            Assert.AreEqual(result.SameModInterferenceLevel, -80);
            Assert.AreEqual(result.DifferentModInterferenceLevel, -80);
            Assert.AreEqual(result.TotalInterferencePower, -79.586073, 1E-6);
            Assert.AreEqual(result.NominalSinr, 9.586073, 1E-6);
        }

        [Test]
        public void TestCalculatePerformance_MFMeasurePointResult_NoSameFrequencySameModCell()
        {
            _cellList[1].Cell.Cell.Frequency = 1825;
            MeasurableCellRepository repository = new MeasurableCellRepository
            {
                CellList = _cellList
            };
            MeasurePointResult result = new MfMeasurePointResult();
            result.StrongestCell = repository.CalculateStrongestCell();
            result.CalculateInterference(repository.CellList, 0.1);
            result.NominalSinr = result.StrongestCell.ReceivedRsrp - result.TotalInterferencePower;
            Assert.AreEqual(result.StrongestCell, _cellList[0]);
            Assert.AreEqual(result.StrongestInterference, null);
            Assert.AreEqual(result.SameModInterferenceLevel, double.MinValue);
            Assert.AreEqual(result.DifferentModInterferenceLevel, -80);
            Assert.AreEqual(result.TotalInterferencePower, -90);
            Assert.AreEqual(result.NominalSinr, 20);
        }

        [Test]
        public void TestCalculatePerformance_MFMeasurePointResult_NoSameFrequencySameModCellOrDiffModCell()
        {
            _cellList[1].Cell.Cell.Frequency = 1825;
            _cellList[2].Cell.Cell.Frequency = 1825;
            MeasurableCellRepository repository = new MeasurableCellRepository
            {
                CellList = _cellList
            };
            MeasurePointResult result = new MfMeasurePointResult();
            result.StrongestCell = repository.CalculateStrongestCell();
            result.CalculateInterference(repository.CellList, 0.1);
            result.NominalSinr = result.StrongestCell.ReceivedRsrp - result.TotalInterferencePower;
            Assert.AreEqual(result.StrongestCell, _cellList[0]);
            Assert.AreEqual(result.StrongestInterference, null);
            Assert.AreEqual(result.SameModInterferenceLevel, double.MinValue);
            Assert.AreEqual(result.DifferentModInterferenceLevel, double.MinValue);
            Assert.AreEqual(result.TotalInterferencePower, double.MinValue);
            Assert.AreEqual(result.NominalSinr, double.MaxValue);
        }
    }
}
