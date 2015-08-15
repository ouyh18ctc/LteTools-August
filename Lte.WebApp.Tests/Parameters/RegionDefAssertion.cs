using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Entities;
using System.Web.Mvc;
using Lte.Parameters.Service.Region;
using NUnit.Framework;

namespace Lte.WebApp.Tests.Parameters
{
    internal static class RegionDefAssertion
    {
        public static void AssertRegionList(this ITownDefViewModel viewModel,
            IEnumerable<Town> towns, ITown town)
        {
            QueryNamesService service = new QueryDistinctCityNamesService(towns);
            Assert.AreEqual(viewModel.CityList.Count, service.QueryCount());
            SelectListItem cityItem = viewModel.CityList.FirstOrDefault(x => x.Text == town.CityName);
            if (cityItem != null)
            {
                Assert.IsTrue(cityItem.Selected);
            }
            service = new QueryDistinctDistrictNamesService(towns, town.CityName);
            Assert.AreEqual(viewModel.DistrictList.Count, service.QueryCount());
            SelectListItem districtItem = viewModel.DistrictList.FirstOrDefault(x => x.Text == town.DistrictName);
            if (districtItem != null)
            {
                Assert.IsTrue(districtItem.Selected);
            }
            service = new QueryDistinctTownNamesService(towns, town.CityName, town.DistrictName);
            Assert.AreEqual(viewModel.TownList.Count, service.QueryCount());
            SelectListItem townItem = viewModel.TownList.FirstOrDefault(x => x.Text == town.TownName);
            if (townItem != null)
            {
                Assert.IsTrue(townItem.Selected);
            }
        }
    }
}
