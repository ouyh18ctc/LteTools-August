using System.Xml.Linq;
using Lte.Evaluations.Entities;
using System.IO;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Entities
{
    [TestFixture]
    public class StatValueFieldRepositoryTest : StatValueFieldTestConfig
    {
        [SetUp]
        public void SetUp()
        {
            Initialize();
        }

        [Test]
        public void TestStatValueFieldRepository_InitializeByFieldList()
        {
            Repository = new StatValueFieldRepository
            {
                FieldList = statValueFieldList
            };
            Assert.AreEqual(Repository.FieldDoc.ToString().Replace("\r\n", "\n"), resultString.Replace("\r\n", "\n"));
        }

        [Test]
        public void TestStatValueFieldRepository_InitializeByFieldDoc()
        {
            Stream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(resultString));
            XDocument document = XDocument.Load(
                new StreamReader(stream, System.Text.Encoding.UTF8));
            Assert.AreEqual(document.ToString().Replace("\r\n", "\n"), resultString.Replace("\r\n", "\n"));
            Repository = new StatValueFieldRepository
            {
                FieldDoc = document
            };
            Assert.IsNotNull(Repository.FieldList);

            Assert.AreEqual(Repository.FieldList.Count, 2);
            Assert.AreEqual(Repository.FieldList[0].FieldName, "field1");
            Assert.AreEqual(Repository.FieldList[1].IntervalList.Count, 2);
            Assert.AreEqual(Repository.FieldList[0].IntervalList[2].IntervalLowLevel, 2);
            Assert.AreEqual(Repository.FieldList[1].IntervalList[0].Color.ColorA, 155);
        }

        [Test]
        public void TestStatValueFieldRepository_DefaultConstruction()
        {
            Repository = new StatValueFieldRepository();
            Assert.IsNotNull(Repository);
            Assert.IsNotNull(Repository.FieldList);
            Assert.AreEqual(Repository.FieldList.Count, 5);
            Assert.AreEqual(Repository.FieldList[0].FieldName, "同模干扰电平");
            Assert.IsNotNull(Repository.FieldList[1].IntervalList);
            Assert.AreEqual(Repository.FieldList[2].IntervalList.Count, 0);
            Assert.AreEqual(Repository.FieldList[4].FieldName, "信号RSRP");
        }
    }
}
