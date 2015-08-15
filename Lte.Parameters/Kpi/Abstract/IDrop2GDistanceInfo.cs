using Lte.Domain.Regular;

namespace Lte.Parameters.Kpi.Abstract
{
    public interface IDrop2GDistanceInfo<T>
    {
        T DistanceTo1000Info { get; set; }

        T DistanceTo1200Info { get; set; }

        T DistanceTo1400Info { get; set; }

        T DistanceTo1600Info { get; set; }

        T DistanceTo1800Info { get; set; }

        T DistanceTo2000Info { get; set; }

        T DistanceTo200Info { get; set; }

        T DistanceTo2200Info { get; set; }

        T DistanceTo2400Info { get; set; }

        T DistanceTo2600Info { get; set; }

        T DistanceTo2800Info { get; set; }

        T DistanceTo3000Info { get; set; }

        T DistanceTo4000Info { get; set; }

        T DistanceTo400Info { get; set; }

        T DistanceTo5000Info { get; set; }

        T DistanceTo6000Info { get; set; }

        T DistanceTo600Info { get; set; }

        T DistanceTo7000Info { get; set; }

        T DistanceTo8000Info { get; set; }

        T DistanceTo800Info { get; set; }

        T DistanceTo9000Info { get; set; }

        T DistanceToInfInfo { get; set; }
    }

    public static class Drop2GDistanceInfoQueries
    {
        public static TInfo GenerateDistanceInfo<TInfo, TValue>(
            this IDrop2GDistanceInfo<string> csvStat)
            where TInfo : class, IDrop2GDistanceInfo<TValue>, new()
        {
            TInfo info = new TInfo();
            csvStat.ConvertProperties<IDrop2GDistanceInfo<string>, IDrop2GDistanceInfo<TValue>>(info);
            return info;
        }

    }
}
