using System;
using Lte.Evaluations.Abstract;
using Lte.Evaluations.Rutrace.Record;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Service
{
    public abstract class InterferenceService<TStat>
    {
        protected readonly TStat _stat;

        protected InterferenceService(TStat stat)
        {
            _stat = stat;
        }

        public abstract double GetValue();

        public string GetColor(IStatValueIntervalList field)
        {
            return field.GetColor(GetValue(), "FFFFFF");
        }

    }

    public class RuInterferenceSourceService : InterferenceService<RuInterferenceStat>
    {
        public RuInterferenceSourceService(RuInterferenceStat stat) : base(stat)
        {
        }

        public override double GetValue()
        {
            return _stat.InterferenceRatio * Math.Log(1 + _stat.VictimCells);
        }
    }

    public class RuInterferenceDistanceService : InterferenceService<RuInterferenceStat>
    {
        public RuInterferenceDistanceService(RuInterferenceStat stat) : base(stat)
        {
        }

        public override double GetValue()
        {
            return _stat.TaAverage / _stat.AverageRtd;
        }
    }

    public class RuInterferenceTaService : InterferenceService<RuInterferenceStat>
    {
        public RuInterferenceTaService(RuInterferenceStat stat) : base(stat)
        {
        }

        public override double GetValue()
        {
            return _stat.TaExcessRate;
        }
    }

    public class MrsCoverageRateService : InterferenceService<MrsCellDateView>
    {
        public MrsCoverageRateService(MrsCellDateView stat) : base(stat)
        {
        }

        public override double GetValue()
        {
            return _stat.CoveragePercentage;
        }
    }

    public class MrsTotalMrsService : InterferenceService<MrsCellDateView>
    {
        public MrsTotalMrsService(MrsCellDateView stat) : base(stat)
        {
        }

        public override double GetValue()
        {
            return _stat.TotalMrs;
        }
    }

    public class MrsCoverageTo110Service : InterferenceService<MrsCellDateView>
    {
        public MrsCoverageTo110Service(MrsCellDateView stat) : base(stat)
        {
        }

        public override double GetValue()
        {
            return _stat.CoverageTo110;
        }
    }

    public class MrsCoverageTo115Service : InterferenceService<MrsCellDateView>
    {
        public MrsCoverageTo115Service(MrsCellDateView stat) : base(stat)
        {
        }

        public override double GetValue()
        {
            return _stat.CoverageTo115;
        }
    }
}
