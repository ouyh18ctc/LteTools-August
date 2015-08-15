using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Lte.Domain.Regular;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Lte
{
    public class SaveLteCellRelationService
    {
        private readonly ILteNeighborCellRepository _repository;

        public SaveLteCellRelationService(ILteNeighborCellRepository repository)
        {
            _repository = repository;
        }

        public void Save(IEnumerable<LteCellRelationCsv> infos)
        {
            foreach (LteCellRelationCsv info in infos)
            {
                string[] fields = info.NeighborRelation.Split(':');
                int eNodebId = fields[3].ConvertToInt(0);
                byte sectorId = fields[4].ConvertToByte(0);
                if (eNodebId < 10000) continue;
                LteNeighborCell nCell = _repository.NeighborCells.FirstOrDefault(x =>
                    x.CellId == info.ENodebId && x.SectorId == info.SectorId
                    && x.NearestCellId == eNodebId && x.NearestSectorId == sectorId);
                if (nCell != null) continue;
                nCell = new LteNeighborCell
                {
                    CellId = info.ENodebId,
                    SectorId = info.SectorId,
                    NearestCellId = eNodebId,
                    NearestSectorId = sectorId
                };
                _repository.AddOneCell(nCell);
                if (eNodebId%1000 == sectorId)
                {
                    _repository.SaveChanges();
                }
            }
            _repository.SaveChanges();
        }

        public static int UpdateNeighborPci(IEnumerable<CellExcel> cells)
        {
            SqlConnection conn = new SqlConnection(
                "Data Source=WIN-E7U0ZAGEQAQ;Initial Catalog=ouyanghui_practise;User ID=ouyanghui;Password=123456");
            conn.Open();
            int count = 0;
            using (SqlCommand cmd = new SqlCommand("sp_UpdateNearestPci", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@eNodebId",
                    SqlDbType = SqlDbType.Int,
                    Value = 0
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@sectorId",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = 0
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@pci",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = 0
                });
                foreach (CellExcel cell in cells)
                {
                    cmd.Parameters[0].Value = cell.ENodebId;
                    cmd.Parameters[1].Value = cell.SectorId;
                    cmd.Parameters[2].Value = cell.Pci;
                    count += cmd.ExecuteNonQuery();
                }
            }
            conn.Close();
            return count;
        }
    }
}