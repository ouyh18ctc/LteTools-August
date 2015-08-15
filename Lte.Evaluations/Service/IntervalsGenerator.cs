using System.Collections.Generic;
using Lte.Evaluations.Entities;

namespace Lte.Evaluations.Service
{
    public abstract class IntervalsGenerator
    {
        protected List<StatValueInterval> _intervalList;

        protected IntervalsGenerator(List<StatValueInterval> intervalList)
        {
            _intervalList = intervalList;
        }

        public abstract void Generate(int intervalLength);

        protected void Generate(double[] values)
        {
            int length = values.Length - 1;
            for (int i = 0; i < length; i++)
            {
                _intervalList.Add(new StatValueInterval
                {
                    IntervalLowLevel = values[i],
                    IntervalUpLevel = values[i + 1]
                });
            }
        }
    }

    public class InterferenceIntervalsGenerator : IntervalsGenerator
    {
        public InterferenceIntervalsGenerator(List<StatValueInterval> intervalList) : base(intervalList)
        {
        }

        public override void Generate(int intervalLength)
        {
            switch (intervalLength)
            {
                case 4:
                    Generate(new[] { 0, 0.5, 0.8, 1.1, 3 });
                    break;
                case 6:
                    Generate(new[] { 0, 0.4, 0.7, 1, 1.2, 1.5, 3 });
                    break;
                default:
                    Generate(new[] { 0, 0.4, 0.6, 0.9, 1.1, 1.3, 1.5, 1.7, 3 });
                    break;
            }
        }
    }

    public class DistanceIntervalsGenerator : IntervalsGenerator
    {
        public DistanceIntervalsGenerator(List<StatValueInterval> intervalList) : base(intervalList)
        {
        }

        public override void Generate(int intervalLength)
        {
            switch (intervalLength)
            {
                case 4:
                    Generate(new double[] { 0, 0.2, 0.5, 1, 3 });
                    break;
                case 6:
                    Generate(new[] { 0, 0.2, 0.3, 0.5, 0.8, 1.1, 3 });
                    break;
                case 8:
                default:
                    Generate(new[] { 0, 0.15, 0.3, 0.5, 0.6, 0.7, 0.8, 1.1, 3 });
                    break;
            }
        }
    }

    public class ExcessIntervalsGenerator : IntervalsGenerator
    {
        public ExcessIntervalsGenerator(List<StatValueInterval> intervalList) : base(intervalList)
        {
        }

        public override void Generate(int intervalLength)
        {
            switch (intervalLength)
            {
                case 4:
                    Generate(new[] { 0, 0.05, 0.1, 0.2, 2 });
                    break;
                case 6:
                    Generate(new[] { 0, 0.03, 0.06, 0.12, 0.25, 0.5, 2 });
                    break;
                case 8:
                default:
                    Generate(new[] { 0, 0.01, 0.03, 0.06, 0.12, 0.2, 0.3, 0.5, 2 });
                    break;
            }
        }
    }

    public class SinrIntervalsGenerator : IntervalsGenerator
    {
        public SinrIntervalsGenerator(List<StatValueInterval> intervalList) : base(intervalList)
        {
        }

        public override void Generate(int intervalLength)
        {
            switch (intervalLength)
            {
                case 4:
                    Generate(new double[] { -50, -3, 3, 12, 50 });
                    break;
                case 6:
                    Generate(new double[] { -50, -3, 0, 3, 9, 15, 50 });
                    break;
                case 8:
                default:
                    Generate(new double[] { -50, -6, -3, 0, 3, 9, 15, 25, 50 });
                    break;
            }
        }
    }

    public class RsrpIntervalsGenerator : IntervalsGenerator
    {
        public RsrpIntervalsGenerator(List<StatValueInterval> intervalList) : base(intervalList)
        {
        }

        public override void Generate(int intervalLength)
        {
            switch (intervalLength)
            {
                case 4:
                    Generate(new double[] { -140, -120, -100, -80, 0 });
                    break;
                case 6:
                    Generate(new double[] { -140, -120, -105, -95, -85, -75, 0 });
                    break;
                case 8:
                default:
                    Generate(new double[] { -140, -120, -110, -105, -100, -95, -85, -75, 0 });
                    break;
            }
        }
    }
}
