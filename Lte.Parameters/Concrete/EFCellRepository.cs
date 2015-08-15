using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Concrete
{
    public class EFCellRepository : LightWeightRepositroyBase<Cell>, ICellRepository
    {
        protected override DbSet<Cell> Entities
        {
            get { return context.Cells; }
        }

        public void AddCells(IEnumerable<Cell> cells)
        {
            foreach (Cell cell in cells)
            {
                Insert(cell);
            }
        }
    }

    public class EFCdmaCellRepository : LightWeightRepositroyBase<CdmaCell>, ICdmaCellRepository
    {
        protected override DbSet<CdmaCell> Entities
        {
            get { return context.CdmaCells; }
        }
    }

    public class EFLteNeighborCellRepository : ILteNeighborCellRepository
    {
        private readonly EFParametersContext context = new EFParametersContext();

        public IQueryable<LteNeighborCell> NeighborCells
        {
            get { return context.LteNeighborCells.AsQueryable(); }
        }

        public IQueryable<NearestPciCell> NearestPciCells 
        { 
            get { return context.NearestPciCells.AsQueryable(); }
        }

        public void AddOneCell(LteNeighborCell cell)
        {
            context.LteNeighborCells.Add(cell);
        }

        public bool RemoveOneCell(LteNeighborCell cell)
        {
            return (context.LteNeighborCells.Remove(cell) != null);
        }

        public void SaveChanges()
        {
            context.SaveChangesWithDelay();
        }
    }

    public class EFMrsCellRepository : LightWeightRepositroyBase<MrsCellDate>, IMrsCellRepository
    {
        protected override DbSet<MrsCellDate> Entities
        {
            get { return context.MrsCells; }
        }
    }

    public class EFMrsCellTaRepository : LightWeightRepositroyBase<MrsCellTa>, IMrsCellTaRepository
    {
        protected override DbSet<MrsCellTa> Entities
        {
            get { return context.MrsTaCells; }
        }
    }

    public class EFMroCellRepository : LightWeightRepositroyBase<MroRsrpTa>, IMroCellRepository
    {
        protected override DbSet<MroRsrpTa> Entities
        {
            get { return context.MroRsrpTaCells; }
        }
    }
}
