using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Parameters.Kpi.Abstract;

namespace Lte.Parameters.Kpi.Entities
{
    public class CdrCallsDistanceInfo : IDrop2GDistanceInfo<int>
    {
        [Key]
        public int TopDrop2GCellDailyId { get; set; }

        public int DistanceTo1000Info { get; set; }

        public int DistanceTo1200Info { get; set; }

        public int DistanceTo1400Info { get; set; }

        public int DistanceTo1600Info { get; set; }

        public int DistanceTo1800Info { get; set; }

        public int DistanceTo2000Info { get; set; }

        public int DistanceTo200Info { get; set; }

        public int DistanceTo2200Info { get; set; }

        public int DistanceTo2400Info { get; set; }

        public int DistanceTo2600Info { get; set; }

        public int DistanceTo2800Info { get; set; }

        public int DistanceTo3000Info { get; set; }

        public int DistanceTo4000Info { get; set; }

        public int DistanceTo400Info { get; set; }

        public int DistanceTo5000Info { get; set; }

        public int DistanceTo6000Info { get; set; }

        public int DistanceTo600Info { get; set; }

        public int DistanceTo7000Info { get; set; }

        public int DistanceTo8000Info { get; set; }

        public int DistanceTo800Info { get; set; }

        public int DistanceTo9000Info { get; set; }

        public int DistanceToInfInfo { get; set; }
    }

    public class CdrDropsDistanceInfo : IDrop2GDistanceInfo<int>
    {
        [Key]
        public int TopDrop2GCellDailyId { get; set; }

        public int DistanceTo1000Info { get; set; }

        public int DistanceTo1200Info { get; set; }

        public int DistanceTo1400Info { get; set; }

        public int DistanceTo1600Info { get; set; }

        public int DistanceTo1800Info { get; set; }

        public int DistanceTo2000Info { get; set; }

        public int DistanceTo200Info { get; set; }

        public int DistanceTo2200Info { get; set; }

        public int DistanceTo2400Info { get; set; }

        public int DistanceTo2600Info { get; set; }

        public int DistanceTo2800Info { get; set; }

        public int DistanceTo3000Info { get; set; }

        public int DistanceTo4000Info { get; set; }

        public int DistanceTo400Info { get; set; }

        public int DistanceTo5000Info { get; set; }

        public int DistanceTo6000Info { get; set; }

        public int DistanceTo600Info { get; set; }

        public int DistanceTo7000Info { get; set; }

        public int DistanceTo8000Info { get; set; }

        public int DistanceTo800Info { get; set; }

        public int DistanceTo9000Info { get; set; }

        public int DistanceToInfInfo { get; set; }
    }

    public class DropEcioDistanceInfo : IDrop2GDistanceInfo<double>
    {
        [Key]
        public int TopDrop2GCellDailyId { get; set; }

        public double DistanceTo1000Info { get; set; }

        public double DistanceTo1200Info { get; set; }

        public double DistanceTo1400Info { get; set; }

        public double DistanceTo1600Info { get; set; }

        public double DistanceTo1800Info { get; set; }

        public double DistanceTo2000Info { get; set; }

        public double DistanceTo200Info { get; set; }

        public double DistanceTo2200Info { get; set; }

        public double DistanceTo2400Info { get; set; }

        public double DistanceTo2600Info { get; set; }

        public double DistanceTo2800Info { get; set; }

        public double DistanceTo3000Info { get; set; }

        public double DistanceTo4000Info { get; set; }

        public double DistanceTo400Info { get; set; }

        public double DistanceTo5000Info { get; set; }

        public double DistanceTo6000Info { get; set; }

        public double DistanceTo600Info { get; set; }

        public double DistanceTo7000Info { get; set; }

        public double DistanceTo8000Info { get; set; }

        public double DistanceTo800Info { get; set; }

        public double DistanceTo9000Info { get; set; }

        public double DistanceToInfInfo { get; set; }
    }

    public class GoodEcioDistanceInfo : IDrop2GDistanceInfo<double>
    {
        [Key]
        public int TopDrop2GCellDailyId { get; set; }

        public double DistanceTo1000Info { get; set; }

        public double DistanceTo1200Info { get; set; }

        public double DistanceTo1400Info { get; set; }

        public double DistanceTo1600Info { get; set; }

        public double DistanceTo1800Info { get; set; }

        public double DistanceTo2000Info { get; set; }

        public double DistanceTo200Info { get; set; }

        public double DistanceTo2200Info { get; set; }

        public double DistanceTo2400Info { get; set; }

        public double DistanceTo2600Info { get; set; }

        public double DistanceTo2800Info { get; set; }

        public double DistanceTo3000Info { get; set; }

        public double DistanceTo4000Info { get; set; }

        public double DistanceTo400Info { get; set; }

        public double DistanceTo5000Info { get; set; }

        public double DistanceTo6000Info { get; set; }

        public double DistanceTo600Info { get; set; }

        public double DistanceTo7000Info { get; set; }

        public double DistanceTo8000Info { get; set; }

        public double DistanceTo800Info { get; set; }

        public double DistanceTo9000Info { get; set; }

        public double DistanceToInfInfo { get; set; }
    }
}
