using Lte.Domain.Geo.Abstract;

namespace Lte.Parameters.Entities
{
    public class MrsCell : ICell
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int[] RsrpCounts { get; set; }

        public int[] TaCounts { get; set; }

        public MrsCell()
        {
            RsrpCounts = new int[48];
            TaCounts = new int[45];
        }
    }

    public interface IMrsCellDate
    {
        string DateString { get; }

        double CoveragePercentage { get; }

        int RsrpTo120 { get; set; }

        int RsrpTo115 { get; set; }

        int RsrpTo110 { get; set; }

        int RsrpTo105 { get; set; }

        int RsrpTo100 { get; set; }

        int RsrpTo95 { get; set; }

        int RsrpTo90 { get; set; }

        int RsrpTo85 { get; set; }

        int RsrpTo80 { get; set; }

        int RsrpTo70 { get; set; }

        int RsrpTo60 { get; set; }

        int RsrpAbove60 { get; set; }
    }
}
