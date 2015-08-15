using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Service.Region;
using Lte.WebApp.Controllers.Parameters;
using System.Collections.Generic;
using Lte.Parameters.Entities;
using Lte.Parameters.Abstract;
using Moq;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;

namespace Lte.WebApp.Tests.ControllerParameters
{
    internal class QueryTestHelper
    {
        private readonly ParametersController controller;
        private readonly IEnumerable<Town> towns;
        private readonly IEnumerable<ENodeb> eNodebs;

        private ViewResult viewResult;
        private ENodebQueryViewModel viewModel;

        public QueryTestHelper(ParametersController controller, IEnumerable<Town> towns, IEnumerable<ENodeb> eNodebs)
        {
            this.controller = controller;
            this.towns = towns;
            this.eNodebs = eNodebs;
            ENodebId = 0;
            AddressId = 0;
        }

        public int ENodebId { get; set; }

        public int AddressId { get; set; }

        public void AssertTest(int cityId = 0, int districtId = 0, int townId = 0)
        {
            viewResult = controller.Query();
            viewModel = viewResult.Model as ENodebQueryViewModel;
            IEnumerable<ENodeb> resultENodebs = new List<ENodeb>();

            SetViewModelParameters(cityId, districtId, townId);
            Assert.IsNotNull(viewModel);
            if (viewModel == null) return;
            Assert.IsNotNull(viewModel.CityList);
            Assert.IsNotNull(viewModel.DistrictList);
            Assert.IsNotNull(viewModel.TownList);
            QueryNamesService service = new QueryDistinctCityNamesService(towns);
            Assert.AreEqual(viewModel.CityList.Count, service.QueryCount());
            if (cityId == 0)
            {
                Assert.IsNull(viewModel.CityName);
                Assert.IsNull(viewModel.DistrictName);
                Assert.IsNull(viewModel.TownName);
                Assert.AreEqual(viewModel.DistrictList.Count, 0);
                Assert.AreEqual(viewModel.TownList.Count, 0);
                Assert.IsNull(viewModel.ENodebs);
            }
            else
            {
                IEnumerable<Town> matchedCityTowns = towns.Where(x => x.CityName == viewModel.CityName);
                if (matchedCityTowns.Any())
                {
                    service = new QueryDistinctDistrictNamesService(towns, viewModel.CityName);
                    Assert.AreEqual(viewModel.DistrictList.Count, service.QueryCount(),
                        "Total towns are not matched!");
                    if (districtId == 0)
                    {
                        Assert.IsNull(viewModel.DistrictName);
                        Assert.IsNull(viewModel.TownName);
                        Assert.AreEqual(viewModel.TownList.Count, 0);                        
                        resultENodebs = from e in eNodebs
                            join t in matchedCityTowns on e.TownId equals t.Id
                            select e;
                    }
                    else
                    {
                        Assert.IsNotNull(viewModel.DistrictName);
                        Assert.AreEqual(viewModel.DistrictName, "District" + districtId);
                        IEnumerable<Town> matchedDistrictTowns = matchedCityTowns.Where(
                            x => x.DistrictName == viewModel.DistrictName);
                        if (matchedDistrictTowns.Any())
                        {
                            service = new QueryDistinctTownNamesService(
                                towns, viewModel.CityName, viewModel.DistrictName);
                            Assert.AreEqual(viewModel.TownList.Count, service.QueryCount(),
                                "Towns matched district are not matched!");
                            if (townId == 0)
                            {
                                Assert.IsNull(viewModel.TownName);
                                resultENodebs = from e in eNodebs
                                    join t in matchedDistrictTowns on e.TownId equals t.Id
                                    select e;
                            }
                            else
                            {
                                Assert.IsNotNull(viewModel.TownName);
                                Assert.AreEqual(viewModel.TownName, "Town" + townId);
                                Town matchedTown = matchedDistrictTowns.FirstOrDefault(x => x.TownName == viewModel.TownName);
                                if (matchedTown != null)
                                {
                                    resultENodebs = eNodebs.Where(x => x.TownId == matchedTown.Id);
                                }
                            }
                        }
                    }
                    AssertENodebs(resultENodebs);
                }
                else
                {
                    Assert.IsNull(viewModel.DistrictName);
                    Assert.IsNull(viewModel.TownName);
                    Assert.AreEqual(viewModel.DistrictList.Count, 0);
                    Assert.AreEqual(viewModel.TownList.Count, 0);
                    Assert.IsNull(viewModel.ENodebs);
                }
            }
        }

        private void AssertENodebs(IEnumerable<ENodeb> resultENodebs)
        {
            if (AddressId > 0)
            {
                resultENodebs = resultENodebs.Where(x => x.ENodebId == AddressId);
                if (resultENodebs.Any())
                {
                    Assert.AreEqual(resultENodebs.Count(), 1);
                    Assert.AreEqual(resultENodebs.ElementAt(0).Address, viewModel.Address);
                }
            }
            else if (ENodebId > 0)
            {
                resultENodebs = resultENodebs.Where(x => x.ENodebId == ENodebId);
                if (resultENodebs.Any())
                {
                    Assert.AreEqual(resultENodebs.ElementAt(0).Name, viewModel.ENodebName);
                }
            }
            if (viewModel.ENodebs != null)
            {
                Assert.AreEqual(viewModel.ENodebs.Count(), resultENodebs.Count(), "Total eNodebs are not matched!");
            }
            else
            {
                Assert.AreEqual(resultENodebs.Count(), 0);
            }
        }

        private void SetViewModelParameters(int cityId, int districtId, int townId)
        {
            if (ENodebId > 0) { viewModel.ENodebName = "ENodeb-" + ENodebId; }
            if (AddressId > 0) { viewModel.Address = "Address-" + AddressId; }
            if (cityId > 0)
            {
                viewModel.CityName = "City" + cityId;
                if (districtId > 0) { viewModel.DistrictName = "District" + districtId; }
                if (townId > 0) { viewModel.TownName = "Town" + townId; }
                viewResult = controller.Query(viewModel);
                viewModel = viewResult.Model as ENodebQueryViewModel;
            }
        }
    }

    [TestFixture]
    public class QueryTest : ParametersConfig
    {
        private readonly Mock<ITownRepository> townRepository = new Mock<ITownRepository>();
        private readonly Mock<IENodebRepository> eNodebRepository = new Mock<IENodebRepository>();
        private readonly Mock<IRegionRepository> regionRepository = new Mock<IRegionRepository>();
        private ParametersController controller;
        private QueryTestHelper helper;

        [SetUp]
        public void TestInitialize()
        {
            townRepository.Setup(x => x.GetAll()).Returns(towns.AsQueryable());
            townRepository.Setup(x => x.GetAllList()).Returns(townRepository.Object.GetAll().ToList());
            townRepository.Setup(x => x.Count()).Returns(townRepository.Object.GetAll().Count());
            eNodebRepository.Setup(x => x.GetAll()).Returns(eNodebs.AsQueryable());
            eNodebRepository.Setup(x => x.GetAllList()).Returns(eNodebRepository.Object.GetAll().ToList());
            eNodebRepository.Setup(x => x.Count()).Returns(eNodebRepository.Object.GetAll().Count());
            controller = new ParametersController(townRepository.Object, eNodebRepository.Object, null, null, null,
                regionRepository.Object, null);
            helper = new QueryTestHelper(controller, towns, eNodebs);
        }

        [Test]
        public void TestQuery_Original()
        {
            helper.AssertTest();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void TestQuery_CanSetCityName_CityId(int cityId)
        {
            helper.AssertTest(cityId);
        }

        [TestCase(1, 0, 1)]
        [TestCase(2, 0, 1)]
        [TestCase(1, 0, 2)]
        [TestCase(1, 0, 3)]
        [TestCase(0, 1, 1)]
        [TestCase(0, 2, 1)]
        [TestCase(0, 1, 2)]
        [TestCase(0, 1, 3)]
        public void TestQuery_CanSetCityName_CanSetENodebName(int eNodebId, int addressId, int cityId)
        {
            helper.ENodebId = eNodebId;
            helper.AddressId = addressId;
            helper.AssertTest(cityId);
        }

        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(1, 3)]
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        [TestCase(2, 3)]
        public void TestQuery_CanSetDistrictName(int cityId, int districtId)
        {
            helper.AssertTest(cityId, districtId);
        }

        [TestCase(1, 0, 1, 1)]
        [TestCase(2, 0, 1, 1)]
        [TestCase(1, 0, 1, 2)]
        [TestCase(2, 0, 1, 2)]
        [TestCase(1, 0, 1, 3)]
        [TestCase(2, 0, 1, 3)]
        [TestCase(1, 0, 2, 1)]
        [TestCase(2, 0, 2, 1)]
        [TestCase(1, 0, 2, 2)]
        [TestCase(2, 0, 2, 2)]
        [TestCase(1, 0, 2, 3)]
        [TestCase(2, 0, 2, 3)]
        [TestCase(0, 1, 1, 1)]
        [TestCase(0, 1, 1, 2)]
        [TestCase(0, 1, 1, 3)]
        [TestCase(0, 1, 2, 1)]
        [TestCase(0, 1, 2, 2)]
        [TestCase(0, 1, 2, 3)]
        [TestCase(0, 2, 1, 1)]
        [TestCase(0, 2, 1, 2)]
        [TestCase(0, 2, 1, 3)]
        [TestCase(0, 2, 2, 1)]
        [TestCase(0, 2, 2, 2)]
        [TestCase(0, 2, 2, 3)]
        public void TestQuery_CanSetDistrictName_CanSetENodebName(int eNodebId, int addressId,
            int cityId, int districtId)
        {
            helper.ENodebId = eNodebId;
            helper.AddressId = addressId;
            helper.AssertTest(cityId, districtId);
        }

        [TestCase(1, 1, 1)]
        [TestCase(1, 1, 2)]
        [TestCase(1, 1, 3)]
        [TestCase(1, 2, 1)]
        [TestCase(1, 2, 3)]
        [TestCase(1, 2, 4)]
        [TestCase(2, 1, 5)]
        [TestCase(2, 3, 2)]
        public void TestQuery_CanSetTownName(int cityId, int districtId, int townId)
        {
            helper.AssertTest(cityId, districtId, townId);
        }

        [TestCase(1, 0)]
        [TestCase(2, 0)]
        [TestCase(0, 1)]
        [TestCase(0, 2)]
        public void TestQuery_CanSetTownName_CanSetENodebName(int eNodebId, int addressId)
        {
            helper.ENodebId = eNodebId;
            helper.AddressId = addressId;
            for (int i = 1; i < 3; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    for (int k = 1; k < 5; k++)
                    {
                        helper.AssertTest(i, j, k);
                    }
                }
            }
        }
    }
}
