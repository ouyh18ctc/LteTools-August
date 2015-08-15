using System.Linq;
using Lte.WinApp.Service;
using NUnit.Framework;

namespace Lte.WinApp.Test.Service
{
    [TestFixture]
    public class QueryValidFileInfosServiceTest : ImportFileInfoTestConfig
    {
        [SetUp]
        public void SetUp()
        {
            Initialize();
            for (int i = 0; i < 3; i++)
            {
                fileInfoList[i].FileType = "txt";
            }
        }

        private int GetFileInfoListLength()
        {
            return importer.Object.Query().Count();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void TestQuery_ModifyOneFileType(int index)
        {
            Assert.AreEqual(GetFileInfoListLength(), 3);
            fileInfoList[index].FileType = "bat";
            Assert.AreEqual(GetFileInfoListLength(), 2);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void TestQuery_ModifyOneFileStat(int index)
        {
            Assert.AreEqual(GetFileInfoListLength(), 3);
            fileInfoList[index].FinishState();
            Assert.AreEqual(GetFileInfoListLength(), 2);
        }
    }
}
