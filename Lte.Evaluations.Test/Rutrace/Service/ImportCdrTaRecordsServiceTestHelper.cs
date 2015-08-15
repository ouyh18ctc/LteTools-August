using System;
using System.Collections.Generic;
using Lte.Evaluations.Rutrace.Record;
using Lte.Parameters.Entities;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Service
{
    public abstract class ImportCdrTaRecordsServiceTestHelper
    {
        private void AssertBasicParameters(CdrTaRecord details, CdrRtdRecord record)
        {
            Assert.AreEqual(details.CellId, record.CellId);
            Assert.AreEqual(details.SectorId, record.SectorId);
            Assert.AreEqual(details.TaMax, record.Rtd, 1E-6);
            Assert.AreEqual(details.TaMin, record.Rtd, 1E-6);
            Assert.AreEqual(details.TaAverage, record.Rtd, 1E-6);
        }

        public void AssertImportUniqueRecordResults(List<CdrTaRecord> details, CdrRtdRecord record)
        {
            AssertResultsWithOneDetails(details, record);
            AssertDistributionParameters(details[0], record);
        }

        public void AssertImportUniqueRecordResults(List<CdrTaRecord> details, CdrRtdRecord record1,
            CdrRtdRecord record2)
        {
            AssertResultsWithOneDetails(details, record1);
            AssertDistributionParameters(details[0], record2);
        }

        public void AssertImportTwoDifferentRecordsResults(List<CdrTaRecord> details, 
            CdrRtdRecord record1, CdrRtdRecord record2)
        {
            AssertResultsWithTwoDetails(details, record1, record2);
            AssertDistributionParameters(details[0], record1);
            AssertDistributionParameters(details[1], record2);
        }

        protected abstract void AssertDistributionParameters(CdrTaRecord details, CdrRtdRecord record);

        public void AssertImportTwoSameRecordsResults(List<CdrTaRecord> details, CdrRtdRecord record)
        {
            AssertResultsWithOneDetails(details, record);
            Assert.AreEqual(details[0].TaSum, 2 * record.Rtd, 1E-6); 
            if (InterferenceStat.IsInnerBound(record.Rtd))
            {
                Assert.AreEqual(details[0].TaInnerIntervalNum, 2);
                Assert.AreEqual(details[0].TaOuterIntervalNum, 0);
            }
            else
            {
                Assert.AreEqual(details[0].TaInnerIntervalNum, 0);
                Assert.AreEqual(details[0].TaOuterIntervalNum, 2);
            }
        }

        protected void AssertResultsWithOneDetails(List<CdrTaRecord> details, CdrRtdRecord record)
        {
            Assert.AreEqual(details.Count, 1);
            AssertBasicParameters(details[0], record);
        }

        protected void AssertResultsWithTwoDetails(List<CdrTaRecord> details, CdrRtdRecord record1,
            CdrRtdRecord record2)
        {
            Assert.AreEqual(details.Count, 2);
            AssertBasicParameters(details[0], record1);
            AssertBasicParameters(details[1], record2);
        }

        public void AssertImportTwoRecordsWithSameCellResults(List<CdrTaRecord> details,
            CdrRtdRecord record1, CdrRtdRecord record2)
        {
            Assert.AreEqual(details.Count, 1);
            Assert.AreEqual(details[0].CellId, record1.CellId);
            Assert.AreEqual(details[0].SectorId, record1.SectorId);
            Assert.AreEqual(details[0].TaMax, Math.Max(record1.Rtd, record2.Rtd), 1E-6);
            Assert.AreEqual(details[0].TaMin, Math.Min(record1.Rtd, record2.Rtd), 1E-6);
            Assert.AreEqual(details[0].TaAverage, (record1.Rtd + record2.Rtd) / 2, 1E-6);
            Assert.AreEqual(details[0].TaSum, record1.Rtd + record2.Rtd, 1E-6);
            Assert.AreEqual(details[0].TaInnerIntervalNum,
                (InterferenceStat.IsInnerBound(record1.Rtd) ? 1 : 0)
                + (InterferenceStat.IsInnerBound(record2.Rtd) ? 1 : 0));
            Assert.AreEqual(details[0].TaOuterIntervalNum,
                (InterferenceStat.IsInnerBound(record1.Rtd) ? 0 : 1)
                + (InterferenceStat.IsInnerBound(record2.Rtd) ? 0 : 1));
        }
    }

    public class ImportMainCdrTaRecordsServiceTestHelper : ImportCdrTaRecordsServiceTestHelper
    {
        protected override void AssertDistributionParameters(CdrTaRecord details, CdrRtdRecord record)
        {
            Assert.AreEqual(details.TaSum, record.Rtd, 1E-6);
            if (InterferenceStat.IsInnerBound(record.Rtd))
            {
                Assert.AreEqual(details.TaInnerIntervalNum, 1);
                Assert.AreEqual(details.TaOuterIntervalNum, 0);
            }
            else
            {
                Assert.AreEqual(details.TaInnerIntervalNum, 0);
                Assert.AreEqual(details.TaOuterIntervalNum, 1);
            }
        }
    }

    public class ImportExcessCdrTaRecordsServiceTestHelper : ImportCdrTaRecordsServiceTestHelper
    {
        protected override void AssertDistributionParameters(CdrTaRecord details, CdrRtdRecord record)
        {
            if (record.Rtd > details.Threshold)
            {
                Assert.AreEqual(
                    InterferenceStat.IsInnerBound(record.Rtd)
                        ? details.TaOuterIntervalExcessNum
                        : details.TaInnerIntervalExcessNum, 0);
                Assert.AreEqual(
                    InterferenceStat.IsInnerBound(record.Rtd)
                        ? details.TaInnerIntervalExcessNum
                        : details.TaOuterIntervalExcessNum, 1);
            }
            else
            {
                Assert.AreEqual(details.TaOuterIntervalExcessNum, 0);
                Assert.AreEqual(details.TaInnerIntervalExcessNum, 0);
            }
        }
    }
}