using System;
using System.Collections.Generic;
using Lte.Domain.Regular;
using Lte.Evaluations.Entities;

namespace Lte.Evaluations.Service
{
    public class MergeMeasurePointListService
    {
        private readonly IEnumerable<MeasurePointInfo> points;
        const double TOLERANCE = 1E-8;

        public MergeMeasurePointListService(IEnumerable<MeasurePointInfo> points)
        {
            this.points = points;
        }

        public List<MeasurePointInfo> Merge()
        {
            List<MeasurePointInfo> list = new List<MeasurePointInfo>();
            MeasurePointInfo point = new MeasurePointInfo
            {
                X1 = 0,
                X2 = 0,
                Y1 = 0,
                Y2 = 0,
                ColorString = ""
            };

            foreach (MeasurePointInfo currentPoint in points)
            {
                if (Math.Abs(currentPoint.Y2 - point.Y2) < TOLERANCE
                    && Math.Abs(currentPoint.Y1 - point.Y1) < TOLERANCE &&
                    currentPoint.ColorString == point.ColorString)
                {
                    point.X2 = currentPoint.X2;
                }
                else
                {
                    if (Math.Abs(point.X1) > TOLERANCE)
                    {
                        MeasurePointInfo tempPoint = new MeasurePointInfo();
                        point.CloneProperties<MeasurePointInfo>(tempPoint);
                        list.Add(tempPoint);
                    }
                    point = new MeasurePointInfo();
                    currentPoint.CloneProperties<MeasurePointInfo>(point);
                }
            }
            return list;
        }
    }
}
