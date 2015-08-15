using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Parameters.Abstract;

namespace Lte.Parameters.Service.Coverage
{
    public static class DCTestService
    {
        public static IEnumerable<AreaTestDate> QueryTestDateInfos()
        {
            IEnumerable<AreaTestDate> result;
            using (DtContextDataContext dc = new DtContextDataContext())
            {
                result = dc.AreaTestDate.ToList();
            }
            return result;
        }

        public static IEnumerable<CsvFilesInfo> QueryDtFileInfos(string district)
        {
            IEnumerable<CsvFilesInfo> result;
            using (DtContextDataContext dc = new DtContextDataContext())
            {
                result =
                    dc.CsvFilesInfo.Where(x =>
                        x.CsvFileName.IndexOf(district, StringComparison.Ordinal) >= 0).ToList();
            }
            return result;
        }

        public static IEnumerable<CsvFilesInfo> QueryDtFileInfos(string district, string keyword, string type)
        {
            IEnumerable<CsvFilesInfo> result;
            using (DtContextDataContext dc = new DtContextDataContext())
            {
                result = dc.sp_getDtFileInfos(keyword, district, type).Select(x =>
                    new CsvFilesInfo
                    {
                        CsvFileName = x.csvFileName,
                        DataType = x.dataType,
                        Direct = x.direct,
                        TestDate = x.testDate
                    }).ToList();
            }
            return result;
        }

        public static IEnumerable<RasterInfo> QueryRasterInfos(string town, string type)
        {
            IEnumerable<RasterInfo> result;
            using (DtContextDataContext dc = new DtContextDataContext())
            {
                switch (type)
                {
                    case "2G":
                        result = dc.sp_get2GRasterInfos(town).Select(x =>
                            new RasterInfo
                            {
                                Area = x.area,
                                Coordinate0 = x.coordinate0,
                                Coordinate1 = x.coordinate1,
                                Coordinate2 = x.coordinate2,
                                Coordinate3 = x.coordinate3,
                                CsvFilesName2G = x.csvFilesName2G,
                                CsvFilesName3G = x.csvFilesName3G,
                                CsvFilesName4G = x.csvFilesName4G,
                                RasterNum = x.rasterNum
                            }).ToList();
                        break;
                    case "3G":
                        result = dc.sp_get3GRasterInfos(town).Select(x =>
                            new RasterInfo
                            {
                                Area = x.area,
                                Coordinate0 = x.coordinate0,
                                Coordinate1 = x.coordinate1,
                                Coordinate2 = x.coordinate2,
                                Coordinate3 = x.coordinate3,
                                CsvFilesName2G = x.csvFilesName2G,
                                CsvFilesName3G = x.csvFilesName3G,
                                CsvFilesName4G = x.csvFilesName4G,
                                RasterNum = x.rasterNum
                            }).ToList();
                        break;
                    default:
                        result = dc.sp_get4GRasterInfos(town).Select(x =>
                            new RasterInfo
                            {
                                Area = x.area,
                                Coordinate0 = x.coordinate0,
                                Coordinate1 = x.coordinate1,
                                Coordinate2 = x.coordinate2,
                                Coordinate3 = x.coordinate3,
                                CsvFilesName2G = x.csvFilesName2G,
                                CsvFilesName3G = x.csvFilesName3G,
                                CsvFilesName4G = x.csvFilesName4G,
                                RasterNum = x.rasterNum
                            }).ToList();
                        break;
                }
            }
            return result;
        }

        private static IEnumerable<TRecord> QueryFileRecordsFromPoints<TRecord>(
            IEnumerable<IGeoPointReadonly<double>> points, double range, 
            Func<IEnumerable<RasterInfo>, IEnumerable<Tuple<string,int>>> rasterInfosGetter,
            Func<string, int, IEnumerable<TRecord>> recordsGetter)
        {
            List<TRecord> result = new List<TRecord>();
            using (DtContextDataContext dc = new DtContextDataContext())
            {
                IEnumerable<Tuple<string, int>> rasterInfos
                    = rasterInfosGetter(dc.RasterInfo.ToList().Query(points, range));
                foreach (Tuple<string, int> info in rasterInfos)
                {
                    result.AddRange(recordsGetter(info.Item1, info.Item2));
                }
            }
            return result;
        }

        public static IEnumerable<FileRecords2G> Query2GFileRecords(IEnumerable<IGeoPointReadonly<double>> points,
            double range)
        {
            return QueryFileRecordsFromPoints(points, range, x => x.Query2GRasterInfos(), Query2GFileRecords);
        }

        public static IEnumerable<FileRecords3G> Query3GFileRecords(IEnumerable<IGeoPointReadonly<double>> points,
            double range)
        {
            return QueryFileRecordsFromPoints(points, range, x => x.Query3GRasterInfos(), Query3GFileRecords);
        }

        public static IEnumerable<FileRecords4G> Query4GFileRecords(IEnumerable<IGeoPointReadonly<double>> points,
            double range)
        {
            return QueryFileRecordsFromPoints(points, range, x => x.Query4GRasterInfos(), Query4GFileRecords);
        }

        public static IEnumerable<FileRecords2G> Query2GFileRecords(string fileName)
        {
            return QueryFileRecords<FileRecords2G>(fileName, "sp_get2GFileContents");
        }

        public static IEnumerable<FileRecords2G> Query2GFileRecords(string fileName, int rasterNum)
        {
            return QueryFileRecords<FileRecords2G>(fileName, "sp_get2GFileContentsRasterConsidered", rasterNum);
        }

        public static IEnumerable<FileRecords3G> Query3GFileRecords(string fileName)
        {
            return QueryFileRecords<FileRecords3G>(fileName, "sp_get3GFileContents");
        }

        public static IEnumerable<FileRecords3G> Query3GFileRecords(string fileName, int rasterNum)
        {
            return QueryFileRecords<FileRecords3G>(fileName, "sp_get3GFileContentsRasterConsidered", rasterNum);
        }

        public static IEnumerable<FileRecords4G> Query4GFileRecords(string fileName)
        {
            return QueryFileRecords<FileRecords4G>(fileName, "sp_get4GFileContents");
        }

        public static IEnumerable<FileRecords4G> Query4GFileRecords(string fileName, int rasterNum)
        {
            return QueryFileRecords<FileRecords4G>(fileName, "sp_get4GFileContentsRasterConsidered", rasterNum);
        }

        private static List<TRecords> QueryFileRecords<TRecords>(string fileName, string spName,
            int rasterNum = -1)
            where TRecords: class, IDataReaderImportable, new()
        {
            List<TRecords> result = new List<TRecords>();
            using (DtContextDataContext dc = new DtContextDataContext())
            {
                SqlConnection conn = new SqlConnection(dc.Connection.ConnectionString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@tableName",
                        SqlDbType = SqlDbType.VarChar,
                        Value = fileName
                    });
                    if (rasterNum >= 0)
                        cmd.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "@rasterNum",
                            SqlDbType = SqlDbType.Int,
                            Value = rasterNum
                        });
                    using (DbDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            TRecords item = new TRecords();
                            item.Import(dr);
                            result.Add(item);
                        }
                    }
                }
                conn.Close();
            }
            return result;
        }
    }
}
