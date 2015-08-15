using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Regular;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Parameters.Entities;
using Lte.WinApp.Import;
using NUnit.Framework;

namespace Lte.WinApp.Test.Import
{
    [TestFixture]
    public class MrsFilesImporterTest
    {
        private readonly Func<string, MrsRecordSet> recordSetGenerator = path =>
        {
            string[] fields = path.GetSplittedFields('-');
            return new MrsRecordSet
            {
                RecordDate = DateTime.Today,
                MrsCells = new List<MrsCell>
                {
                    new MrsCell
                    {
                        CellId = fields[0].ConvertToInt(50000),
                        SectorId = fields[1].ConvertToByte(0),
                        RsrpCounts = Enumerable.Repeat(fields[2].ConvertToInt(0), 48).ToArray(),
                        TaCounts = Enumerable.Repeat(fields[3].ConvertToInt(0), 45).ToArray()
                    }
                }
            };
        };

        private MrsFilesImporter importer;

        [SetUp]
        public void SetUp()
        {
            importer=new MrsFilesImporter();
        }

        [TestCase(new[] { "50001-0-4-5" }, new[] { 4 }, new[] { 10 })]
        [TestCase(new[] { "50002-0-4-5" }, new[] { 4 }, new[] { 10 })]
        [TestCase(new[] { "50001-2-4-5" }, new[] { 4 }, new[] { 10 })]
        [TestCase(new[] { "50001-0-7-5" }, new[] { 7 }, new[] { 10 })]
        [TestCase(new[] { "50001-0-4-7" }, new[] { 4 }, new[] { 14 })]
        [TestCase(new[] { "50001-0-4-5", "50001-0-7-5" }, new[] { 11 }, new[] { 20 })]
        [TestCase(new[] { "50001-2-4-5", "50000-0-7-5" }, new[] { 4, 7 }, new[] { 10, 10 })]
        [TestCase(new[] { "50001-0-8-5", "50001-0-7-5" }, new[] { 15 }, new[] { 20 })]
        [TestCase(new[] { "50001-0-4-6", "50001-0-7-5" }, new[] { 11 }, new[] { 22 })]
        [TestCase(new[] { "50002-0-4-6", "50001-0-7-7" }, new[] { 4, 7 }, new[] { 12, 14 })]
        public void Test_ImportOnce_RecordSet(string[] paths, int[] rsrpCounts, int[] taCounts)
        {
            importer.Import(paths, recordSetGenerator);
            Assert.AreEqual(importer.RsrpStatList.Count, rsrpCounts.Length);
            Assert.AreEqual(importer.TaStatList.Count, taCounts.Length);
            for (int i = 0; i < rsrpCounts.Length; i++)
            {
                Assert.AreEqual(importer.RsrpStatList[i].RsrpTo120, rsrpCounts[i]);
            }
            for (int i = 0; i < taCounts.Length; i++)
            {
                Assert.AreEqual(importer.TaStatList[i].TaTo2, taCounts[i]);
            }
        }

        [TestCase(new[] { "50001-0-4-5" }, new[] { "50002-0-4-5" }, new[] { 4, 4 }, new[] { 10, 10 })]
        [TestCase(new[] { "50002-0-4-5" }, new[] { "50001-2-4-5" }, new[] { 4, 4 }, new[] { 10, 10 })]
        [TestCase(new[] { "50001-2-4-5" }, new[] { "50001-0-7-5" }, new[] { 4, 7 }, new[] { 10, 10 })]
        [TestCase(new[] { "50001-0-7-5" }, new[] { "50001-0-4-7" }, new[] { 11 }, new[] { 24 })]
        [TestCase(new[] { "50001-0-4-7" }, new[] { "50001-0-4-5", "50001-0-7-5" }, 
            new[] { 15 }, new[] { 34 })]
        [TestCase(new[] { "50001-0-4-5", "50001-0-7-5" }, new[] { "50001-2-4-5", "50000-0-7-5" }, 
            new[] { 11, 4, 7 }, new[] { 20, 10, 10 })]
        [TestCase(new[] { "50001-2-4-5", "50000-0-7-5" }, new[] { "50001-0-8-5", "50001-0-7-5" }, 
            new[] { 4, 7, 15 }, new[] { 10, 10, 20 })]
        [TestCase(new[] { "50001-0-8-5", "50001-0-7-5" }, new[] { "50001-0-4-6", "50001-0-7-5" }, 
            new[] { 26 }, new[] { 42 })]
        [TestCase(new[] { "50001-0-4-6", "50001-0-7-5" }, new[] { "50002-0-4-6", "50001-0-7-7" }, 
            new[] { 18, 4 }, new[] { 36, 12 })]
        public void Test_ImportTwice_RecordSet(string[] paths1, string[] paths2, int[] rsrpCounts, int[] taCounts)
        {
            importer.Import(paths1, recordSetGenerator);
            importer.Import(paths2, recordSetGenerator);
            Assert.AreEqual(importer.RsrpStatList.Count, rsrpCounts.Length);
            Assert.AreEqual(importer.TaStatList.Count, taCounts.Length);
            for (int i = 0; i < rsrpCounts.Length; i++)
            {
                Assert.AreEqual(importer.RsrpStatList[i].RsrpTo120, rsrpCounts[i]);
            }
            for (int i = 0; i < taCounts.Length; i++)
            {
                Assert.AreEqual(importer.TaStatList[i].TaTo2, taCounts[i]);
            }
        }
    }
}
