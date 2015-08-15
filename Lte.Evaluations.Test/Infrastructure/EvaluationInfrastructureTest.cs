using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Entities;
using Lte.Parameters.Entities;
using Lte.Evaluations.Infrastructure;
using Lte.Domain.Measure;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Infrastructure
{
    [TestFixture]
    public class EvaluationInfrastructureTest
    {
        private EvaluationInfrastructure infrastructure;
        private List<EvaluationOutdoorCell> cellList;
        private const double eps = 1E-6;

        [SetUp]
        public void TestInitialize()
        {
            cellList = new List<EvaluationOutdoorCell>();
            cellList.Add(new EvaluationOutdoorCell
            {
                Pci = 0,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.001,
                Lattitute = 23.001,
                Azimuth = 60,
                Height = 10
            });
            cellList.Add(new EvaluationOutdoorCell
            {
                Pci = 1,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.001,
                Lattitute = 23.001,
                Azimuth = 180,
                Height = 10
            });
            cellList.Add(new EvaluationOutdoorCell 
            {
                Pci = 2,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.001,
                Lattitute = 23.001,
                Azimuth = 300,
                Height = 10
            });
            cellList.Add(new EvaluationOutdoorCell
            {
                Pci = 3,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.002,
                Lattitute = 23.00,
                Azimuth = 60,
                Height = 10
            });
            cellList.Add(new EvaluationOutdoorCell 
            {
                Pci = 4,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.002,
                Lattitute = 23.002,
                Azimuth = 180,
                Height = 10
            });
            cellList.Add(new EvaluationOutdoorCell 
            {
                Pci = 5,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.002,
                Lattitute = 23.002,
                Azimuth = 300,
                Height = 10
            });
        }

        [Test]
        public void TestEvaluationInfrastructure_DefaultConstructor()
        {
            infrastructure = new EvaluationInfrastructure(
                new GeoPoint(113, 23), new GeoPoint(113.003, 23.003), cellList);
            Assert.IsNotNull(infrastructure);
            Assert.IsNotNull(infrastructure.MeasurePointList);
            Assert.IsNotNull(infrastructure.Region);
            Assert.AreEqual(infrastructure.Region.Length, 49, "region length");
            Assert.AreEqual(infrastructure.MeasurePointList.Count(), 49, "points");
            Assert.IsNotNull(infrastructure.Region[5].Result);
            Assert.AreEqual(infrastructure.Region[5].Result.NominalSinr, double.MinValue);
            Assert.AreEqual(infrastructure.Region[5].CellRepository.CellList.Count, 6);
            Assert.AreEqual(infrastructure.Region[5].CellRepository.CellList[0].Cell.Cell.Longtitute, 113.001);
        }

        [Test]
        public void TestEvaluationInfrastructure_DefaultConstructor_CalculatePerformance()
        {
            infrastructure = new EvaluationInfrastructure(
                new GeoPoint(113, 23), new GeoPoint(113.003, 23.003), cellList);
            infrastructure.Region.CalculatePerformance(0.1);
            Assert.IsTrue(infrastructure.Region[5].Result.NominalSinr > 2);
            Assert.IsTrue(infrastructure.Region[7].Result.NominalSinr > 12);
            MeasurePoint point16 = infrastructure.MeasurePointList[16];
            Assert.AreEqual(point16.Longtitute, 113.000899, eps);
            Assert.AreEqual(point16.Lattitute, 23.000899, eps);
            Assert.IsTrue(point16.Result.NominalSinr > 15);
            MeasurePoint point24 = infrastructure.MeasurePointList[24];
            Assert.AreEqual(point24.Longtitute, 113.001349, eps);
            Assert.AreEqual(point24.Lattitute, 23.001349, eps);
            Assert.IsTrue(point24.Result.NominalSinr > 21);
            IEnumerable<MeasurePoint> orderedList = infrastructure.MeasurePointList.OrderByDescending(
                x => x.Result.NominalSinr);
            MeasurePoint point = orderedList.ElementAt(0);
            Assert.IsTrue(point.Result.NominalSinr > 28);
            Assert.AreEqual(point.Longtitute, 113.0022483, eps);
            Assert.AreEqual(point.Lattitute, 23, eps);
        }

        [Test]
        public void TestEvaluationInfrastructure_CellConstructor()
        {
            infrastructure = new EvaluationInfrastructure();
            infrastructure.ImportCellList(cellList);
            Assert.IsNotNull(infrastructure);
            Assert.IsNotNull(infrastructure.MeasurePointList);
            Assert.IsNotNull(infrastructure.Region);
            Assert.AreEqual(infrastructure.Region.Length, 4956);
            Assert.AreEqual(infrastructure.MeasurePointList.Count(), 4956);
            Assert.IsNotNull(infrastructure.Region[5].Result);
            Assert.AreEqual(infrastructure.Region[5].Result.NominalSinr, double.MinValue);
            Assert.AreEqual(infrastructure.Region[5].CellRepository.CellList.Count, 6);
            Assert.AreEqual(infrastructure.Region[5].CellRepository.CellList[0].Cell.Cell.Longtitute, 113.001);
            Assert.AreEqual(infrastructure.Region.DegreeInterval, 0.00045, eps);
        }

        [Test]
        public void TestEvaluationInfrastructure_CellConstructor_CalculatePerformance()
        {
            infrastructure = new EvaluationInfrastructure();
            infrastructure.ImportCellList(cellList);
            infrastructure.Region.CalculatePerformance(0.1);
            Assert.IsTrue(infrastructure.Region[5].Result.NominalSinr > 1);
            Assert.IsTrue(infrastructure.Region[7].Result.NominalSinr > 1);
            IEnumerable<MeasurePoint> orderedList = infrastructure.MeasurePointList.OrderByDescending(
                x => x.Result.NominalSinr);
            MeasurePoint point = orderedList.ElementAt(0);
            Assert.IsTrue(point.Result.NominalSinr > 30);
            Assert.IsTrue(point.Result.StrongestCell.ReceivedRsrp > -65);
            Assert.IsTrue(point.Result.StrongestCell.DistanceInMeter < 30);
            Assert.AreEqual(point.CellRepository.CellList.Count, 6);
            Assert.AreEqual(point.CellRepository.CellList[0].ReceivedRsrp, point.Result.StrongestCell.ReceivedRsrp, eps);
           
            Assert.AreEqual(point.CellRepository.CellList[0].PciModx, 2);
        }
    }
}
