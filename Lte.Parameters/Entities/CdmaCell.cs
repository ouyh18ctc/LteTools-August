using System;
using System.Collections.Generic;
using System.Globalization;
using Abp.Domain.Entities;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;
using Lte.Parameters.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Lte.Parameters.Entities
{
    public class CdmaCellBase : Entity
    {
        [Display(Name = "基站编号")]
        public int BtsId { get; set; }

        [Display(Name = "扇区编号")]
        public byte SectorId { get; set; }

        [Display(Name = "小区类别")]
        public string CellType { get; set; }

        [Display(Name = "频点列表")]
        public int Frequency { get; set; }

        public CdmaCellBase()
        {
            BtsId = -1;
            SectorId = 31;
            Frequency = 0;
        }

        public CdmaCellBase(CdmaCell cell)
            : this()
        {
            cell.CloneProperties(this);
        }

        public void AddFrequency(int freq)
        {
            switch (freq)
            {
                case 37:
                    Frequency += 1;
                    break;
                case 78:
                    Frequency += 2;
                    break;
                case 119:
                    Frequency += 4;
                    break;
                case 160:
                    Frequency += 8;
                    break;
                case 201:
                    Frequency += 16;
                    break;
                case 242:
                    Frequency += 32;
                    break;
                case 283:
                    Frequency += 64;
                    break;
                case 1013:
                    Frequency += 128;
                    break;
                default:
                    Frequency += 256;
                    break;
            }
        }

        public bool HasFrequency(int freq)
        {
            switch (freq)
            {
                case 37:
                    return (Frequency & 1) != 0;
                case 78:
                    return (Frequency & 2) != 0;
                case 119:
                    return (Frequency & 4) != 0;
                case 160:
                    return (Frequency & 8) != 0;
                case 201:
                    return (Frequency & 16) != 0;
                case 242:
                    return (Frequency & 32) != 0;
                case 283:
                    return (Frequency & 64) != 0;
                case 1013:
                    return (Frequency & 128) != 0;
                default:
                    return (Frequency & 256) != 0;
            }
        }
    }

    public class CdmaCell : CdmaCellBase, IDisposable, ICdmaCell, IExcelImportable<CdmaCellExcel>
    {
        [Display(Name = "小区编号")]
        public int CellId { get; set; }

        [Display(Name = "LAC")]
        public string Lac { get; set; }

        [Display(Name = "PN")]
        public short Pn { get; set; }

        [Display(Name = "经度")]
        public double Longtitute { get; set; }

        [Display(Name = "纬度")]
        public double Lattitute { get; set; }

        [Display(Name = "高度")]
        public double Height { get; set; }

        [Display(Name = "机械下倾")]
        public double MTilt { get; set; }

        [Display(Name = "电子下倾")]
        public double ETilt { get; set; }

        [Display(Name = "方位角")]
        public double Azimuth { get; set; }

        [Display(Name = "天线增益")]
        public double AntennaGain { get; set; }

        public bool IsOutdoor { get; set; }

        public short Frequency1 { get; set; }

        public short Frequency2 { get; set; }

        public short Frequency3 { get; set; }

        public short Frequency4 { get; set; }

        public short Frequency5 { get; set; }

        public string CellName
        {
            get 
            {
                if (Frequency1 == -1) { return "空"; }
                string result = Frequency1.ToString(CultureInfo.InvariantCulture);
                if (Frequency2 == -1) { return result; }
                result += "&" + Frequency2;
                if (Frequency3 == -1) { return result; }
                result += "&" + Frequency3;
                if (Frequency4 == -1) { return result; }
                result += "&" + Frequency4;
                if (Frequency5 == -1) { return result; }
                result += "&" + Frequency5;
                return result; 
            }
        }

        public short Pci { get; set; }

        public double RsPower { get; set; }

        public static bool UpdateFirstFrequency { get; set; }

        public CdmaCell()
        {
            Frequency1 = -1;
            Frequency2 = -1;
            Frequency3 = -1;
            Frequency4 = -1;
            Frequency5 = -1;
            CellId = -1;
            Pn = -1;
            Height = -1;
        }

        public void Import(CdmaCellExcel cellExcelInfo, bool importNewInfo)
        {
            short currentFrequency = (short)cellExcelInfo.Frequency;
            if (currentFrequency == Frequency1 && UpdateFirstFrequency)
            {
                cellExcelInfo.CloneProperties(this, true);
                IsOutdoor = (cellExcelInfo.IsIndoor.Trim() == "否");
            }
            if (currentFrequency == Frequency1 || currentFrequency == Frequency2
                || currentFrequency == Frequency3 || currentFrequency == Frequency4
                || currentFrequency == Frequency5)
            {
                return;
            }
            if (Frequency1 == -1)
            {
                cellExcelInfo.CloneProperties(this, !importNewInfo);
                IsOutdoor = (cellExcelInfo.IsIndoor.Trim() == "否");
                Frequency1 = currentFrequency;
                Frequency = 0;
                AddFrequency(currentFrequency);
                return;
            }
            AddFrequency(currentFrequency);
            if (Frequency2 == -1)
            { Frequency2 = currentFrequency; }
            else if (Frequency3 == -1)
            { Frequency3 = currentFrequency; }
            else if (Frequency4 == -1)
            { Frequency4 = currentFrequency; }
            else if (Frequency5 == -1)
            { Frequency5 = currentFrequency; }
        }

        public void Dispose()
        { }
    }

    public class CdmaCellComperer : IEqualityComparer<CdmaCell>
    {
        public bool Equals(CdmaCell x, CdmaCell y)
        {
            if (x == null) return y == null;
            return x.BtsId == y.BtsId && x.SectorId == y.SectorId && x.CellType == y.CellType;
        }

        public int GetHashCode(CdmaCell obj)
        {
            return obj == null ? 0 : (obj.BtsId + obj.SectorId + obj.CellType).GetHashCode();
        }
    }
}
