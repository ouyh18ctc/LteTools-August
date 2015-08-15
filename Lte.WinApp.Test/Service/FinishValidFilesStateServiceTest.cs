using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.WinApp.Service;
using NUnit.Framework;

namespace Lte.WinApp.Test.Service
{
    [TestFixture]
    public class FinishValidFilesStateServiceTest:ImportFileInfoTestConfig
    {
        private FinishValidFilesStateService service;

        private void AssertInitialStates()
        {
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(fileInfoList[i].Result, "未知");
                Assert.AreEqual(fileInfoList[i].CurrentState, "未读取");
            }
        }

        [SetUp]
        public void SetUp()
        {
            Initialize();
        }

        [Test]
        public void TestFinishAllFiles()
        {
            AssertInitialStates();
            service = new FinishValidFilesStateService(fileInfoList);
            service.Finish();
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(fileInfoList[i].Result, "导入成功");
                Assert.AreEqual(fileInfoList[i].CurrentState, "已读取");
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void TestFinishOneFile(int id)
        {
            AssertInitialStates();
            service = new FinishValidFilesStateService(fileInfoList.Where(x => x.FilePath == "path" + id));
            service.Finish();
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(fileInfoList[i].Result, i == id - 1 ? "导入成功" : "未知");
                Assert.AreEqual(fileInfoList[i].CurrentState, i == id - 1 ? "已读取" : "未读取");
            }
        }
    }
}
