using System;
using System.Data;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Public;

namespace Lte.Parameters.Service.Coverage
{
    public class FileRecords2G : IDataReaderImportable
    {
        [SimpleExcelColumn(Name = "rasterNum", DefaultValue = "0")]
        public int RasterNum { get; set; }

        [SimpleExcelColumn(Name = "testTime", DefaultValue = "2015-01-01")]
        public DateTime Time { get; set; }

        [SimpleExcelColumn(Name = "lon", DefaultValue = "0")]
        public double Longtitute { get; set; }

        [SimpleExcelColumn(Name = "lat", DefaultValue = "0")]
        public double Lattitute { get; set; }

        public double BaiduLongtitute
        {
            get { return Longtitute + GeoMath.BaiduLongtituteOffset; }
        }

        public double BaiduLattitute
        {
            get { return Lattitute + GeoMath.BaiduLattituteOffset; }
        }

        [SimpleExcelColumn(Name = "refPN", DefaultValue = "0")]
        public short RefPn { get; set; }

        [SimpleExcelColumn(Name = "EcIo", DefaultValue = "0")]
        public double EcIo { get; set; }

        [SimpleExcelColumn(Name = "rxAGC", DefaultValue = "0")]
        public double RxAGC { get; set; }

        [SimpleExcelColumn(Name = "txAGC", DefaultValue = "0")]
        public double TxAGC{get;set;}

        [SimpleExcelColumn(Name = "txPower", DefaultValue = "0")]
        public double TxPower { get; set; }

        [SimpleExcelColumn(Name = "txGain", DefaultValue = "0")]
        public double TxGain { get; set; }

        public void Import(IDataReader tableReader)
        {
            ReadExcelTableService<FileRecords2G, SimpleExcelColumnAttribute> service
                = new ReadExcelTableService<FileRecords2G, SimpleExcelColumnAttribute>(this);
            service.Import(tableReader);
        }
    }

    public class FileRecords3G : IDataReaderImportable
    {
        [SimpleExcelColumn(Name = "rasterNum", DefaultValue = "0")]
        public int RasterNum { get; set; }

        [SimpleExcelColumn(Name = "testTime", DefaultValue = "2015-01-01")]
        public DateTime Time { get; set; }

        [SimpleExcelColumn(Name = "lon", DefaultValue = "0")]
        public double Longtitute { get; set; }

        [SimpleExcelColumn(Name = "lat", DefaultValue = "0")]
        public double Lattitute { get; set; }

        public double BaiduLongtitute
        {
            get { return Longtitute + GeoMath.BaiduLongtituteOffset; }
        }

        public double BaiduLattitute
        {
            get { return Lattitute + GeoMath.BaiduLattituteOffset; }
        }

        [SimpleExcelColumn(Name = "refPN", DefaultValue = "0")]
        public short RefPn { get; set; }

        [SimpleExcelColumn(Name = "SINR", DefaultValue = "0")]
        public double Sinr { get; set; }

        [SimpleExcelColumn(Name = "RxAGC0", DefaultValue = "0")]
        public double RxAgc0 { get; set; }

        [SimpleExcelColumn(Name = "RxAGC1", DefaultValue = "0")]
        public double RxAgc1 { get; set; }

        [SimpleExcelColumn(Name = "txAGC", DefaultValue = "0")]
        public double TxAGC { get; set; }

        [SimpleExcelColumn(Name = "totalC2I", DefaultValue = "0")]
        public double TotalC2I { get; set; }

        [SimpleExcelColumn(Name = "DRCValue", DefaultValue = "0")]
        public double DrcValue { get; set; }

        [SimpleExcelColumn(Name = "RLPThrDL", DefaultValue = "0")]
        public double RlpThrDl { get; set; }

        public void Import(IDataReader tableReader)
        {
            ReadExcelTableService<FileRecords3G, SimpleExcelColumnAttribute> service
                = new ReadExcelTableService<FileRecords3G, SimpleExcelColumnAttribute>(this);
            service.Import(tableReader);
        }
    }

    public class FileRecords4G : IDataReaderImportable, ILogRecord
    {
        [SimpleExcelColumn(Name = "rasterNum", DefaultValue = "0")]
        public int RasterNum { get; set; }

        [SimpleExcelColumn(Name = "testTime", DefaultValue = "2015-01-01")]
        public DateTime Time { get; set; }

        [SimpleExcelColumn(Name = "lon", DefaultValue = "0")]
        public double Longtitute { get; set; }

        [SimpleExcelColumn(Name = "lat", DefaultValue = "0")]
        public double Lattitute { get; set; }

        public double BaiduLongtitute
        {
            get { return Longtitute + GeoMath.BaiduLongtituteOffset; }
        }

        public double BaiduLattitute
        {
            get { return Lattitute + GeoMath.BaiduLattituteOffset; }
        }

        [SimpleExcelColumn(Name = "eNodeBID", DefaultValue = "0")]
        public int ENodebId { get; set; }

        [SimpleExcelColumn(Name = "cellID", DefaultValue = "0")]
        public byte SectorId { get; set; }

        [SimpleExcelColumn(Name = "freq", DefaultValue = "0")]
        public int Earfcn { get; set; }

        [SimpleExcelColumn(Name = "PCI", DefaultValue = "0")]
        public short Pci { get; set; }

        [SimpleExcelColumn(Name = "RSRP", DefaultValue = "0")]
        public double Rsrp { get; set; }

        [SimpleExcelColumn(Name = "SINR", DefaultValue = "0")]
        public double Sinr { get; set; }

        [SimpleExcelColumn(Name = "DLBler", DefaultValue = "0")]
        public double DlBler { get; set; }

        [SimpleExcelColumn(Name = "CQIave", DefaultValue = "0")]
        public double AverageCqi { get; set; }

        [SimpleExcelColumn(Name = "ULMCS", DefaultValue = "0")]
        public short UlMcs { get; set; }

        [SimpleExcelColumn(Name = "DLMCS", DefaultValue = "0")]
        public short DlMcs { get; set; }

        [SimpleExcelColumn(Name = "PDCPThrUL", DefaultValue = "0")]
        public double UlPdcpThroughput { get; set; }

        public int UlThroughput
        {
            get { return (int)UlPdcpThroughput; }
            set { UlPdcpThroughput = value; }
        }

        [SimpleExcelColumn(Name = "PDCPThrDL", DefaultValue = "0")]
        public double DlPdcpThroughput { get; set; }

        public int DlThroughput
        {
            get { return (int) DlPdcpThroughput; }
            set { DlPdcpThroughput = value; }
        }

        [SimpleExcelColumn(Name = "PHYThrDL", DefaultValue = "0")]
        public double DlPhyThroughput { get; set; }

        [SimpleExcelColumn(Name = "MACThrDL", DefaultValue = "0")]
        public double DlMacThroughput { get; set; }

        [SimpleExcelColumn(Name = "PUSCHRbNum", DefaultValue = "0")]
        public int PuschRbRate { get; set; }

        [SimpleExcelColumn(Name = "PDSCHRbNum", DefaultValue = "0")]
        public int PdschRbRate { get; set; }

        [SimpleExcelColumn(Name = "PUSCHTBSizeAve", DefaultValue = "0")]
        public int PuschTb { get; set; }

        [SimpleExcelColumn(Name = "PDSCHTBSizeAve", DefaultValue = "0")]
        public int PdschTbCode0 { get; set; }

        public int PdschTbCode1 { get; set; }

        [SimpleExcelColumn(Name = "n1PCI", DefaultValue = "0")]
        public short FirstNeighborPci { get; set; }

        [SimpleExcelColumn(Name = "n1RSRP", DefaultValue = "0")]
        public double FirstNeighborRsrp { get; set; }

        [SimpleExcelColumn(Name = "n2PCI", DefaultValue = "0")]
        public short SecondNeighborPci { get; set; }

        [SimpleExcelColumn(Name = "n2RSRP", DefaultValue = "0")]
        public double SecondNeighborRsrp { get; set; }

        [SimpleExcelColumn(Name = "n3PCI", DefaultValue = "0")]
        public short ThirdNeighborPci { get; set; }

        [SimpleExcelColumn(Name = "n3RSRP", DefaultValue = "0")]
        public double ThirdNeighborRsrp { get; set; }

        public void Import(IDataReader tableReader)
        {
            ReadExcelTableService<FileRecords4G, SimpleExcelColumnAttribute> service
                = new ReadExcelTableService<FileRecords4G, SimpleExcelColumnAttribute>(this);
            service.Import(tableReader);
        }
    }
}
