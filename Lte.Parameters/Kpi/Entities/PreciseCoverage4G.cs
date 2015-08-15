using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.LinqToCsv;
using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Kpi.Entities
{
    public class PreciseCoverage4G : ICell, ITimeStat, IImportStat<PreciseCoverage4GCsv>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime StatTime { get; set; }

        public string DateString
        {
            get { return StatTime.ToShortDateString(); }
        }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int TotalMrs { get; set; }

        public int ThirdNeighbors { get; set; }

        public int SecondNeighbors { get; set; }

        public int FirstNeighbors { get; set; }

        public double FirstRate
        {
            get { return 100*(double) FirstNeighbors/TotalMrs; }
        }

        public double SecondRate
        {
            get { return 100*(double) SecondNeighbors/TotalMrs; }
        }

        public double ThirdRate
        {
            get { return 100*(double) ThirdNeighbors/TotalMrs; }
        }

        public void Import(PreciseCoverage4GCsv cellExcel)
        {
            cellExcel.CloneProperties(this);
            ThirdNeighbors = (int)(TotalMrs * cellExcel.ThirdNeighborRate)/100;
            SecondNeighbors = (int)(TotalMrs * cellExcel.SecondNeighborRate)/100;
            FirstNeighbors = (int)(TotalMrs * cellExcel.FirstNeighborRate)/100;
        }
    }

    public class TownPreciseCoverage4GStat : ITimeStat, ITownId, IImportStat<IEnumerable<PreciseCoverage4GCsv>>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime StatTime { get; set; }

        public int TownId { get; set; }

        public int TotalMrs { get; set; }

        public int ThirdNeighbors { get; set; }

        public int SecondNeighbors { get; set; }

        public int FirstNeighbors { get; set; }

        public void Import(IEnumerable<PreciseCoverage4GCsv> cellExcel)
        {
            if (!cellExcel.Any()) return;
            StatTime = cellExcel.ElementAt(0).StatTime;
            TotalMrs = cellExcel.Sum(x => x.TotalMrs);
            ThirdNeighbors = (int) cellExcel.Sum(x => x.TotalMrs*x.ThirdNeighborRate)/100;
            SecondNeighbors = (int) cellExcel.Sum(x => x.TotalMrs*x.SecondNeighborRate)/100;
            FirstNeighbors = (int) cellExcel.Sum(x => x.TotalMrs*x.FirstNeighborRate)/100;
        }
    }

    public class RegionPrecise4GStat : IRegionStat, IDateStat
    {
        public DateTime StatDate { get; set; }

        public string Region { get; set; }

        public int TotalMrs { get; private set; }

        public int SecondNeighbors { get; private set; }

        public double PreciseRate
        {
            get { return 1 - (double)SecondNeighbors / TotalMrs; }
        }

        public RegionPrecise4GStat()
        {
        }

        public RegionPrecise4GStat(TownPrecise4GView view)
        {
            StatDate = view.StatDate;
            Region = view.Town;
            TotalMrs = view.TotalMrs;
            SecondNeighbors = view.SecondNeighbors;
        }
    }

    public class TownPrecise4GView : IDateStat
    {
        public DateTime StatDate { get; set; }

        [Display(Name = "城市")]
        public string City { get; private set; }

        [Display(Name = "区域")]
        public string District { get; private set; }

        [Display(Name = "镇街")]
        public string Town { get; private set; }

        [Display(Name = "MR总数")]
        public int TotalMrs { get; private set; }

        [Display(Name = "精确覆盖率")]
        public double PreciseRate {
            get { return 1 - (double) SecondNeighbors/TotalMrs; }
        }

        public int SecondNeighbors { get; private set; }

        public TownPrecise4GView(TownPreciseCoverage4GStat stat, IEnumerable<Town> towns)
        {
            StatDate = stat.StatTime.Date;
            Town town = towns.FirstOrDefault(x => x.Id == stat.TownId);
            if (town == null)
            {
                City = "未知";
                District = "未知";
                Town = "未知";
            }
            else
            {
                City = town.CityName;
                District = town.DistrictName;
                Town = town.TownName;
            }
            TotalMrs = stat.TotalMrs;
            SecondNeighbors = stat.SecondNeighbors;
        }
    }

    public class MonthPreciseCoverage4GStat : ICell, IMonthStat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public short Year { get; set; }

        public byte Month { get; set; }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int TotalMrs { get; set; }

        public int ThirdNeighbors { get; set; }

        public int SecondNeighbors { get; set; }

        public int FirstNeighbors { get; set; }
    }

    public class PreciseCoverage4GCsv : ICell, ITimeStat
    {
        [CsvColumn(Name = "时间")]
        public DateTime StatTime { get; set; }

        [CsvColumn(Name = "BTS")]
        public int CellId { get; set; }

        [CsvColumn(Name = "SECTOR")]
        public byte SectorId { get; set; }

        [CsvColumn(Name = "MR总数")]
        public int TotalMrs { get; set; }

        [CsvColumn(Name = "第三强邻区MR重叠覆盖率")]
        public double ThirdNeighborRate { get; set; }

        [CsvColumn(Name = "第二强邻区MR重叠覆盖率")]
        public double SecondNeighborRate { get; set; }

        [CsvColumn(Name = "第一强邻区MR重叠覆盖率")]
        public double FirstNeighborRate { get; set; }
    }
}
