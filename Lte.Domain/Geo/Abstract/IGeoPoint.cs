using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Entities;

namespace Lte.Domain.Geo.Abstract
{
    public interface IGeoPointReadonly<out T>
    {
        T Longtitute { get; }

        T Lattitute { get; }
    }

    public interface IGeoPoint<T>
    {
        T Longtitute { get; set; }
        T Lattitute { get; set; }
    }

    public static class GeoPointOperations
    {
        public static IGeoPoint<double> GetLeftBottomOffsetPoint(
            this IGeoPoint<double> center, IEnumerable<IGeoPoint<double>> pointList,
            double defaultLongtituteOffset, double defaultLattituteOffset)
        {
            var geoPoints = pointList as IGeoPoint<double>[] ?? pointList.ToArray();
            var leftPoints = geoPoints.GetLeftPoints(center);
            var longtituteOffset = leftPoints.GetLongtituteOffset(center, defaultLongtituteOffset);
            var bottomPoints = geoPoints.GetBottomPoints(center);
            var lattituteOffset = bottomPoints.GetLattituteOffset(center, defaultLattituteOffset);

            return new GeoPoint(center, longtituteOffset, lattituteOffset);
        }

        private static double GetLattituteOffset(this IGeoPoint<double>[] bottomPoints,
            IGeoPoint<double> center, double defaultLattituteOffset)
        {
            double lattituteOffset = (bottomPoints.Any())
                ? (bottomPoints.Select(x => x.Lattitute).Average() - center.Lattitute) * 2
                : defaultLattituteOffset;
            return lattituteOffset;
        }

        private static double GetLongtituteOffset(this IGeoPoint<double>[] leftPoints,
            IGeoPoint<double> center, double defaultLongtituteOffset)
        {
            double longtituteOffset = (leftPoints.Any())
                ? (leftPoints.Select(x => x.Longtitute).Average() - center.Longtitute) * 2
                : defaultLongtituteOffset;
            return longtituteOffset;
        }

        public static IGeoPoint<double>[] GetBottomPoints(this IGeoPoint<double>[] geoPoints,
            IGeoPoint<double> center)
        {
            IEnumerable<IGeoPoint<double>> bottomPoints
                = geoPoints.Where(x => x.Lattitute <= center.Lattitute);
            var enumerable = bottomPoints as IGeoPoint<double>[] ?? bottomPoints.ToArray();
            return enumerable;
        }

        public static IGeoPoint<double>[] GetLeftPoints(this IGeoPoint<double>[] geoPoints, 
            IGeoPoint<double> center)
        {
            IEnumerable<IGeoPoint<double>> leftPoints
                = geoPoints.Where(x => x.Longtitute <= center.Longtitute);
            var points = leftPoints as IGeoPoint<double>[] ?? leftPoints.ToArray();
            return points;
        }

        public static IGeoPoint<double> GetRightTopOffsetPoint(
            this IGeoPoint<double> center, IEnumerable<IGeoPoint<double>> pointList,
            double defaultLongtituteOffset, double defaultLattituteOffset)
        {
            var geoPoints = pointList as IGeoPoint<double>[] ?? pointList.ToArray();
            var rightPoints = geoPoints.GetRightPoints(center);
            double longtituteOffset = rightPoints.GetLongtituteOffset(center, defaultLongtituteOffset);
            var topPoints = geoPoints.GetTopPoints(center);
            double lattituteOffset = topPoints.GetLattituteOffset(center, defaultLattituteOffset);

            return new GeoPoint(center, longtituteOffset, lattituteOffset);
        }

        public static IGeoPoint<double>[] GetTopPoints(this IGeoPoint<double>[] geoPoints,
            IGeoPoint<double> center)
        {
            IEnumerable<IGeoPoint<double>> topPoints
                = geoPoints.Where(x => x.Lattitute >= center.Lattitute);
            var points = topPoints as IGeoPoint<double>[] ?? topPoints.ToArray();
            return points;
        }

        public static IGeoPoint<double>[] GetRightPoints(this IGeoPoint<double>[] geoPoints,
            IGeoPoint<double> center)
        {
            IEnumerable<IGeoPoint<double>> rightPoints
                = geoPoints.Where(x => x.Longtitute >= center.Longtitute);
            var enumerable = rightPoints as IGeoPoint<double>[] ?? rightPoints.ToArray();
            return enumerable;
        }
    }
}
