using System;
using System.Collections.Generic;
using System.IO;
using Lte.Evaluations.Abstract;
using Lte.Evaluations.Infrastructure.Abstract;

namespace Lte.Evaluations.Rutrace.Entities
{
    public class RuRecordSet : IRecordSet<RuRecord, ReferenceCell, NeighborCell>
    {
        public List<RuRecord> RecordList { get; set; }

        public DateTime RecordDate { get; set; }

        public RuRecordSet() { RecordList = new List<RuRecord>(); }

        public RuRecordSet(Stream instream)
            : this()
        {
            ImportRecordSet(instream);
        }

        public void ImportRecordSet(Stream instream)
        {
            int currentIndex = instream.ReadByte();
            byte[] tempContent = new byte[currentIndex - 1];
            instream.Read(tempContent, 0, currentIndex - 1);

            while (currentIndex < instream.Length - 5)
            {
                tempContent = new byte[4];
                instream.Read(tempContent, 0, 4);
                int recordLength = tempContent[3];
                currentIndex += 4;
                tempContent = new byte[recordLength];
                instream.Read(tempContent, 0, recordLength);
                int begin = (tempContent[0] == 1) ? 14 : 3;
                RecordList.Add(new RuRecord(tempContent, begin));
                currentIndex += recordLength;
            }
            instream.Close();
        }
    }
}
