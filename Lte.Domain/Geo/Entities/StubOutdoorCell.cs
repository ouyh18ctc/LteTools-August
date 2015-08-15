using Lte.Domain.Geo.Abstract;

namespace Lte.Domain.Geo.Entities
{
    public class StubOutdoorCell : IOutdoorCell, IGeoPointReadonly<double>
    {
        public double Longtitute { get; set; }

        public double Lattitute { get; set; }

        public double Height { get; set; }

        public double Azimuth { get; set; }

        public double MTilt { get; set; }

        public double ETilt { get; set; }

        public double AntennaGain { get; set; }

        public double RsPower { get; set; }

        public short Pci { get; set; }

        public string CellName { get; set; }

        public int ENodebId { get; set; }

        public short SectorId { get; set; }

        public int Frequency { get; set; }

        public StubOutdoorCell() { }

        public StubOutdoorCell(double x, double y, double azimuth = 0, double height = 30)
        {
            Longtitute = x;
            Lattitute = y;
            Azimuth = azimuth;
            Height = height;
        }

        public StubOutdoorCell(IGeoPoint<double> p, double azimuth = 0, double height = 30)
            : this(p.Longtitute, p.Lattitute, azimuth, height)
        {

        }

    }
}
