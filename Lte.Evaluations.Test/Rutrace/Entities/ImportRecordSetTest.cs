using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.Geo.Abstract;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Moq;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Entities
{
    [TestFixture]
    public class ImportRecordSetTest
    {
        private readonly Mock<INearestPciCellRepository> mockRepository = new Mock<INearestPciCellRepository>();

        [SetUp]
        public void SetUp()
        {
            mockRepository.Setup(x => x.Import(It.IsAny<ICell>(), It.IsAny<short>())).Returns(new NearestPciCell
            {
                NearestCellId = 0,
                NearestSectorId = 0
            });
        }

        [TestCase(50001, 0, 50002, 1, 101, 100, 50002, 1)]
        [TestCase(50001, 0, 50003, 4, 101, 100, 50003, 4)]
        [TestCase(50001, 0, 50002, 1, 101, 1825, 0, 0)]
        public void Test_OneRef_OneNeighbor(int refCellId, byte refSectorId, int nbCellId, byte nbSectorId, short pci, 
            short frequency, int resultCellId, byte resultSectorId)
        {
            MrRecordSet recordSet = new MroRecordSet
            {
                RecordDate = DateTime.Today,
                RecordList = new List<MrRecord>
                {
                    new MroRecord
                    {
                        RefCell = new MrReferenceCell
                        {
                            CellId = refCellId,
                            SectorId = refSectorId
                        },
                        NbCells = new List<MrNeighborCell>
                        {
                            new MrNeighborCell
                            {
                                CellId = 0,
                                SectorId = 0,
                                Pci = pci,
                                Frequency = frequency
                            }
                        }
                    }
                }
            };
            mockRepository.SetupGet(x => x.NearestPciCells).Returns(
                new List<NearestPciCell>
                {
                    new NearestPciCell
                    {
                        CellId = refCellId,
                        SectorId = refSectorId,
                        NearestCellId = nbCellId,
                        NearestSectorId = nbSectorId,
                        Pci = pci
                    }
                });
            recordSet.ImportRecordSet(mockRepository.Object);
            Assert.AreEqual(recordSet.RecordList[0].NbCells[0].CellId,resultCellId);
            Assert.AreEqual(recordSet.RecordList[0].NbCells[0].SectorId,resultSectorId);
        }
    }
}
