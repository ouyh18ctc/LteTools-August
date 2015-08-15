using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Evaluations.Rutrace.Record;
using Lte.Parameters.Region.Entities;

namespace Lte.Evaluations.Rutrace.Entities
{
    public class RuInterferenceDetails : ICell
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public List<RuInterferenceVictim> Victims { get; set; }

        public RuInterferenceDetails(ICell cell)
        {
            CellId = cell.CellId;
            SectorId = cell.SectorId;
            Victims = new List<RuInterferenceVictim>();
        }

        public void Import(RuInterferenceRecord record, int i)
        {
            RuInterferenceVictim victim =
                Victims.FirstOrDefault(x => x.CellId == record.CellId && x.SectorId == record.SectorId);
            if (victim == null)
            {
                victim = new RuInterferenceVictim(record);
                Victims.Add(victim);
            }
            victim.MeasuredTimes += record.MeasuredTimes;
            victim.InterferenceTimes += record.Interferences[i].InterferenceTimes;
        }

        public void UpdateInfo(InterferenceStat stat)
        {
            stat.VictimCells = Victims.Count;
            stat.InterferenceCells = Victims.Count(x => x.InterferenceRatio > RuInterferenceStat.RatioThreshold);
        }
    }
}
