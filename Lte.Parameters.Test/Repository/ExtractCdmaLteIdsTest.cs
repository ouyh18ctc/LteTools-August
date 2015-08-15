using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Service.Public;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Repository
{
    [TestFixture]
    public class ExtractCdmaLteIdsTest
    {
        private IEnumerable<CdmaLteIds> ExtractCdmaLteIds(IEnumerable<CellExcel> cellExcelList)
        {
            CdmaLteIdsService service = new CdmaLteIdsService(cellExcelList);
            return service.Query();
        }

        [Test]
        public void TestExtractCdmaLteIds()
        {
            List<CellExcel> cellExcelList = new List<CellExcel>{
                new CellExcel{ENodebId=5,CdmaCellId="1_6_6"},
                new CellExcel{ENodebId=5,CdmaCellId="1_6_6"},
                new CellExcel{ENodebId=6,CdmaCellId="1_2006_6"},
                new CellExcel{ENodebId=6,CdmaCellId="1_2006_66"},
                new CellExcel{ENodebId=6,CdmaCellId="1_2007_2"}
            };

            IEnumerable<CdmaLteIds> ids = ExtractCdmaLteIds(cellExcelList);
            Assert.AreEqual(ids.Count(), 3);
            Assert.AreEqual(ids.ElementAt(1).ENodebId, 6);
            Assert.AreEqual(ids.ElementAt(1).CdmaCellId, 2006);
        }

        [Test]
        public void TestExtractCdmaLteIds_SomeIllegalValues()
        {
            List<CellExcel> cellExcelList = new List<CellExcel>{
                new CellExcel{ENodebId=5,CdmaCellId="1_6_6"},
                new CellExcel{ENodebId=5,CdmaCellId="1_6_6"},
                new CellExcel{ENodebId=6,CdmaCellId="1_2006_6"},
                new CellExcel{ENodebId=6,CdmaCellId="1_2006_66"},
                new CellExcel{ENodebId=6,CdmaCellId="1_2007_2"},
                new CellExcel{ENodebId=6,CdmaCellId="aaa"}
            };

            IEnumerable<CdmaLteIds> ids = ExtractCdmaLteIds(cellExcelList);
            Assert.AreEqual(ids.Count(), 3);
        }
    }
}
