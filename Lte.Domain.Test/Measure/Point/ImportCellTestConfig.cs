using System.Collections.Generic;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Domain.Measure;
using Moq;

namespace Lte.Domain.Test.Measure.Point
{
    public class ImportCellTestConfig
    {
        protected IList<ILinkBudget<double>> budgetList;
        protected IBroadcastModel model;
        protected IList<IOutdoorCell> outdoorCellList;
        protected const double eps = 1E-6;
        protected MeasurePoint measurablePoint;

        protected void Initialize()
        {
            budgetList = new List<ILinkBudget<double>>();
            model = new BroadcastModel();
            outdoorCellList = new List<IOutdoorCell>();
            measurablePoint = new MeasurePoint();
        }

        protected void ImportOneCell()
        {
            Mock<IOutdoorCell> outdoorCell = new Mock<IOutdoorCell>();
            outdoorCell.MockOutdoorCell(112, 23, 0, 15.2, 18);
            outdoorCellList.Add(outdoorCell.Object);

            measurablePoint.ImportCells(outdoorCellList, budgetList, model);
        }

        protected void ImportTwoCellsInOneStation()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18);
            outdoorCellList.Add(outdoorCell1.Object);
            outdoorCellList.Add(outdoorCell2.Object);

            measurablePoint.ImportCells(outdoorCellList, budgetList, model);
        }

        protected void ImportTwoCellsInOneStation_WithDifferentMods()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18, 1);
            outdoorCellList.Add(outdoorCell1.Object);
            outdoorCellList.Add(outdoorCell2.Object);

            measurablePoint.ImportCells(outdoorCellList, budgetList, model);
        }

        protected void ImportThreeCellsInOneStation()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell3 = new Mock<IOutdoorCell>();
            outdoorCell3.MockOutdoorCell(112, 23, 90, 15.2, 18);
            outdoorCellList.Add(outdoorCell1.Object);
            outdoorCellList.Add(outdoorCell2.Object);
            outdoorCellList.Add(outdoorCell3.Object);

            measurablePoint.ImportCells(outdoorCellList, budgetList, model);
        }

        protected void ImportThreeCellsInDifferentStations()
        {
            Mock<IOutdoorCell> outdoorCell1 = new Mock<IOutdoorCell>();
            outdoorCell1.MockOutdoorCell(112, 23, 0, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell2 = new Mock<IOutdoorCell>();
            outdoorCell2.MockOutdoorCell(112, 23, 45, 15.2, 18);
            Mock<IOutdoorCell> outdoorCell3 = new Mock<IOutdoorCell>();
            outdoorCell3.MockOutdoorCell(111.99, 23, 90, 15.2, 18);
            outdoorCellList.Add(outdoorCell1.Object);
            outdoorCellList.Add(outdoorCell2.Object);
            outdoorCellList.Add(outdoorCell3.Object);

            measurablePoint.ImportCells(outdoorCellList, budgetList, model);
        }
    }
    
}
