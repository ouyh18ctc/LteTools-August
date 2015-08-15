using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;

namespace Lte.Domain.Geo.Entities
{
    public class GeoPoint : IGeoPoint<double>, IGeoPointReadonly<double>
    {
        public GeoPoint(double x, double y)
        {
            Longtitute = x;
            Lattitute = y;
        }

        public double Longtitute { get; set; }

        public double Lattitute { get; set; }

        public GeoPoint() { }

        public GeoPoint(IEnumerable<IGeoPoint<double>> pointList)
        {
            var geoPoints = pointList as IGeoPoint<double>[] ?? pointList.ToArray();
            Longtitute = geoPoints.Select(x => x.Longtitute).Average();
            Lattitute = geoPoints.Select(x => x.Lattitute).Average();
        }

        public GeoPoint(IGeoPoint<double> center, double longtituteOffset, double lattituteOffset)
        {
            Longtitute = center.Longtitute + longtituteOffset;
            Lattitute = center.Lattitute + lattituteOffset;
        }
    }

    public class StubGeoPoint : GeoPoint
    {
        public StubGeoPoint(double x, double y)
            : base(x, y)
        {

        }

        public StubGeoPoint(IGeoPoint<double> p, double offsetDistance, double angle = 0)
            : base(p.Longtitute, p.Lattitute)
        {
            Longtitute += offsetDistance * Math.Cos(angle * Math.PI / 180);
            Lattitute += offsetDistance * Math.Sin(angle * Math.PI / 180);
        }
    }
}
