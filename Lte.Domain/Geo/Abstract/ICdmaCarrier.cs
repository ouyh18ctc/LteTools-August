using Lte.Domain.Regular;

namespace Lte.Domain.Geo.Abstract
{
    public interface ICdmaCarrier : ICdmaCell
    {
        short Frequency { get; set; }

        short BscId { get; set; }
    }

    public static class CdmaCarrierQueries
    {
        public static void ImportCarrierInfo(this ICdmaCarrier stat, string[] fields)
        {
            if (fields.Length > 4)
            {
                stat.BscId = fields[0].ConvertToShort(1);
                stat.BtsId = fields[1].ConvertToInt(1);
                stat.CellId = fields[2].ConvertToInt(0);
                stat.SectorId = fields[3].ConvertToByte(0);
                stat.Frequency = fields[4].ConvertToShort(283);
            }
        }

    }

    public interface ICarrierName
    {
        string Carrier { get; }
    }

    public interface ICdmaLteNames
    {
        int ENodebId { get; set; }

        int CdmaCellId { get; set; }

        string CdmaName { get; set; }

        string LteName { get; set; }

        byte SectorId { get; set; }
    }
}