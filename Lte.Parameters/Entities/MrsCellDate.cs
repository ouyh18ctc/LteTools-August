using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Lte.Domain.Geo.Abstract;

namespace Lte.Parameters.Entities
{
    public class MrsCellDate : Entity, ICell, IMrsCellDate
    {
        public DateTime RecordDate { get; set; }
        
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int[] RsrpCounts { get; set; }

        public MrsCellDate()
        {
            RsrpCounts = new int[48];
        }

        public string DateString
        {
            get { return RecordDate.ToShortDateString(); }
        }

        public double CoverageRate
        {
            get
            {
                int num = RsrpAbove60 + RsrpTo100 + RsrpTo95 + RsrpTo90 + RsrpTo85 + RsrpTo80 + RsrpTo70 + RsrpTo60;
                int dem = num + RsrpTo105 + RsrpTo110 + RsrpTo115 + RsrpTo120;
                return dem == 0 ? 0 : (double)num / dem;
            }
        }

        public double CoveragePercentage
        {
            get { return (int) (CoverageRate*10000)/100.00; }
        }

        public void UpdateStats()
        {
            RsrpTo120 = RsrpCounts[0];
            RsrpTo115 = RsrpCounts[1];
            RsrpTo110 = RsrpCounts[2] + RsrpCounts[3] + RsrpCounts[4] + RsrpCounts[5] + RsrpCounts[6];
            RsrpTo105 = RsrpCounts[7] + RsrpCounts[8] + RsrpCounts[9] + RsrpCounts[10] + RsrpCounts[11];
            RsrpTo100 = RsrpCounts[12] + RsrpCounts[13] + RsrpCounts[14] + RsrpCounts[15] + RsrpCounts[16];
            RsrpTo95 = RsrpCounts[17] + RsrpCounts[18] + RsrpCounts[19] + RsrpCounts[20] + RsrpCounts[21];
            RsrpTo90 = RsrpCounts[22] + RsrpCounts[23] + RsrpCounts[24] + RsrpCounts[25] + RsrpCounts[26];
            RsrpTo85 = RsrpCounts[27] + RsrpCounts[28] + RsrpCounts[29] + RsrpCounts[30] + RsrpCounts[31];
            RsrpTo80 = RsrpCounts[32] + RsrpCounts[33] + RsrpCounts[34] + RsrpCounts[35] + RsrpCounts[36];
            RsrpTo70 = RsrpCounts[37] + RsrpCounts[38] + RsrpCounts[39] + RsrpCounts[40] + RsrpCounts[41];
            RsrpTo60 = RsrpCounts[42] + RsrpCounts[43] + RsrpCounts[44] + RsrpCounts[45] + RsrpCounts[46];
            RsrpAbove60 = RsrpCounts[47];
        }

        public int RsrpTo120 { get; set; }

        public int RsrpTo115 { get; set; }

        public int RsrpTo110 { get; set; }

        public int RsrpTo105 { get; set; }

        public int RsrpTo100 { get; set; }

        public int RsrpTo95 { get; set; }

        public int RsrpTo90 { get; set; }

        public int RsrpTo85 { get; set; }

        public int RsrpTo80 { get; set; }

        public int RsrpTo70 { get; set; }

        public int RsrpTo60 { get; set; }

        public int RsrpAbove60 { get; set; }
    }

    public class MrsCellDateComparer : IComparer<MrsCellDate>
    {
        public int Compare(MrsCellDate x, MrsCellDate y)
        {
            return (int)(x.CoverageRate * 10000 - y.CoverageRate * 10000);
        }
    }

    public class MrsCellDateView : IMrsCellDate, ICell
    {
        [Display(Name="统计日期")]
        public string DateString { get; set; }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

        [Display(Name = "覆盖率(>-105dBm)")]
        public double CoveragePercentage
        {
            get
            {
                return TotalMrs == 0
                    ? 0
                    : 100 * (TotalMrs - RsrpTo120 - RsrpTo115 - RsrpTo110 - RsrpTo105)/(double)TotalMrs;
            }
        }

        [Display(Name = "覆盖率(>-110dBm)")]
        public double CoverageTo110
        {
            get
            {
                return TotalMrs == 0
                    ? 0
                    : 100 * (TotalMrs - RsrpTo120 - RsrpTo115 - RsrpTo110) / (double)TotalMrs;
            }
        }

        [Display(Name = "覆盖率(>-115dBm)")]
        public double CoverageTo115
        {
            get
            {
                return TotalMrs == 0
                    ? 0
                    : 100 * (TotalMrs - RsrpTo120 - RsrpTo115) / (double)TotalMrs;
            }
        }

        [Display(Name = "小区名称")]
        public string CellName { get; set; }

        [Display(Name = "MR总数")]
        public int TotalMrs
        {
            get
            {
                return RsrpAbove60 + RsrpTo100 + RsrpTo105 + RsrpTo110 + RsrpTo115 + RsrpTo120 + RsrpTo60
                       + RsrpTo70 + RsrpTo80 + RsrpTo85 + RsrpTo90 + RsrpTo95;
            }
        }

        public string StatInfo
        {
            get
            {
                return "；</br>MR总数: " + TotalMrs
                       + "；</br>覆盖率(>-105dBm): " + CoveragePercentage
                       + "；</br>覆盖率(>-110dBm): " + CoverageTo110
                       + "；</br>覆盖率(>-115dBm): " + CoverageTo115;
            }
        }

        public int RsrpTo120 { get; set; }
        public int RsrpTo115 { get; set; }
        public int RsrpTo110 { get; set; }
        public int RsrpTo105 { get; set; }
        public int RsrpTo100 { get; set; }
        public int RsrpTo95 { get; set; }
        public int RsrpTo90 { get; set; }
        public int RsrpTo85 { get; set; }
        public int RsrpTo80 { get; set; }
        public int RsrpTo70 { get; set; }
        public int RsrpTo60 { get; set; }
        public int RsrpAbove60 { get; set; }
    }
}