using System.Linq;
using System.Threading.Tasks;
using Lte.Domain.Regular;
using Lte.WinApp.Models;
using NUnit.Framework;

namespace Lte.WinApp.Test.Models
{
    [TestFixture]
    public class FakeFileInfoListImporterTest
    {
        private FileInfoListImporterAsync<StubTimeStat, FakeTopCellRepository> _importerAsync;

        [TestCase("123", new[] { "D:\\20120513.txt" }, "\nD:\\20120513.txt完成导入数量：3")]
        [TestCase("123456", new[] { "D:\\aa20120513.txt" }, "\nD:\\aa20120513.txt完成导入数量：6")]
        [TestCase("123456", new[] { "D:\\aa20120513.txt", "D:\\bb20120513.txt" },
            "\nD:\\aa20120513.txt完成导入数量：6\nD:\\bb20120513.txt完成导入数量：6")]
        public async void TestImport(string contents, string[] paths, string info)
        {
            _importerAsync = new FakeFileInfoListImporterAsync(contents);
            ImportedFileInfo[] validFileInfos = paths.Select(x =>
                new ImportedFileInfo
                {
                    FilePath = x,
                    FileType = "Test"
                }).ToArray();

            await Task.WhenAll(Task.Run(() =>_importerAsync.Import(validFileInfos)), Task.Delay(300));
            
            Assert.AreEqual(_importerAsync.Result, info);
            for (int i = 0; i < paths.Length; i++)
            {
                ImportedFileInfo fileInfo = validFileInfos.ElementAt(i);
                Assert.AreEqual(fileInfo.FilePath,paths[i]);
                Assert.AreEqual(fileInfo.CurrentState, "已读取");
                Assert.AreEqual(fileInfo.Result, "导入成功");
            }
        }

        [TestCase("123", new[] { "D:\\20120513.txt" }, new[] { true })]
        [TestCase("123456", new[] { "D:\\aa20120513.txt" }, new[] { true })]
        [TestCase("1234567", new[] { "D:\\aa20120513.txt", "D:\\bb20120513.txt" },
            new[] { true, false })]
        public async void TestImport_RepositoryConsidered(string contents, string[] paths, bool[] results)
        {
            _importerAsync = new FakeFileInfoListImporterAsyncWithRepository(contents);
            ImportedFileInfo[] validFileInfos = paths.Select(x =>
                new ImportedFileInfo
                {
                    FilePath = x,
                    FileType = "Test"
                }).ToArray();
            await Task.WhenAll(Task.Run(() => _importerAsync.Import(validFileInfos)), Task.Delay(500));
            string info = "";
            for (int i = 0; i < paths.Length; i++)
            {
                ImportedFileInfo fileInfo = validFileInfos.ElementAt(i);
                Assert.AreEqual(fileInfo.FilePath, paths[i]);
                Assert.AreEqual(fileInfo.CurrentState, "已读取");
                if (results[i])
                {
                    Assert.AreEqual(fileInfo.Result, "导入成功");
                    info += "\n" + fileInfo.FilePath + "完成导入数量：" + contents.Length;
                }
                else
                {
                    Assert.AreEqual(fileInfo.Result, "无需导入");
                    info += "\n日期：" + fileInfo.FilePath.RetrieveFileNameBody().GetDateExtend().ToShortDateString() 
                        + "的统计记录已导入！";
                }
            }
            Assert.AreEqual(_importerAsync.Result, info);
        }
    }
}
