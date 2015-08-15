using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lte.Evaluations.Infrastructure.Abstract;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Rutrace.Record
{
    public static class RecordSetImporter
    {
        public static MrRecordSet Generate(this List<CdrRtdRecord> taRecordList,
            ILteNeighborCellRepository repository, IEnumerable<Cell> cells, string path)
        {
            NearestPciCellRepository neighborRepository = new NearestPciCellRepository(cells);
            MrRecordSet recordSet;
            using (StreamReader stream = new StreamReader(path))
            {
                recordSet = new MroRecordSet(stream);
                neighborRepository.AddNeighbors(repository, recordSet.ENodebId);
                taRecordList.Update(neighborRepository, recordSet);
            }
            return recordSet;
        }

        private static void Update(this List<CdrRtdRecord> taRecordList,
            NearestPciCellRepository neighborRepository, MrRecordSet recordSet)
        {
            recordSet.ImportRecordSet(neighborRepository);
            taRecordList.AddRange(recordSet.RecordList.Select(x => new CdrRtdRecord(x)));
        }

        public static RuRecordSet Generate(string path)
        {
            RuRecordSet recordSet;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                recordSet = new RuRecordSet(stream);
            }
            return recordSet;
        }

        public static List<MrRecordSet> Import(this List<CdrRtdRecord> taRecordList,
            ILteNeighborCellRepository repository, IEnumerable<Cell> cells, IEnumerable<string> paths)
        {
            return paths.Select(x => taRecordList.Generate(repository, cells, x)).ToList();
        }

        public static List<RuRecordSet> ImportRuRecordSets(IEnumerable<string> paths)
        {
            return paths.Select(Generate).ToList();
        }
    }
}
