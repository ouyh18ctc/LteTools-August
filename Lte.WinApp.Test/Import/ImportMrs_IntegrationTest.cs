using System;
using System.IO;
using System.Linq;
using Lte.Evaluations.Rutrace.Entities;
using Lte.WinApp.Import;
using NUnit.Framework;

namespace Lte.WinApp.Test.Import
{
    [TestFixture]
    public class ImportMrs_IntegrationTest
    {
        private readonly string testDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XmlFiles");
        private readonly MrsFilesImporter importer = new MrsFilesImporter();

        [Test]
        public void Test_EmptyFiles()
        {
            string mrsFileName = Path.Combine(testDirectory, "FDD-LTE_MRS_ZTE_OMC440601_503387_20150713221500.xml");
            importer.Import(new []{mrsFileName},x=>new MrsRecordSet(new StreamReader(x)));
            Assert.IsNotNull(importer);
            Assert.AreEqual(importer.RsrpStatList.Count, 0);
            Assert.AreEqual(importer.TaStatList.Count, 0);
        }

        [Test]
        public void Test_NotEmptyFiles()
        {
            string mrsFileName = Path.Combine(testDirectory, "FDD-LTE_MRS_ZTE_OMC1_501250_20150713223000.xml");
            importer.Import(new[] { mrsFileName }, x => new MrsRecordSet(new StreamReader(x)));
            Assert.IsNotNull(importer);
            Assert.AreEqual(importer.RsrpStatList.Count, 3);
            Assert.AreEqual(importer.RsrpStatList[0].RsrpCounts.Sum(), 33);
            Assert.AreEqual(importer.TaStatList.Count, 3);
            Assert.AreEqual(importer.TaStatList[0].TaCounts.Sum(), 27);
        }
    }
}
