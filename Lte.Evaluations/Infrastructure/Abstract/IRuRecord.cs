using System;
using System.Collections.Generic;
using Lte.Domain.Geo.Abstract;

namespace Lte.Evaluations.Infrastructure.Abstract
{
    public interface IRuRecord<out TRef, TNei>
        where TRef : class, IRefCell, new()
        where TNei : class, INeiCell, new()
    {
        TRef RefCell { get; }

        List<TNei> NbCells { get; }
    }

    public interface INeiCell : ICell
    {
        short Frequency { get; }

        double Strength { get; }
    }

    public interface IRefCell : ICell
    {
        short Frequency { get; set; }

        double Strength { get; }
    }

    public interface IRecordSet<TRecord, TRef, TNei>
        where TRecord : IRuRecord<TRef, TNei>
        where TRef : class, IRefCell, new()
        where TNei : class, INeiCell, new()
    {
        List<TRecord> RecordList { get; }

        DateTime RecordDate { get; set; }
    }
}