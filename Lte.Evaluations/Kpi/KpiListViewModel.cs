using System;
using System.Collections.Generic;
using Lte.Parameters.Kpi.Entities;

namespace Lte.Evaluations.Kpi
{
    public class KpiListViewModel : IStatDateViewModel, IDateSpanViewModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StatDate { get; set; }

        public IEnumerable<CdmaRegionStat> CdmaStats { get; set; }
        public IEnumerable<string> Cities { get; set; }
    }

    public class Precise4GViewModel : IStatDateViewModel, IDateSpanViewModel
    {
        public DateTime StatDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public IEnumerable<TownPrecise4GView> PreciseStats { get; set; }

        public IEnumerable<string> Districts { get; set; } 

    }
}
