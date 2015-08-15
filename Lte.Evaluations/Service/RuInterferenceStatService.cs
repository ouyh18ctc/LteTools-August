using System;
using Lte.Evaluations.Abstract;
using Lte.Evaluations.Rutrace.Record;

namespace Lte.Evaluations.Service
{
    public abstract class RuInterferenceStatService
    {
        protected readonly RuInterferenceStat _stat;

        protected RuInterferenceStatService(RuInterferenceStat stat)
        {
            _stat = stat;
        }

        public abstract double GetValue();

        public string GetColor(IStatValueIntervalList field)
        {
            return field.GetColor(GetValue(), "FFFFFF");
        }

    }

    public class InterferenceSourceStatService : RuInterferenceStatService
    {
        public InterferenceSourceStatService(RuInterferenceStat stat) : base(stat)
        {
        }

        public override double GetValue()
        {
            return _stat.InterferenceRatio * Math.Log(1 + _stat.VictimCells);
        }
    }

    public class InterferenceDistanceStatService : RuInterferenceStatService
    {
        public InterferenceDistanceStatService(RuInterferenceStat stat) : base(stat)
        {
        }

        public override double GetValue()
        {
            return _stat.TaAverage / _stat.AverageRtd;
        }
    }

    public class InterferenceTaStatService : RuInterferenceStatService
    {
        public InterferenceTaStatService(RuInterferenceStat stat) : base(stat)
        {
        }

        public override double GetValue()
        {
            return _stat.TaExcessRate;
        }
    }
}
