using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Kpi.Abstract;

namespace Lte.Parameters.Test.Kpi.Service
{
    public class FakeCsvInfo : ICarrierName
    {
        public string Carrier { get; set; }
    }

    public class FakeStat : ITimeStat
    {
        public DateTime StatTime { get; set; }
    }

    public class FakeCarrierTimeStat : ITimeStat, ICarrierName
    {
        public DateTime StatTime { get; set; }
        public string Carrier { get; set; }
    }

    public class FakeTimeStat : IImportStat<FakeCarrierTimeStat>, ITimeStat
    {
        public void Import(FakeCarrierTimeStat cellExcel)
        {
            StatTime = cellExcel.StatTime;
        }

        public DateTime StatTime { get; set; }
    }

    public class FakeCityTimeStat : IImportStat<FakeCarrierTimeStat>, ITimeStat, ICityStat
    {
        public void Import(FakeCarrierTimeStat cellExcel)
        {
            StatTime = cellExcel.StatTime;
        }

        public DateTime StatTime { get; set; }
        public string City { get; set; }
    }

    public class FakeCell : ICell, ITimeStat
    {
        public int CellId { get; set; }
        public byte SectorId { get; set; }
        public int Kpi { get; set; }
        public DateTime StatTime { get; set; }
    }

    public class FakeTownTimeStat : ITimeStat, ITownId, IImportStat<IEnumerable<FakeCell>>
    {
        public DateTime StatTime { get; set; }

        public int TownId { get; set; }

        public int Kpi { get; set; }

        public void Import(IEnumerable<FakeCell> cellExcel)
        {
            Kpi = cellExcel.Sum(x => x.Kpi);
        }
    }

}
