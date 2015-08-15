using System;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Moq;
using NUnit.Framework;

namespace Lte.Domain.Test.Measure.Point
{
    [TestFixture]
    public class CalculatePerformanceTest : ImportCellTestConfig
    {
        [SetUp]
        public void SetUp()
        {
            Initialize();
        }

        [Test]
        public void TestCalculatePerformance_OneCell()
        {
            ImportOneCell();
            measurablePoint.CalculatePerformance(0.1);

            Assert.AreEqual(measurablePoint.Result.StrongestCell.Cell.Cell, outdoorCellList[0]);
            Assert.IsNull(measurablePoint.Result.StrongestInterference);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(0), -136.877442, eps);
            Assert.AreEqual(measurablePoint.Result.SameModInterferenceLevel, Double.MinValue);
            Assert.AreEqual(measurablePoint.Result.DifferentModInterferenceLevel, Double.MinValue);
            Assert.AreEqual(measurablePoint.Result.TotalInterferencePower, Double.MinValue);
            Assert.AreEqual(measurablePoint.Result.NominalSinr, 100);
        }

        [Test]
        public void TestCalculatePerformance_TwoCells_InOneStation_WithSamePci()
        {
            ImportTwoCellsInOneStation();
            measurablePoint.CalculatePerformance(0.1);

            Assert.AreEqual(measurablePoint.Result.StrongestCell.Cell.Cell, outdoorCellList[1]);
            Assert.AreEqual(measurablePoint.Result.StrongestInterference.Cell.Cell, outdoorCellList[0]);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(0), -113.512679, eps);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(1), -136.877442, eps);
            //Assert.AreEqual(measurablePoint.Result.SameModInterferenceLevel, -136.877442, eps);
            Assert.AreEqual(measurablePoint.Result.DifferentModInterferenceLevel, Double.MinValue);
            //Assert.AreEqual(measurablePoint.Result.TotalInterferencePower, -136.877442, eps);
            Assert.AreEqual(measurablePoint.Result.NominalSinr, 0, eps);
        }

        [Test]
        public void TestCalculatePerformance_TwoCells_InOneStation_WithDifferentPcis()
        {
            ImportTwoCellsInOneStation_WithDifferentMods();
            measurablePoint.CalculatePerformance(0.1);

            Assert.AreEqual(measurablePoint.Result.StrongestCell.Cell.Cell, outdoorCellList[1]);
            Assert.IsNull(measurablePoint.Result.StrongestInterference);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(0), -113.512679, eps);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(1), -136.877442, eps);
            Assert.AreEqual(measurablePoint.Result.SameModInterferenceLevel, Double.MinValue);
            //Assert.AreEqual(measurablePoint.Result.DifferentModInterferenceLevel, -136.877442, eps);
            //Assert.AreEqual(measurablePoint.Result.TotalInterferencePower, -146.877442, eps);
            Assert.AreEqual(measurablePoint.Result.NominalSinr, 10, eps);
        }

        [Test]
        public void TestCalculatePerformance_ThreeCells_InOneStation_WithSameMods()
        {
            ImportThreeCellsInOneStation();
            measurablePoint.CalculatePerformance(0.1);

            Assert.AreEqual(measurablePoint.Result.StrongestCell.Cell.Cell, outdoorCellList[2]);
            Assert.AreEqual(measurablePoint.Result.StrongestInterference.Cell.Cell, outdoorCellList[1]);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(0), -106.877442, eps);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(1), -113.512679, eps);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(2), -136.877442, eps);
            //Assert.AreEqual(measurablePoint.Result.SameModInterferenceLevel, -113.492713, eps);
            Assert.AreEqual(measurablePoint.Result.DifferentModInterferenceLevel, Double.MinValue);
            //Assert.AreEqual(measurablePoint.Result.TotalInterferencePower, -113.492713, eps);
            Assert.AreEqual(measurablePoint.Result.NominalSinr, -3.0103, eps);
        }

        protected void ImportThreeCellsInOneStation_AllInterferenceWithDifferentModsFromStrongestCell()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18, 1);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18, 1);
            Mock<IOutdoorCell> outdoorCell3 = new Mock<IOutdoorCell>();
            outdoorCell3.MockOutdoorCell(112, 23, 90, 15.2, 18);
            outdoorCellList.Add(outdoorCell1.Object);
            outdoorCellList.Add(outdoorCell2.Object);
            outdoorCellList.Add(outdoorCell3.Object);

            measurablePoint.ImportCells(outdoorCellList, budgetList, model);
        }

        [Test]
        public void TestCalculatePerformance_ThreeCells_InOneStation_AllInterferenceWithDifferentMods()
        {
            ImportThreeCellsInOneStation_AllInterferenceWithDifferentModsFromStrongestCell();
            measurablePoint.CalculatePerformance(0.1);

            Assert.AreEqual(measurablePoint.Result.StrongestCell.Cell.Cell, outdoorCellList[2]);
            Assert.IsNull(measurablePoint.Result.StrongestInterference);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(0), -106.877442, eps);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(1), -113.512679, eps);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(2), -136.877442, eps);
            Assert.AreEqual(measurablePoint.Result.SameModInterferenceLevel, Double.MinValue);
            //Assert.AreEqual(measurablePoint.Result.DifferentModInterferenceLevel, -113.492713, eps);
            //Assert.AreEqual(measurablePoint.Result.TotalInterferencePower, -123.492713, eps);
            Assert.AreEqual(measurablePoint.Result.NominalSinr, 6.9897, eps);
        }

        protected void ImportThreeCellsInOneStation_OneInterferenceSameMod_OtherInterferenceDifferentMod()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18, 1);
            Mock<IOutdoorCell> outdoorCell3 = new Mock<IOutdoorCell>();
            outdoorCell3.MockOutdoorCell(112, 23, 90, 15.2, 18);
            outdoorCellList.Add(outdoorCell1.Object);
            outdoorCellList.Add(outdoorCell2.Object);
            outdoorCellList.Add(outdoorCell3.Object);

            measurablePoint.ImportCells(outdoorCellList, budgetList, model);
        }

        [Test]
        public void TestCalculatePerformance_ThreeCells_InOneStation_OneInterferenceSameMod_OtherDifferentMod()
        {
            ImportThreeCellsInOneStation_OneInterferenceSameMod_OtherInterferenceDifferentMod();
            measurablePoint.CalculatePerformance(0.1);

            Assert.AreEqual(measurablePoint.Result.StrongestCell.Cell.Cell, outdoorCellList[2]);
            Assert.AreEqual(measurablePoint.Result.StrongestInterference.Cell.Cell, outdoorCellList[0]);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(0), -106.877442, eps);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(1), -113.512679, eps);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(2), -136.877442, eps);
            //Assert.AreEqual(measurablePoint.Result.SameModInterferenceLevel, -136.877442, eps);
            //Assert.AreEqual(measurablePoint.Result.DifferentModInterferenceLevel, -113.512679, eps);
            //Assert.AreEqual(measurablePoint.Result.TotalInterferencePower, -123.317025, eps);
            Assert.AreEqual(measurablePoint.Result.NominalSinr, -0.413927, eps);
        }

        [Test]
        public void TestCalculatePerformance_ThreeCells_DifferentStations_SameMod()
        {
            ImportThreeCellsInDifferentStations();
            measurablePoint.CalculatePerformance(0.1);

            Assert.AreEqual(measurablePoint.Result.StrongestCell.Cell.Cell, outdoorCellList[2], "cell1");
            Assert.AreEqual(measurablePoint.Result.StrongestInterference.Cell.Cell, outdoorCellList[1], "cell2");
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(0), -113.512679, eps);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(1), -117.676163, eps);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(2), -136.877442, eps);
            //Assert.AreEqual(measurablePoint.Result.SameModInterferenceLevel, -117.624276, eps);
            Assert.AreEqual(measurablePoint.Result.DifferentModInterferenceLevel, Double.MinValue);
            //Assert.AreEqual(measurablePoint.Result.TotalInterferencePower, -117.624276, eps);
            Assert.AreEqual(measurablePoint.Result.NominalSinr, -3.00902, eps);
        }

        protected void ImportThreeCellsInDifferentStations_OneDifferentModInterference_OneSameModInterference()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18, 1);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18, 1);
            Mock<IOutdoorCell> outdoorCell3 = new Mock<IOutdoorCell>();
            outdoorCell3.MockOutdoorCell(111.99, 23, 90, 15.2, 18);
            outdoorCellList.Add(outdoorCell1.Object);
            outdoorCellList.Add(outdoorCell2.Object);
            outdoorCellList.Add(outdoorCell3.Object);

            measurablePoint.ImportCells(outdoorCellList, budgetList, model);
        }

        [Test]
        public void TestCalculatePerformance_ThreeCells_DifferentStations_OneSameModInterferenceAndOneDifferentMod()
        {
            ImportThreeCellsInDifferentStations_OneDifferentModInterference_OneSameModInterference();
            measurablePoint.CalculatePerformance(0.1);

            Assert.AreEqual(measurablePoint.Result.StrongestCell.Cell.Cell, outdoorCellList[2]);
            Assert.IsNull(measurablePoint.Result.StrongestInterference);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(0), -113.512679, eps);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(1), -117.676163, eps);
            //Assert.AreEqual(measurablePoint.ReceivedRsrpAt(2), -136.877442, eps);
            //Assert.AreEqual(measurablePoint.Result.SameModInterferenceLevel, -136.877442, eps);
            //Assert.AreEqual(measurablePoint.Result.DifferentModInterferenceLevel, -117.676163, eps);
            //Assert.AreEqual(measurablePoint.Result.TotalInterferencePower, -127.183242, eps);
            Assert.AreEqual(measurablePoint.Result.NominalSinr, 6.99098, eps);
        }
    }    
}
