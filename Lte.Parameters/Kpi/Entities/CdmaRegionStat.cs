using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Public;

namespace Lte.Parameters.Kpi.Entities
{
    public class CdmaRegionStat : IDataReaderImportable, IDateStat, IRegionStat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [SimpleExcelColumn(Name = "地市")]
        [Display(Name = "地市/片区")]
        public string Region { get; set; }

        [SimpleExcelColumn(Name = "日期", DefaultValue = "2015-01-01")]
        public DateTime StatDate { get; set; }

        [SimpleExcelColumn(Name = "2G全天话务量含切换", DefaultValue = "0")]
        [Display(Name = "2G全天话务量")]
        public double ErlangIncludingSwitch { get; set; }

        [SimpleExcelColumn(Name = "2G全天话务量不含切换", DefaultValue = "0")]
        public double ErlangExcludingSwitch { get; set; }

        [SimpleExcelColumn(Name = "掉话分子", DefaultValue = "0")]
        public int Drop2GNum { get; set; }

        [SimpleExcelColumn(Name = "掉话分母", DefaultValue = "1")]
        public int Drop2GDem { get; set; }

        [Display(Name = "掉话率")]
        public double Drop2GRate
        {
            get { return (double)Drop2GNum / Drop2GDem; }
        }

        [SimpleExcelColumn(Name = "呼建分子", DefaultValue = "1")]
        public int CallSetupNum { get; set; }

        [SimpleExcelColumn(Name = "呼建分母", DefaultValue = "1")]
        public int CallSetupDem { get; set; }

        [Display(Name = "2G呼建")]
        public double CallSetupRate
        {
            get { return (double)CallSetupNum / CallSetupDem; }
        }

        [SimpleExcelColumn(Name = "EcIo分子", DefaultValue = "1")]
        public long EcioNum { get; set; }

        [SimpleExcelColumn(Name = "EcIo分母", DefaultValue = "1")]
        public long EcioDem { get; set; }

        [Display(Name = "Ec/Io优良率")]
        public double Ecio
        {
            get { return (double)EcioNum / EcioDem; }
        }

        [SimpleExcelColumn(Name = "2G利用率分子", DefaultValue = "1")]
        public int Utility2GNum { get; set; }

        [SimpleExcelColumn(Name = "2G利用率分母", DefaultValue = "2")]
        public int Utility2GDem { get; set; }

        public double Utility2GRate
        {
            get { return (double)Utility2GNum / Utility2GDem; }
        }

        [SimpleExcelColumn(Name = "全天流量MB", DefaultValue = "0")]
        [Display(Name = "全天流量(GB)")]
        public double Flow { get; set; }

        [SimpleExcelColumn(Name = "DO全天话务量erl", DefaultValue = "0")]
        public double Erlang3G { get; set; }

        [SimpleExcelColumn(Name = "掉线分子", DefaultValue = "0")]
        public int Drop3GNum { get; set; }

        [SimpleExcelColumn(Name = "掉线分母", DefaultValue = "1")]
        public int Drop3GDem { get; set; }

        [Display(Name = "掉线率")]
        public double Drop3GRate
        {
            get { return (double)Drop3GNum / Drop3GDem; }
        }

        [SimpleExcelColumn(Name = "连接分子", DefaultValue = "1")]
        public int ConnectionNum { get; set; }

        [SimpleExcelColumn(Name = "连接分母", DefaultValue = "1")]
        public int ConnectionDem { get; set; }

        [Display(Name = "3G连接")]
        public double ConnectionRate
        {
            get { return (double)ConnectionNum / ConnectionDem; }
        }

        [SimpleExcelColumn(Name = "CI分子", DefaultValue = "1")]
        public long CiNum { get; set; }

        [SimpleExcelColumn(Name = "CI分母", DefaultValue = "1")]
        public long CiDem { get; set; }

        [Display(Name = "C/I优良率")]
        public double Ci
        {
            get { return (double)CiNum / CiDem; }
        }

        [SimpleExcelColumn(Name = "反向链路繁忙率分子", DefaultValue = "0")]
        public int LinkBusyNum { get; set; }

        [SimpleExcelColumn(Name = "反向链路繁忙率分母", DefaultValue = "1")]
        public int LinkBusyDem { get; set; }

        public double LinkBusyRate
        {
            get { return (double)LinkBusyNum / LinkBusyDem; }
        }

        [SimpleExcelColumn(Name = "3G切2G流量比分子", DefaultValue = "10")]
        public long DownSwitchNum { get; set; }

        [SimpleExcelColumn(Name = "3G切2G流量比分母", DefaultValue = "1")]
        public int DownSwitchDem { get; set; }

        [Display(Name = "3G切2G流量比")]
        public double DownSwitchRate
        {
            get { return (double)DownSwitchNum / DownSwitchDem; }
        }

        [SimpleExcelColumn(Name = "3G利用率分子", DefaultValue = "1")]
        public int Utility3GNum { get; set; }

        [SimpleExcelColumn(Name = "3G利用率分母_载扇数", DefaultValue = "1")]
        public int Utility3GDem { get; set; }

        public double Utility3GRate
        {
            get { return (double)Utility3GNum / Utility3GDem; }
        }

        public void Import(IDataReader tableReader)
        {
            ReadExcelTableService<CdmaRegionStat, SimpleExcelColumnAttribute> service
                = new ReadExcelTableService<CdmaRegionStat, SimpleExcelColumnAttribute>(this);
            service.Import(tableReader);
        }
    }
}
