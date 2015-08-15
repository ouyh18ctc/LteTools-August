using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Domain.Regular;
using Lte.Parameters.Abstract;

namespace Lte.Evaluations.Dingli
{
    public class CoverageStat : IGeoPoint<double>, ILteCell
    {
        public double Longtitute { get; set; }

        public double Lattitute { get; set; }

        public double BaiduLongtitute
        { get { return Longtitute + GeoMath.BaiduLongtituteOffset; } }

        public double BaiduLattitute
        { get { return Lattitute + GeoMath.BaiduLattituteOffset; } }

        public double Rsrp { get; set; }

        public double Sinr { get; set; }

        public int ENodebId { get; set; }

        public byte SectorId { get; set; }

        public int Earfcn { get; set; }

        public void Import<TLogRecord>(TLogRecord record) where
            TLogRecord : class, ILogRecord, new()
        {
            record.CloneProperties(this);
        }

    }
}
