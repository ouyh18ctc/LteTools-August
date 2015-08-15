using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Lte.Domain.Geo.Abstract;
using Lte.Parameters.Abstract;

namespace Lte.Parameters.Entities
{
    public interface IPureInterferenceStat
    {
        int VictimCells { get; set; }
        int InterferenceCells { get; set; }
        int FirstVictimCellId { get; set; }
        byte FirstVictimSectorId { get; set; }
        int FirstVictimTimes { get; set; }
        int FirstInterferenceTimes { get; set; }
        int SecondVictimCellId { get; set; }
        byte SecondVictimSectorId { get; set; }
        int SecondVictimTimes { get; set; }
        int SecondInterferenceTimes { get; set; }
        int ThirdVictimCellId { get; set; }
        byte ThirdVictimSectorId { get; set; }
        int ThirdVictimTimes { get; set; }
        int ThirdInterferenceTimes { get; set; }
    }

    public class PureInterferenceStat : Entity, ICell, IInterferenceDb, IPureInterferenceStat
    {
        public DateTime RecordDate { get; set; }

        public string DateString
        {
            get { return RecordDate.ToShortDateString(); }
        }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int VictimCells { get; set; }

        public int InterferenceCells { get; set; }

        public int FirstVictimCellId { get; set; }

        public byte FirstVictimSectorId { get; set; }

        public int FirstVictimTimes { get; set; }

        public int FirstInterferenceTimes { get; set; }

        public int SecondVictimCellId { get; set; }

        public byte SecondVictimSectorId { get; set; }

        public int SecondVictimTimes { get; set; }

        public int SecondInterferenceTimes { get; set; }

        public int ThirdVictimCellId { get; set; }

        public byte ThirdVictimSectorId { get; set; }

        public int ThirdVictimTimes { get; set; }

        public int ThirdInterferenceTimes { get; set; }

        public double InterferenceRatio
        {
            get
            {
                return (VictimCells == 0) ? 0 : InterferenceCells / (double)VictimCells;
            }
        }

        public PureInterferenceStat()
        {
            InterferenceCells = 0;
            VictimCells = 0;
            FirstInterferenceTimes = 0;
            FirstVictimCellId = 0;
            FirstVictimSectorId = 0;
            FirstVictimTimes = 0;
            SecondInterferenceTimes = 0;
            SecondVictimCellId = 0;
            SecondVictimSectorId = 0;
            SecondVictimTimes = 0;
            ThirdInterferenceTimes = 0;
            ThirdVictimCellId = 0;
            ThirdVictimSectorId = 0;
            ThirdVictimTimes = 0;
        }
    }
}