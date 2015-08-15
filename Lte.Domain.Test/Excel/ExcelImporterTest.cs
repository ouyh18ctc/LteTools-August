using Lte.Domain.Regular;
using Ninject;
using Ninject.Modules;
using Ninject.Activation;
using NUnit.Framework;

namespace Lte.Domain.Test.Excel
{
    [TestFixture]
    public class ExcelImporterTest
    {
        [Test]
        public void TestExcelImporter()
        {
            var kernel = new StandardKernel(new ExcelImporterModule());
            ExcelImporterService service = kernel.Get<ExcelImporterService>();

            IExcelImporter importer = service.Importer;
            Assert.IsNotNull(importer["基站级"]);
            Assert.IsNotNull(importer["小区级"]);
            Assert.AreEqual(importer["基站级"].TableName, "基站级");
        }
    }

    public class ExcelImporterService
    {
        readonly IExcelImporter _importer;

        public IExcelImporter Importer
        {
            get { return _importer; }
        }

        public ExcelImporterService(IExcelImporter importer)
        {
            _importer = importer;
        }

    }

    public class ExcelImporterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IExcelImporter>().ToProvider(new SheetNamesComponentPorvider());
        }
    }

    public class SheetNamesComponentPorvider : Provider<IExcelImporter>
    {
        protected override IExcelImporter CreateInstance(IContext context)
        {
            string[] tableNames = { "基站级", "小区级" };
            return new StubExcelImporter(tableNames);
        }
    }

}
