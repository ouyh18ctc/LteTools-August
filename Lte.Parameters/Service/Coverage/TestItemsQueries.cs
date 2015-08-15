using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;

namespace Lte.Parameters.Service.Coverage
{
    public partial class RasterInfo : IGeoPointReadonly<double>
    {
        public double Longtitute
        {
            get
            {
                return (double.Parse(Coordinate0.Split(',')[0])
                        + double.Parse(Coordinate2.Split(',')[0]))/2;
            }
        }

        public double Lattitute
        {
            get
            {
                return (double.Parse(Coordinate0.Split(',')[1])
                        + double.Parse(Coordinate1.Split(',')[1]))/2;
            }
        }
    }

    public static class TestItemsQueries
    {
        public static IEnumerable<RasterInfo> Query(this IEnumerable<RasterInfo> source,
            IGeoPointReadonly<double> center, double range)
        {
            return center.FilterGeoPointList(source, range);
        }

        public static IEnumerable<RasterInfo> Query(this IEnumerable<RasterInfo> source,
            IEnumerable<IGeoPointReadonly<double>> points, double range)
        {
            if (range <= 0) range = 0.01;
            double west = points.Any() ? points.Min(x => x.Longtitute) : 113 - range;
            double east = points.Any() ? points.Max(x => x.Longtitute) : 113 + range;
            double south = points.Any() ? points.Min(x => x.Lattitute) : 23 - range;
            double north = points.Any() ? points.Max(x => x.Lattitute) : 23 + range;
            return source.FilterGeoPointList(west, south, east, north);
        }

        private static IEnumerable<Tuple<string, int>> QueryRasterInfos(this IEnumerable<RasterInfo> source,
            Func<RasterInfo,string> fileNamesGetter)
        {
            List<Tuple<string, int>> result = new List<Tuple<string, int>>();
            foreach (RasterInfo info in source.Where(x => fileNamesGetter(x) != null))
            {
                IEnumerable<string> fileNames = fileNamesGetter(info).Split(';');
                foreach (string fileName in fileNames)
                {
                    if (result.FirstOrDefault(x => x.Item1 == fileName && x.Item2 == info.RasterNum) == null)
                    {
                        result.Add(new Tuple<string, int>(fileName, info.RasterNum ?? -1));
                    }
                }
            }
            return result;
        }

        public static IEnumerable<Tuple<string, int>> Query2GRasterInfos(this IEnumerable<RasterInfo> source)
        {
            return source.QueryRasterInfos(x => x.CsvFilesName2G);
        }

        public static IEnumerable<Tuple<string, int>> Query3GRasterInfos(this IEnumerable<RasterInfo> source)
        {
            return source.QueryRasterInfos(x => x.CsvFilesName3G);
        }

        public static IEnumerable<Tuple<string, int>> Query4GRasterInfos(this IEnumerable<RasterInfo> source)
        {
            return source.QueryRasterInfos(x => x.CsvFilesName4G);
        }
    }
}
