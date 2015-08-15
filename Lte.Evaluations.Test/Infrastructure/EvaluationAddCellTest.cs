using System.Linq;
using Lte.Parameters.Entities;
using Lte.Evaluations.Infrastructure;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Infrastructure
{
    [TestFixture]
    public class EvaluationAddCellTest
    {
        private EvaluationInfrastructure infrastructure;

        [SetUp]
        public void SetUp()
        {
            infrastructure = new EvaluationInfrastructure();
        }

        private void TestRegionAndMeasurePointListWithValidCells()
        {
            Assert.IsTrue(infrastructure.Region.Length > 4000);
            Assert.AreEqual(infrastructure.MeasurePointList.Count(), infrastructure.Region.Length);
        }

        [Test]
        public void TestEvaluationAddCell_AddCell_WithoutInitailizingRegion()
        {
            infrastructure.AddCell(new EvaluationOutdoorCell
            {
                Pci = 0,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825, 
                Longtitute = 113.001,
                Lattitute = 23.001,
                Azimuth = 60,
                Height = 20
            });
            TestRegionAndMeasurePointListWithValidCells();
            Assert.AreEqual(infrastructure.Region[5].CellRepository.CellList.Count, 1);
            infrastructure.AddCell(new EvaluationOutdoorCell
            {
                Pci = 0,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.001,
                Lattitute = 23.001,
                Azimuth = 60,
                Height = 20
            });
            TestRegionAndMeasurePointListWithValidCells();
            Assert.AreEqual(infrastructure.Region[5].CellRepository.CellList.Count, 1);
            infrastructure.AddCell(new EvaluationOutdoorCell
            {
                Pci = 0,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.001,
                Lattitute = 23.001,
                Azimuth = 180,
                Height = 20
            });
            TestRegionAndMeasurePointListWithValidCells();
            Assert.AreEqual(infrastructure.Region[5].CellRepository.CellList.Count, 2);
            infrastructure.AddCell(new EvaluationOutdoorCell
            {
                Pci = 0,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.001,
                Lattitute = 23.002,
                Azimuth = 180,
                Height = 20
            });
            TestRegionAndMeasurePointListWithValidCells();
            Assert.AreEqual(infrastructure.Region[5].CellRepository.CellList.Count, 3);
        }

        [Test]
        public void TestEvaluationAddCell_AddFourCells_WithInitailizingRegion()
        {
            infrastructure.AddCell(new EvaluationOutdoorCell
            {
                Pci = 0,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.001,
                Lattitute = 23.001,
                Azimuth = 60,
                Height = 20
            });
            infrastructure.InitializeRegion();
            TestRegionAndMeasurePointListWithValidCells();
            Assert.AreEqual(infrastructure.Region[5].CellRepository.CellList.Count, 1);
            infrastructure.AddCell(new EvaluationOutdoorCell
            {
                Pci = 0,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.001,
                Lattitute = 23.001,
                Azimuth = 60,
                Height = 20
            });
            infrastructure.InitializeRegion();
            TestRegionAndMeasurePointListWithValidCells();
            Assert.AreEqual(infrastructure.Region[5].CellRepository.CellList.Count, 1);
            infrastructure.AddCell(new EvaluationOutdoorCell
            {
                Pci = 0,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.001,
                Lattitute = 23.001,
                Azimuth = 180,
                Height = 20
            });
            infrastructure.InitializeRegion();
            TestRegionAndMeasurePointListWithValidCells();
            Assert.AreEqual(infrastructure.Region[5].CellRepository.CellList.Count, 2);
            infrastructure.AddCell(new EvaluationOutdoorCell
            {
                Pci = 0,
                AntennaGain = 18,
                RsPower = 16.2,
                Frequency = 1825,
                Longtitute = 113.001,
                Lattitute = 23.002,
                Azimuth = 180,
                Height = 20
            });
            infrastructure.InitializeRegion();
            TestRegionAndMeasurePointListWithValidCells();
            Assert.AreEqual(infrastructure.Region[5].CellRepository.CellList.Count, 3);
        }
    }
}
