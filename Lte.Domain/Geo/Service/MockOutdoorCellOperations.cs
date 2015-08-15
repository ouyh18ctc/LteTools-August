using Lte.Domain.Geo.Abstract;
using Moq;

namespace Lte.Domain.Geo.Service
{
    public static class MockOutdoorCellOperations
    {
        public static void MockStandardOutdoorCell(this Mock<IOutdoorCell> oc, double longtitute,
            double lattitute, double azimuth, double height = 40, double etilt = 1, double mtilt = 4)
        {
            oc.SetupGet(x => x.Longtitute).Returns(longtitute);
            oc.SetupGet(x => x.Lattitute).Returns(lattitute);
            oc.SetupGet(x => x.Height).Returns(height);
            oc.SetupGet(x => x.MTilt).Returns(mtilt);
            oc.SetupGet(x => x.ETilt).Returns(etilt);
            oc.SetupGet(x => x.Azimuth).Returns(azimuth);
        }

        private static void MockStandardOutdoorCell<TOutdoorCell>(this Mock<TOutdoorCell> oc, double longtitute,
            double lattitute, double azimuth, double height = 40, double etilt = 1, double mtilt = 4)
            where TOutdoorCell : class, IOutdoorCell
        {
            oc.SetupGet(x => x.Longtitute).Returns(longtitute);
            oc.SetupGet(x => x.Lattitute).Returns(lattitute);
            oc.SetupGet(x => x.Height).Returns(height);
            oc.SetupGet(x => x.MTilt).Returns(mtilt);
            oc.SetupGet(x => x.ETilt).Returns(etilt);
            oc.SetupGet(x => x.Azimuth).Returns(azimuth);
        }

        public static void MockOutdoorCell(this Mock<IOutdoorCell> oc, double longtitute, double lattitute, double azimuth,
            double rsPower, double antennaGain, short pci = 0, double height = 40, double etilt = 1, double mtilt = 4)
        {
            oc.MockStandardOutdoorCell<IOutdoorCell>(longtitute, lattitute, azimuth, height, etilt, mtilt);
            oc.SetupGet(x => x.RsPower).Returns(rsPower);
            oc.SetupGet(x => x.AntennaGain).Returns(antennaGain);
            oc.SetupGet(x => x.Pci).Returns(pci);
        }
    }
}
