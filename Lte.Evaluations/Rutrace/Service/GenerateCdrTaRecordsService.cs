using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lte.Domain.Regular;
using Lte.Evaluations.Rutrace.Record;

namespace Lte.Evaluations.Rutrace.Service
{
    public abstract class GenerateCdrTaRecordsService
    {
        protected List<CdrTaRecord> _details;

        protected GenerateCdrTaRecordsService()
        {
            _details = new List<CdrTaRecord>();
        }

        public List<CdrTaRecord> Generate()
        {
            GenerateDetails();

            foreach (CdrTaRecord record in _details)
            {
                record.CorrectRemoteFactor();
            }

            return _details;
        }

        protected abstract void GenerateDetails();
    }

    public class GenerateCdrTaRecordsFromFilesService : GenerateCdrTaRecordsService
    {
        private readonly string[] _paths;

        public GenerateCdrTaRecordsFromFilesService(string[] paths)
        {
            _paths = paths;
        }

        private static void Import(TextReader reader, Action<CdrRtdRecord> ImportAction)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] segments = line.GetSplittedFields(';');

                for (int i = 1; i < segments.Length; i++)
                {
                    string[] fields = segments[i].GetSplittedFields('_');
                    if (fields[0] == "D0")
                    {
                        CdrRtdRecord record = new CdrRtdRecord(fields);
                        ImportAction(record);
                    }
                }
            }
        }

        protected override void GenerateDetails()
        {
            foreach (StreamReader reader in _paths.Select(path => new StreamReader(path)))
            {
                Import(reader, x =>
                {
                    ImportCdrTaRecordsService service = new ImportMainCdrTaRecordsService(
                        _details, x);
                    service.Import();
                });
                reader.Close();
            }
            foreach (StreamReader reader in _paths.Select(path => new StreamReader(path)))
            {
                Import(reader, x =>
                {
                    ImportCdrTaRecordsService service = new ImportExcessCdrTaRecordsService(
                        _details, x);
                    service.Import();
                });
                reader.Close();
            }
        }
    }

    public class GenerateCdrTaRecordsFromTaRecordsService : GenerateCdrTaRecordsService
    {
        private readonly IEnumerable<CdrRtdRecord> _taRecordList;

        public GenerateCdrTaRecordsFromTaRecordsService(IEnumerable<CdrRtdRecord> taRecordList)
        {
            _taRecordList = taRecordList;
        }

        protected override void GenerateDetails()
        {
            foreach (ImportMainCdrTaRecordsService service in _taRecordList.Select(
                record => new ImportMainCdrTaRecordsService(
                _details, record)))
            {
                service.Import();
            }
            foreach (ImportExcessCdrTaRecordsService service in _taRecordList.Select(
                record => new ImportExcessCdrTaRecordsService(
                _details, record)))
            {
                service.Import();
            }
        }
    }
}
