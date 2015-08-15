using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Lte.Evaluations.Dingli;
using Lte.Parameters.Service.Coverage;

namespace Lte.WebApp.Controllers.Dt
{
    public class FileNamesController : ApiController
    {
        [Route("api/FileNames/{town}/{type}")]
        public IEnumerable<string> Get(string town, string type)
        {
            IEnumerable<string> filesList = DCTestService.QueryRasterInfos(town, type).Select(x =>
                type == "2G"
                    ? x.CsvFilesName2G.Trim()
                    : (type == "3G" ? x.CsvFilesName3G.Trim() : x.CsvFilesName4G.Trim()));
            List<string> result = new List<string>();
            foreach (string files in filesList)
            {
                result.AddRange(files.Split(';'));
            }
            return result.Distinct();
        }
    }

    public class DtFileInfo
    {
        public string TestDate { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }
    }

    public class DtFileInfosController : ApiController
    {
        [Route("api/DtFileInfos/{district}")]
        public IEnumerable<DtFileInfo> Get(string district)
        {
            IEnumerable<CsvFilesInfo> result = DCTestService.QueryDtFileInfos(district).ToList();
            return result.Select(x => new DtFileInfo
            {
                TestDate = (x.TestDate ?? DateTime.Today).ToShortDateString(),
                Type = x.DataType.Trim(),
                Name = x.CsvFileName.Trim()
            });
        }

        [Route("api/DtFileInfos/{district}/{keyword}/{type}")]
        public IEnumerable<DtFileInfo> GetByKeyword(string district, string keyword, string type = "")
        {
            IEnumerable<CsvFilesInfo> result = DCTestService.QueryDtFileInfos(district, keyword, type);
            return result.Select(x => new DtFileInfo
            {
                TestDate = (x.TestDate ?? DateTime.Today).ToShortDateString(),
                Type = x.DataType.Trim(),
                Name = x.CsvFileName.Trim()
            });
        }
    }

    public class TestDateInfosController : ApiController
    {
        [Route("api/TestDateInfos")]
        public IEnumerable<AreaTestDate> Get()
        {
            return DCTestService.QueryTestDateInfos().ToList();
        }
    }

    public class EcioPointsController : ApiController
    {
        [Route("api/EcioPoints/{low}/{high}")]
        public IEnumerable<DtRecordPoint> Get(double low, double high)
        {
            return FileRecordsRepository.FileRecords2GList == null ? new List<DtRecordPoint>() :
                FileRecordsRepository.FileRecords2GList.Where(x =>
                x.EcIo >= low && x.EcIo < high).Select(x => new DtRecordPoint
                {
                    Lon = x.BaiduLongtitute,
                    Lat = x.BaiduLattitute
                });
        }
    }

    public class RxPointsController : ApiController
    {
        [Route("api/RxPoints/{low}/{high}")]
        public IEnumerable<DtRecordPoint> Get(double low, double high)
        {
            return FileRecordsRepository.FileRecords2GList == null ? new List<DtRecordPoint>() :
                FileRecordsRepository.FileRecords2GList.Where(x =>
                x.RxAGC >= low && x.RxAGC < high).Select(x => new DtRecordPoint
                {
                    Lon = x.BaiduLongtitute,
                    Lat = x.BaiduLattitute
                });
        }
    }

    public class SinrPointsController : ApiController
    {
        [Route("api/SinrPoints/{low}/{high}")]
        public IEnumerable<DtRecordPoint> Get(double low, double high)
        {
            return FileRecordsRepository.FileRecords3GList == null ? new List<DtRecordPoint>() :
                FileRecordsRepository.FileRecords3GList.Where(x =>
                x.Sinr >= low && x.Sinr < high).Select(x => new DtRecordPoint
                {
                    Lon = x.BaiduLongtitute,
                    Lat = x.BaiduLattitute
                });
        }
    }

    public class Rx0PointsController : ApiController
    {
        [Route("api/Rx0Points/{low}/{high}")]
        public IEnumerable<DtRecordPoint> Get(double low, double high)
        {
            return FileRecordsRepository.FileRecords3GList == null ? new List<DtRecordPoint>() :
                FileRecordsRepository.FileRecords3GList.Where(x =>
                x.RxAgc0 >= low && x.RxAgc0 < high).Select(x => new DtRecordPoint
                {
                    Lon = x.BaiduLongtitute,
                    Lat = x.BaiduLattitute
                });
        }
    }

    public class Rx1PointsController : ApiController
    {
        [Route("api/Rx1Points/{low}/{high}")]
        public IEnumerable<DtRecordPoint> Get(double low, double high)
        {
            return FileRecordsRepository.FileRecords3GList == null ? new List<DtRecordPoint>() :
                FileRecordsRepository.FileRecords3GList.Where(x =>
                x.RxAgc1 >= low && x.RxAgc1 < high).Select(x => new DtRecordPoint
                {
                    Lon = x.BaiduLongtitute,
                    Lat = x.BaiduLattitute
                });
        }
    }

    public class Sinr4GPointsController : ApiController
    {
        [Route("api/Sinr4GPoints/{low}/{high}")]
        public IEnumerable<DtRecordPoint> Get(double low, double high)
        {
            return FileRecordsRepository.FileRecords4GList == null ? new List<DtRecordPoint>() :
                FileRecordsRepository.FileRecords4GList.Where(x =>
                x.Sinr >= low && x.Sinr < high).Select(x => new DtRecordPoint
                {
                    Lon = x.BaiduLongtitute,
                    Lat = x.BaiduLattitute
                });
        }
    }

    public class RsrpPointsController : ApiController
    {
        [Route("api/RsrpPoints/{low}/{high}")]
        public IEnumerable<DtRecordPoint> Get(double low, double high)
        {
            return FileRecordsRepository.FileRecords4GList == null ? new List<DtRecordPoint>() :
                FileRecordsRepository.FileRecords4GList.Where(x =>
                x.Rsrp >= low && x.Rsrp < high).Select(x => new DtRecordPoint
                {
                    Lon = x.BaiduLongtitute,
                    Lat = x.BaiduLattitute
                });
        }
    }
}
