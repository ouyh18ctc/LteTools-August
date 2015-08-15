using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Entities;
using Lte.Domain.Geo.Service;

namespace Lte.Domain.Geo.Abstract
{
    public interface IOutdoorCell : IGeoPoint<double>
    {
        double AntennaGain { get; set; }

        double RsPower { get; set; }

        short Pci { get; }

        string CellName { get; }

        double Height { get; set; }

        double Azimuth { get; set; }

        double MTilt { get; set; }

        double ETilt { get; set; }

        int ENodebId { get; set; }

        short SectorId { get; set; }

        int Frequency { get; set; }
    }

    public static class OutdoorCellQueries
    {
        public static string Info(this IOutdoorCell outdoorCell)
        {
            return "小区名称：" + outdoorCell.CellName + "；频点：" + outdoorCell.Frequency
                + "；<br/>站高：" + outdoorCell.Height + "；方位角：" + outdoorCell.Azimuth
                + "；<br/>机械下倾：" + outdoorCell.MTilt + "；电子下倾：" + outdoorCell.ETilt;
        }

        public static List<SectorTriangle> GetSectors(this IEnumerable<IOutdoorCell> outdoorCells)
        {
            return outdoorCells.Select(t => t.GetSectorPoints(50)).ToList();
        }
    }
}
