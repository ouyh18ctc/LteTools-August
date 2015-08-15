using System.Collections.Generic;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Kpi.Service
{
    public class ParametersDumpInfrastructure
    {
        public int ENodebInserted { get; set; }

        public int ENodebsUpdated { get; set; }

        public int CellsInserted { get; set; }

        public int CellsUpdated { get; set; }

        public int CdmaBtsUpdated { get; set; }

        public int CdmaCellsInserted { get; set; }

        public int CdmaCellsUpdated { get; set; }

        public int NeighborPciUpdated { get; set; }

        public IExcelBtsImportRepository<ENodebExcel> LteENodebRepository { get; set; }

        public IExcelCellImportRepository<CellExcel> LteCellRepository { get; set; }

        public List<IMmlImportRepository<CdmaBts, CdmaCell, BtsExcel, CdmaCellExcel>> MmlRepositoryList { get; set; }

        public IExcelBtsImportRepository<BtsExcel> CdmaBtsRepository { get; set; }

        public IExcelCellImportRepository<CdmaCellExcel> CdmaCellRepository { get; set; }

        private bool LteENodebListIsEmpty
        {
            get
            {
                return LteENodebRepository == null ||
                    LteENodebRepository.BtsExcelList == null || LteENodebRepository.BtsExcelList.Count == 0;
            }
        }

        private bool LteCellListIsEmpty
        {
            get
            {
                return LteCellRepository == null ||
                    LteCellRepository.CellExcelList == null || LteCellRepository.CellExcelList.Count == 0;
            }
        }

        public bool MmlListIsEmpty
        {
            get
            {
                return MmlRepositoryList == null || MmlRepositoryList.Count == 0;
            }
        }

        private bool CdmaBtsListIsEmpty 
        {
            get
            {
                return CdmaBtsRepository == null ||
                    CdmaBtsRepository.BtsExcelList == null || CdmaBtsRepository.BtsExcelList.Count == 0;
            }
        }

        public bool CdmaCellListIsEmpty
        {
            get
            {
                return CdmaCellRepository == null ||
                    CdmaCellRepository.CellExcelList == null || CdmaCellRepository.CellExcelList.Count == 0;
            }
        }

        public bool CdmaListsAreEmpty
        {
            get
            {
                return (CdmaBtsListIsEmpty && CdmaCellListIsEmpty);
            }
        }
    }
}
