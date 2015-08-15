using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;
using Lte.WebApp.Controllers.Kpi;
using NUnit.Framework;
using Moq;

namespace Lte.WebApp.Tests.ControllerParametersQuery
{
    [TestFixture]
    public class QueryLteStatsTest
    {
        private readonly Mock<ITownRepository> townRepository = new Mock<ITownRepository>();

        private readonly Mock<ITopCellRepository<TownPreciseCoverage4GStat>> cellRepository =
            new Mock<ITopCellRepository<TownPreciseCoverage4GStat>>();

        private static void AssertValuesWithOneDistrictAndTown(string statDate, int townId, int mrs, int neighbors,
            string district)
        {
            IEnumerable<string> dates = (new QueryLteDistrictDatesController()).Get(district);
            Assert.AreEqual(dates.Count(), 1);
            Assert.AreEqual(dates.ElementAt(0), DateTime.Parse(statDate).ToShortDateString());
            IEnumerable<int> mrList = (new QueryLteMrsController()).Get(district);
            Assert.AreEqual(mrList.Count(), 1);
            Assert.AreEqual(mrList.ElementAt(0), mrs);
            IEnumerable<double> rates = (new QueryPreciseRatesController()).Get(district);
            Assert.AreEqual(rates.Count(), 1);
            Assert.AreEqual(rates.ElementAt(0), (double) 100*(mrs - neighbors)/mrs, 1E-6);
            string town = "T-" + townId;
            IEnumerable<string> towns = (new QueryLteKpiTownListController()).Get(district);
            Assert.AreEqual(towns.Count(), 1);
            Assert.AreEqual(towns.ElementAt(0), town);
            mrList = (new QueryLteMrsController()).Get(district, town);
            Assert.AreEqual(mrList.Count(), 1);
            Assert.AreEqual(mrList.ElementAt(0), mrs);
            rates = (new QueryPreciseRatesController()).Get(district, town);
            Assert.AreEqual(rates.Count(), 1);
            Assert.AreEqual(rates.ElementAt(0), (double) 100*(mrs - neighbors)/mrs, 1E-6);
        }

        [SetUp]
        public void SetUp()
        {
            townRepository.Setup(x => x.GetAll()).Returns(new List<Town>
            {
                new Town {Id = 1, DistrictName = "D-1", TownName = "T-1"},
                new Town {Id = 2, DistrictName = "D-1", TownName = "T-2"},
                new Town {Id = 3, DistrictName = "D-2", TownName = "T-3"},
                new Town {Id = 4, DistrictName = "D-2", TownName = "T-4"},
                new Town {Id = 5, DistrictName = "D-2", TownName = "T-5"}
            }.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
            KpiStatContainer.AllLteDailyStatList = null;
        }

        [TestCase("2015-01-01", 1, 100, 10, "2014-12-31", "2015-01-01", "D-1")]
        [TestCase("2015-01-01", 2, 100, 10, "2014-12-31", "2015-01-01", "D-1")]
        [TestCase("2015-01-01", 3, 100, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase("2015-01-01", 3, 1000, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase("2015-01-01", 4, 100, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase("2015-01-01", 5, 100, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase("2015-01-01", 5, 100, 20, "2014-12-31", "2015-01-04", "D-2")]
        public void Test_OneStat(string statDate,int townId,int mrs,int neighbors, string begin, string end,
            string district)
        {
            cellRepository.SetupGet(x => x.Stats).Returns(new List<TownPreciseCoverage4GStat>
            {
                new TownPreciseCoverage4GStat
                {
                    StatTime = DateTime.Parse(statDate),
                    TownId = townId,
                    TotalMrs = mrs,
                    SecondNeighbors = neighbors
                }
            }.AsQueryable());
            QueryLteStatController queryController = new QueryLteStatController(cellRepository.Object,
                townRepository.Object);
            Assert.AreEqual(queryController.Get(DateTime.Parse(begin), DateTime.Parse(end)), 1);
            Assert.AreEqual(KpiStatContainer.AllLteDailyStatList.Stats.Count(), 1);
            AssertValuesWithOneDistrictAndTown(statDate, townId, mrs, neighbors, district);
        }

        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 1, 100, 10, "2014-12-31", "2015-01-01", "D-1")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 2, 100, 10, "2014-12-31", "2015-01-01", "D-1")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 3, 100, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 3, 1000, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 4, 100, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 5, 100, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 5, 100, 20, "2014-12-31", "2015-01-04", "D-2")]
        public void Test_TwoStats_OneMatchDateRange(string[] statDates, 
            int townId, int mrs, int neighbors, string begin,
            string end,
            string district)
        {
            cellRepository.SetupGet(x => x.Stats).Returns(new List<TownPreciseCoverage4GStat>
            {
                new TownPreciseCoverage4GStat
                {
                    StatTime = DateTime.Parse(statDates[0]),
                    TownId = townId,
                    TotalMrs = mrs,
                    SecondNeighbors = neighbors
                },
                new TownPreciseCoverage4GStat
                {
                    StatTime = DateTime.Parse(statDates[1]),
                    TownId = townId,
                    TotalMrs = mrs,
                    SecondNeighbors = neighbors
                }
            }.AsQueryable());
            QueryLteStatController queryController = new QueryLteStatController(cellRepository.Object,
                townRepository.Object);
            Assert.AreEqual(queryController.Get(DateTime.Parse(begin), DateTime.Parse(end)), 1);
            Assert.AreEqual(KpiStatContainer.AllLteDailyStatList.Stats.Count(), 1);
            AssertValuesWithOneDistrictAndTown(statDates[0], townId, mrs, neighbors, district);
        }

        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 1, new[] { 100, 200 }, 10, "2014-12-31", "2015-01-01", "D-1")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 2, new[] { 100, 200 }, 10, "2014-12-31", "2015-01-01", "D-1")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 3, new[] { 100, 200 }, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 3, new[] { 1000, 2000 }, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 4, new[] { 100, 200 }, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 5, new[] { 100, 200 }, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 5, new[] { 100, 200 }, 20, "2014-12-31", "2015-01-04", "D-2")]
        public void Test_TwoStats_OneMatchDateRange_WithDifferentMrs(string[] statDates,
            int townId, int[] mrs, int neighbors, string begin,
            string end,
            string district)
        {
            cellRepository.SetupGet(x => x.Stats).Returns(new List<TownPreciseCoverage4GStat>
            {
                new TownPreciseCoverage4GStat
                {
                    StatTime = DateTime.Parse(statDates[0]),
                    TownId = townId,
                    TotalMrs = mrs[0],
                    SecondNeighbors = neighbors
                },
                new TownPreciseCoverage4GStat
                {
                    StatTime = DateTime.Parse(statDates[1]),
                    TownId = townId,
                    TotalMrs = mrs[1],
                    SecondNeighbors = neighbors
                }
            }.AsQueryable());
            QueryLteStatController queryController = new QueryLteStatController(cellRepository.Object,
                townRepository.Object);
            Assert.AreEqual(queryController.Get(DateTime.Parse(begin), DateTime.Parse(end)), 1);
            Assert.AreEqual(KpiStatContainer.AllLteDailyStatList.Stats.Count(), 1);
            AssertValuesWithOneDistrictAndTown(statDates[0], townId, mrs[0], neighbors, district);
        }

        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 1, new[] { 100, 200 }, 10, "2014-12-31", "2015-05-01", "D-1")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 2, new[] { 100, 200 }, 10, "2014-12-31", "2015-05-01", "D-1")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 3, new[] { 100, 200 }, 10, "2014-12-31", "2015-05-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 3, new[] { 1000, 2000 }, 10, "2014-12-31", "2015-05-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 4, new[] { 100, 200 }, 10, "2014-12-31", "2015-05-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 5, new[] { 100, 200 }, 10, "2014-12-31", "2015-05-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, 5, new[] { 100, 200 }, 20, "2014-12-31", "2015-05-04", "D-2")]
        public void Test_TwoStats_BothMatchDateRange_WithDifferentMrs(string[] statDates,
            int townId, int[] mrs, int neighbors, string begin,
            string end,
            string district)
        {
            cellRepository.SetupGet(x => x.Stats).Returns(new List<TownPreciseCoverage4GStat>
            {
                new TownPreciseCoverage4GStat
                {
                    StatTime = DateTime.Parse(statDates[0]),
                    TownId = townId,
                    TotalMrs = mrs[0],
                    SecondNeighbors = neighbors
                },
                new TownPreciseCoverage4GStat
                {
                    StatTime = DateTime.Parse(statDates[1]),
                    TownId = townId,
                    TotalMrs = mrs[1],
                    SecondNeighbors = neighbors
                }
            }.AsQueryable());
            QueryLteStatController queryController = new QueryLteStatController(cellRepository.Object,
                townRepository.Object);
            Assert.AreEqual(queryController.Get(DateTime.Parse(begin), DateTime.Parse(end)), 1);
            Assert.AreEqual(KpiStatContainer.AllLteDailyStatList.Stats.Count(), 1);
            IEnumerable<string> dates = (new QueryLteDistrictDatesController()).Get(district);
            Assert.AreEqual(dates.Count(), 2, "dates count");
            Assert.AreEqual(dates.ElementAt(0), DateTime.Parse(statDates[0]).ToShortDateString());
            Assert.AreEqual(dates.ElementAt(1), DateTime.Parse(statDates[1]).ToShortDateString());
            IEnumerable<int> mrList = (new QueryLteMrsController()).Get(district);
            Assert.AreEqual(mrList.Count(), 2, "District Mr List Count");
            Assert.AreEqual(mrList.ElementAt(0), mrs[0]);
            Assert.AreEqual(mrList.ElementAt(1), mrs[1]);
            IEnumerable<double> rates = (new QueryPreciseRatesController()).Get(district);
            Assert.AreEqual(rates.Count(), 2);
            Assert.AreEqual(rates.ElementAt(0), (double)100 * (mrs[0] - neighbors) / mrs[0], 1E-6);
            Assert.AreEqual(rates.ElementAt(1), (double)100 * (mrs[1] - neighbors) / mrs[1], 1E-6);
            string town = "T-" + townId;
            IEnumerable<string> towns = (new QueryLteKpiTownListController()).Get(district);
            Assert.AreEqual(towns.Count(), 1);
            Assert.AreEqual(towns.ElementAt(0), town);
            mrList = (new QueryLteMrsController()).Get(district, town);
            Assert.AreEqual(mrList.Count(), 2);
            Assert.AreEqual(mrList.ElementAt(0), mrs[0]);
            Assert.AreEqual(mrList.ElementAt(1), mrs[1]);
            rates = (new QueryPreciseRatesController()).Get(district, town);
            Assert.AreEqual(rates.Count(), 2);
            Assert.AreEqual(rates.ElementAt(0), (double)100 * (mrs[0] - neighbors) / mrs[0], 1E-6);
            Assert.AreEqual(rates.ElementAt(1), (double)100 * (mrs[1] - neighbors) / mrs[1], 1E-6);
        }

        [TestCase(new[] { "2015-01-01", "2015-03-01" }, new[] { 2, 5 }, new[] { 100, 200 }, 11,
            "2014-12-31", "2015-05-01", new[] { "D-1", "D-2" })]
        [TestCase(new[] { "2015-01-01", "2015-02-01" }, new[] { 3, 5 }, new[] { 100, 200 }, 21,
            "2014-12-31", "2015-05-04", new[] { "D-2", "D-2" })]
        [TestCase(new[] { "2015-01-01", "2015-03-15" }, new[] { 1, 2 }, new[] { 100, 200 }, 12,
            "2014-12-31", "2015-05-01", new[] { "D-1", "D-1" })]
        [TestCase(new[] { "2015-01-01", "2015-02-04" }, new[] { 2, 3 }, new[] { 100, 200 }, 13,
            "2014-12-31", "2015-05-01", new[] { "D-1", "D-2" })]
        [TestCase(new[] { "2015-01-01", "2015-02-09" }, new[] { 3, 4 }, new[] { 100, 200 }, 14,
            "2014-12-31", "2015-05-01", new[] { "D-2", "D-2" })]
        [TestCase(new[] { "2015-01-01", "2015-03-02" }, new[] { 3, 5 }, new[] { 1000, 2000 }, 15,
            "2014-12-31", "2015-05-01", new[] { "D-2", "D-2" })]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, new[] { 4, 2 }, new[] { 100, 200 }, 16,
            "2014-12-31", "2015-05-01", new[] { "D-2", "D-1" })]
        public void Test_TwoStats_BothMatchDateRange_WithDifferentMrsAndTowns(string[] statDates,
            int[] townIds, int[] mrs, int neighbors, string begin,
            string end,
            string[] districts)
        {
            cellRepository.SetupGet(x => x.Stats).Returns(new List<TownPreciseCoverage4GStat>
            {
                new TownPreciseCoverage4GStat
                {
                    StatTime = DateTime.Parse(statDates[0]),
                    TownId = townIds[0],
                    TotalMrs = mrs[0],
                    SecondNeighbors = neighbors
                },
                new TownPreciseCoverage4GStat
                {
                    StatTime = DateTime.Parse(statDates[1]),
                    TownId = townIds[1],
                    TotalMrs = mrs[1],
                    SecondNeighbors = neighbors
                }
            }.AsQueryable());
            QueryLteStatController queryController = new QueryLteStatController(cellRepository.Object,
                townRepository.Object);
            if (districts[0] == districts[1])
            {
                Assert.AreEqual(queryController.Get(DateTime.Parse(begin), DateTime.Parse(end)), 1);
                Assert.AreEqual(KpiStatContainer.AllLteDailyStatList.Stats.Count(), 1);
                IEnumerable<string> dates = (new QueryLteDistrictDatesController()).Get(districts[0]);
                Assert.AreEqual(dates.Count(), 2, "dates count");
                Assert.AreEqual(dates.ElementAt(0), DateTime.Parse(statDates[0]).ToShortDateString());
                Assert.AreEqual(dates.ElementAt(1), DateTime.Parse(statDates[1]).ToShortDateString());
                IEnumerable<int> mrList = (new QueryLteMrsController()).Get(districts[0]);
                Assert.AreEqual(mrList.Count(), 2, "District Mr List Count");
                Assert.AreEqual(mrList.ElementAt(0), mrs[0]);
                Assert.AreEqual(mrList.ElementAt(1), mrs[1]);
                IEnumerable<double> rates = (new QueryPreciseRatesController()).Get(districts[0]);
                Assert.AreEqual(rates.Count(), 2);
                Assert.AreEqual(rates.ElementAt(0), (double) 100*(mrs[0] - neighbors)/mrs[0], 1E-6);
                Assert.AreEqual(rates.ElementAt(1), (double) 100*(mrs[1] - neighbors)/mrs[1], 1E-6);

                IEnumerable<string> towns = (new QueryLteKpiTownListController()).Get(districts[0]);
                Assert.AreEqual(towns.Count(), 2);

                mrList = (new QueryLteMrsController()).Get(districts[0], "T-" + townIds[0]);
                Assert.AreEqual(mrList.Count(), 2);
                Assert.AreEqual(mrList.ElementAt(0), mrs[0], "mr0");
                Assert.AreEqual(mrList.ElementAt(1), 0, "mr0");
                mrList = (new QueryLteMrsController()).Get(districts[0], "T-" + townIds[1]);
                Assert.AreEqual(mrList.Count(), 2);
                Assert.AreEqual(mrList.ElementAt(0), 0, "mr1");
                Assert.AreEqual(mrList.ElementAt(1), mrs[1], "mr1");
                rates = (new QueryPreciseRatesController()).Get(districts[0], "T-" + townIds[0]);
                Assert.AreEqual(rates.Count(), 2);
                Assert.AreEqual(rates.ElementAt(0), (double) 100*(mrs[0] - neighbors)/mrs[0], 1E-6);
                Assert.AreEqual(rates.ElementAt(1), 0);
                rates = (new QueryPreciseRatesController()).Get(districts[0], "T-" + townIds[1]);
                Assert.AreEqual(rates.Count(), 2);
                Assert.AreEqual(rates.ElementAt(0), 0);
                Assert.AreEqual(rates.ElementAt(1), (double)100 * (mrs[1] - neighbors) / mrs[1], 1E-6);
            }
            else
            {
                Assert.AreEqual(queryController.Get(DateTime.Parse(begin), DateTime.Parse(end)), 2, "dates");
                Assert.AreEqual(KpiStatContainer.AllLteDailyStatList.Stats.Count(), 2, "statsCount");

                AssertValuesWithOneDistrictAndTown(statDates[0], townIds[0], mrs[0], neighbors, districts[0]);
                AssertValuesWithOneDistrictAndTown(statDates[1], townIds[1], mrs[1], neighbors, districts[1]);
            }
        }

        [TestCase(new[] { "2015-01-01", "2015-04-01" }, new[] { 1, 2 }, 100, 10, "2014-12-31", "2015-01-01", "D-1")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, new[] { 2, 3 }, 100, 10, "2014-12-31", "2015-01-01", "D-1")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, new[] { 3, 4 }, 100, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, new[] { 3, 5 }, 1000, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, new[] { 4, 5 }, 100, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, new[] { 5, 2 }, 100, 10, "2014-12-31", "2015-01-01", "D-2")]
        [TestCase(new[] { "2015-01-01", "2015-04-01" }, new[] { 5, 3 }, 100, 20, "2014-12-31", "2015-01-04", "D-2")]
        public void Test_TwoStats_OneMatchDateRange_WithDifferentTowns(string[] statDates,
            int[] townIds, int mrs, int neighbors, string begin,
            string end,
            string district)
        {
            cellRepository.SetupGet(x => x.Stats).Returns(new List<TownPreciseCoverage4GStat>
            {
                new TownPreciseCoverage4GStat
                {
                    StatTime = DateTime.Parse(statDates[0]),
                    TownId = townIds[0],
                    TotalMrs = mrs,
                    SecondNeighbors = neighbors
                },
                new TownPreciseCoverage4GStat
                {
                    StatTime = DateTime.Parse(statDates[1]),
                    TownId = townIds[1],
                    TotalMrs = mrs,
                    SecondNeighbors = neighbors
                }
            }.AsQueryable());
            QueryLteStatController queryController = new QueryLteStatController(cellRepository.Object,
                townRepository.Object);
            Assert.AreEqual(queryController.Get(DateTime.Parse(begin), DateTime.Parse(end)), 1);
            Assert.AreEqual(KpiStatContainer.AllLteDailyStatList.Stats.Count(), 1);
            AssertValuesWithOneDistrictAndTown(statDates[0], townIds[0], mrs, neighbors, district);
        }
    }
}
