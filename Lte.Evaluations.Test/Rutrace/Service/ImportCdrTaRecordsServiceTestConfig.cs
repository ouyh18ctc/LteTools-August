using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Evaluations.Rutrace;
using Lte.Evaluations.Rutrace.Record;

namespace Lte.Evaluations.Test.Rutrace.Service
{
    public abstract class ImportCdrTaRecordsServiceTestConfig
    {
        protected ImportCdrTaRecordsServiceTestHelper helper;

        protected List<CdrTaRecord> details;

        protected void InitializeEmptyDetailsList()
        {
            details = new List<CdrTaRecord>();
        }

        protected CdrRtdRecord InitializeRecord(int cellId, byte sectorId, double rtd)
        {
            return new CdrRtdRecord
            {
                CellId = cellId,
                SectorId = sectorId,
                Rtd = rtd
            };
        }
    }
}
