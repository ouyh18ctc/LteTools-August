using System;
using System.Collections.Generic;
using Lte.Domain.Regular;

namespace Lte.Evaluations.Dingli
{
    public class LogRecordRepository
    {
        public List<LogRecord> LogRecordList { get; set; }

        public static int ThroughputCalculationDelay { get; set; }

        public List<HandoverInfo> GetHandoverInfoList()
        {
            List<HandoverInfo> resultList = new List<HandoverInfo>();
            HandoverInfo currentInfo = null;
            for (int i = 0; i < LogRecordList.Count; i++)
            {
                if (LogRecordList[i].Event == null) { continue; }
                string[] eventContent = LogRecordList[i].Event.GetSplittedFields(":-");
                if (eventContent.Length == 0) { continue; }
                for (int j = 0; j < eventContent.Length; j++)
                {
                    DingliEventDef currentEvent = eventContent[j].GetEventType();
                    switch (currentEvent)
                    {
                        case DingliEventDef.HandoverRequest:
                            if (currentInfo == null)
                            {
                                currentInfo = new HandoverInfo(LogRecordList[i]);
                                if (i > 0 && currentInfo.PciBefore != LogRecordList[i - 1].Pci &&
                                    LogRecordList[i - 1].ENodebId > 0)
                                {
                                    currentInfo.UpdateCellInfoBefore(LogRecordList[i - 1]);
                                }
                                CalculateThroughputBefore(currentInfo, i);
                                CalculateMeasureTime(currentInfo, i);
                            }
                            else
                            {
                                resultList.Add(currentInfo);
                                currentInfo = null;
                            }
                            break;
                        case DingliEventDef.HandoverSuccess:
                            if (currentInfo != null)
                            {
                                currentInfo.Success(LogRecordList[i]);
                                if (currentInfo.PciBefore == currentInfo.PciAfter)
                                {
                                    currentInfo.UpdateCellInfoAfter(LogRecordList[i + 1]);
                                }
                                CalculateThroughputAfter(currentInfo, i);
                                resultList.Add(currentInfo);
                                currentInfo = null;
                            }
                            break;
                        case DingliEventDef.HandoverFailure:
                            if (currentInfo != null)
                            {
                                currentInfo.Fail(LogRecordList[i]);
                                if (currentInfo.PciBefore == currentInfo.PciAfter)
                                {
                                    currentInfo.UpdateCellInfoAfter(LogRecordList[i + 1]);
                                }
                                CalculateThroughputAfter(currentInfo, i);
                                resultList.Add(currentInfo);
                                currentInfo = null;
                            }
                            break;
                    }
                }
            }
            return resultList;
        }

        private void CalculateMeasureTime(HandoverInfo currentInfo, int requestIndex)
        {
            for (int i = requestIndex; i >= 0; i--)
            {
                if (LogRecordList[i].MessageType.IndexOf("LTE RRC-->Measurement Report", StringComparison.Ordinal) >= 0)
                {
                    currentInfo.MeasureTime = (i == requestIndex) ?
                        LogRecordList[i].Time.AddMilliseconds(-15) :
                        LogRecordList[i].Time;
                    return;
                }
            }
        }

        private void CalculateThroughputBefore(HandoverInfo currentInfo, int requestIndex)
        {
            double DlThroughputSum = 0;
            double UlThroughputSum = 0;
            int DlThroughputCount = 0;
            int UlThroughputCount = 0;
            DateTime requestTime = LogRecordList[requestIndex].Time;
            for (int i = requestIndex - 1; i >= 0; i--)
            {
                if (LogRecordList[i].Time < requestTime.AddSeconds(-ThroughputCalculationDelay)) { break; }
                if (LogRecordList[i].DlThroughput > 0)
                {
                    DlThroughputSum += LogRecordList[i].DlThroughput;
                    DlThroughputCount++;
                }
                if (LogRecordList[i].UlThroughput > 0)
                {
                    UlThroughputSum += LogRecordList[i].UlThroughput;
                    UlThroughputCount++;
                }
            }
            if (DlThroughputCount > 0) { currentInfo.DlThroughputBefore = (int)DlThroughputSum / DlThroughputCount; }
            if (UlThroughputCount > 0) { currentInfo.UlThroughputBefore = (int)UlThroughputSum / UlThroughputCount; }
        }

        private void CalculateThroughputAfter(HandoverInfo currentInfo, int finishIndex)
        {
            double DlThroughputSum = 0;
            double UlThroughputSum = 0;
            int DlThroughputCount = 0;
            int UlThroughputCount = 0;
            DateTime finishTime = LogRecordList[finishIndex].Time;
            for (int i = finishIndex + 1; i <LogRecordList.Count; i++)
            {
                if (LogRecordList[i].Time > finishTime.AddSeconds(ThroughputCalculationDelay)) { break; }
                if (LogRecordList[i].DlThroughput > 0)
                {
                    DlThroughputSum += LogRecordList[i].DlThroughput;
                    DlThroughputCount++;
                }
                if (LogRecordList[i].UlThroughput > 0)
                {
                    UlThroughputSum += LogRecordList[i].UlThroughput;
                    UlThroughputCount++;
                }
            }
            if (DlThroughputCount > 0) { currentInfo.DlThroughputAfter = (int)DlThroughputSum / DlThroughputCount; }
            if (UlThroughputCount > 0) { currentInfo.UlThroughputAfter = (int)UlThroughputSum / UlThroughputCount; }
        }
    }
}
