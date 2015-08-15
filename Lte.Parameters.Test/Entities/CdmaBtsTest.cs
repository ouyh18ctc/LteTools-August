using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Service;
using Lte.Parameters.Entities;
using Lte.Parameters.Abstract;
using System.Data;
using Moq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Entities
{
    [TestFixture]
    public class MmlBtsTest : CdmaTestConfig
    {
        private readonly Mock<IDataReader> mockReader = new Mock<IDataReader>();

        [SetUp]
        public void TestInitilize()
        {
            Initialize();
            Tuple<string, string>[] contents =
            {
                new Tuple<string,string>("基站名称","New Bts"),
                new Tuple<string,string>("基站编号","51"),
                new Tuple<string,string>("BSC编号","3"),
                new Tuple<string,string>("行政区域","Chancheng"),
                new Tuple<string,string>("所属镇区","Zhangcha"),
                new Tuple<string, string>("经度","112.123"), 
                new Tuple<string, string>("纬度","23.456"), 
                new Tuple<string, string>("地址","My address is here..."), 
            };
            mockReader.MockReaderContents(contents);
        }

        [Test]
        public void TestCdmaBts()
        {
            BtsExcel btsExcel = new BtsExcel(mockReader.Object);
            btsExcel.Import();
            Assert.AreEqual(btsExcel.DistrictName, "Chancheng");
            Assert.AreEqual(btsExcel.TownName, "Zhangcha");
            Assert.AreEqual(btsExcel.Name, "New Bts");
            bts.Import(btsExcel, false);
            Assert.IsNotNull(bts);
            Assert.AreEqual(bts.ENodebId, -1);
            Assert.AreEqual(bts.BtsId, 51);
            Assert.AreEqual(bts.Name, "张槎工贸");
            Assert.AreEqual(bts.Longtitute, 112.123);
            Assert.AreEqual(bts.Lattitute, 23.456);

            bts.Longtitute = 0;
            bts.Lattitute = 0;
            bts.TownId = 100;
            bts.Import(btsExcel, true);
            Assert.AreEqual(bts.ENodebId, -1);
            Assert.AreEqual(bts.BtsId, 51);
            Assert.AreEqual(bts.Name, "New Bts");
            Assert.AreEqual(bts.BscId, 3);

            Mock<IENodebRepository> mockENodebList = new Mock<IENodebRepository>();
            mockENodebList.Setup(x => x.GetAll()).Returns(
                new List<ENodeb> {
                    new ENodeb{
                        Name = "New Bts",
                        Longtitute = 112.1231,
                        Lattitute = 23.4561,
                        ENodebId = 2,
                        TownId = 100
                    }
                }.AsQueryable());
            mockENodebList.Setup(x => x.GetAllList()).Returns(mockENodebList.Object.GetAll().ToList());
            Assert.AreEqual(bts.Distance(mockENodebList.Object.GetAll().ElementAt(0)), 0.015, 1E-4);
            bts.ImportLteInfo(mockENodebList.Object.GetAllList());
            Assert.AreEqual(bts.ENodebId, 2);
        }
    }

    [TestFixture]
    public class MmlLineInfoTest : CdmaTestConfig
    {
        [SetUp]
        public void TestInitilize()
        {
            Initialize();
        }

        [Test]
        public void TestMmlLineInfo_GenerateCdmaBts()
        {
            Assert.IsNotNull(btsLineInfo);
            Assert.AreEqual(btsLineInfo.KeyWord, "ADD BSCBTSINF");
            Assert.AreEqual(btsLineInfo.FieldInfos.Count, 22);
            Assert.AreEqual(btsLineInfo.FieldInfos["BTSID"], "50");
            Assert.AreEqual(btsLineInfo.FieldInfos["BTSNM"], "张槎工贸");

            Assert.IsNotNull(bts);
            Assert.AreEqual(bts.ENodebId, -1);
            Assert.AreEqual(bts.BtsId, 50);
            Assert.AreEqual(bts.Name, "张槎工贸");
        }

        [Test]
        public void TestMmlLineInfo_CreateCdmaCell()
        {
            Assert.IsNotNull(cellLineInfo);
            Assert.AreEqual(cellLineInfo.KeyWord, "ADD CELL");
            Assert.AreEqual(cellLineInfo.FieldInfos.Count, 23);
            Assert.AreEqual(cellLineInfo.FieldInfos["BTSID"], "4");
            Assert.AreEqual(cellLineInfo.FieldInfos["CN"], "3964");
            Assert.AreEqual(cellLineInfo.FieldInfos["SCTIDLST"], "3");

            Assert.IsNotNull(cell);
            Assert.AreEqual(cell.BtsId, 4);
            Assert.AreEqual(cell.CellId, 3964);
            Assert.AreEqual(cell.SectorId, 3);
            Assert.AreEqual(cell.Pn, 198);
            Assert.AreEqual(cell.CellType, "DO");
        }
    }
}
