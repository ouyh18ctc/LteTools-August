using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lte.Domain.Geo.Abstract;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;
using Lte.WebApp.Models;

namespace Lte.WebApp.Controllers.Parameters
{
    public class RegionController : Controller
    {
        private readonly ITownRepository townRepository;
        private readonly IENodebRepository eNodebRepository;
        private readonly IRegionRepository regionRepository;
        private readonly IBtsRepository btsRepository;

        public RegionController(ITownRepository townRepository, IENodebRepository eNodebRepository,
            IBtsRepository btsRepository, IRegionRepository regionRepository)
        {
            this.townRepository = townRepository;
            this.eNodebRepository = eNodebRepository;
            this.regionRepository = regionRepository;
            this.btsRepository = btsRepository;
        }

        [Authorize]
        public ViewResult Region()
        {
            RegionViewModel viewModel = TempData["RegionViewModel"] as RegionViewModel;
            if (viewModel == null || viewModel.CityName == null)
            {
                List<Town> townList = townRepository.GetAllList();
                Town town = (townList.Count > 0) ? townList.First() : null;

                viewModel = new RegionViewModel("addTown");
                viewModel.InitializeTownList(townRepository, town);
                viewModel.InitializeRegionList(regionRepository, town);
            }
            else
            {
                viewModel.InitializeTownList(townRepository, viewModel);
                viewModel.InitializeRegionList(regionRepository, viewModel);
            }

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("RegionProcess")]
        [OnlyIfPostedFromButton(SubmitButton = "addTown", ViewModelSubmitButton = "SubmitButtonName")]
        public ActionResult AddTown(RegionViewModel viewModel)
        {
            Town addConditions = viewModel.AddTownConditions;
            if (!addConditions.IsAddConditionsValid())
            {
                TempData["error"] = "输入有误！城市、区域、镇区都不能为空。";
                return RedirectToAction("Region");
            }
            TownOperationService service = new TownOperationService(townRepository, addConditions);
            service.SaveOneTown();
            TempData["success"] = "增加镇街:" + addConditions.GetAddConditionsInfo() + "成功";
            TempData["RegionViewModel"] = new RegionViewModel("addTown")
            {
                CityName = addConditions.CityName,
                DistrictName = addConditions.DistrictName,
                TownName = addConditions.TownName,
                RegionName = viewModel.RegionName
            };
            return RedirectToAction("Region");
        }

        [HttpPost]
        [ActionName("RegionProcess")]
        [OnlyIfPostedFromButton(SubmitButton = "deleteTown", ViewModelSubmitButton = "SubmitButtonName")]
        public ActionResult DeleteTown(RegionViewModel viewModel)
        {
            TownOperationService service = new TownOperationService(townRepository, viewModel);
            bool result = service.DeleteOneTown(eNodebRepository, btsRepository);
            if (result)
            {
                TempData["success"] = viewModel.DeleteSuccessMessage;
            }
            else
            {
                TempData["error"] = viewModel.DeleteFailMessage;
            }
            return RedirectToAction("Region");
        }

        [HttpPost]
        [ActionName("RegionProcess")]
        [OnlyIfPostedFromButton(SubmitButton = "modifyRegion", ViewModelSubmitButton = "SubmitButtonName")]
        public ActionResult ModifyRegion(RegionViewModel viewModel)
        {
            RegionOperationService service = new RegionOperationService(regionRepository, 
                viewModel.CityName, viewModel.DistrictName, viewModel.RegionName);
            bool result = service.SaveOneRegion(viewModel.ForceSwapRegionDistricts);
            if (result)
            {
                TempData["success"] = viewModel.SaveRegionSuccessMessage;
            }
            else
            {
                TempData["error"] = viewModel.SaveRegionFailMessage;
            }
            TempData["RegionViewModel"] = new RegionViewModel("modifyRegion")
            {
                CityName = viewModel.CityName,
                DistrictName = viewModel.DistrictName,
                TownName = viewModel.TownName,
                RegionName = viewModel.RegionName
            };
            return RedirectToAction("Region");
        }

        [HttpPost]
        [ActionName("RegionProcess")]
        [OnlyIfPostedFromButton(SubmitButton = "deleteRegion", ViewModelSubmitButton = "SubmitButtonName")]
        public ActionResult DeleteRegion(RegionViewModel viewModel)
        {
            RegionOperationService service = new RegionOperationService(regionRepository,
                viewModel.CityName, viewModel.DistrictName, viewModel.RegionName);
            bool result = service.DeleteOneRegion();
            if (result)
            {
                TempData["success"] = viewModel.DeleteRegionSuccessMessage;
            }
            else
            {
                TempData["error"] = viewModel.DeleteRegionFailMessage;
            }
            return RedirectToAction("Region");
        }
    }
}
