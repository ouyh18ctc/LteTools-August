using System;
using System.Collections.Generic;
using System.Data;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Abstract;
using Lte.Parameters.Service.Public;

namespace Lte.Parameters.Entities
{
    public class BtsExcelBase
    {
        [LteExcelColumn(Name = "eNodeBName")]
        [CdmaExcelColumn(Name = "基站名称")]
        public string Name { get; set; }

        [LteExcelColumn(Name = "区域")]
        [CdmaExcelColumn(Name = "行政区域")]
        public string DistrictName { get; set; }

        [LteExcelColumn(Name = "镇区")]
        [CdmaExcelColumn(Name = "所属镇区")]
        public string TownName { get; set; }

        [LteExcelColumn(Name = "经度", DefaultValue = "113.0001")]
        [CdmaExcelColumn(Name = "经度", DefaultValue = "113.0001")]
        public double Longtitute { get; set; }

        [LteExcelColumn(Name = "纬度", DefaultValue = "23.0001")]
        [CdmaExcelColumn(Name = "纬度", DefaultValue = "23.0001")]
        public double Lattitute { get; set; }

        [LteExcelColumn(Name = "地址")]
        [CdmaExcelColumn(Name = "地址")]
        public string Address { get; set; }

    }

    public class ENodebExcel : BtsExcelBase, IValueImportable, ITown
    {
        [LteExcelColumn(Name = "地市")]
        public string CityName { get; set; }

        [LteExcelColumn(Name = "网格")]
        public string Grid { get; set; }

        [LteExcelColumn(Name = "厂家")]
        public string Factory { get; set; }

        public IpAddress Ip { get; set; }

        [LteExcelColumn(Name = "eNodeB ID", DefaultValue = "1")]
        public int ENodebId { get; set; }

        public string IpString 
        { 
            get 
            { 
                return Ip.AddressString; 
            } 
        }

        public IpAddress Gateway { get; set; }

        public string GatewayString 
        { 
            get { return Gateway.AddressString; } 
        }

        [LteExcelColumn(Name = "规划编号(设计院)")]
        public string PlanNum { get; set; }

        [LteExcelColumn(Name = "制式")]
        public string DivisionDuplex { get; set; }

        [LteExcelColumn(Name = "入网日期")]
        public DateTime OpenDate { get; set; }

        public ENodebExcel() 
        {
            DivisionDuplex = "FDD";
            Gateway = new IpAddress("0.0.0.0");
            Ip = new IpAddress("0.0.0.0");
        }

        private ReadExcelValueService<ENodebExcel, LteExcelColumnAttribute> service;

        public ENodebExcel(IDataReader tableReader) : this()
        {
            service = new ReadExcelValueService<ENodebExcel, LteExcelColumnAttribute>(this, tableReader);
            Ip = new IpAddress(tableReader.GetField("IP"));
            Gateway = new IpAddress(tableReader.GetField("网关"));
        }

        public virtual void Import()
        {
            service.Import();
        }
    }

    public class BtsExcel : BtsExcelBase, IValueImportable
    {
        [CdmaExcelColumn(Name = "基站编号", DefaultValue = "1")]
        public int BtsId { get; set; }

        [CdmaExcelColumn(Name = "BSC编号", DefaultValue = "1")]
        public short BscId { get; set; }

        private ReadExcelValueService<BtsExcel, CdmaExcelColumnAttribute> service;

        public BtsExcel(){ }

        public BtsExcel(IDataReader tableReader)
        {
            service = new ReadExcelValueService<BtsExcel, CdmaExcelColumnAttribute>(this, tableReader);
        }

        public virtual void Import()
        { 
            service.Import();
        }
    }

    public class ENodebExcelComparer : IEqualityComparer<ENodebExcel>
    {
        public bool Equals(ENodebExcel x, ENodebExcel y)
        {
            if (x == null) return y == null;
            return x.ENodebId == y.ENodebId;
        }

        public int GetHashCode(ENodebExcel obj)
        {
            return obj == null ? 0 : obj.ENodebId.GetHashCode();
        }
    }

    public class ENodebExcelNameComparer : IEqualityComparer<ENodebExcel>
    {
        public bool Equals(ENodebExcel x, ENodebExcel y)
        {
            if (x == null) return y == null;
            return x.Name == y.Name;
        }

        public int GetHashCode(ENodebExcel obj)
        {
            return obj == null ? 0 : obj.Name.GetHashCode();
        }
    }

    public class BtsExcelComparer : IEqualityComparer<BtsExcel>
    {
        public bool Equals(BtsExcel x, BtsExcel y)
        {
            if (x == null) return y == null;
            return x.BtsId == y.BtsId;
        }

        public int GetHashCode(BtsExcel obj)
        {
            return obj == null ? 0 : obj.BtsId.GetHashCode();
        }
    }
}
