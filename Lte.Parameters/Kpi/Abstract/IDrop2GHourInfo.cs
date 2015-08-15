using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lte.Domain.Regular;
using Lte.Parameters.Kpi.Entities;

namespace Lte.Parameters.Kpi.Abstract
{
    public interface IDrop2GHourInfo<T>
    {
        T Hour0Info { get; set; }

        T Hour10Info { get; set; }

        T Hour11Info { get; set; }

        T Hour12Info { get; set; }

        T Hour13Info { get; set; }

        T Hour14Info { get; set; }

        T Hour15Info { get; set; }

        T Hour16Info { get; set; }

        T Hour17Info { get; set; }

        T Hour18Info { get; set; }

        T Hour19Info { get; set; }

        T Hour1Info { get; set; }

        T Hour20Info { get; set; }

        T Hour21Info { get; set; }

        T Hour22Info { get; set; }

        T Hour23Info { get; set; }

        T Hour2Info { get; set; }

        T Hour3Info { get; set; }

        T Hour4Info { get; set; }

        T Hour5Info { get; set; }

        T Hour6Info { get; set; }

        T Hour7Info { get; set; }

        T Hour8Info { get; set; }

        T Hour9Info { get; set; }
    }

    public static class Drop2GHourInfoQueries
    {
        public static TInfo GenerateHourInfo<TInfo, TValue>(
            this IDrop2GHourInfo<string> csvStat)
            where TInfo : class,IDrop2GHourInfo<TValue>, new()
        {
            TInfo info = new TInfo();
            csvStat.ConvertProperties<IDrop2GHourInfo<string>, IDrop2GHourInfo<TValue>>(info);
            return info;
        }

        public static List<AlarmHourInfo> GenerateAlarmHourInfos(
            this IDrop2GHourInfo<string> csvStat)
        {
            return csvStat.GenerateHourInfos<AlarmHourInfo>((i, s, h) => i.UpdateInfos(s, h));
        }

        public static List<NeighborHourInfo> GenerateNeighborHourInfos(
            this IDrop2GHourInfo<string> csvStat)
        {
            return csvStat.GenerateHourInfos<NeighborHourInfo>((i, s, h) => i.UpdateInfos(s, h));
        }

        public static List<T> GenerateHourInfos<T>(
            this IDrop2GHourInfo<string> csvStat,
            Action<List<T>, object, short> UpdateInfos)
        {
            List<T> infos = new List<T>();
            PropertyInfo[] properties = (typeof(IDrop2GHourInfo<string>)).GetProperties();

            for (short hour = 0; hour < 24; hour++)
            {
                string propertyName = "Hour" + hour + "Info";
                PropertyInfo property = properties.FirstOrDefault(x => x.Name == propertyName);
                if (property != null)
                {
                    object statContent = property.GetValue(csvStat);
                    UpdateInfos(infos, statContent, hour);
                }
            }
            return infos;
        }

    }
}
