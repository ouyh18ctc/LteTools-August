using System;
using Abp.Domain.Entities;
using Lte.Domain.Geo.Abstract;

namespace Lte.Parameters.Entities
{
    public class MroRsrpTa : Entity, ICell
    {
        public DateTime RecordDate { get; set; }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public RsrpInterval RsrpInterval { get; set; }

        public string DateString
        {
            get { return RecordDate.ToShortDateString(); }
        }

        public string RsrpDescription
        {
            get { return RsrpInterval.Description(); }
        }

        public int TaTo4 { get; set; }

        public int TaTo8 { get; set; }

        public int TaTo16 { get; set; }

        public int TaTo24 { get; set; }

        public int TaTo40 { get; set; }

        public int TaTo56 { get; set; }

        public int TaTo80 { get; set; }

        public int TaTo128 { get; set; }

        public int TaAbove128 { get; set; }

        public MroRsrpTa()
        {
            TaTo4 = 0;
            TaTo8 = 0;
            TaTo16 = 0;
            TaTo24 = 0;
            TaTo40 = 0;
            TaTo56 = 0;
            TaTo80 = 0;
            TaTo128 = 0;
            TaAbove128 = 0;
        }

        public void UpdateTa(byte ta)
        {
            if (ta < 4) TaTo4++;
            else if (ta < 8) TaTo8++;
            else if (ta < 16) TaTo16++;
            else if (ta < 24) TaTo24++;
            else if (ta < 40) TaTo40++;
            else if (ta < 56) TaTo56++;
            else if (ta < 80) TaTo80++;
            else if (ta < 128) TaTo128++;
            else TaAbove128++;
        }
    }

    public enum RsrpInterval
    {
        Below120,
        Below110,
        Below105,
        Below100,
        Below90,
        Below80,
        Below60,
        Above60
    }

    public static class RsrpIntervalQueries
    {
        /// <summary>
        /// 从RSRP计算所在区间
        /// </summary>
        /// <param name="rsrp">以-140dBm为起点的RSRP</param>
        /// <returns></returns>
        public static RsrpInterval GetInterval(this byte rsrp)
        {
            if (rsrp < 20) return RsrpInterval.Below120;
            if (rsrp < 30) return RsrpInterval.Below110;
            if (rsrp < 35) return RsrpInterval.Below105;
            if (rsrp < 40) return RsrpInterval.Below100;
            if (rsrp < 50) return RsrpInterval.Below90;
            if (rsrp < 60) return RsrpInterval.Below80;
            return rsrp < 80 ? RsrpInterval.Below60 : RsrpInterval.Above60;
        }

        public static string Description(this RsrpInterval interval)
        {
            switch (interval)
            {
                case RsrpInterval.Below120:
                    return "<-120";
                case RsrpInterval.Below110:
                    return "-120~-110";
                case RsrpInterval.Below105:
                    return "-110~-105";
                case RsrpInterval.Below100:
                    return "-105~-100";
                case RsrpInterval.Below90:
                    return "-100~-90";
                case RsrpInterval.Below80:
                    return "-90~-80";
                case RsrpInterval.Below60:
                    return "-80~-60";
                default:
                    return ">=-60";
            }
        }
    }
}