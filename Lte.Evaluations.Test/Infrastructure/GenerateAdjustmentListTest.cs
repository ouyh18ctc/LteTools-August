using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Service;
using Lte.Parameters.Entities;
using Lte.Parameters.Abstract;
using Lte.Evaluations.Dingli;
using Moq;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Infrastructure
{
    [TestFixture]
    public class GenerateAdjustmentListTest
    {
        private List<CoverageStat> _coveragePoints;
        private readonly Mock<ICellRepository> mockCellRepository = new Mock<ICellRepository>();
        private IEnumerable<ENodeb> _eNodebs;

        [SetUp]
        public void TestInitialize()
        {
            _coveragePoints = new List<CoverageStat>{
                new CoverageStat {
                    ENodebId = 1, SectorId = 2, Earfcn = 100,
                    Longtitute = 113.0001, Lattitute = 23.0001,
                    Rsrp = -94.5 },
                new CoverageStat {
                    ENodebId = 1, SectorId = 2, Earfcn = 100,
                    Longtitute = 113.0002, Lattitute = 23.0001,
                    Rsrp = -96.5 },
                new CoverageStat {
                    ENodebId = 1, SectorId = 2, Earfcn = 1825,
                    Longtitute = 113.0002, Lattitute = 23.0001,
                    Rsrp = -94.3 }
            };
            mockCellRepository.Setup(x => x.GetAll()).Returns(
                new List<Cell> {
                    new Cell { 
                        ENodebId = 1, SectorId = 2, Frequency = 100,
                        RsPower = 15.2, AntennaGain = 17.5, Height =30,
                        Azimuth = 30, ETilt = 0, MTilt = 5,
                        Longtitute = 113.00015, Lattitute = 23.00015
                    },
                    new Cell { 
                        ENodebId = 1, SectorId = 2, Frequency = 1825,
                        RsPower = 15.2, AntennaGain = 17.5, Height =30,
                        Azimuth = 30, ETilt = 0, MTilt = 5,
                        Longtitute = 113.00015, Lattitute = 23.00015
                    },
                    new Cell { 
                        ENodebId = 1, SectorId = 3, Frequency = 100,
                        RsPower = 15.2, AntennaGain = 17.5, Height =30,
                        Azimuth = 90, ETilt = 0, MTilt = 5,
                        Longtitute = 113.00015, Lattitute = 23.00015
                    },
                    new Cell { 
                        ENodebId = 2, SectorId = 2, Frequency = 100,
                        RsPower = 15.2, AntennaGain = 17.5, Height =30,
                        Azimuth = 30, ETilt = 0, MTilt = 5,
                        Longtitute = 113.00025, Lattitute = 23.00015
                    }
                }.AsQueryable());
            mockCellRepository.Setup(x => x.GetAllList()).Returns(mockCellRepository.Object.GetAll().ToList());
            _eNodebs = new List<ENodeb>{
                new ENodeb { ENodebId = 1, Name = "ENodeb-1" },
                new ENodeb { ENodebId = 2, Name = "ENodeb-2" }
            };
        }

        private void AddOnePointBesideFirstPoint()
        {
            _coveragePoints.Add(new CoverageStat
            {
                ENodebId = 1,
                SectorId = 2,
                Earfcn = 100,
                Longtitute = 113.000101,
                Lattitute = 23.000101,
                Rsrp = -96.5
            });
        }

        private void AddOnePointBesideSecondPoint()
        {
            _coveragePoints.Add(new CoverageStat
            {
                ENodebId = 1,
                SectorId = 2,
                Earfcn = 100,
                Longtitute = 113.000201,
                Lattitute = 23.000101,
                Rsrp = -96.5
            });
        }

        private void AddOnePointFacingTheCell()
        {
            _coveragePoints.Add(new CoverageStat
            {
                ENodebId = 1,
                SectorId = 2,
                Earfcn = 100,
                Longtitute = 113.0002,
                Lattitute = 23.0002,
                Rsrp = -62.5
            });
        }

        [Test]
        public void TestGenerateAdjustmentList_ThreePoints()
        {
            IEnumerable<CoverageAdjustment> results = _coveragePoints.GenerateAdjustmentList(
                mockCellRepository.Object, _eNodebs);
            IEnumerable<CoverageAdjustment> coverageAdjustments = results as CoverageAdjustment[] ?? results.ToArray();
            Assert.AreEqual(coverageAdjustments.Count(), 2);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).ENodebId, 1);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).SectorId, 2);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Frequency, 100);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Factor105, 4.369684, 1E-6);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Factor135m, 2.369684, 1E-6);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).ENodebId, 1);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).SectorId, 2);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).Frequency, 1825);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).Factor105, 3.624325, 1E-6);
        }

        [Test]
        public void TestGenerateAdjustmentList_FourPoints_WithNewPointBesideFirstPoint()
        {
            AddOnePointBesideFirstPoint();
            IEnumerable<CoverageAdjustment> results = _coveragePoints.GenerateAdjustmentList(
                mockCellRepository.Object, _eNodebs);
            IEnumerable<CoverageAdjustment> coverageAdjustments = results as CoverageAdjustment[] ?? results.ToArray();
            Assert.AreEqual(coverageAdjustments.Count(), 2);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).ENodebId, 1);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).SectorId, 2);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Frequency, 100);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Factor105, 4.369684, 1E-6);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Factor135m, 3.524214, 1E-6);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).ENodebId, 1);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).SectorId, 2);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).Frequency, 1825);
        }

        [Test]
        public void TestGenerateAdjustmentList_FourPoints_WithNewPointBesideSecondPoint()
        {
            AddOnePointBesideSecondPoint();
            IEnumerable<CoverageAdjustment> results = _coveragePoints.GenerateAdjustmentList(
                mockCellRepository.Object, _eNodebs);
            IEnumerable<CoverageAdjustment> coverageAdjustments = results as CoverageAdjustment[] ?? results.ToArray();
            Assert.AreEqual(coverageAdjustments.Count(), 2);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).ENodebId, 1);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).SectorId, 2);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Frequency, 100);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Factor105, 4.368154, 1E-6);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Factor135m, 2.369684, 1E-6);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).ENodebId, 1);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).SectorId, 2);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).Frequency, 1825);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).Factor105, 3.624325, 1E-6);
        }

        [Test]
        public void TestGenerateAdjustmentList_FourPoints_WithNewPointFacingTheCell()
        {
            AddOnePointFacingTheCell();
            IEnumerable<CoverageAdjustment> results = _coveragePoints.GenerateAdjustmentList(
                mockCellRepository.Object, _eNodebs);
            IEnumerable<CoverageAdjustment> coverageAdjustments = results as CoverageAdjustment[] ?? results.ToArray();
            Assert.AreEqual(coverageAdjustments.Count(), 2);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).ENodebId, 1);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).SectorId, 2);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Frequency, 100);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Factor105, 4.369684, 1E-6);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Factor135m, 2.369684, 1E-6);
            Assert.AreEqual(coverageAdjustments.ElementAt(0).Factor15, 0.016774, 1E-6);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).ENodebId, 1);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).SectorId, 2);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).Frequency, 1825);
            Assert.AreEqual(coverageAdjustments.ElementAt(1).Factor105, 3.624325, 1E-6);
        }
    }
}
