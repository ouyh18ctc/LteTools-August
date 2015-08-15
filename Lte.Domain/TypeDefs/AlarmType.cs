using System;
using System.Linq;

namespace Lte.Domain.TypeDefs
{
    public enum AlarmType : short
    {
        CeNotEnough,
        StarUnlocked,
        TrunkProblem,
        RssiProblem,
        CellDown,
        VswrProblem,
        Unimportant,
        Others
    }

    public static class AlarmTypeQueries
    {
        private static readonly Tuple<AlarmType, string>[] alarmTypeDescriptionList =
        {
            new Tuple<AlarmType,string>(AlarmType.CeNotEnough,"CE不足"),
            new Tuple<AlarmType,string>(AlarmType.StarUnlocked,"锁星问题"),
            new Tuple<AlarmType,string>(AlarmType.TrunkProblem,"传输问题"),
            new Tuple<AlarmType,string>(AlarmType.RssiProblem,"RSSI问题"),
            new Tuple<AlarmType,string>(AlarmType.CellDown,"小区退服"),
            new Tuple<AlarmType,string>(AlarmType.VswrProblem,"驻波比问题"),
            new Tuple<AlarmType,string>(AlarmType.Unimportant,"不影响业务问题"),
            new Tuple<AlarmType,string>(AlarmType.Others,"其他告警")
        };

        public static string GetAlarmTypeDescription(this AlarmType type)
        {
            Tuple<AlarmType, string> tuple = alarmTypeDescriptionList.FirstOrDefault(x => x.Item1 == type);
            return (tuple != null) ? tuple.Item2 : "其他告警";
        }

        public static AlarmType GetAlarmType(this string description)
        {
            Tuple<AlarmType, string> tuple = alarmTypeDescriptionList.FirstOrDefault(x => x.Item2 == description);
            return (tuple != null) ? tuple.Item1 : AlarmType.Others;
        }
    }
}
