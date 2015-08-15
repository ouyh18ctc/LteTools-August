using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;

namespace Lte.Domain.Geo.Service
{
    public abstract class QueryOutdoorCellService<TOutdoorCell>
        where TOutdoorCell : class, IOutdoorCell
    {
        protected readonly IEnumerable<TOutdoorCell> _cellList;
        protected const double Eps = 1E-6;

        public QueryOutdoorCellService(IEnumerable<TOutdoorCell> cellList)
        {
            _cellList = cellList;
        }

        private bool IsEmpty()
        {
            return (_cellList == null || !_cellList.Any());
        }

        public TOutdoorCell QueryByName(string name)
        {
            return IsEmpty() ? null : _cellList.FirstOrDefault(x => x.CellName == name);
        }

        public abstract IOutdoorCell QueryCell(IOutdoorCell cell);
    }

    public class QueryOutdoorCellFrequencyConsidered<TCell> : QueryOutdoorCellService<TCell>
        where TCell : class, IOutdoorCell
    {
        public QueryOutdoorCellFrequencyConsidered(IEnumerable<TCell> cellList) : base(cellList)
        {
        }

        public override IOutdoorCell QueryCell(IOutdoorCell cell)
        {
            return _cellList.FirstOrDefault(x => Math.Abs(x.Longtitute - cell.Longtitute) < Eps
                                                 && Math.Abs(x.Lattitute - cell.Lattitute) < Eps
                                                 && Math.Abs(x.Azimuth - cell.Azimuth) < Eps
                                                 && x.Frequency == cell.Frequency);
        }
    }

    public class QueryOutdoorCellFrequencyInconsidered<TCell> : QueryOutdoorCellService<TCell>
        where TCell : class, IOutdoorCell
    {
        public QueryOutdoorCellFrequencyInconsidered(IEnumerable<TCell> cellList) : base(cellList)
        {
        }

        public override IOutdoorCell QueryCell(IOutdoorCell cell)
        {
            return _cellList.FirstOrDefault(x => Math.Abs(x.Longtitute - cell.Longtitute) < Eps
                                            && Math.Abs(x.Lattitute - cell.Lattitute) < Eps
                                            && Math.Abs(x.Azimuth - cell.Azimuth) < Eps);
        }
    }
}
