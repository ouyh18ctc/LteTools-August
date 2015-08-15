using System.Collections.Generic;
using Lte.WinApp.Models;
using Lte.WinApp.Service;
using NUnit.Framework;

namespace Lte.WinApp.Test.Models
{
    [TestFixture]
    public class FileInfoListImporterTest
    {
        private readonly FileInfoListImporter importer = new StubFileInfoListImporter();
        private List<ImportedFileInfo> fileInfoList;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            importer.FileType = "txt";
        }

        [SetUp]
        public void SetUp()
        {
            fileInfoList = new List<ImportedFileInfo>
            {
                new ImportedFileInfo
                {
                    FilePath = "path1",
                    FileType = "txt"
                }
            };
            importer.FileInfoList = fileInfoList;
        }

        [TestCase(1, false)]
        [TestCase(2, true)]
        [TestCase(4, true)]
        [TestCase(8, true)]
        [TestCase(100, true)]
        public void TestSimpleImport(int id, bool add)
        {
            importer.ImportFiles(new List<string>
            {
                "path" + id
            });
            Assert.AreEqual(fileInfoList.Count, 1 + (add ? 1 : 0));
            if (add)
            {
                Assert.AreEqual(importer.FileInfoList[1].FilePath, "path" + id);
                Assert.AreEqual(importer.FileInfoList[1].FileType, "txt");
                Assert.AreEqual(importer.FileInfoList[1].CurrentState, "未读取");
            }
        }
    }
}
