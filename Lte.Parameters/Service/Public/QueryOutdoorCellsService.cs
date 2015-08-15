using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Public
{
    public interface IQueryOutdoorCellsService<TOutdoorCell>
        where TOutdoorCell : IOutdoorCell, IGeoPointReadonly<double>
    {
        List<TOutdoorCell> Query();
    }

    public abstract class FromCdmaQueryOutdoorCellService<TOutdoorCell> : IQueryOutdoorCellsService<TOutdoorCell>
        where TOutdoorCell : IOutdoorCell, IGeoPointReadonly<double>
    {
        protected ICdmaCellRepository _repository;

        protected FromCdmaQueryOutdoorCellService(ICdmaCellRepository repository)
        {
            _repository = repository;
        }

        public abstract List<TOutdoorCell> Query();
    }

    public abstract class FromLteQueryOutdoorCellService<TOutdoorCell> : IQueryOutdoorCellsService<TOutdoorCell>
        where TOutdoorCell : IOutdoorCell, IGeoPointReadonly<double>
    {
        protected ICellRepository _repository;

        protected FromLteQueryOutdoorCellService(ICellRepository repository)
        {
            _repository = repository;
        }

        public abstract List<TOutdoorCell> Query();
    }

    public class ByBtsQueryOutdoorCellService : FromCdmaQueryOutdoorCellService<EvaluationOutdoorCell>
    {
        private readonly CdmaBts _bts;

        public ByBtsQueryOutdoorCellService(ICdmaCellRepository repository, CdmaBts bts)
            : base(repository)
        {
            _bts = bts;
        }

        public override List<EvaluationOutdoorCell> Query()
        {
            IEnumerable<CdmaCell> cells = _repository.GetAllList().Where(x => x.BtsId == _bts.BtsId && x.Height > 0);
            return cells.Select(cell => new EvaluationOutdoorCell(_bts, cell)).ToList();
        }
    }

    public class ByENodebQueryOutdoorCellService<TOutdoorCell> : FromLteQueryOutdoorCellService<TOutdoorCell>
        where TOutdoorCell : class, IOutdoorCell, IGeoPointReadonly<double>
    {
        private readonly ENodeb _eNodeb;
        private readonly Func<ENodeb, Cell, TOutdoorCell> _outdoorCellConstructor;

        public ByENodebQueryOutdoorCellService(ICellRepository repository, ENodeb eNodeb,
            Func<ENodeb, Cell, TOutdoorCell> outdoorCellConstructor)
            : base(repository)
        {
            _eNodeb = eNodeb;
            _outdoorCellConstructor = outdoorCellConstructor;
        }

        public override List<TOutdoorCell> Query()
        {
            IEnumerable<Cell> cells 
                = _repository.GetAll().Where(x => x.ENodebId == _eNodeb.ENodebId && x.Height > 0).ToList();
            return cells.Select(cell => _outdoorCellConstructor(_eNodeb, cell)).ToList();
        }
    }

    public class ByBtsListQueryOutdoorCellService : FromCdmaQueryOutdoorCellService<EvaluationOutdoorCell>
    {
        private readonly IEnumerable<CdmaBts> _btss;

        public ByBtsListQueryOutdoorCellService(ICdmaCellRepository repository, IEnumerable<CdmaBts> btss)
            : base(repository)
        {
            _btss = btss;
        }

        public override List<EvaluationOutdoorCell> Query()
        {
            List<EvaluationOutdoorCell> outdoorCells = new List<EvaluationOutdoorCell>();
            foreach (CdmaBts bts in _btss)
            {
                IEnumerable<CdmaCell> cells = _repository.GetAllList().Where(x => x.BtsId == bts.BtsId && x.Height > 0);
                outdoorCells.AddRange(cells.Select(cell => new EvaluationOutdoorCell(bts, cell)));
            }
            return outdoorCells;
        }
    }

    public static class ByENodebListQueryOutdoorCellService
    {
        public static List<EvaluationOutdoorCell> Query(
            this ICellRepository repository, IEnumerable<ENodeb> eNodebs)
        {
            var cells = from a in repository.GetAllList()
                        join b in eNodebs.ToList()
                        on a.ENodebId equals b.ENodebId
                        where a.Height > 0
                        select new { Cell = a, EName = b.Name };

            IEnumerable<EvaluationOutdoorCell> outdoorCells =
                cells.Select(x => new EvaluationOutdoorCell(x.EName, x.Cell));
            return outdoorCells.ToList();
        }

        public static List<EvaluationOutdoorCell> Query<TCell>(
            this ICellRepository repository, IEnumerable<TCell> cells, IEnumerable<ENodeb> eNodebs)
            where TCell : ICell
        {
            var results = from a in cells.ToList()
                join b in eNodebs.ToList()
                    on a.CellId equals b.ENodebId
                join c in repository.GetAllList()
                    on new { a.CellId, a.SectorId } equals new { CellId = c.ENodebId, c.SectorId}
                select new {Cell = a, EName = b.Name, Info = c};
            IEnumerable<EvaluationOutdoorCell> outdoorCells =
                results.Select(x => new EvaluationOutdoorCell(x.EName, x.Info));
            return outdoorCells.ToList();
        }
    }
}
