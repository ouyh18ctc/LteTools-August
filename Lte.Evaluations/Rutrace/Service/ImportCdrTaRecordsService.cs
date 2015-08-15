using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Evaluations.Rutrace.Record;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.Rutrace.Service
{
    public abstract class ImportCdrTaRecordsService
    {
        protected readonly List<CdrTaRecord> _details;
        protected CdrTaRecord _detail;
        protected readonly CdrRtdRecord _record;

        protected ImportCdrTaRecordsService(List<CdrTaRecord> details, CdrRtdRecord record)
        {
            _details = details;
            _record = record;
            _detail = details.FirstOrDefault(x => x.CellId == record.CellId && x.SectorId == record.SectorId);
        }

        protected abstract void ImportWhenDetailIsNull();

        protected abstract void IncreaseTaInnerIntervalNum();

        protected abstract void IncreaseTaOuterIntervalNum();

        protected abstract void CalculateThreshold();

        public void Import()
        {
            if (_detail == null)
            {
                ImportWhenDetailIsNull();
            }
            if (_detail == null) return;
            CalculateThreshold();
            if (InterferenceStat.IsInnerBound(_record.Rtd))
            {
                IncreaseTaInnerIntervalNum();
            }
            else
            {
                IncreaseTaOuterIntervalNum();
            }
        }
    }

    public class ImportMainCdrTaRecordsService : ImportCdrTaRecordsService
    {
        public ImportMainCdrTaRecordsService(List<CdrTaRecord> details, CdrRtdRecord record)
            : base(details, record)
        {
        }

        protected override void ImportWhenDetailIsNull()
        {
            _detail = new CdrTaRecord
            {
                CellId = _record.CellId,
                SectorId = _record.SectorId
            };
            _details.Add(_detail);
        }

        protected override void IncreaseTaInnerIntervalNum()
        {
            _detail.TaInnerIntervalNum++;
        }

        protected override void IncreaseTaOuterIntervalNum()
        {
            _detail.TaOuterIntervalNum++;
        }

        protected override void CalculateThreshold()
        {
            double rtd = _record.Rtd;
            _detail.TaMin = Math.Min(_detail.TaMin, rtd);
            _detail.TaMax = Math.Max(_detail.TaMax, rtd);
            _detail.TaSum += rtd;
        }
    }

    public class ImportExcessCdrTaRecordsService : ImportCdrTaRecordsService
    {
        private double _threshold;

        public ImportExcessCdrTaRecordsService(List<CdrTaRecord> details, CdrRtdRecord record)
            : base(details, record)
        {
            
        }

        protected override void ImportWhenDetailIsNull()
        {
            
        }

        protected override void IncreaseTaInnerIntervalNum()
        {
            if (_record.Rtd > _threshold)
            { _detail.TaInnerIntervalExcessNum++; }
        }

        protected override void IncreaseTaOuterIntervalNum()
        {
            if (_record.Rtd > _threshold)
            { _detail.TaOuterIntervalExcessNum++; }
        }

        protected override void CalculateThreshold()
        {
            _threshold = _detail.Threshold;
        }
    }
}
