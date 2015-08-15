using System.Collections.Generic;
using Lte.WinApp.Service;
using NUnit.Framework;

namespace Lte.WinApp.Test.Service
{
    [TestFixture]
    public class ImportFileInfoServiceTest: ImportFileInfoTestConfig
    {
        [SetUp]
        public void SetUp()
        {
            Initialize();
        }

        private void AssertFileInfoListUnchanged()
        {
            Assert.AreEqual(importer.Object.FileInfoList[0].FilePath, "path1");
            Assert.AreEqual(importer.Object.FileInfoList[1].FilePath, "path2");
            Assert.AreEqual(importer.Object.FileInfoList[2].FilePath, "path3");
            Assert.IsFalse(importer.Object.FileInfoList[0].IsSelected);
            Assert.IsFalse(importer.Object.FileInfoList[1].IsSelected);
            Assert.IsFalse(importer.Object.FileInfoList[2].IsSelected);
        }

        [Test]
        public void TestImport_EmptyList()
        {
            IEnumerable<string> fileNames = new List<string>();
            importer.Object.ImportFiles(fileNames);
            AssertFileInfoListUnchanged();
        }

        [Test]
        public void TestImport_OneFile_PathNotFound()
        {
            IEnumerable<string> fileNames = new List<string>
            {
                "path4"
            };
            importer.Object.ImportFiles(fileNames);
            AssertFileInfoListUnchanged();
            Assert.AreEqual(importer.Object.FileInfoList.Count, 4);
            Assert.AreEqual(importer.Object.FileInfoList[3].FilePath, "path4");
            Assert.IsTrue(importer.Object.FileInfoList[3].IsSelected);
        }

        [Test]
        public void TestImport_Path1()
        {
            IEnumerable<string> fileNames = new List<string>
            {
                "path1"
            };
            importer.Object.ImportFiles(fileNames);
            AssertFileInfoListUnchanged();
        }

        [Test]
        public void TestImport_Path2()
        {
            IEnumerable<string> fileNames = new List<string>
            {
                "path2"
            };
            importer.Object.ImportFiles(fileNames);
            AssertFileInfoListUnchanged();
        }

        [Test]
        public void TestImport_Path1_And_Path3()
        {
            IEnumerable<string> fileNames = new List<string>
            {
                "path1",
                "path3"
            };
            importer.Object.ImportFiles(fileNames);
            AssertFileInfoListUnchanged();
        }
    }
}
