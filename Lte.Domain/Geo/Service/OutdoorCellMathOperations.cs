using System;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Entities;

namespace Lte.Domain.Geo.Service
{
    public static class OutdoorCellMathOperations
    {
        public static double AngleFromCellAzimuth(this IOutdoorCell c, IGeoPoint<double> p)
        {
            double pa = p.PositionAzimuth(c);
            return GeoMath.AllAngleBetweenAzimuths(pa, c.Azimuth);
        }

        public static double TiltFromCell(this IOutdoorCell c, IGeoPoint<double> p)
        {
            double d = p.SimpleDistance(c);
            return Math.Atan2(c.Height, d * 1000) * 180 / Math.PI;
        }

        public static double AngleFromCellTilt(this IOutdoorCell c, IGeoPoint<double> p)
        {
            return Math.Abs(c.MTilt + c.ETilt - c.TiltFromCell(p));
        }

        public static SectorTriangle GetSectorPoints(this IOutdoorCell outdoorCell, double radiusInMeter)
        {
            IGeoPoint<double> point1 = outdoorCell.Move(radiusInMeter, outdoorCell.Azimuth + 30);
            IGeoPoint<double> point2 = outdoorCell.Move(radiusInMeter, outdoorCell.Azimuth - 30);
            return new SectorTriangle
            {
                X1 = outdoorCell.Longtitute + GeoMath.BaiduLongtituteOffset,
                Y1 = outdoorCell.Lattitute + GeoMath.BaiduLattituteOffset,
                X2 = point1.Longtitute + GeoMath.BaiduLongtituteOffset,
                Y2 = point1.Lattitute + GeoMath.BaiduLattituteOffset,
                X3 = point2.Longtitute + GeoMath.BaiduLongtituteOffset,
                Y3 = point2.Lattitute + GeoMath.BaiduLattituteOffset,
                Info = outdoorCell.Info(),
                CellName = outdoorCell.CellName,
                ColorString = "8C8C8C"
            };
        }
    }
}
