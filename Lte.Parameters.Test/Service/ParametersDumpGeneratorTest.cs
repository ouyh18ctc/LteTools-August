using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Service;
using Lte.Parameters.Service.Public;
using Moq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Service
{
    [TestFixture]
    public class ParametersDumpGeneratorTest
    {
        ParametersDumpInfrastructure infrastructure=new ParametersDumpInfrastructure();
        Mock<IParametersDumpController> controller=new Mock<IParametersDumpController>();
        Mock<IBtsDumpRepository<ENodebExcel>> eNodebDumpRepository=new Mock<IBtsDumpRepository<ENodebExcel>>();
        Mock<ICellDumpRepository<CellExcel>> cellDumpRepository=new Mock<ICellDumpRepository<CellExcel>>();
        Mock<IBtsDumpRepository<BtsExcel>> btsDumpRepository=new Mock<IBtsDumpRepository<BtsExcel>>();
        Mock<ICellDumpRepository<CdmaCellExcel>> cdmaDumpRepository=new Mock<ICellDumpRepository<CdmaCellExcel>>();
        Mock<IMmlDumpRepository<CdmaBts, CdmaCell, BtsExcel, CdmaCellExcel>> mmlDumpRepository
            =new Mock<IMmlDumpRepository<CdmaBts, CdmaCell, BtsExcel, CdmaCellExcel>>();

        private ParametersDumpGenerator generator;
        Mock<IParametersDumpConfig> config=new Mock<IParametersDumpConfig>();

        [TestFixtureSetUp]
        public void Initialize()
        {
            generator = new ParametersDumpGenerator
            {
                LteENodebDumpGenerator = (c, i) => eNodebDumpRepository.Object,
                LteCellDumpGenerator = (c, i) => cellDumpRepository.Object,
                CdmaBtsDumpGenerator = (c, i) => btsDumpRepository.Object,
                CdmaCellDumpGenerator = (c, i) => cdmaDumpRepository.Object,
                MmlDumpGenerator = (c, i) => mmlDumpRepository.Object
            };
        }

        [Test]
        public void Test_DumpLteENodebData()
        {
            eNodebDumpRepository.Setup(x => x.InvokeAction(It.IsAny<IExcelBtsImportRepository<ENodebExcel>>()))
                .Callback<IExcelBtsImportRepository<ENodebExcel>>(x =>
                    eNodebDumpRepository.SetupGet(e => e.ImportBts).Returns(true));
            Mock<IExcelBtsImportRepository<ENodebExcel>> eNodebInfos
                = new Mock<IExcelBtsImportRepository<ENodebExcel>>();
            infrastructure.LteENodebRepository = eNodebInfos.Object;
            Assert.IsFalse(eNodebDumpRepository.Object.ImportBts);
            generator.DumpLteData(infrastructure,controller.Object,config.Object);
            Assert.IsTrue(eNodebDumpRepository.Object.ImportBts);
        }

        [Test]
        public void Test_DumpLteCellData()
        {
            cellDumpRepository.Setup(x => x.InvokeAction(It.IsAny<IExcelCellImportRepository<CellExcel>>()))
                .Callback<IExcelCellImportRepository<CellExcel>>(x =>
                    cellDumpRepository.SetupGet(c => c.ImportCell).Returns(true));
            Mock<IExcelCellImportRepository<CellExcel>> cellInfos
                =new Mock<IExcelCellImportRepository<CellExcel>>();
            infrastructure.LteCellRepository = cellInfos.Object;
            Assert.IsFalse(cellDumpRepository.Object.ImportCell);
            generator.DumpLteData(infrastructure, controller.Object, config.Object);
            Assert.IsTrue(cellDumpRepository.Object.ImportCell);
        }

        [Test]
        public void Test_DumpCdmaBtsData()
        {
            btsDumpRepository.Setup(x => x.InvokeAction(It.IsAny<IExcelBtsImportRepository<BtsExcel>>()))
                .Callback<IExcelBtsImportRepository<BtsExcel>>(x =>
                    btsDumpRepository.SetupGet(e => e.ImportBts).Returns(true));
            Mock<IExcelBtsImportRepository<BtsExcel>> btsInfos
                = new Mock<IExcelBtsImportRepository<BtsExcel>>();
            infrastructure.CdmaBtsRepository = btsInfos.Object;
            Assert.IsFalse(btsDumpRepository.Object.ImportBts);
            generator.DumpCdmaData(infrastructure, controller.Object, config.Object);
            Assert.IsTrue(btsDumpRepository.Object.ImportBts);
        }

        [Test]
        public void Test_DumpCdmaCellData()
        {
            cdmaDumpRepository.Setup(x => x.InvokeAction(It.IsAny<IExcelCellImportRepository<CdmaCellExcel>>()))
                .Callback<IExcelCellImportRepository<CdmaCellExcel>>(x =>
                    cdmaDumpRepository.SetupGet(c => c.ImportCell).Returns(true));
            Mock<IExcelCellImportRepository<CdmaCellExcel>> cellInfos
                = new Mock<IExcelCellImportRepository<CdmaCellExcel>>();
            infrastructure.CdmaCellRepository = cellInfos.Object;
            Assert.IsFalse(cdmaDumpRepository.Object.ImportCell);
            generator.DumpCdmaData(infrastructure, controller.Object, config.Object);
            Assert.IsTrue(cdmaDumpRepository.Object.ImportCell);
        }
    }
}
