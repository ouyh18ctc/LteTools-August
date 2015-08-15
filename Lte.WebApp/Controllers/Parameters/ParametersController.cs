using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lte.Evaluations.Service;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.WebApp.Controllers.Parameters
{
    public class ParametersController : Controller
    {
        private readonly ITownRepository _townRepository;
        private readonly IENodebRepository _eNodebRepository;
        private readonly ICellRepository _cellRepository;
        private readonly IBtsRepository _btsRepository;
        private readonly ICdmaCellRepository _cdmaCellRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IENodebPhotoRepository _photoRepository;

        public ParametersController(
            ITownRepository townRepository,
            IENodebRepository eNodebRepository,
            ICellRepository cellRepository,
            IBtsRepository btsRepository,
            ICdmaCellRepository cdmaCellRepository,
            IRegionRepository regionRepository,
            IENodebPhotoRepository photoRepository)
        {
            _townRepository = townRepository;
            _eNodebRepository = eNodebRepository;
            _cellRepository = cellRepository;
            _btsRepository = btsRepository;
            _cdmaCellRepository = cdmaCellRepository;
            _regionRepository = regionRepository;
            _photoRepository = photoRepository;
        }

        public ViewResult List(ParametersContainer container)
        {
            container.ImportTownENodebStats(_townRepository, _eNodebRepository, _regionRepository);
            return View(container.TownENodebStats);
        }

        public ViewResult ENodebList(int townId, int page = 1)
        {
            ENodebListViewModel viewModel 
                = new ENodebListViewModel(_eNodebRepository, _townRepository, townId, page);
            return View(viewModel);
        }

        public ViewResult Query()
        {
            ENodebQueryViewModel viewModel = new ENodebQueryViewModel();
            viewModel.InitializeTownList(_townRepository);
            return View(viewModel);
        }

        [HttpPost]
        public ViewResult Query(ENodebQueryViewModel viewModel)
        {
            ParametersContainer.QueryENodebs = viewModel.ENodebs = _eNodebRepository.GetAllWithNames(_townRepository,
                viewModel, viewModel.ENodebName, viewModel.Address);

            viewModel.InitializeTownList(_townRepository, viewModel);
            if (viewModel.ENodebs != null)
            {
                return View(viewModel);
            } 
            
            viewModel = new ENodebQueryViewModel();
            viewModel.InitializeTownList(_townRepository);
            return View(viewModel);
        }

        public ActionResult ENodebEdit(int eNodebId)
        {
            ENodebDetailsViewModel viewModel = new ENodebDetailsViewModel();
            viewModel.Import(eNodebId, _eNodebRepository, _cellRepository, _btsRepository, _cdmaCellRepository,
                _photoRepository);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UpdateENodebInfo(ENodeb item)
        {
            ENodeb eNodeb = _eNodebRepository.GetAll().FirstOrDefault(x => x.ENodebId == item.ENodebId);
            if (eNodeb == null) return View("ENodebEdit", new ENodebDetailsViewModel());
            eNodeb.Address = item.Address;
            eNodeb.Name = item.Name;
            eNodeb.Factory = item.Factory;
            ENodebDetailsViewModel viewModel = new ENodebDetailsViewModel();
            viewModel.Import(eNodeb.ENodebId, _eNodebRepository, _cellRepository, _btsRepository, _cdmaCellRepository,
                _photoRepository);
            return View("ENodebEdit",viewModel);
        }

        [HttpPost]
        public ActionResult UpdateBtsInfo(CdmaBts item)
        {
            CdmaBts bts = _btsRepository.GetAll().FirstOrDefault(x => x.ENodebId == item.ENodebId);
            if (bts == null) return View("ENodebEdit", new ENodebDetailsViewModel());
            bts.Address = item.Address;
            bts.Name = item.Name;
            _btsRepository.Update(bts);
            ENodebDetailsViewModel viewModel = new ENodebDetailsViewModel();
            viewModel.Import(bts.ENodebId, _eNodebRepository, _cellRepository, _btsRepository, _cdmaCellRepository,
                _photoRepository);
            return View("ENodebEdit", viewModel);
        }

        [HttpPost]
        public ActionResult UpdateImage()
        {
            int eNodebId = int.Parse(Request["ENodeb.ENodebId"]);
            string name = Request["ENodeb.Name"];
            if (Request.Files["btsImage"] != null && !string.IsNullOrEmpty(Request.Files["btsImage"].FileName))
            {
                HttpImporter btsImporter = new ImageFileImporter(Request.Files["btsImage"], name);
                ENodebPhoto btsPhoto = _photoRepository.Photos.FirstOrDefault(
                    x => x.ENodebId == eNodebId && x.SectorId == 255 && x.Angle == -1);
                if (btsPhoto == null)
                {
                    btsPhoto = new ENodebPhoto
                    {
                        ENodebId = eNodebId,
                        SectorId = 255,
                        Angle = -1,
                        Path = btsImporter.FilePath
                    };
                    _photoRepository.AddOnePhoto(btsPhoto);
                }
                _photoRepository.SaveChanges();
            }
            IEnumerable<Cell> cells = _cellRepository.GetAll().Where(x => x.ENodebId == eNodebId).ToList();
            foreach (Cell cell in cells)
            {
                HttpPostedFileBase file = Request.Files["cellImage-" + cell.SectorId];
                if (file != null && !string.IsNullOrEmpty(file.FileName))
                {
                    HttpImporter cellImporter=new ImageFileImporter(file,name);
                    byte sectorId = cell.SectorId;
                    ENodebPhoto cellPhoto = _photoRepository.Photos.FirstOrDefault(
                        x => x.ENodebId == eNodebId && x.SectorId == sectorId && x.Angle == -1);
                    if (cellPhoto == null)
                    {
                        cellPhoto = new ENodebPhoto
                        {
                            ENodebId = eNodebId,
                            SectorId = sectorId,
                            Angle = -1,
                            Path = cellImporter.FilePath
                        };
                        _photoRepository.AddOnePhoto(cellPhoto);
                    }
                    _photoRepository.SaveChanges();
                }
            }
            ENodebDetailsViewModel viewModel = new ENodebDetailsViewModel();
            viewModel.Import(eNodebId, _eNodebRepository, _cellRepository, _btsRepository, _cdmaCellRepository,
                _photoRepository);
            return View("ENodebEdit", viewModel);
        }

        public ViewResult CellList(int eNodebId)
        {
            ENodebDetailsViewModel viewModel = new ENodebDetailsViewModel();
            viewModel.Import(eNodebId, _eNodebRepository, _cellRepository, _btsRepository, _cdmaCellRepository,
                _photoRepository);
            return View(viewModel);
        }

        public JsonResult GetDistrictENodebsStat(ParametersContainer container, string cityName)
        {
            if (container.TownENodebStats == null)
            { container.ImportTownENodebStats(_townRepository, _eNodebRepository, _regionRepository); }

            return Json(container.GetENodebsByDistrict(cityName).Select(
                x => new { D = x.Key, N = x.Value }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTownENodebsStat(ParametersContainer container,
            string cityName, string districtName)
        {
            if (container.TownENodebStats == null)
            { container.ImportTownENodebStats(_townRepository, _eNodebRepository, _regionRepository); }

            return Json(container.GetENodebsByTown(cityName, districtName).Select(
                x => new { T = x.Key, N = x.Value }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetENodebImage(int eNodebId)
        {
            ENodebPhoto photo =
                _photoRepository.Photos.FirstOrDefault(x => x.ENodebId == eNodebId && x.SectorId == 255 && x.Angle == -1);
            string path = (photo == null) ? "" : photo.Path;
            return File(path, "image/jpg");
        }

        public ActionResult GetCellImage(int eNodebId, byte sectorId)
        {
            ENodebPhoto photo =
                _photoRepository.Photos.FirstOrDefault(x => x.ENodebId == eNodebId && x.SectorId == sectorId && x.Angle == -1);
            string path = (photo == null) ? "" : photo.Path;
            return File(path, "image/jpg");
        }
    }
}
