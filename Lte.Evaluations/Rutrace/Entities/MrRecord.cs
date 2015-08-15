using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Regular;
using System.Xml.Linq;
using Lte.Evaluations.Infrastructure.Abstract;

namespace Lte.Evaluations.Rutrace.Entities
{
    public abstract class MrRecord : IRuRecord<MrReferenceCell, MrNeighborCell>
    {
        public MrReferenceCell RefCell { get; set; }

        public List<MrNeighborCell> NbCells { get; set; }

        public MrRecord()
        {
            NbCells = new List<MrNeighborCell>();
        }

    }

    public class MreRecord : MrRecord
    {
        public MreRecord(int eNodebId, string line)
        {
            string[] contents = line.GetSplittedFields(' ');
            int cgi = contents[2].ConvertToInt(0);
            RefCell = new MrReferenceCell
            {
                Frequency = contents[0].ConvertToShort(100),
                CellId = eNodebId,
                SectorId = cgi.GetLastByte(),
                Rsrp = contents[3].ConvertToByte(0)
            };
            for (int i = 0; i < 3; i++)
            {
                if (contents[5 + i*4] == "NIL")
                {
                    break;
                }
                NbCells.Add(new MrNeighborCell
                {
                    Frequency = contents[5 + i*4].ConvertToShort(100),
                    Pci = contents[6 + i*4].ConvertToShort(0),
                    Rsrp = contents[7 + i*4].ConvertToByte(0)
                });
            }
        }
    }

    public class MroRecord : MrRecord
    {
        public MroRecord()
        {
        }

        public MroRecord(int eNodebId, XElement doc)
        {
            IEnumerable<string> values = doc.Descendants("v").Select(x => x.Value);
            int cgi = doc.Attribute("id").Value.ConvertToInt(0);
            bool firstElement = true;
            foreach (string value in values)
            {
                string[] contents = value.GetSplittedFields(' ');
                if (firstElement)
                {
                    RefCell = new MrReferenceCell
                    {
                        Frequency = contents[0].ConvertToShort(100),
                        CellId = eNodebId,
                        SectorId = cgi.GetLastByte(),
                        Rsrp = contents[3] == "NIL" ? (byte)255 : contents[3].ConvertToByte(0),
                        Ta = contents[5] == "NIL" ? (byte)255 : (contents[5].ConvertToShort(0) >> 4).GetLastByte()
                    };
                    firstElement = false;
                }
                if (contents[12] == "NIL")
                {
                    return;
                }
                NbCells.Add(new MrNeighborCell
                {
                    Frequency = contents[11].ConvertToShort(100),
                    Pci = contents[12].ConvertToShort(0),
                    Rsrp = contents[13].ConvertToByte(0)
                });
            }
        }
    }
}
