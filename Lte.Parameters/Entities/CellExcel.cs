using System;
using System.Collections.Generic;
using System.Data;
using Lte.Domain.LinqToExcel.Entities;
using Lte.Parameters.Abstract;
using Lte.Parameters.Service.Public;

namespace Lte.Parameters.Entities
{
    public class CellExcelBase
    {
        [CloneProtection]
        [LteExcelColumn(Name = "CELL_ID", DefaultValue = "0")]
        [CdmaExcelColumn(Name = "扇区标识", DefaultValue = "0")]
        public byte SectorId { get; set; }

        [CloneProtection]
        [LteExcelColumn(Name = "频点", DefaultValue = "100")]
        [CdmaExcelColumn(Name = "频点", DefaultValue = "283")]
        public int Frequency { get; set; }

        [LteExcelColumn(Name = "是否接室分")]
        [CdmaExcelColumn(Name = "覆盖类型(室内/室外/地铁)")]
        public string IsIndoor { get; set; }

        [LteExcelColumn(Name = "经度", DefaultValue = "113.0001")]
        [CdmaExcelColumn(Name = "经度", DefaultValue = "113.0001")]
        public double Longtitute { get; set; }

        [LteExcelColumn(Name = "纬度", DefaultValue = "23.0001")]
        [CdmaExcelColumn(Name = "纬度", DefaultValue = "23.0001")]
        public double Lattitute { get; set; }

        [LteExcelColumn(Name = "天线挂高", DefaultValue = "30")]
        [CdmaExcelColumn(Name = "挂高", DefaultValue = "40")]
        public double Height { get; set; }

        [LteExcelColumn(Name = "机械下倾角", DefaultValue = "5")]
        [CdmaExcelColumn(Name = "下倾角（机械）", DefaultValue = "5")]
        public double MTilt { get; set; }

        [LteExcelColumn(Name = "电下倾角", DefaultValue = "4")]
        [CdmaExcelColumn(Name = "下倾角（电调）", DefaultValue = "4")]
        public double ETilt { get; set; }

        [LteExcelColumn(Name = "方位角", DefaultValue = "60")]
        [CdmaExcelColumn(Name = "方位角", DefaultValue = "60")]
        public double Azimuth { get; set; }

        [LteExcelColumn(Name = "天线增益", DefaultValue = "17.5")]
        [CdmaExcelColumn(Name = "天线增益（dBi）", DefaultValue = "17.5")]
        public double AntennaGain { get; set; }

        protected CellExcelBase()
        {
            IsIndoor = "否";
        }
    }

    public class CellExcel : CellExcelBase, IValueImportable
    {
        [LteExcelColumn(Name = "eNodeB ID", DefaultValue = "1")]
        public int ENodebId { get; set; }

        [LteExcelColumn(Name = "频段号", DefaultValue = "1")]
        public byte BandClass { get; set; }

        [LteExcelColumn(Name = "天线信息")]
        public string AntennaInfo { get; set; }

        [LteExcelColumn(Name = "收发类型")]
        public string TransmitReceive { get; set; }

        [LteExcelColumn(Name = "共天线信息")]
        public string ShareCdmaInfo { get; set; }

        [LteExcelColumn(Name = "PCI", DefaultValue = "0")]
        public short Pci { get; set; }

        [LteExcelColumn(Name = "根序列索引", DefaultValue = "0")]
        public short Prach { get; set; }

        [LteExcelColumn(Name = "TAC", DefaultValue = "65535")]
        public int Tac { get; set; }

        [LteExcelColumn(Name = "参考信号功率", DefaultValue = "15.2")]
        public double RsPower { get; set; }

        [LteExcelColumn(Name = "C网共站小区ID", DefaultValue = "1")]
        public string CdmaCellId { get; set; }

        private readonly ReadExcelValueService<CellExcel, LteExcelColumnAttribute> service;

        public CellExcel() { }

        public CellExcel(IDataReader tableReader)
        {
           service = new ReadExcelValueService<CellExcel,LteExcelColumnAttribute>(this, tableReader);
        }

        public virtual void Import()
        {
            service.Import();
        }
    }

    public class CdmaCellExcel : CellExcelBase, IValueImportable
    {
        [CloneProtection]
        [CdmaExcelColumn(Name = "基站编号", DefaultValue = "1")]
        public int BtsId { get; set; }

        [CloneProtection]
        [CdmaExcelColumn(Name = "小区标识", DefaultValue = "1")]
        public int CellId { get; set; }

        [CloneProtection]
        [CdmaExcelColumn(Name = "载扇类型(1X/DO)")]
        public string CellType { get; set; }

        [CloneProtection]
        [CdmaExcelColumn(Name = "LAC")]
        public string Lac { get; set; }

        [CloneProtection]
        [CdmaExcelColumn(Name = "PN码", DefaultValue = "2")]
        public short Pn { get; set; }

        public CdmaCellExcel()
        { }

        private readonly ReadExcelValueService<CdmaCellExcel, CdmaExcelColumnAttribute> service;

        public CdmaCellExcel(IDataReader tableReader)
        {
            service = new ReadExcelValueService<CdmaCellExcel, CdmaExcelColumnAttribute>(this, tableReader);
        }

        public virtual void Import()
        {
            service.Import();
        }
    }

    public class Top30CellExcel
    {
        [SimpleExcelColumn(Name = "地市")]
        public string City { get; set; }

        [SimpleExcelColumn(Name = "日期", DefaultValue = "2015-01-01")]
        public DateTime StatDate { get; set; }

        [SimpleExcelColumn(Name = "时", DefaultValue = "20")]
        public int StatHour { get; set; }

        [SimpleExcelColumn(Name = "站号", DefaultValue = "1")]
        public int BtsId { get; set; }

        [SimpleExcelColumn(Name = "扇区", DefaultValue = "0")]
        public byte SectorId { get; set; }

        [SimpleExcelColumn(Name = "载波", DefaultValue = "283")]
        public short Frequency { get; set; }

        [SimpleExcelColumn(Name = "中文名")]
        public string CellName { get; set; }
    }

    public class TopDrop2GCellExcel : Top30CellExcel, IDataReaderImportable
    {
        [SimpleExcelColumn(Name = "业务信道掉话次数", DefaultValue = "0")]
        public int Drops { get; set; }

        [SimpleExcelColumn(Name = "主叫业务信道分配成功次数", DefaultValue = "1")]
        public int MoAssignmentSuccess { get; set; }

        [SimpleExcelColumn(Name = "被叫业务信道分配成功次数", DefaultValue = "1")]
        public int MtAssignmentSuccess { get; set; }

        [SimpleExcelColumn(Name = "业务信道分配成功次数", DefaultValue = "2")]
        public int TrafficAssignmentSuccess { get; set; }

        [SimpleExcelColumn(Name = "呼叫尝试总次数", DefaultValue = "2")]
        public int CallAttempts { get; set; }

        public void Import(IDataReader tableReader)
        {
            ReadExcelTableService<TopDrop2GCellExcel, SimpleExcelColumnAttribute> service
                = new ReadExcelTableService<TopDrop2GCellExcel, SimpleExcelColumnAttribute>(this);
            service.Import(tableReader);
        }
    }

    public class TopConnection3GCellExcel : Top30CellExcel, IDataReaderImportable
    {
        [SimpleExcelColumn(Name = "无线掉线次数", DefaultValue = "0")]
        public int WirelessDrop { get; set; }

        [SimpleExcelColumn(Name = "连接尝试次数", DefaultValue = "1")]
        public int ConnectionAttempts { get; set; }

        [SimpleExcelColumn(Name = "连接失败次数", DefaultValue = "0")]
        public int ConnectionFails { get; set; }

        [SimpleExcelColumn(Name = "反向链路繁忙率", DefaultValue = "0")]
        public double LinkBusyRate { get; set; }

        public void Import(IDataReader tableReader)
        {
            ReadExcelTableService<TopConnection3GCellExcel, SimpleExcelColumnAttribute> service
                = new ReadExcelTableService<TopConnection3GCellExcel, SimpleExcelColumnAttribute>(this);
            service.Import(tableReader);
        }
    }

    public class CellExcelComparer : IEqualityComparer<CellExcel>
    {
        public bool Equals(CellExcel x, CellExcel y)
        {
            if (x == null) return y == null;
            return x.ENodebId == y.ENodebId && x.SectorId == y.SectorId && x.Frequency == y.Frequency;
        }

        public int GetHashCode(CellExcel obj)
        {
            return obj == null ? 0 : (obj.ENodebId + obj.SectorId + obj.Frequency).GetHashCode();
        }
    }

    public class CdmaCellExcelComparer : IEqualityComparer<CdmaCellExcel>
    {
        public bool Equals(CdmaCellExcel x, CdmaCellExcel y)
        {
            if (x == null) return y == null;
            return x.BtsId == y.BtsId && x.SectorId == y.SectorId && x.Frequency == y.Frequency;
        }

        public int GetHashCode(CdmaCellExcel obj)
        {
            return obj == null ? 0 : (obj.BtsId + obj.SectorId + obj.Frequency).GetHashCode();
        }
    }
}
