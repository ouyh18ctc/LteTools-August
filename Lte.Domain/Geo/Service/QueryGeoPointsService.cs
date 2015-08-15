using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;

namespace Lte.Domain.Geo.Service
{
    public static class QueryGeoPointsService
    {
        public static List<TOutPoint> Query<TInPoint, TOutPoint>(this IEnumerable<TInPoint> cellList,
            double degreeSpan, double degreeInterval)
            where TInPoint : IGeoPoint<double>
            where TOutPoint : class, IGeoPoint<double>, new()
        {
            double minLattitute = cellList.Select(x => x.Lattitute).Min() - degreeSpan;
            double maxLattitute = cellList.Select(x => x.Lattitute).Max() + degreeSpan;
            List<TOutPoint> tempPointList = new List<TOutPoint>();
            for (double lattitute = minLattitute; lattitute <= maxLattitute; lattitute += degreeInterval)
            {
                IEnumerable<TInPoint> subCellList = cellList.Where(x =>
                    lattitute - degreeSpan <= x.Lattitute && x.Lattitute <= lattitute + degreeSpan);
                if (!subCellList.Any()) continue;
                double minLongtitute = subCellList.Min(x => x.Longtitute) - degreeSpan;
                double maxLongtitute = subCellList.Max(x => x.Longtitute) + degreeSpan;
                for (double longtitute = minLongtitute; longtitute <= maxLongtitute; longtitute += degreeInterval)
                {
                    tempPointList.Add(new TOutPoint {Longtitute = longtitute, Lattitute = lattitute});
                }
            }
            return tempPointList;
        }

        public static List<TOutPoint> QueryByDistance<TInPoint, TOutPoint>(this IEnumerable<TInPoint> cellList,
            double distanceInMeter, double degreeInterval)
            where TInPoint : IGeoPoint<double>
            where TOutPoint : class, IGeoPoint<double>, new()
        {
            return cellList.Query<TInPoint, TOutPoint>(distanceInMeter.GetDegreeInterval(), degreeInterval);
        }
    }
}
