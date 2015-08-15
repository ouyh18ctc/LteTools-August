using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Public
{
    public class CdmaLteIdsService
    {
        private readonly IEnumerable<CellExcel> _cellExcelList;

        public CdmaLteIdsService(IEnumerable<CellExcel> cellExcelList)
        {
            _cellExcelList = cellExcelList;
        }

        public IEnumerable<CdmaLteIds> Query()
        {
            return _cellExcelList.Select(x => new
            {
                ENodebId = x.ENodebId,
                CdmaCellInfo = x.CdmaCellId
            }).Distinct().Select(x => new CdmaLteIds
            {
                ENodebId = x.ENodebId,
                CdmaCellId = x.CdmaCellInfo.IndexOf('_') < 0
                    ? -1
                    : x.CdmaCellInfo.GetSplittedFields('_')[1].ConvertToInt(0)
            }).Where(x => x.CdmaCellId > 0).Distinct(p => p.CdmaCellId);
        }
    }
}
