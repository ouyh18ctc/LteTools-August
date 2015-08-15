using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Geo.Service;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Rutrace.Record
{
    public class NearestPciCellRepository : INearestPciCellRepository
    {
        public List<NearestPciCell> NearestPciCells { get; private set; }

        private readonly IEnumerable<Cell> _cells;

        public NearestPciCellRepository(IEnumerable<Cell> cells)
        {
            _cells = cells;
        }

        public NearestPciCellRepository(IEnumerable<Cell> cells, IEnumerable<LteNeighborCell> neighborCells)
            : this(cells)
        {
            NearestPciCells = (from c in cells
                join nc in neighborCells on new { CellId = c.ENodebId, c.SectorId } 
                    equals new { CellId = nc.CellId, nc.SectorId}
                select new NearestPciCell
                {
                    CellId = c.ENodebId,
                    SectorId = c.SectorId,
                    NearestCellId = nc.NearestCellId,
                    NearestSectorId = nc.SectorId,
                    Pci = c.Pci
                }).ToList();
        }

        public void AddNeighbors(ILteNeighborCellRepository repository, int eNodebId)
        {
            if (NearestPciCells == null) NearestPciCells = new List<NearestPciCell>();
            if (NearestPciCells.FirstOrDefault(x => x.CellId == eNodebId) != null) return;
            NearestPciCells = new List<NearestPciCell>();
            NearestPciCells.AddRange(repository.NearestPciCells.Where(x => x.CellId == eNodebId));
        }

        public NearestPciCell Import(ICell cell, short pci)
        {
            int neiCellId = 0;
            byte neiSecId = 255;
            Cell refCell 
                = _cells.FirstOrDefault(x => x.ENodebId == cell.CellId && x.SectorId == cell.SectorId);
            if (refCell == null) return null;
            Cell innerCell 
                = _cells.FirstOrDefault(x => x.ENodebId == cell.CellId && x.SectorId != cell.SectorId && x.Pci == pci);
            if (innerCell != null)
            {
                neiCellId = cell.CellId;
                neiSecId = innerCell.SectorId;
            }
            else
            {
                IEnumerable<Cell> candidateCells = _cells.Where(x => x.Pci == pci);
                if (candidateCells.Any())
                {
                    double distance = Double.MaxValue;
                    Cell objCell = candidateCells.ElementAt(0);
                    foreach (Cell candidateCell in candidateCells)
                    {
                        double tempDistance = candidateCell.SimpleDistance(refCell);
                        if (tempDistance < distance)
                        {
                            objCell = candidateCell;
                            distance = tempDistance;
                        }
                    }
                    neiCellId = objCell.ENodebId;
                    neiSecId = objCell.SectorId;
                }
            }
            NearestPciCell newCell =
                NearestPciCells.FirstOrDefault(x => x.CellId == cell.CellId && x.SectorId == cell.SectorId
                                                    && x.Pci == pci);
            if (newCell == null)
            {
                newCell = new NearestPciCell
                {
                    CellId = cell.CellId,
                    SectorId = cell.SectorId,
                    NearestCellId = neiCellId,
                    NearestSectorId = neiSecId,
                    Pci = pci
                };
                NearestPciCells.Add(newCell);
            }
            else
            {
                newCell.NearestCellId = neiCellId;
                newCell.NearestSectorId = neiSecId;
            }
            return newCell;
        }
    }
}