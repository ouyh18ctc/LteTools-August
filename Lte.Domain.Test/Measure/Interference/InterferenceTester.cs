using System.Collections.Generic;
using Lte.Domain.Measure;

namespace Lte.Domain.Test.Measure.Interference
{
    public abstract class InterferenceTester
    {
        protected SfMeasurePointResult Result = new SfMeasurePointResult();
        protected List<MeasurableCell> CellList = new List<MeasurableCell>();

        public abstract void AssertValues(IEnumerable<MeasurableCell> interference);

        public IEnumerable<MeasurableCell> UpdateDifferentModInterference()
        {
            return Result.UpdateDifferentModInterference(CellList);
        }

        public IEnumerable<MeasurableCell> UpdateSameModInterference()
        {
            return Result.UpdateSameModInterference(CellList);
        }
    }

    public abstract class TwoCellCalculateSameModInterferenceTester : InterferenceTester
    {
        protected MeasurableCell Mcell1 = new MeasurableCell();
        protected MeasurableCell Mcell2 = new MeasurableCell();

        public TwoCellCalculateSameModInterferenceTester(byte firstMod3, byte secondMod3)
        {
            Mcell1.Cell.PciModx = firstMod3;
            Mcell2.Cell.PciModx = secondMod3;
        }
    }

    public abstract class ThreeCellCalculateSameModInterferenceTester : InterferenceTester
    {
        protected MeasurableCell Mcell1 = new MeasurableCell();
        protected MeasurableCell Mcell2 = new MeasurableCell();
        protected MeasurableCell Mcell3 = new MeasurableCell();       

        public ThreeCellCalculateSameModInterferenceTester(
            byte firstMod3, byte secondMod3, byte thirdMod3)
        {
            Mcell1.Cell.PciModx = firstMod3;
            Mcell2.Cell.PciModx = secondMod3;
            Mcell3.Cell.PciModx = thirdMod3;
            CellList = new List<MeasurableCell>
            {
                Mcell1,
                Mcell2,
                Mcell3
            };
        }
    }
}
