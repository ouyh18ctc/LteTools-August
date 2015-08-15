using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Lte.Domain.Geo.Abstract;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;

namespace Lte.Evaluations.ViewHelpers
{
    public interface ITownDefViewModel
    {
        List<SelectListItem> CityList { get; set; }

        List<SelectListItem> DistrictList { get; set; }

        List<SelectListItem> TownList { get; set; }
    }

    public static class ITownViewModelOperations
    {
        public static void Initialize(this ITownDefViewModel viewModel, IEnumerable<Town> towns)
        {
            QueryNamesService service = new QueryDistinctCityNamesService(towns);
            viewModel.CityList = service.Query().ToList().Select(
                    x => new SelectListItem { Value = x, Text = x }).ToList();
            viewModel.DistrictList = new List<SelectListItem>();
            viewModel.TownList = new List<SelectListItem>();
        }

        public static void Initialize(this ITownDefViewModel viewModel, IEnumerable<Town> towns, ITown town)
        {
            QueryNamesService service = new QueryDistinctCityNamesService(towns);
            viewModel.CityList
                    = service.Query().ToList().Select(
                    x => new SelectListItem { Value = x, Text = x, Selected = x == town.CityName }).ToList();
            service = new QueryDistinctDistrictNamesService(towns, town.CityName);
            viewModel.DistrictList
                = service.Query().ToList().Select(
                    x => new SelectListItem { Value = x, Text = x, Selected = x == town.DistrictName }).ToList();
            service = new QueryDistinctTownNamesService(towns, town.CityName, town.DistrictName);
            viewModel.TownList
                = service.Query().ToList().Select(
                    x => new SelectListItem { Value = x, Text = x, Selected = x == town.TownName }).ToList();
        }
    }

    public interface IRegionViewModel
    {
        string RegionName { get; set; }
    }

    public static class RegionViewModelOperations
    {
        public static void Initialize(this IRegionViewModel viewModel,
            IEnumerable<OptimizeRegion> regions, ITown town)
        {
            QueryRegionService service = new ByDistrictQueryRegionService(regions,
                town.CityName, town.DistrictName);
            OptimizeRegion region = service.Query();
            if (region != null)
            {
                viewModel.RegionName = region.Region;
            }
        }
    }
    public class RegionViewModel : ITown, ITownDefViewModel, IRegionViewModel
    {
        public RegionViewModel(string submitButtonName)
        {
            SubmitButtonName = submitButtonName;
        }

        [Display(Name = "城市")]
        public string CityName { get; set; }

        [Display(Name = "区域")]
        public string DistrictName { get; set; }

        [Display(Name = "镇区")]
        public string TownName { get; set; }

        [Display(Name = "网优片区")]
        public string RegionName { get; set; }

        public string NewCityName { get; set; }

        public string NewDistrictName { get; set; }

        public string NewTownName { get; set; }

        [Display(Name = "强制修改冲突的网优片区设定")]
        public bool ForceSwapRegionDistricts { get; set; }

        public string SubmitButtonName { get; private set; }

        public List<SelectListItem> CityList { get; set; }

        public List<SelectListItem> DistrictList { get; set; }

        public List<SelectListItem> TownList { get; set; }

        public Town AddTownConditions
        {
            get
            {
                Town town = new Town();
                if (string.IsNullOrEmpty(NewCityName))
                {
                    town.CityName = CityName;
                    town.DistrictName = (string.IsNullOrEmpty(NewDistrictName)) ? DistrictName : NewDistrictName;
                }
                else
                {
                    town.CityName = NewCityName;
                    town.DistrictName = NewDistrictName;
                }
                town.TownName = NewTownName;
                return town;
            }
        }

        public string DeleteSuccessMessage
        {
            get
            {
                return "删除镇街:" + CityName + "-" + DistrictName + "-" + TownName + "成功";
            }
        }

        public string DeleteRegionSuccessMessage
        {
            get
            {
                return "删除区域:" + CityName + "-" + DistrictName + "-" + RegionName + "成功";
            }
        }

        public string SaveRegionSuccessMessage
        {
            get
            {
                return "保存区域:" + CityName + "-" + DistrictName + "-" + RegionName + "成功";
            }
        }

        public string DeleteFailMessage
        {
            get
            {
                return "删除镇街:" + CityName + "-" + DistrictName + "-" + TownName 
                    + "失败。该镇街不存在或镇街下面还带有基站！";
            }
        }

        public string DeleteRegionFailMessage
        {
            get
            {
                return "删除区域:" + CityName + "-" + DistrictName + "-" + RegionName
                    + "失败。该区域不存在！";
            }
        }

        public string SaveRegionFailMessage
        {
            get
            {
                return "保存区域:" + (string.IsNullOrEmpty(CityName) ? "<Empty>" : CityName)
                    + "-" + (string.IsNullOrEmpty(DistrictName) ? "<Empty>" : DistrictName) 
                    + "-" + (string.IsNullOrEmpty(RegionName) ? "<Empty>" : RegionName)
                    + "失败。输入条件部分为空，或者该片区已存在，或该片区与已存在的片区有冲突，且设置不允许修改！";
            }
        }

        public void InitializeTownList(ITownRepository townRepository, ITown town)
        {
            if (town != null)
            {
                CityName = town.CityName;
                DistrictName = town.DistrictName;
                TownName = town.TownName;
                this.Initialize(townRepository.GetAll(), town);
            }
            else
            {
                this.Initialize(townRepository.GetAll());
            }
        }

        public void InitializeRegionList(IRegionRepository regionRepository, ITown town)
        {
            if (regionRepository == null) { return; }
            if (town != null)
            {
                this.Initialize(regionRepository.GetAll(), town);
            }
        }
    }
}
