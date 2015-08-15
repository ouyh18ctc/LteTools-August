using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Evaluations.Rutrace.Record;

namespace Lte.Evaluations.Rutrace.Entities
{
    public class InterferenceDetails : ICell
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public DateTime RecordDate { get; set; }

        public List<InterferenceVictim> Victims { get; set; }

        public InterferenceDetails()
        {
            Victims = new List<InterferenceVictim>();
        }

        public void Import(RuInterferenceRecord record, int i)
        {
            InterferenceVictim victim =
                Victims.FirstOrDefault(x => x.CellId == record.CellId && x.SectorId == record.SectorId);
            if (victim == null)
            {
                victim = new InterferenceVictim(record);
                Victims.Add(victim);
            }
            victim.MeasuredTimes += record.MeasuredTimes;
            victim.InterferenceTimes += record.Interferences[i].InterferenceTimes;
        }

        public void Import(MrInterferenceRecord record, int i)
        {
            InterferenceVictim victim =
                Victims.FirstOrDefault(x => x.CellId == record.CellId && x.SectorId == record.SectorId);
            if (victim == null)
            {
                victim = new InterferenceVictim(record);
                Victims.Add(victim);
            }
            victim.MeasuredTimes += record.MeasuredTimes;
            victim.InterferenceTimes += record.Interferences[i].InterferenceTimes;
        }
    }
}
