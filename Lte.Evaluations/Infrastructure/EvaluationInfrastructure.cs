using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Domain.Measure;
using Lte.Evaluations.Infrastructure.Entities;
using Lte.Parameters.Entities;
using Lte.Parameters.Abstract;
using Lte.Parameters.Service.Public;

namespace Lte.Evaluations.Infrastructure
{
    public class EvaluationInfrastructure
    {
        public EvaluationRegion Region { get; private set; }

        public List<EvaluationOutdoorCell> CellList { get; private set; }

        public List<MeasurePoint> MeasurePointList
        {
            get { return Region.ValidPointList.ToList(); }
        }

        private readonly QueryOutdoorCellService<EvaluationOutdoorCell> _service;

        public EvaluationInfrastructure()
        {
            CellList = new List<EvaluationOutdoorCell>();
            _service = new QueryOutdoorCellFrequencyInconsidered<EvaluationOutdoorCell>(CellList);
        }

        public EvaluationInfrastructure(IGeoPoint<double> leftBottom, IGeoPoint<double> rightTop,
            IEnumerable<EvaluationOutdoorCell> cells)
            : this()
        {
            Region = new EvaluationRegion(leftBottom, rightTop, EvaluationSettings.DistanceInMeter);
            CellList = cells.Where(x => x.Height > 0).ToList();
            Region.InitializeParameters(CellList, EvaluationSettings.DegreeSpan);
        }

        public void ImportCellList(IEnumerable<EvaluationOutdoorCell> cells)
        {
            CellList = cells.Where(x => x.Height > 0).ToList();
            Region = new EvaluationRegion(CellList, 
                EvaluationSettings.DistanceInMeter, EvaluationSettings.DegreeSpan);
            Region.InitializeParameters(CellList, EvaluationSettings.DegreeSpan);
        }

        public void ImportCellList(IENodebRepository eNodebRepository, ICellRepository cellRepository,
            IGeoPoint<double> southWest, IGeoPoint<double> northEast)
        {
            IEnumerable<ENodeb> eNodebs = eNodebRepository.GetAll().FilterGeoPointList(
                southWest.Longtitute - GeoMath.BaiduLongtituteOffset,
                southWest.Lattitute - GeoMath.BaiduLattituteOffset,
                northEast.Longtitute - GeoMath.BaiduLongtituteOffset,
                northEast.Lattitute - GeoMath.BaiduLattituteOffset).ToList();
            ImportCellList(cellRepository.Query(eNodebs));
        }

        public void AddCell(EvaluationOutdoorCell cell)
        {
            IOutdoorCell existedCell = _service.QueryCell(cell);
            if (existedCell == null)
            {
                existedCell = _service.QueryCell(cell);
                CellList.Add(cell);
                if (existedCell == null)
                {
                    Region = new EvaluationRegion(CellList, 
                        EvaluationSettings.DistanceInMeter, EvaluationSettings.DegreeSpan);
                }
                Region.InitializeParameters(CellList, EvaluationSettings.DegreeSpan);
            }
        }

        public void AddCells(IEnumerable<EvaluationOutdoorCell> cells)
        {
            foreach (EvaluationOutdoorCell evaluationOutdoorCell in cells)
            {
                AddCell(evaluationOutdoorCell);
            }
        }

        public void InitializeRegion()
        {
            if (CellList.Count <= 0) return;
            Region = new EvaluationRegion(CellList,  
                EvaluationSettings.DistanceInMeter, EvaluationSettings.DegreeSpan);
            Region.InitializeParameters(CellList, EvaluationSettings.DegreeSpan);
        }

        public void RemoveCell(string cellName)
        {
            EvaluationOutdoorCell cell = _service.QueryByName(cellName);
            if (cell == null) {
                return;
            }
            CellList.Remove(cell);
        }

    }
}
