using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Domain.Measure;
using Lte.Evaluations.Entities;
using Lte.Evaluations.Service;

namespace Lte.Evaluations.Infrastructure.Entities
{
    public class EvaluationRegion
    {
        private readonly double degreeInterval;

        public double DegreeInterval
        {
            get { return degreeInterval; }
        }

        private readonly IList<ILinkBudget<double>> budgetList = new List<ILinkBudget<double>>();

        private readonly MeasurePoint[] pointList;

        public EvaluationRegion(IGeoPoint<double> leftBottom, IGeoPoint<double> rightTop,
            double distanceInMeter)
        {
            if ((leftBottom.Longtitute >= rightTop.Longtitute) || (leftBottom.Lattitute >= rightTop.Lattitute))
            {
                throw new ArgumentOutOfRangeException("leftBottom");
            }

            degreeInterval = distanceInMeter.GetDegreeInterval();
            int xDimensionPoints = (int)Math.Ceiling((rightTop.Longtitute - leftBottom.Longtitute) / degreeInterval);
            int yDimensionPoints = (int)Math.Ceiling((rightTop.Lattitute - leftBottom.Lattitute) / degreeInterval);
            pointList = new MeasurePoint[xDimensionPoints * yDimensionPoints];

            for (int i = 0; i < xDimensionPoints; i++)
            {
                for (int j = 0; j < yDimensionPoints; j++)
                {
                    pointList[i * yDimensionPoints + j] = new MeasurePoint
                    {
                        Longtitute = leftBottom.Longtitute + i*degreeInterval,
                        Lattitute = leftBottom.Lattitute + j*degreeInterval
                    };
                }
            }
        }

        public EvaluationRegion(IEnumerable<IOutdoorCell> cellList, double distanceInMeter, double degreeSpan)
        {
            if (!cellList.Any()) return;
            degreeInterval = distanceInMeter.GetDegreeInterval();
            degreeSpan = Math.Abs(degreeSpan / 2);
            pointList = cellList.Query<IOutdoorCell, MeasurePoint> (degreeSpan, degreeInterval).ToArray();
        }

        public MeasurePoint this[int i]
        {
            get 
            {
                return (i >= Length) ? null : pointList[i];
            }
        }

        public int Length
        {
            get { return (pointList == null) ? 0 : pointList.Length; }
        }

        public IEnumerable<MeasurePoint> ValidPointList {
            get { return pointList.Where(x => x.CellRepository.CellList.Count > 0); }
        }

        public List<MeasurePointInfo> GetMeasureInfoList(StatValueField field, double distanceInMeter)
        {
            return ValidPointList.Select(x => new MeasurePointInfo(
                x, field, distanceInMeter.GetDegreeInterval())).ToList();
        }

        public List<MeasurePointInfo> GetMeasureMergedList(StatValueField field, double distanceInMeter)
        {
            MergeMeasurePointListService service = new MergeMeasurePointListService(
                ValidPointList.Select(validPoint => new MeasurePointInfo(validPoint, field,
                    distanceInMeter.GetDegreeInterval())));

            return service.Merge();
        }

        public void CalculatePerformance(double trafficLoad)
        {
            foreach (MeasurePoint point in ValidPointList)
            {
                point.CalculatePerformance(trafficLoad);
            }
        }

        public void InitializeParameters<TOutdoorCell>(List<TOutdoorCell> cellList, double degreeSpan)
            where TOutdoorCell : IOutdoorCell, IGeoPointReadonly<double>
        {
            foreach (MeasurePoint point in pointList)
            {
                IEnumerable<TOutdoorCell> tempCellList
                    = point.FilterGeoPointList(cellList, degreeSpan);
                if (tempCellList.Any())
                {
                    point.ImportCells(tempCellList as IEnumerable<IOutdoorCell>, budgetList);
                }
            }
        }
    }
}
