using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Lte.Domain.Regular;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Concrete
{
    public class EFCoverageRepository : LightWeightRepositroyBase<CoverageAdjustment>, ICoverageRepository
    {
        public void Save(IEnumerable<CoverageAdjustment> adjustments)
        {
            foreach (CoverageAdjustment adjustment in adjustments)
            {
                CoverageAdjustment item = GetAll().FirstOrDefault(
                x => x.ENodebId == adjustment.ENodebId && x.SectorId == adjustment.SectorId
                    && x.Frequency == adjustment.Frequency);
                if (item == null)
                {
                    Insert(adjustment);
                }
                else
                {
                    adjustment.CloneProperties<CoverageAdjustment>(item);
                    Update(item);
                }
            }
        }

        protected override DbSet<CoverageAdjustment> Entities
        {
            get { return context.CoverageAdjustments; }
        }
    }

    public class EFInterferenceStatRepository : LightWeightRepositroyBase<InterferenceStat>, IInterferenceStatRepository
    {
        public void Save(IEnumerable<InterferenceStat> stats)
        {
            foreach (InterferenceStat stat in stats)
            {
                InterferenceStat item = GetAll().FirstOrDefault(x =>
                    x.CellId == stat.CellId && x.SectorId == stat.SectorId);
                if (item == null)
                {
                    Insert(stat);
                }
                else
                {
                    stat.UpdateInterferenceInfo(item);
                    stat.UpdateRtdInfo(item);
                    stat.UpdateTaInfo(item);
                    Update(item);
                }
            }
        }

        protected override DbSet<InterferenceStat> Entities
        {
            get { return context.InterferenceStats; }
        }
    }

    public class EFPureInterferenceStatRepository : LightWeightRepositroyBase<PureInterferenceStat>,
        IPureInterferenceStatRepository
    {
        protected override DbSet<PureInterferenceStat> Entities
        {
            get { return context.PureInterferenceStats; }
        }

        public void Save(IEnumerable<PureInterferenceStat> stats)
        {
            foreach (PureInterferenceStat stat in stats)
            {
                PureInterferenceStat item = GetAll().FirstOrDefault(x =>
                    x.CellId == stat.CellId && x.SectorId == stat.SectorId
                    && x.RecordDate == stat.RecordDate);
                if (item == null)
                {
                    Insert(stat);
                }
            }
        }

    }
}
