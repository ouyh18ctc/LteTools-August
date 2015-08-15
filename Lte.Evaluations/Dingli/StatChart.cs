using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Regular;

namespace Lte.Evaluations.Dingli
{
    public interface IStatChart<T> where T: class, new()
    {
        List<T> StatList { get; set; }

        void Import(List<T> stats);
    }

    public class CoverageStatChart : IStatChart<CoverageStat>
    {
        public CoverageStatChart()
        {
            StatList = new List<CoverageStat>();
        }

        public List<CoverageStat> StatList { get; set; }

        public void Import(List<CoverageStat> stats)
        {
            var result = from s in stats.Where(x => x.Longtitute > 1 && x.Lattitute > 1 && x.Rsrp > -180)
                         group s by new
                         {
                             s.Longtitute,
                             s.Lattitute,
                             s.ENodebId,
                             s.SectorId,
                             s.Earfcn
                         } into g
                         select new
                         {
                             g.Key.Longtitute,
                             g.Key.Lattitute,
                             g.Key.ENodebId,
                             g.Key.SectorId,
                             g.Key.Earfcn,
                             Rsrp = g.Average(s => s.Rsrp),
                             Sinr = g.Average(s => s.Sinr)
                         };
            StatList = result.Select(x => new CoverageStat
            {
                Longtitute = x.Longtitute,
                Lattitute = x.Lattitute,
                Rsrp = x.Rsrp,
                Sinr = x.Sinr,
                ENodebId = x.ENodebId,
                SectorId = x.SectorId,
                Earfcn = x.Earfcn
            }).ToList();
        }
    }

    public class RateStatChart : IStatChart<BasicRateStat>
    {
        private static readonly Dictionary<double, double> theoryLine = new Dictionary<double, double> {
            { -6.71, 0.1523 }, { -5.11, 0.2344 }, { -3.15, 0.377 }, { -0.879, 0.6016 }, { 0.701, 0.877 },	
            { 2.529, 1.1758 }, { 4.606,	1.4766 }, { 6.431, 1.9141 }, { 8.326, 2.4063 },	
            { 10.3,	2.7305 }, { 12.22, 3.3223 }, { 14.01, 3.9023 },	{ 15.81, 4.5234 },	
        	{ 17.68, 5.1152 }, { 19.61, 5.5547 }
        };

        public Dictionary<double, double> TheoryLine
        {
            get { return theoryLine; }
        }

        public List<BasicRateStat> StatList { get; set; }

        public RateStatChart()
        {
            StatList = new List<BasicRateStat>();
        }

        public void Import(List<BasicRateStat> stats)
        {
            StatList.Clear();
            for (int i = 0; i < stats.Count; i++)
            {
                BasicRateStat stat = new BasicRateStat();
                stats[i].CloneProperties<BasicRateStat>(stat);
                stat.PassedTimeInSeconds = (stats[i].Time - stats[0].Time).TotalSeconds;
                StatList.Add(stat);
            }
        }
    }
}
