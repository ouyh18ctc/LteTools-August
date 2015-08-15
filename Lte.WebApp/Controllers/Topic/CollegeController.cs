using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lte.Domain.Regular;
using Lte.Evaluations.Dingli;
using Lte.Evaluations.Entities;
using Lte.Evaluations.Service;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Public;

namespace Lte.WebApp.Controllers.Topic
{
    public class CollegeController : Controller, ICollegeController
    {
        private readonly ICollegeRepository _repository;
        private readonly ITownRepository _townRepository;
        private readonly IInfrastructureRepository _infrastructureRepository;
        private readonly IIndoorDistributioinRepository _indoorDistributioinRepository;
        private readonly IENodebRepository _eNodebRepository;
        private readonly ICellRepository _cellRepository;
        private readonly IBtsRepository _btsRepository;
        private readonly ICdmaCellRepository _cdmaCellRepository;

        public CollegeController(ICollegeRepository repository, ITownRepository townRepository,
            IInfrastructureRepository infrastructureRepository, IIndoorDistributioinRepository indoorDistributioinRepository,
            IENodebRepository eNodebRepository, ICellRepository cellRepository,
            IBtsRepository btsRepository, ICdmaCellRepository cdmaCellRepository)
        {
            _repository = repository;
            _townRepository = townRepository;
            _infrastructureRepository = infrastructureRepository;
            _indoorDistributioinRepository = indoorDistributioinRepository;
            _eNodebRepository = eNodebRepository;
            _cellRepository = cellRepository;
            _btsRepository = btsRepository;
            _cdmaCellRepository = cdmaCellRepository;
        }

        public IIndoorDistributioinRepository IndoorDistributioinRepository
        {
            get { return _indoorDistributioinRepository; }
        }

        public IENodebRepository ENodebRepository
        {
            get { return _eNodebRepository; }
        }

        public ICellRepository CellRepository
        {
            get { return _cellRepository; }
        }

        public IBtsRepository BtsRepository
        {
            get { return _btsRepository; }
        }

        public ICdmaCellRepository CdmaCellRepository
        {
            get { return _cdmaCellRepository; }
        }

        public IInfrastructureRepository InfrastructureRepository
        {
            get { return _infrastructureRepository; }
        }

        public ActionResult List()
        {
            IEnumerable<CollegeInfo> infos = _repository.GetAllList();
            IEnumerable<Town> towns = _townRepository.GetAllList();
            CollegeViewModel viewModel = new CollegeViewModel
            {
                Colleges = infos.Select(x => new CollegeDto(x, towns))
            };
            return View(viewModel);
        }

        [Authorize]
        public ActionResult CollegeDetails(int id)
        {
            CollegeInfo info = _repository.Get(id);
            IEnumerable<Town> towns = _townRepository.GetAllList();
            CollegeDto dto = (info == null)
                ? new CollegeDto()
                : new CollegeDto(info, towns);
            CollegeEditViewModel viewModel = new CollegeEditViewModel
            {
                CollegeDto = dto
            };
            viewModel.Initialize(towns, new Town
            {
                CityName = dto.CityName,
                DistrictName = dto.DistrictName,
                TownName = dto.TownName
            });
            return View(viewModel);
        }

        public ActionResult CollegeCells(int id)
        {
            CollegeInfo info = _repository.Get(id);
            if (info == null)
                return View(new InfrastructureInfoViewModel(id));

            ParametersContainer.UpdateCollegeInfos(this, info);

            return View(new InfrastructureInfoViewModel(id, info.Name)
            {
                StartDate = DateTime.Today.AddDays(-7),
                EndDate = DateTime.Today
            });
        }

        [HttpPost]
        public ActionResult CollgeEdit(CollegeEditViewModel viewModel)
        {
            Town town = _townRepository.GetAll().FirstOrDefault(x =>
                x.CityName == viewModel.CollegeDto.CityName
                && x.DistrictName == viewModel.CollegeDto.DistrictName
                && x.TownName == viewModel.CollegeDto.TownName);
            int townId = town == null ? -1 : town.Id;

            CollegeInfo info = viewModel.CollegeDto.Id == -1
                ? new CollegeInfo()
                : _repository.Get(viewModel.CollegeDto.Id);
            if (info == null)
            {
                TempData["error"] = "该校园不存在。无法修改！";
                return RedirectToAction("List");
            }
            int oldTownId = info.TownId;
            string oldName = info.Name;
            viewModel.CollegeDto.CloneProperties(info);
            info.TownId = townId;
            if (viewModel.CollegeDto.Id == -1)
            {
                _repository.Insert(info);
                TempData["success"] = "新增校园" + info.Name + "信息成功！";
            }
            else
            {
                info.TownId = oldTownId;
                info.Name = oldName;
                TempData["success"] = "修改校园" + info.Name + "信息成功！";
                _repository.Update(info);
            }
            return RedirectToAction("List");
        }

        public ActionResult InfrastructureImport()
        {
            HttpPostedFileBase lteFileBase = Request.Files["lte"];
            HttpPostedFileBase cdmaFileBase = Request.Files["cdma"];
            SaveCollegeInfrastructureService service = new SaveCollegeInfrastructureService(InfrastructureRepository);
            string info = "";

            if (lteFileBase != null && lteFileBase.FileName != "")
            {
                CollegeLteExcelModel lteResults = lteFileBase.ImportLteInfos();
                int eNodebCounts = service.SaveENodebs(lteResults.BtsExcels, ENodebRepository);
                int cellCounts = service.SaveCells(lteResults.CellExcels, CellRepository);
                int lteIndoorCounts = service.SaveLteIndoorDistributions(lteResults.IndoorExcels,
                    IndoorDistributioinRepository);
                info += "新增ENodeB" + eNodebCounts + "个；新增LTE小区" + cellCounts + "个；新增LTE室分" + lteIndoorCounts + "个\n";
            }
            if (cdmaFileBase != null && cdmaFileBase.FileName != "")
            {
                CollegeCdmaExcelImporter cdmaResults = cdmaFileBase.ImportCdmaInfos();
                int btsCounts = service.SaveBtss(cdmaResults.BtsExcels, BtsRepository);
                int cdmaCellCounts = service.SaveCdmaCells(cdmaResults.CellExcels, CdmaCellRepository);
                int cdmaIndoorCounts = service.SaveCdmaIndoorDistributions(cdmaResults.IndoorExcels,
                    IndoorDistributioinRepository);
                info += "新增BTS" + btsCounts + "个；新增CDMA小区" + cdmaCellCounts + "个；新增CDMA室分" + cdmaIndoorCounts + "个";
            }
            TempData["info"] = info;
            return RedirectToAction("List");
        }

        public ActionResult CollegeCoverage(int id)
        {
            CollegeInfo info = _repository.Get(id);
            if (info == null)
                return View(new InfrastructureCoverageViewModel(id));

            ParametersContainer.UpdateCollegeInfos(this, info);
            FileRecordsRepository.UpdateCoverageInfos(ParametersContainer.QueryENodebs, 0.03);
            return View(new InfrastructureCoverageViewModel(id, info.Name));
        }
    }
}