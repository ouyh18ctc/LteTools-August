using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Domain.LinqToCsv;
using Lte.Domain.TypeDefs;
using Lte.Domain.Regular;
using System.ComponentModel.DataAnnotations;

namespace Lte.Parameters.Entities
{
    public class CellBase : Entity, ILteCell
    {
        [Display(Name = "基站编号")]
        public int ENodebId { get; set; }

        [Display(Name = "扇区编号")]
        public byte SectorId { get; set; }

        [Display(Name = "频点")]
        public int Frequency { get; set; }

    }

    public class Cell : CellBase, IGeoPoint<double>, IGeoPointReadonly<double>
    {
        [Display(Name = "频带编号")]
        public byte BandClass { get; set; }

        [Display(Name = "PCI")]
        public short Pci { get; set; }

        [Display(Name = "PRACH")]
        public short Prach { get; set; }

        [Display(Name = "RS功率")]
        public double RsPower { get; set; }

        public bool IsOutdoor { get; set; }

        [Display(Name = "TAC")]
        public int Tac { get; set; }

        [Display(Name = "经度")]
        public double Longtitute { get; set; }

        [Display(Name = "纬度")]
        public double Lattitute { get; set; }

        [Display(Name = "高度")]
        public double Height { get; set; }

        [Display(Name = "方位角")]
        public double Azimuth { get; set; }

        [Display(Name = "机械下倾")]
        public double MTilt { get; set; }

        [Display(Name = "电子下倾")]
        public double ETilt { get; set; }

        [Display(Name = "天线增益")]
        public double AntennaGain { get; set; }

        public string CellName
        {
            get { return ENodebId + "-" + SectorId; }
        }

        public AntennaPortsConfigure AntennaPorts { get; set; }

        public void Import(CellExcel cellExcelInfo)
        {
            cellExcelInfo.CloneProperties(this);
            
            AntennaPorts = cellExcelInfo.TransmitReceive.GetAntennaPortsConfig();
            IsOutdoor = (cellExcelInfo.IsIndoor.Trim() == "否");
        }
    }

    [Table("dbo.LteNeighborCells")]
    public class NearestPciCell : LteNeighborCell, ICell
    {
        public short Pci { get; set; }

    }

    [Table("dbo.LteNeighborCells")]
    public class LteNeighborCell : ICell
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int NearestCellId { get; set; }

        public byte NearestSectorId { get; set; }
    }

    public class LteCellRelationCsv
    {
        [CsvColumn(Name = "网元")]
        public int ENodebId { get; set; }

        [CsvColumn(Name = "小区")]
        public byte SectorId { get; set; }

        [CsvColumn(Name = "邻区关系")]
        public string NeighborRelation { get; set; }
    }

    public class EvaluationOutdoorCell : IOutdoorCell, IGeoPointReadonly<double>
    {
        public double Height { get; set; }

        public double Azimuth { get; set; }

        public double MTilt { get; set; }

        public double ETilt { get; set; }

        public double Longtitute { get; set; }

        public double Lattitute { get; set; }

        public double BaiduLongtitute
        { get { return Longtitute + GeoMath.BaiduLongtituteOffset; } }

        public double BaiduLattitute
        { get { return Lattitute + GeoMath.BaiduLattituteOffset; } }

        public double AntennaGain { get; set; }

        public double RsPower { get; set; }

        public short Pci { get; set; }

        public string CellName { get; set; }

        public int ENodebId { get; set; }

        public short SectorId { get; set; }

        public int Frequency { get; set; }

        public EvaluationOutdoorCell() { }

        public EvaluationOutdoorCell(ENodeb eNodeb, Cell cell) :
            this(eNodeb.Name, cell)
        {

        }

        public EvaluationOutdoorCell(string eNodebName, Cell cell)
        {
            cell.CloneProperties(this);
            SectorId = cell.SectorId;
            CellName = eNodebName + "-" + cell.SectorId;

        }

        public EvaluationOutdoorCell(CdmaBts bts, CdmaCell cell)
        {
            cell.CloneProperties(this);
            CellName = bts.Name + "-" + cell.SectorId;

        }
    }
}
