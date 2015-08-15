using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Domain.LinqToCsv;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Dingli
{
    public interface IServingCellRecord : IGeoPoint<double>, ILteCell
    {
        short Pci { get; set; }

        int Earfcn { get; set; }
    }

    public static class ServingCellRecordQueries
    {
        public static void UpdateCellInfo(this IServingCellRecord record, IEnumerable<Cell> cells)
        {
            IEnumerable<Cell> pciCells = cells.Where(x => x.Pci == record.Pci);
            if (pciCells.Any())
            {
                double minDistance = pciCells.Min(x => x.SimpleDistance(record));
                Cell cell = pciCells.FirstOrDefault(x => Math.Abs(x.SimpleDistance(record) - minDistance) < 1E-10);

                if (cell != null)
                {
                    record.ENodebId = cell.ENodebId;
                    record.SectorId = cell.SectorId;
                    record.Earfcn = cell.Frequency;
                }
            }
        }

        public static void UpdateCellInfo<TRecord>(this TRecord record, IEnumerable<Cell> cells, double degreeSpan)
            where TRecord : IServingCellRecord, IGeoPointReadonly<double>
        {
            record.UpdateCellInfo(record.FilterGeoPointList(cells,
                degreeSpan));
        }
    }

    public class HugelandRecord : IServingCellRecord, ILogRecord
    {
        [CsvColumn(Name = "Index")]
        public int Id { get; set; }

        [CsvColumn(Name = "Time", OutputFormat = "HH:mm:ss.fff")]
        public DateTime Time { get; set; }

        [CsvColumn(Name = "Lon")]
        public double Longtitute { get; set; }

        [CsvColumn(Name = "Lat")]
        public double Lattitute { get; set; }

        [CsvColumn(Name = "eNodeB ID")]
        public int ENodebId { get; set; }

        [CsvColumn(Name = "Cell ID")]
        public byte SectorId { get; set; }

        [CsvColumn(Name = "PCI")]
        public short Pci { get; set; }

        [CsvColumn(Name = "Frequency DL(MHz)")]
        public double DownlinkFrequency { get; set; }

        public int Earfcn
        {
            get { return DownlinkFrequency.GetEarfcn(); }
            set { DownlinkFrequency = value.GetFrequency(); }
        }

        [CsvColumn(Name = "CRS RSRP")]
        public double Rsrp { get; set; }

        [CsvColumn(Name = "CRS SINR")]
        public double Sinr { get; set; }

        [CsvColumn(Name = "DL BLER(%)")]
        public double PdschBler { get; set; }

        [CsvColumn(Name = "CQI Average")]
        public double AverageCqi { get; set; }

        [CsvColumn(Name = "UL MCS Value # Average")]
        public short UlMcs { get; set; }

        [CsvColumn(Name = "DL MCS Value # Average")]
        public short DlMcs { get; set; }

        [CsvColumn(Name = "PDCP Thr'put UL(kb/s)")]
        public double UlThroughputInkbps { get; set; }

        public int UlThroughput
        {
            get { return (int)(UlThroughputInkbps * 1024); }
            set { UlThroughputInkbps = (double)value / 1024; }
        }

        [CsvColumn(Name = "PDCP Thr'put DL(kb/s)")]
        public double DlThroughputInkbps { get; set; }

        public int DlThroughput
        {
            get { return (int)(DlThroughputInkbps * 1024); }
            set { DlThroughputInkbps = (double)value / 1024; }
        }

        [CsvColumn(Name = "PHY Thr'put DL(kb/s)")]
        public double PhyThroughputCode0Inkbps { get; set; }

        public int PhyThroughputCode0
        {
            get { return (int)(PhyThroughputCode0Inkbps * 1024); }
            set { PhyThroughputCode0Inkbps = (double)value / 1024; }
        }

        public int PhyThroughputCode1
        {
            get { return 0; }
            set { }
        }

        [CsvColumn(Name = "PUSCH Rb Num/s")]
        public int PuschRbRate { get; set; }

        [CsvColumn(Name = "PDSCH Rb Num/s")]
        public int PdschRbRate { get; set; }

        [CsvColumn(Name = "PDSCH TB Size Ave(bits)")]
        public int PdschTbCode0 { get; set; }

        [CsvColumn(Name = "PDSCH TB Size code1 (Byte)")]
        public int PdschTbCode1
        {
            get { return 0; }
            set { }
        }

        public HugelandRecord()
        {
            Rsrp = -9999;
            Sinr = -9999;
            UlThroughput = 0;
            DlThroughput = 0;
            Pci = -1;
            ENodebId = -1;
            SectorId = 255;
        }

        public HugelandRecord Normalize()
        {
            while (PdschRbRate > 200000)
            {
                PdschRbRate /= 2;
                PuschRbRate /= 2;
                DlThroughputInkbps /= 2;
                UlThroughputInkbps /= 2;
                PhyThroughputCode0Inkbps /= 2;
            }
            return this;
        }
    }
}
