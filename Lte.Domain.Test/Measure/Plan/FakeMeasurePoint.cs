using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Measure;

namespace Lte.Domain.Test.Measure.Plan
{
    public class FakeMeasurePoint
    {
        private readonly MeasurePoint mMeasurePoint = new MeasurePoint();

        private IList<MeasurableCell> MeasurableCellList
        {
            set 
            {
                mMeasurePoint.CellRepository.CellList = value;
                if ((value == null) || (!value.Any())) { return; }
                double maxRsrp = value.Select(x => x.ReceivedRsrp).Max();
                mMeasurePoint.Result.StrongestCell = value.FirstOrDefault(x => Math.Abs(x.ReceivedRsrp - maxRsrp) < 1E-6);
            }
        }

        public MeasurePoint MeasurePoint
        {
            get { return mMeasurePoint; }
        }

        public static MeasurePoint GenerateMeasurePoint(IOutdoorCell[] cellList,
            byte[] pciModxList, double[] receivedRsrpList)
        {
            IList<MeasurableCell> mCellList = new List<MeasurableCell>();

            for (int i = 0; i < cellList.Length; i++)
            {
                FakeMeasurableCell mmCell = new FakeMeasurableCell()
                {
                    OutdoorCell = cellList[i],
                    PciModx = pciModxList[i],
                    ReceivedRsrp = receivedRsrpList[i]
                };
                mCellList.Add(mmCell);                
            }

            FakeMeasurePoint mmPoint = new FakeMeasurePoint()
            {
                MeasurableCellList = mCellList
            };

            return mmPoint.MeasurePoint;
        }
    }
}
