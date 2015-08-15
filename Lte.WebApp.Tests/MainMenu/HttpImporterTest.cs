using System.IO;
using System.Web;
using Lte.Evaluations.ViewHelpers;
using Lte.WebApp.Models;
using Moq;
using NUnit.Framework;

namespace Lte.WebApp.Tests.MainMenu
{
    class HttpTestImporter : HttpImporter
    {
        public HttpTestImporter(HttpPostedFileBase file)
            : base(file)
        {
            FilePath = Path.Combine(BaseDirectory, FileName ?? "");
            ConstructReader(file);
        }

        public override string BaseDirectory
        {
            get { return @"C:\base\uploads\"; }
        }

        protected override void ConstructReader(HttpPostedFileBase file)
        {
            const string input = "Hello world!";
            byte[] stringAsByteArray = System.Text.Encoding.UTF8.GetBytes(input);
            Stream stream = new MemoryStream(stringAsByteArray);
            Reader = new StreamReader(stream, System.Text.Encoding.UTF8);
        }
    }

    [TestFixture]
    public class HttpImporterTest
    {
        Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();

        [Test]
        public void TestHttpImporter_EmptyFileName()
        {
            mockFile.SetupGet(x => x.FileName).Returns("");
            HttpImporter import = new HttpTestImporter(mockFile.Object);
            Assert.IsNotNull(import);
            Assert.IsFalse(import.Success);
        }

        [TestCase("D:\\aaa\\", "MyFile.txt")]
        [TestCase("D:\\abcd\\", "MyFile.txt")]
        [TestCase("D:\\abcd\\fewr\\", "fewewle.txt")]
        public void TestHttpImporter_ValidFileName(string clientDirectory, string fileName)
        {
            mockFile.SetupGet(x => x.FileName).Returns(clientDirectory+fileName);
            HttpImporter import = new HttpTestImporter(mockFile.Object);
            Assert.IsNotNull(import);
            Assert.IsTrue(import.Success);
            Assert.AreEqual(import.FileName, fileName);
            Assert.AreEqual(import.FilePath, "C:\\base\\uploads\\" + fileName);
            Assert.IsNotNull(import.Reader);
            string content = import.Reader.ReadLine();
            Assert.AreEqual(content, "Hello world!");
        }
    }
}
