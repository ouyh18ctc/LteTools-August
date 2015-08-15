using System;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.LinqToCsv;
using Lte.Parameters.Abstract;

namespace Lte.Evaluations.Dingli
{
    public class LogRecord : IGeoPoint<double>, ILogRecord
    {
        [CsvColumn(Name="Index")]
        public int Id { get; set; }

        [CsvColumn(Name = "Time", OutputFormat="HH:mm:ss.fff")]
        public DateTime Time { get; set; }

        [CsvColumn(Name = "Longitude")]
        public double Longtitute { get; set; }

        [CsvColumn(Name = "Latitude")]
        public double Lattitute { get; set; }

        [CsvColumn(Name = "eNodeBID")]
        public int ENodebId { get; set; }

        [CsvColumn(Name = "SectorID")]
        public byte SectorId { get; set; }

        [CsvColumn(Name = "PCI")]
        public short Pci { get; set; }

        [CsvColumn(Name = "EARFCN DL")]
        public int Earfcn { get; set; }

        [CsvColumn(Name = "RSRP (dBm)")]
        public double Rsrp { get; set; }

        [CsvColumn(Name = "SINR (dB)")]
        public double Sinr { get; set; }

        [CsvColumn(Name = "PDSCH BLER")]
        public double PdschBler { get; set; }

        [CsvColumn(Name = "WideBand CQI")]
        public short WidebanCqi { get; set; }

        public double AverageCqi
        {
            get { return WidebanCqi; }
            set { WidebanCqi = Convert.ToInt16(value); }
        }

        [CsvColumn(Name = "MCS Average UL /s")]
        public short UlMcs { get; set; }

        [CsvColumn(Name = "MCS Average DL /s")]
        public short DlMcs { get; set; }

        [CsvColumn(Name = "PDCP Throughput UL (bps)")]
        public int UlThroughput { get; set; }

        [CsvColumn(Name = "PDCP Throughput DL (bps)")]
        public int DlThroughput { get; set; }

        [CsvColumn(Name = "PHY Throughput code0 DL")]
        public int PhyThroughputCode0 { get; set; }

        [CsvColumn(Name = "PHY Throughput code1 DL")]
        public int PhyThroughputCode1 { get; set; }

        [CsvColumn(Name = "Event")]
        public string Event { get; set; }

        [CsvColumn(Name = "Message Type")]
        public string MessageType { get; set; }

        [CsvColumn(Name = "PUSCH RB Count /s")]
        public int PuschRbRate { get; set; }

        [CsvColumn(Name = "PDSCH RB Count /s")]
        public int PdschRbRate { get; set; }

        [CsvColumn(Name = "PUSCH Scheduled slot Count /s")]
        public int PuschScheduledSlots { get; set; }

        [CsvColumn(Name = "PDSCH Scheduled slot Count /s")]
        public int PdschScheduledSlots { get; set; }

        [CsvColumn(Name = "PDSCH TB Size code0 (Byte)")]
        public int PdschTbCode0 { get; set; }

        [CsvColumn(Name = "PDSCH TB Size code1 (Byte)")]
        public int PdschTbCode1 { get; set; }

        public LogRecord()
        {
            Rsrp = -9999;
            Sinr = -9999;
            UlThroughput = 0;
            DlThroughput = 0;
            Pci = -1;
            ENodebId = -1;
            SectorId = 255;
        }
    }
}
