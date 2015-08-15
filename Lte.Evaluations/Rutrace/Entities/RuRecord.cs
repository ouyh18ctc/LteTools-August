using System;
using System.Collections.Generic;
using Lte.Evaluations.Abstract;
using Lte.Evaluations.Infrastructure.Abstract;

namespace Lte.Evaluations.Rutrace.Entities
{
    public class RuRecord : IRuRecord<ReferenceCell, NeighborCell>
    {
        public ReferenceCell RefCell { get; private set; }

        public List<NeighborCell> NbCells { get; private set; }

        private RuRecord()
        {
            NbCells = new List<NeighborCell>();
        }

        public RuRecord(byte[] contents, int begin)
            : this()
        {
            RefCell = new ReferenceCell
            {
                Frequency = contents[begin],
                CellId = contents[begin + 5] * 16 + (contents[begin + 6] >> 4),
                SectorId = (byte)(contents[begin + 6] & 0x0F),
                EcIo = (byte)(contents[begin + 7] & 0x3F),
                Rtd = (double)244 / 8 * contents[begin + 9]
            };
            int start = begin + 10;
            while (start < contents.Length - 5)
            {
                double shortCode = (contents[start + 7] & 0x7F) * 256 + contents[start + 8];
                short pn = (short)(Math.Round(shortCode / 128) * 2);
                double rtd = Math.Abs(shortCode - pn * 64) * 244;
                if (contents[start + 9] == 0)
                {
                    NbCells.Add(new NeighborCell
                    {
                        Frequency = RefCell.Frequency,
                        CellId = contents[start + 4] * 16 + (contents[start + 5] >> 4),
                        SectorId = (byte)(contents[start + 5] & 0x0F),
                        EcIo = (byte)(contents[start + 6] & 0x3F),
                        FollowPn = (contents[start + 4] == 0xFF && contents[start + 5] == 0xFF),
                        Pn = pn,
                        Rtd = rtd
                    });
                    start += 10;
                }
                else
                {
                    NbCells.Add(new NeighborCell
                    {
                        Frequency = contents[start + 12],
                        CellId = contents[start + 4] * 16 + (contents[start + 5] >> 4),
                        SectorId = (byte)(contents[start + 5] & 0x0F),
                        EcIo = (byte)(contents[start + 6] & 0x3F),
                        FollowPn = (contents[start + 4] == 0xFF && contents[start + 5] == 0xFF),
                        Pn = pn,
                        Rtd = rtd
                    });
                    start += 13;
                }
            }
        }
    }
}
