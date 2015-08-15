using System.Collections.Generic;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;
using NUnit.Framework;
using Moq;

namespace Lte.Domain.Test.Geo.Service
{
    [TestFixture]
    public class QueryOutdoorCellServiceTest
    {
        private IEnumerable<StubOutdoorCell> cellList;
        private QueryOutdoorCellService<StubOutdoorCell> service;

        [SetUp]
        public void SetUp()
        {
            cellList = new List<StubOutdoorCell>
            {
                new StubOutdoorCell
                {
                    CellName = "cell-1", 
                    Longtitute = 112, 
                    Lattitute = 23, 
                    Height = 0, 
                    Azimuth = 30,
                    Frequency = 100
                },
                new StubOutdoorCell
                {
                    CellName = "cell-2",
                    Longtitute = 112.01,
                    Lattitute = 23.01,
                    Height = 30,
                    Azimuth = 40,
                    Frequency = 1825
                },
                new StubOutdoorCell
                {
                    CellName = "cell-3",
                    Longtitute = 112.01,
                    Lattitute = 23,
                    Height = 40,
                    Azimuth = 50,
                    Frequency = 100
                },
                new StubOutdoorCell
                {
                    CellName = "cell-4",
                    Longtitute = 112,
                    Lattitute = 23.01,
                    Height = 40,
                    Azimuth = 80,
                    Frequency = 1825
                }
            };
        }

        [TestCase("cell-1",true)]
        [TestCase("cell-2",true)]
        [TestCase("cell-3",true)]
        [TestCase("cell-4",true)]
        [TestCase("cell-5",false)]
        public void TestQueryByName(string name, bool success)
        {
            service = new QueryOutdoorCellFrequencyConsidered<StubOutdoorCell>(cellList);
            IOutdoorCell cell = service.QueryByName(name);
            if (success)
            {
                Assert.IsNotNull(cell);
                Assert.AreEqual(cell.CellName, name);
            }
            else
            {
                Assert.IsNull(cell);
            }
        }

        [TestCase(112,23,30,100,true,true)]
        [TestCase(112,23,30,1825,true,false)]
        [TestCase(112,23,30,1825,false,true)]
        [TestCase(112,23.01,80,1825,true,true)]
        [TestCase(112,23.01,80,100,false,true)]
        public void TestQueryCell(double longtitute, double lattitute, double azimuth,
            int frequency, bool frequencyConsidered, bool success)
        {
            Mock<IOutdoorCell> cell = new Mock<IOutdoorCell>();
            cell.SetupGet(x => x.Longtitute).Returns(longtitute);
            cell.SetupGet(x => x.Lattitute).Returns(lattitute);
            cell.SetupGet(x => x.Azimuth).Returns(azimuth);
            cell.SetupGet(x => x.Frequency).Returns(frequency);
            if (frequencyConsidered)
                service = new QueryOutdoorCellFrequencyConsidered<StubOutdoorCell>(cellList);
            else
            {
                service=new QueryOutdoorCellFrequencyInconsidered<StubOutdoorCell>(cellList);
            }
            IOutdoorCell result = service.QueryCell(cell.Object);
            if (success)
            {
                Assert.IsNotNull(result);
            }
            else
            {
                Assert.IsNull(result);
            }
        }
    }
}
