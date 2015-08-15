using System.Collections.Generic;
using Lte.Domain.Regular;
using Lte.Evaluations.Rutrace.Record;

namespace Lte.Evaluations.Rutrace.Service
{
    public class ImportCdrRtdRecordsService
    {
        private List<CdrRtdRecord> _records;
        private string[] _segments;

        public ImportCdrRtdRecordsService(List<CdrRtdRecord> records, string line)
        {
            _records = records;
            _segments = line.GetSplittedFields(';');
        }

        public void Import()
        {
            for (int i = 1; i < _segments.Length; i++)
            {
                string[] fields = _segments[i].GetSplittedFields('_');
                if (fields[0] == "D0")
                {
                    _records.Add(new CdrRtdRecord
                    {
                        CellId = fields[3].ConvertToInt(-1),
                        SectorId = fields[4].ConvertToByte(15),
                        Rtd = fields[7].ConvertToDouble(0) * 244 / 8
                    });
                }
            }
        }
    }
}
