using System.Collections.Generic;
using System.Data;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Parameters.Test.Import
{
    [TestFixture]
    public class ImportENodebExcelListTest
    {
        [TestCase(1, new[] { "C-1" }, new[] { "D-1" }, new[] { "T-1" },
            new[] { 1 }, new[] { "Address-1" }, new[] { "10.10.1.1" }, new[] { "10.10.0.1" })]
        [TestCase(2, new[] { "C-1", "C-1" }, new[] { "D-1", "D-2" }, new[] { "T-1", "T-2" },
            new[] { 1, 2 }, new[] { "Address-1", "Address-2" },
            new[] { "10.10.1.1", "10.10.1.2" }, new[] { "10.10.0.1", "10.10.0.1" })]
        [TestCase(3, new[] { "C-1", "C-1", "C-2" }, new[] { "D-1", "D-2", "D-1" }, new[] { "T-1", "T-2", "T-3" },
            new[] { 1, 2, 3 }, new[] { "Address-1", "Address-2", "Address-3" },
            new[] { "10.10.1.1", "10.10.1.2", "10.10.1.3" }, 
            new[] { "10.10.0.1", "10.10.0.1", "10.10.0.2" })]
        public void Test(int rows, string[] cityNames, string[] districtNames, string[] townNames,
            int[] eNodebIds, string[] addresses, string[] ips, string[] gateways)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("地市", typeof (string));
            dataTable.Columns.Add("区域", typeof (string));
            dataTable.Columns.Add("镇区", typeof (string));
            dataTable.Columns.Add("eNodeB ID", typeof (int));
            dataTable.Columns.Add("地址", typeof (string));
            dataTable.Columns.Add("IP", typeof (string));
            dataTable.Columns.Add("网关", typeof (string));
            for (int i = 0; i < rows; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["地市"] = cityNames[i];
                dr["区域"] = districtNames[i];
                dr["镇区"] = townNames[i];
                dr["eNodeB ID"] = eNodebIds[i];
                dr["地址"] = addresses[i];
                dr["IP"] = ips[i];
                dr["网关"] = gateways[i];
                dataTable.Rows.Add(dr);
            }

            ImportExcelValueService<ENodebExcel> service =
                new ImportExcelValueService<ENodebExcel>(dataTable, x=>new ENodebExcel(x));
            List<ENodebExcel> importList = service.ExcelList;

            service.Import();
            Assert.IsNotNull(importList);
            Assert.AreEqual(importList.Count, rows);
            for (int i = 0; i < rows; i++)
            {
                Assert.AreEqual(importList[i].CityName, cityNames[i]);
                Assert.AreEqual(importList[i].DistrictName, districtNames[i]);
                Assert.AreEqual(importList[i].TownName, townNames[i]);
                Assert.AreEqual(importList[i].ENodebId, eNodebIds[i]);
                Assert.AreEqual(importList[i].Address, addresses[i]);
                Assert.AreEqual(importList[i].IpString, ips[i]);
                Assert.AreEqual(importList[i].GatewayString, gateways[i]);
            }
        }
    }
}
