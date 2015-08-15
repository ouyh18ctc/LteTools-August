using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Coverage;

namespace Lte.Evaluations.Dingli
{
    public class BasicRateStat
    {
        public DateTime Time { get; set; }

        public double Rsrp { get; set; }

        public double Sinr { get; set; }

        public double AverageCqi { get; set; }

        public short UlMcs { get; set; }

        public short DlMcs { get; set; }

        public int UlThroughput { get; set; }

        public long DlThroughput { get; set; }

        public long PhyThroughputCode0 { get; set; }

        public long PhyThroughputCode1 { get; set; }

        public int PuschRbRate { get; set; }

        public int PdschRbRate { get; set; }

        public double PhyRatePerRb
        {
            get
            {
                return Math.Max(PhyThroughputCode0, PhyThroughputCode1) / (double)PdschRbRate;
            }
        }

        public double DlFrequencyEfficiency
        {
            get { return Math.Min(5, PhyRatePerRb * 2 / 200); }
        }

        public double DlRbsPerSlot
        {
            get { return (double)PdschRbRate / 2000; }
        }

        public double PassedTimeInSeconds { get; set; }

        public BasicRateStat()
        {
            this.ResetProperties();
        }
    }

    public class RateStat : BasicRateStat
    {
        public short Pci { get; set; }

        public int Earfcn { get; set; }

        public int PdschTbCode0 { get; set; }

        public int PdschTbCode1 { get; set; }

        public void Import<TLogRecord>(TLogRecord record) where 
            TLogRecord : class, ILogRecord, new()
        {
            record.CloneProperties(this);
        }
    }

    public enum DingliEventDef
    {
        HandoverRequest,
        HandoverSuccess,
        HandoverFailure,
        Undefined
    }

    public static class DingliEventQueries
    {
        private static Dictionary<DingliEventDef, string> list = new Dictionary<DingliEventDef, string>{
            {DingliEventDef.HandoverRequest,"RRC Connection ReconfigurationLTE Handover Request;"},
            {DingliEventDef.HandoverSuccess,"RRC Connection Reconfiguration CompleteLTE Handover Success;"},
            {DingliEventDef.HandoverFailure,"RRC Connection Reestablish RequestLTE Handover Failure;"},
            {DingliEventDef.Undefined,"Undefined"}
        };

        public static string GetEventDescription(this DingliEventDef dingliEvent)
        {
            return list[dingliEvent];
        }

        public static DingliEventDef GetEventType(this string eventDescription)
        {
            return (list.ContainsValue(eventDescription)) ?
                list.FirstOrDefault(x => x.Value == eventDescription).Key :
                DingliEventDef.Undefined;
        }
    }

    public class TestListViewModel
    {
        public List<AreaTestDate> AreaTestDateList { get; set; }

        public IEnumerable<Town> TownList { get; set; }
    }

    public class TestFileRecordsViewModel
    {
        public string FileName { get; set; }

        public double CenterX { get; set; }

        public double CenterY { get; set; }
    }
}
