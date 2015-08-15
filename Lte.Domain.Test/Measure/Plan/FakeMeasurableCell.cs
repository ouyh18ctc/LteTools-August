using Lte.Domain.Geo.Abstract;
using Lte.Domain.Measure;

namespace Lte.Domain.Test.Measure.Plan
{
    public class FakeMeasurableCell : MeasurableCell
    {
        public IOutdoorCell OutdoorCell
        {
            set { Cell.Cell = value; }
        }

        public new byte PciModx
        {
            set { Cell.PciModx = value; }
        }

    }
}
