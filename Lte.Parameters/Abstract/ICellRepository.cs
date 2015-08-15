using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Lte.Domain.Geo.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Abstract
{
    public interface ICellRepository : IRepository<Cell>
    {
        void AddCells(IEnumerable<Cell> cells);
    }

    public interface ICdmaCellRepository : IRepository<CdmaCell>
    {
    }

    public interface INearestPciCellRepository
    {
        List<NearestPciCell> NearestPciCells { get; }
            
        NearestPciCell Import(ICell cell, short pci);

        void AddNeighbors(ILteNeighborCellRepository repository, int eNodebId);
    }

    public interface ILteNeighborCellRepository
    {
        IQueryable<LteNeighborCell> NeighborCells { get; }

        IQueryable<NearestPciCell> NearestPciCells { get; } 

        void AddOneCell(LteNeighborCell cell);

        bool RemoveOneCell(LteNeighborCell cell);

        void SaveChanges();
    }

    public interface IExcelCellImportRepository<TCell>
        where TCell : class, IValueImportable, new()
    {
        List<TCell> CellExcelList { get; }
    }

    public interface IMrsCellRepository : IRepository<MrsCellDate>
    {
    }

    public interface IMrsCellTaRepository : IRepository<MrsCellTa>
    {
    }

    public interface IMroCellRepository : IRepository<MroRsrpTa>
    {
    }
}
