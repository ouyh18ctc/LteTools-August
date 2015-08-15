using System;
using System.Collections.Generic;
using Lte.Parameters.Kpi.Entities;

namespace Lte.Evaluations.Kpi
{
    public class TopDrop2GViewModel : ITopViewModel<TopDrop2GCellView>
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StatDate { get; set; }

        public int TopCounts { get; set; }

        public IEnumerable<TopDrop2GCellView> StatList { get; set; }

        public IEnumerable<string> Cities { get; set; }
    }

    public class TopDrop2GDailyViewModel : ITopViewModel<TopDrop2GCellDailyView>
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StatDate { get; set; }

        public int TopCounts { get; set; }

        public IEnumerable<TopDrop2GCellDailyView> StatList { get; set; }

        public IEnumerable<string> Cities { get; set; }
    }

    public class TopConnection3GViewModel : ITopViewModel<TopConnection3GCellView>
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StatDate { get; set; }

        public int TopCounts { get; set; }

        public IEnumerable<TopConnection3GCellView> StatList { get; set; }

        public IEnumerable<string> Cities { get; set; }
    }
}
