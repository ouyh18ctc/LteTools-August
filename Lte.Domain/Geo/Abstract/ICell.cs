using System;

namespace Lte.Domain.Geo.Abstract
{
    public interface ICell
    {
        int CellId { get; set; }

        byte SectorId { get; set; }
    }

    public interface ICdmaCell : ICell
    {
        int BtsId { get; set; }
    }

    public interface ITownId
    {
        int TownId { get; set; }
    }

    public interface ILteCell
    {
        int ENodebId { get; set; }

        byte SectorId { get; set; }
    }

    public interface ILogRecord : ILteCell
    {
        double Rsrp { get; set; }

        double Sinr { get; set; }

        double AverageCqi { get; set; }

        short UlMcs { get; set; }

        short DlMcs { get; set; }

        int UlThroughput { get; set; }

        int DlThroughput { get; set; }

        int PuschRbRate { get; set; }

        int PdschRbRate { get; set; }

        DateTime Time { get; set; }

        short Pci { get; set; }

        int Earfcn { get; set; }

        int PdschTbCode0 { get; set; }

        int PdschTbCode1 { get; set; }
    }
}
