using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Measure;

namespace Lte.Domain.Geo.Service
{
    public static class GeoMath
    {
        private const double EarthRadius = 6371;

        private static double _baiduLongtituteOffset = 0.0123;

        private const double Eps = 1E-6;

        public static double BaiduLongtituteOffset
        {
            get { return _baiduLongtituteOffset; }
            set { _baiduLongtituteOffset = value; }
        }

        private static double _baiduLattituteOffset = 0.003;

        public static double BaiduLattituteOffset
        {
            get { return _baiduLattituteOffset; }
            set { _baiduLattituteOffset = value; }
        }

        public static double Distance(this IGeoPoint<double> p1, IGeoPoint<double> p2)
        {
            return (EarthRadius * Math.Acos(Math.Sin(p1.Lattitute * (Math.PI / 180)) * Math.Sin(p2.Lattitute * (Math.PI / 180))
                + Math.Cos(p1.Lattitute * (Math.PI / 180)) * Math.Cos(p2.Lattitute * (Math.PI / 180))
                * Math.Cos((p1.Longtitute - p2.Longtitute) * (Math.PI / 180))));
        }

        public static double SimpleDistance(this IGeoPoint<double> p1, IGeoPoint<double> p2)
        {
            return EarthRadius * Math.PI / 180 * Math.Sqrt((p1.Lattitute - p2.Lattitute) * (p1.Lattitute - p2.Lattitute)
                + (p1.Longtitute - p2.Longtitute) * (p1.Longtitute - p2.Longtitute));
        }

        public static double GetDegreeInterval(this double distanceInMeter)
        {
            return distanceInMeter * 180 / (EarthRadius * Math.PI * 1000);
        }

        public static double GetDistanceInMeter(this double degreeInterval)
        {
            return degreeInterval * EarthRadius * Math.PI * 1000 / 180;
        }

        public static double PositionAzimuth(this IGeoPoint<double> p, IGeoPoint<double> c)
        {
            return (Math.Abs(p.Lattitute - c.Lattitute) < Eps) ? ((p.Longtitute >= c.Longtitute) ? 90 : 270) :
                180 / Math.PI * Math.Atan2(p.Longtitute - c.Longtitute, p.Lattitute - c.Lattitute);
        }

        public static double AngleBetweenAzimuths(double a1, double a2)
        {
            double diff = Math.Abs(a1 - a2) % 360;
            return (diff <= 180) ? diff : 360 - diff;
        }

        public static double AllAngleBetweenAzimuths(double a1, double a2)
        {
            double diff = (a1 - a2) % 360;
            if (diff <= 180 && diff >= -180) { return diff; }
            if (diff > 180) { return diff - 360; }
            return diff + 360;
        }

        public static IGeoPoint<double> Move(this IGeoPoint<double> origin, double radiusInMeter, double azimuth)
        {
            double degree = radiusInMeter * 180 / 1000 / EarthRadius / Math.Cos(origin.Lattitute * Math.PI / 180);
            return new GeoPoint(origin.Longtitute + degree * Math.Sin(azimuth * Math.PI / 180),
                origin.Lattitute + degree * Math.Cos(azimuth * Math.PI / 180));
        }

        public static IEnumerable<TPoint> FilterGeoPointList<TPoint>(this IGeoPointReadonly<double> center,
            IEnumerable<TPoint> inputCellList, double degreeSpan)
            where TPoint : IGeoPointReadonly<double>
        {
            if (degreeSpan <= 0) { degreeSpan = 0.01; }
            return inputCellList.Where(s => s.Longtitute >= center.Longtitute - degreeSpan
                && s.Longtitute <= center.Longtitute + degreeSpan
                && s.Lattitute >= center.Lattitute - degreeSpan
                && s.Lattitute <= center.Lattitute + degreeSpan);
        }

        public static IEnumerable<TPoint> FilterGeoPointList<TPoint>(this IEnumerable<TPoint> inputCellList, 
            double west, double south, double east, double north)
            where TPoint : IGeoPointReadonly<double>
        {
            return inputCellList.Where(s =>
                s.Longtitute >= west && s.Lattitute >= south
                && s.Longtitute <= east && s.Lattitute <= north);
        }

        public static ComparableCell[] GenerateComparableCellList(this IGeoPoint<double> p, IOutdoorCell[] cl)
        {
            int length = cl.Length;
            ComparableCell[] ccl = new ComparableCell[length];
            for (int i = 0; i < length; i++)
            { ccl[i] = new ComparableCell(p, cl[i]); }
            return ccl;
        }
    }
}
