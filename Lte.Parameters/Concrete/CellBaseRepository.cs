using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Entities;
using Lte.Parameters.Abstract;

namespace Lte.Parameters.Concrete
{
    public class CellBaseRepository : IDisposable
    {
        private List<CellBase> cellBaseList = new List<CellBase>();

        public List<CellBase> CellBaseList
        {
            get { return cellBaseList; }
        }

        public CellBaseRepository(ICellRepository inputRepository)
        {
            cellBaseList.Clear();
            foreach (Cell cell in inputRepository.GetAllList())
            {
                cellBaseList.Add(new CellBase
                {
                    ENodebId = cell.ENodebId,
                    SectorId = cell.SectorId
                });
            }
        }

        public CellBase QueryCell(int eNodebId, byte sectorId)
        {
            return cellBaseList.FirstOrDefault(
                x => x.ENodebId == eNodebId && x.SectorId == sectorId);
        }

        public void ImportNewCellInfo(CellExcel cellInfo)
        {
            CellBase cell = QueryCell(cellInfo.ENodebId, cellInfo.SectorId);
            if (cell == null)
            {
                cellBaseList.Add(new CellBase
                {
                    ENodebId = cellInfo.ENodebId,
                    SectorId = cellInfo.SectorId
                });
            }
        }

        public void Dispose()
        {
            cellBaseList = null;
        }
    }

    public class CdmaCellBaseRepository : IDisposable
    {
        private List<CdmaCellBase> cellBaseList = new List<CdmaCellBase>();

        public List<CdmaCellBase> CellBaseList
        {
            get { return cellBaseList; }
        }

        public CdmaCellBaseRepository(ICdmaCellRepository inputRepository)
        {
            cellBaseList.Clear();
            foreach (CdmaCell cell in inputRepository.GetAllList())
            {
                cellBaseList.Add(new CdmaCellBase(cell));
            }
        }

        public CdmaCellBase QueryCell(int btsId, byte sectorId, string cellType)
        {
            return cellBaseList.FirstOrDefault(
                x => x.BtsId == btsId && x.SectorId == sectorId && x.CellType == cellType);
        }

        public void ImportNewCellInfo(CdmaCellExcel cellInfo)
        {
            CdmaCellBase cell = QueryCell(cellInfo.BtsId, cellInfo.SectorId, cellInfo.CellType);
            if (cell == null)
            {
                CdmaCellBase cellBase = new CdmaCellBase
                {
                    BtsId = cellInfo.BtsId,
                    SectorId = cellInfo.SectorId,
                    CellType = cellInfo.CellType
                };
                cellBase.AddFrequency(cellInfo.Frequency);
                cellBaseList.Add(cellBase);
            }
            else
            { cell.AddFrequency(cellInfo.Frequency); }
        }

        public void Dispose()
        {
            cellBaseList = null;
        }
    }
}
