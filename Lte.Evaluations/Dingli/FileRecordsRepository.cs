using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.Geo.Abstract;
using Lte.Parameters;
using Lte.Parameters.Service.Coverage;

namespace Lte.Evaluations.Dingli
{
    public static class FileRecordsRepository
    {
        public static List<FileRecords2G> FileRecords2GList { get; set; }

        public static List<FileRecords3G> FileRecords3GList { get; set; }

        public static List<FileRecords4G> FileRecords4GList { get; set; }

        public static void Update2GList(List<FileRecords2G> list)
        {
            FileRecords2GList = list;
            FileRecords3GList = null;
            FileRecords4GList = null;
        }

        public static void Update3GList(List<FileRecords3G> list)
        {
            FileRecords2GList = null;
            FileRecords3GList = list;
            FileRecords4GList = null;
        }

        public static void Update4GList(List<FileRecords4G> list)
        {
            FileRecords2GList = null;
            FileRecords3GList = null;
            FileRecords4GList = list;
        }

        public static void UpdateCoverageInfos(IEnumerable<IGeoPointReadonly<double>> points,
            double range)
        {
            FileRecords2GList = DCTestService.Query2GFileRecords(points, range).ToList();
            FileRecords3GList = DCTestService.Query3GFileRecords(points, range).ToList();
            FileRecords4GList = DCTestService.Query4GFileRecords(points, range).ToList();
        }
    }

    public class DtRecordPoint
    {
        public double Lon { get; set; }

        public double Lat { get; set; }
    }
}
