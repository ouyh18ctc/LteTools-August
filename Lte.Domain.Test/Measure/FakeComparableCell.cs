using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lte.Domain.Measure;
using Lte.Domain.Geo;

namespace Lte.Domain.Test.Measure
{
    public class FakeComparableCell : ComparableCell
    {
        public new double MetricCalculate(HorizontalProperty property = null,
            DistanceAzimuthMetric metric = null)
        {
            return base.MetricCalculate(property, metric);
        }

        public static FakeComparableCell Parse(ComparableCell cell)
        {
            return new FakeComparableCell()
            {
                Cell = cell.Cell,
                AzimuthAngle = cell.AzimuthAngle,
                Budget = cell.Budget,
                Distance = cell.Distance,
                PciModx = cell.PciModx
            };
        }
    }
}
