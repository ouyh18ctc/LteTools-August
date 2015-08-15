using System;
using Lte.Domain.LinqToCsv;

namespace Lte.Evaluations.Dingli
{
    public class HandoverInfo
    {
        [CsvColumn(Name = "切换请求时间", OutputFormat = "HH:mm:ss.fff", FieldIndex = 1)]
        public DateTime RequestTime { get; set; }

        [CsvColumn(Name = "切换完成时间", OutputFormat = "HH:mm:ss.fff", FieldIndex = 2)]
        public DateTime FinishedTime { get; set; }

        [CsvColumn(Name = "测量报告时间", OutputFormat = "HH:mm:ss.fff", FieldIndex = 3)]
        public DateTime MeasureTime { get; set; }

        [CsvColumn(Name = "切换是否成功", FieldIndex = 4)]
        public bool HandoverSuccess { get; set; }

        [CsvColumn(Name = "切换前PCI", FieldIndex = 5)]
        public short PciBefore { get; set; }

        [CsvColumn(Name = "切换前ENodebId", FieldIndex = 6)]
        public int ENodebIdBefore { get; set; }

        [CsvColumn(Name = "切换前SectorId", FieldIndex = 7)]
        public short SectorIdBefore { get; set; }

        [CsvColumn(Name = "切换前频点", FieldIndex = 8)]
        public int EarfcnBefore { get; set; }

        [CsvColumn(Name = "切换前RSRP", FieldIndex = 9)]
        public double RsrpBefore { get; set; }

        [CsvColumn(Name = "切换前下行速率", FieldIndex = 10)]
        public int DlThroughputBefore { get; set; }

        [CsvColumn(Name = "切换前上行速率", FieldIndex = 11)]
        public int UlThroughputBefore { get; set; }

        [CsvColumn(Name = "切换后PCI", FieldIndex = 12)]
        public short PciAfter { get; set; }

        [CsvColumn(Name = "切换后频点", FieldIndex = 15)]
        public int EarfcnAfter { get; set; }

        [CsvColumn(Name = "切换后ENodebId", FieldIndex = 13)]
        public int ENodebIdAfter { get; set; }

        [CsvColumn(Name = "切换后SectorId", FieldIndex = 14)]
        public short SectorIdAfter { get; set; }

        [CsvColumn(Name = "切换后RSRP", FieldIndex = 16)]
        public double RsrpAfter { get; set; }

        [CsvColumn(Name = "切换后下行速率", FieldIndex = 17)]
        public int DlThroughputAfter{ get; set; }

        [CsvColumn(Name = "切换后上行速率", FieldIndex = 18)]
        public int UlThroughputAfter { get; set; }

        [CsvColumn(Name = "切换请求经度", FieldIndex = 19)]
        public double RequestLongitude { get; set; }

        [CsvColumn(Name = "切换请求纬度", FieldIndex = 20)]
        public double RequestLatitude { get; set; }

        [CsvColumn(Name = "切换完成经度", FieldIndex = 21)]
        public double FinishLongtitude { get; set; }

        [CsvColumn(Name = "切换完成纬度", FieldIndex = 22)]
        public double FinishLatitude { get; set; }

        public HandoverInfo()
        {
        }

        public HandoverInfo(LogRecord handoverRequestRecord) : this()
        {
            RequestLongitude = handoverRequestRecord.Longtitute;
            RequestLatitude = handoverRequestRecord.Lattitute;
            RequestTime = handoverRequestRecord.Time;
            PciBefore = handoverRequestRecord.Pci;
            EarfcnBefore = handoverRequestRecord.Earfcn;
            ENodebIdBefore = handoverRequestRecord.ENodebId;
            SectorIdBefore = handoverRequestRecord.SectorId;
            RsrpBefore = handoverRequestRecord.Rsrp;
        }

        public void UpdateCellInfoAfter(LogRecord record)
        {
            PciAfter = record.Pci;
            EarfcnAfter = record.Earfcn;
            ENodebIdAfter = record.ENodebId;
            SectorIdAfter = record.SectorId;
        }

        public void UpdateCellInfoBefore(LogRecord record)
        {
            PciBefore = record.Pci;
            EarfcnBefore = record.Earfcn;
            ENodebIdBefore = record.ENodebId;
            SectorIdBefore = record.SectorId;
        }

        public void Success(LogRecord successRecord)
        {
            HandoverSuccess = true;
            FinishedTime = successRecord.Time;
            FinishLongtitude = successRecord.Longtitute;
            FinishLatitude = successRecord.Lattitute;
            UpdateCellInfoAfter(successRecord);
            RsrpAfter = successRecord.Rsrp;
        }

        public void Fail(LogRecord failRecord)
        {
            HandoverSuccess = false;
            FinishedTime = failRecord.Time;
            FinishLongtitude = failRecord.Longtitute;
            FinishLatitude = failRecord.Lattitute;
            UpdateCellInfoAfter(failRecord);
        }
    }
}
