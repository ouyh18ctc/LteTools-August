using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Kpi.Entities
{
    public abstract class TopCell : ICityStat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime StatTime { get; set; }

        public string City { get; set; }

        public int BtsId { get; set; }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

    }

    public class TopDrop2GCell : TopCell, ICdmaCell, ITimeStat, IImportStat<TopDrop2GCellExcel>
    {
        public short Frequency { get; set; }

        public int Drops { get; set; }

        public int MoAssignmentSuccess { get; set; }

        public int MtAssignmentSuccess { get; set; }

        public int TrafficAssignmentSuccess { get; set; }

        public int CallAttempts { get; set; }

        public double DropRate
        {
            get
            {
                return (double)Drops / TrafficAssignmentSuccess * 100;
            }
        }

        public void Import(TopDrop2GCellExcel cellExcel)
        {
            cellExcel.CloneProperties(this);
            StatTime = cellExcel.StatDate.AddHours(cellExcel.StatHour);
            CellId = cellExcel.CellName.GetSubStringInFirstPairOfChars('[', ']').ConvertToInt(1);
        }
    }

    public class TopConnection3GCell : TopCell, ICdmaCell, ITimeStat, IImportStat<TopConnection3GCellExcel>
    {
        public int WirelessDrop { get; set; }

        public int ConnectionAttempts { get; set; }

        public int ConnectionFails { get; set; }

        public double LinkBusyRate { get; set; }

        public double ConnectionRate 
        {
            get
            {
                return (double)(ConnectionAttempts - ConnectionFails) / ConnectionAttempts;
            }
        }

        public double DropRate
        {
            get
            {
                return (double)WirelessDrop / (ConnectionAttempts - ConnectionFails);
            }
        }

        public void Import(TopConnection3GCellExcel cellExcel)
        {
            cellExcel.CloneProperties(this);
            StatTime = cellExcel.StatDate.AddHours(cellExcel.StatHour);
            CellId = cellExcel.CellName.GetSubStringInFirstPairOfChars('[', ']').ConvertToInt(1);
        }
    }

    public abstract class TopCellView
    {
        public int ENodebId { get; set; }

        [Display(Name = "城市")]
        public string City { get; set; }

        [Display(Name = "小区编号")]
        public int CdmaCellId { get; set; }

        [Display(Name = "CDMA基站名称")]
        public string CdmaName { get; set; }

        [Display(Name = "LTE基站名称")]
        public string LteName { get; set; }

        [Display(Name = "扇区编号")]
        public byte SectorId { get; set; }

    }

    public class TopDrop2GCellView : TopCellView, ICdmaLteNames, IGetTopCellView
    {
        [Display(Name = "频点")]
        public short Frequency { get; set; }

        [Display(Name = "掉话次数")]
        public int Drops { get; set; }

        public int MoAssignmentSuccess { get; set; }

        public int MtAssignmentSuccess { get; set; }

        public int TrafficAssignmentSuccess { get; set; }

        public int CallAttempts { get; set; }

        [Display(Name = "掉话率")]
        public double DropRate
        {
            get { return (double)Drops / TrafficAssignmentSuccess; }
        }
    }

    public class TopDrop2GCellDailyView : TopCellView, ICdmaLteNames, IGetTopCellView
    {
        [Display(Name = "频点")]
        public short Frequency { get; set; }

        [Display(Name = "CDR掉话次数")]
        public int CdrDrops { get; set; }

        public int Drops
        {
            get { return CdrDrops; }
        }

        [Display(Name = "CDR掉话率")]
        public double CdrDropRate { get; set; }

        [Display(Name = "网管掉话率")]
        public double KpiDropRate { get; set; }

        [Display(Name = "平均RSSI")]
        public double AverageRssi { get; set; }

        [Display(Name = "掉话点平均Ec/Io")]
        public double AverageDropEcio { get; set; }

        [Display(Name = "掉话点平均距离")]
        public double AverageDropDistance { get; set; }
    }

    public class TopConnection3GCellView : TopCellView, ICdmaLteNames
    {
        public int Drops { get; set; }

        public int ConnectionAttempts { get; set; }

        [Display(Name = "连接失败次数")]
        public int ConnectionFails { get; set; }

        [Display(Name = "反向链路繁忙率")]
        public double LinkBusyRate { get; set; }

        [Display(Name = "连接成功率")]
        public double ConnectionRate
        {
            get
            {
                return (double)(ConnectionAttempts - ConnectionFails) / ConnectionAttempts;
            }
        }
    }
}
