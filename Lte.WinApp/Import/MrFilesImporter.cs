using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Evaluations.Rutrace.Record;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Evaluations.Rutrace.Service;

namespace Lte.WinApp.Import
{
    public class MroFilesImporter
    {
        public List<PureInterferenceStat> InterferenceStats { get; private set; }
        public List<MroRsrpTa> RsrpTaStatList { get; private set; }
        private readonly NearestPciCellRepository _neighborRepository;
        private readonly ILteNeighborCellRepository _neighborCellRepository;

        public MroFilesImporter(ICellRepository cellRepository,
            ILteNeighborCellRepository neighborCellRepository)
        {
            RsrpTaStatList = new List<MroRsrpTa>();
            InterferenceStats = new List<PureInterferenceStat>();
            _neighborRepository
                = new NearestPciCellRepository(cellRepository.GetAllList());
            _neighborCellRepository = neighborCellRepository;
        }

        public void Import(IEnumerable<string> paths, Func<string, MroRecordSet> recordSetGenerator)
        {
            List<MrRecordSet> mrRecordSets = new List<MrRecordSet>();

            foreach (MroRecordSet recordSet in paths.Select(recordSetGenerator))
            {
                _neighborRepository.AddNeighbors(_neighborCellRepository, recordSet.ENodebId);
                RsrpTaStatList.Import(recordSet);
                recordSet.ImportRecordSet(_neighborRepository);
                mrRecordSets.Add(recordSet);
            }
            InterferenceStats.Import(mrRecordSets);
        }
    }

    public class MrsFilesImporter
    {
        public List<MrsCellDate> RsrpStatList { get; private set; }
        public List<MrsCellTa> TaStatList { get; private set; }

        public MrsFilesImporter()
        {
            RsrpStatList = new List<MrsCellDate>();
            TaStatList = new List<MrsCellTa>();
        }

        public void Import(IEnumerable<string> paths, Func<string, MrsRecordSet> recordSetGenerator)
        {
            foreach (MrsRecordSet recordSet in paths.Select(recordSetGenerator))
            {
                RsrpStatList.Import(recordSet);
                TaStatList.Import(recordSet);
            }

            foreach (MrsCellDate stat in RsrpStatList) stat.UpdateStats();
            foreach (MrsCellTa stat in TaStatList) stat.UpdateStats();
        }
    }
}
