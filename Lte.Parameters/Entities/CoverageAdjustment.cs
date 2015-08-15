using System;
using Abp.Domain.Entities;

namespace Lte.Parameters.Entities
{
    public class CoverageAdjustment : Entity
    {
        public int ENodebId { get; set; }

        public byte SectorId { get; set; }

        public int Frequency { get; set; }

        public double Factor165m { get; set; }

        public double Factor135m { get; set; }

        public double Factor105m { get; set; }

        public double Factor75m { get; set; }

        public double Factor45m { get; set; }

        public double Factor15m { get; set; }

        public double Factor15 { get; set; }

        public double Factor45 { get; set; }

        public double Factor75 { get; set; }

        public double Factor105 { get; set; }

        public double Factor135 { get; set; }

        public double Factor165 { get; set; }

        public CoverageAdjustment()
        {
            Factor105 = 0;
            Factor105m = 0;
            Factor135 = 0;
            Factor135m = 0;
            Factor15 = 0;
            Factor15m = 0;
            Factor165 = 0;
            Factor165m = 0;
            Factor45 = 0;
            Factor45m = 0;
            Factor75 = 0;
            Factor75m = 0;
        }

        public void SetAdjustFactor(double azimuth, double factor)
        {
            int interval = (int)Math.Ceiling(azimuth / 30);
            switch (interval)
            { 
                case -6:
                    Factor165m = factor;
                    break;
                case -5:
                    Factor135m = factor;
                    break;
                case -4:
                    Factor105m = factor;
                    break;
                case -3:
                    Factor75m = factor;
                    break;
                case -2:
                    Factor45m = factor;
                    break;
                case -1:
                    Factor15m = factor;
                    break;
                case 1:
                case 0:
                    Factor15 = factor;
                    break;
                case 2:
                    Factor45 = factor;
                    break;
                case 3:
                    Factor75 = factor;
                    break;
                case 4:
                    Factor105 = factor;
                    break;
                case 5:
                    Factor135 = factor;
                    break;
                case 6:
                    Factor165 = factor;
                    break;
            }
        }
    }
}
