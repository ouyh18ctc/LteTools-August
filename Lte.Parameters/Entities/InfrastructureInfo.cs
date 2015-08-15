using System.ComponentModel.DataAnnotations;
using System.Data;
using Abp.Domain.Entities;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Service.Public;

namespace Lte.Parameters.Entities
{
    public class InfrastructureInfo : Entity
    {
        public HotspotType HotspotType { get; set; }

        public string HotspotName { get; set; }

        public InfrastructureType InfrastructureType { get; set; }

        public int InfrastructureId { get; set; }
    }

    public enum HotspotType : byte
    {
        College,
        Hospital,
        ShoppingMall,
        Building,
        Transportation
    }

    public enum InfrastructureType : byte
    {
        ENodeb,
        Cell,
        CdmaBts,
        CdmaCell,
        LteIndoor,
        CdmaIndoor
    }

    public enum RegionType : byte
    {
        Circle,
        Rectangle,
        Polygon,
        PolyLine
    }

    public class IndoorDistribution : Entity, IGeoPoint<double>
    {
        [SimpleExcelColumn(Name = "室分名称")]
        [Display(Name = "室分名称")]
        public string Name { get; set; }

        [SimpleExcelColumn(Name = "覆盖范围")]
        [Display(Name = "覆盖范围")]
        public string Range { get; set; }

        [SimpleExcelColumn(Name = "施主基站")]
        [Display(Name = "施主基站")]
        public string SourceName { get; set; }

        [SimpleExcelColumn(Name = "信源种类")]
        [Display(Name = "信源种类")]
        public string SourceType { get; set; }

        [SimpleExcelColumn(Name = "经度")]
        [Display(Name = "经度")]
        public double Longtitute { get; set; }

        [SimpleExcelColumn(Name = "纬度")]
        [Display(Name = "纬度")]
        public double Lattitute { get; set; }

        public double BaiduLongtitute
        { get { return Longtitute + GeoMath.BaiduLongtituteOffset; } }

        public double BaiduLattitute
        { get { return Lattitute + GeoMath.BaiduLattituteOffset; } }
    }

    public class CollegeIndoorExcel : IndoorDistribution, IValueImportable
    {
        [SimpleExcelColumn(Name = "学校")]
        public string CollegeName { get; set; }

        private readonly ReadExcelValueService<CollegeIndoorExcel, SimpleExcelColumnAttribute> service;

        public CollegeIndoorExcel()
        {
        }

        public CollegeIndoorExcel(IDataReader tableReader) : this()
        {
            service = new ReadExcelValueService<CollegeIndoorExcel, SimpleExcelColumnAttribute>(this, tableReader);
        }

        public void Import()
        {
            service.Import();
        }
    }

    public class CollegeBtsExcel : BtsExcel, IValueImportable
    {
        [CdmaExcelColumn(Name = "校园名称")]
        public string CollegeName { get; set; }

        public CollegeBtsExcel()
        {
        }

        private readonly ReadExcelValueService<CollegeBtsExcel, CdmaExcelColumnAttribute> service;

        public CollegeBtsExcel(IDataReader tableReader) : this()
        {
            service = new ReadExcelValueService<CollegeBtsExcel, CdmaExcelColumnAttribute>(this, tableReader);
        }

        public override void Import()
        {
            service.Import();
        }
    }

    public class CollegeENodebExcel : ENodebExcel, IValueImportable
    {
        [LteExcelColumn(Name = "校园")]
        public string CollegeName { get; set; }

        public CollegeENodebExcel()
        {
        }

        private readonly ReadExcelValueService<CollegeENodebExcel, LteExcelColumnAttribute> service;

        public CollegeENodebExcel(IDataReader tableReader) : this()
        {
            service = new ReadExcelValueService<CollegeENodebExcel, LteExcelColumnAttribute>(this, tableReader);
        }

        public override void Import()
        {
            service.Import();
        }
    }

    public class CollegeCdmaCellExcel : CdmaCellExcel, IValueImportable
    {
        [CdmaExcelColumn(Name = "校园名称")]
        public string CollegeName { get; set; }

        public CollegeCdmaCellExcel()
        {
        }

        private readonly ReadExcelValueService<CollegeCdmaCellExcel, CdmaExcelColumnAttribute> service;

        public CollegeCdmaCellExcel(IDataReader tableReader) : this()
        {
            service = new ReadExcelValueService<CollegeCdmaCellExcel, CdmaExcelColumnAttribute>(this, tableReader);
        }

        public override void Import()
        {
            service.Import();
        }
    }

    public class CollegeCellExcel : CellExcel, IValueImportable
    {
        [LteExcelColumn(Name = "校园")]
        public string CollegeName { get; set; }

        public CollegeCellExcel()
        {
        }

        private readonly ReadExcelValueService<CollegeCellExcel, LteExcelColumnAttribute> service;

        public CollegeCellExcel(IDataReader tableReader) : this()
        {
            service = new ReadExcelValueService<CollegeCellExcel, LteExcelColumnAttribute>(this, tableReader);
        }

        public override void Import()
        {
            service.Import();
        }
    }
}
