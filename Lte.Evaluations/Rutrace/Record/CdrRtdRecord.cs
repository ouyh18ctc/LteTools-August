using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;
using Lte.Evaluations.Rutrace.Entities;

namespace Lte.Evaluations.Rutrace.Record
{
    public class CdrRtdRecord : ICell
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public double Rtd { get; set; }

        public CdrRtdRecord() { }

        public CdrRtdRecord(string[] fields)
        {
            CellId = fields[3].ConvertToInt(-1);
            SectorId = fields[4].ConvertToByte(15);
            Rtd = fields[7].ConvertToDouble(0) * 244 / 8;
        }

        public CdrRtdRecord(MrRecord mrRecord)
        {
            mrRecord.RefCell.CloneProperties(this);
            Rtd = mrRecord.RefCell.Ta*78.12;
        }
    }

}
