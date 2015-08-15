using System;
using Abp.Domain.Entities;
using Lte.Domain.Geo.Abstract;

namespace Lte.Parameters.Entities
{
    public class MrsCellTa : Entity, ICell
    {
        public DateTime RecordDate { get; set; }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int[] TaCounts { get; set; }

        public string DateString
        {
            get { return RecordDate.ToShortDateString(); }
        }

        public MrsCellTa()
        {
            TaCounts = new int[45];
        }

        public void UpdateStats()
        {
            TaTo2 = TaCounts[0] + TaCounts[1];
            TaTo4 = TaCounts[2] + TaCounts[3];
            TaTo6 = TaCounts[4] + TaCounts[5];
            TaTo8 = TaCounts[6] + TaCounts[7];
            TaTo12 = TaCounts[8] + TaCounts[9] + TaCounts[10] + TaCounts[11];
            TaTo16 = TaCounts[12] + TaCounts[13];
            TaTo20 = TaCounts[14] + TaCounts[15];
            TaTo24 = TaCounts[16] + TaCounts[17];
            TaTo32 = TaCounts[18] + TaCounts[19] + TaCounts[20] + TaCounts[21];
            TaTo40 = TaCounts[22] + TaCounts[23] + TaCounts[24] + TaCounts[25];
            TaTo48 = TaCounts[26] + TaCounts[27] + TaCounts[28] + TaCounts[29];
            TaTo56 = TaCounts[30] + TaCounts[31] + TaCounts[32] + TaCounts[33];
            TaTo64 = TaCounts[34] + TaCounts[35] + TaCounts[36] + TaCounts[37];
            TaTo80 = TaCounts[38];
            TaTo96 = TaCounts[39];
            TaTo128 = TaCounts[40] + TaCounts[41];
            TaTo192 = TaCounts[42];
            TaTo256 = TaCounts[43];
            TaAbove256 = TaCounts[44];
        }

        public int TaTo2 { get; set; }

        public int TaTo4 { get; set; }

        public int TaTo6 { get; set; }

        public int TaTo8 { get; set; }

        public int TaTo12 { get; set; }

        public int TaTo16 { get; set; }

        public int TaTo20 { get; set; }

        public int TaTo24 { get; set; }

        public int TaTo32 { get; set; }

        public int TaTo40 { get; set; }

        public int TaTo48 { get; set; }

        public int TaTo56 { get; set; }

        public int TaTo64 { get; set; }

        public int TaTo80 { get; set; }

        public int TaTo96 { get; set; }

        public int TaTo128 { get; set; }

        public int TaTo192 { get; set; }

        public int TaTo256 { get; set; }

        public int TaAbove256 { get; set; }
    }
}