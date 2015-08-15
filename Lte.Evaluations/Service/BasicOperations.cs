using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Measure;
using Lte.Evaluations.Dingli;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Public;
using Lte.Parameters.Service.Region;

namespace Lte.Evaluations.Service
{
    public static class BasicOperations
    {
        private static CoverageAdjustment CalculateAdjumentFromCell(this CoverageStat coveragePoint,
            IOutdoorCell cell, byte modBase = 3)
        {
            MeasurableCell mCell = new MeasurableCell(coveragePoint, cell, modBase);
            mCell.CalculateRsrp();
            CoverageAdjustment adjustment = new CoverageAdjustment
            {
                ENodebId = coveragePoint.ENodebId,
                SectorId = coveragePoint.SectorId,
                Frequency = coveragePoint.Earfcn
            };
           
            adjustment.SetAdjustFactor(mCell.Cell.AzimuthAngle, mCell.ReceivedRsrp - coveragePoint.Rsrp);
            return adjustment;
        }

        public static IEnumerable<CoverageAdjustment> GenerateAdjustmentList(
            this IEnumerable<CoverageStat> coveragePoints,
            ICellRepository cellRepository, IEnumerable<ENodeb> eNodebs, byte modBase = 3)
        {
            IEnumerable<IOutdoorCell> cellList = cellRepository.Query(eNodebs);
            List<CoverageAdjustment> adjustmentList = 
                (from stat in coveragePoints.Where(x => x.ENodebId > 0) 
                 let cell = cellList.FirstOrDefault(x => x.ENodebId == stat.ENodebId 
                     && x.SectorId == stat.SectorId && x.Frequency == stat.Earfcn) 
                 where cell != null 
                 select stat.CalculateAdjumentFromCell(cell, modBase)).ToList();
            return adjustmentList.MergeList();
        }

        public static IEnumerable<XElement> ImportMeasurementDoc(this XElement element)
        {
            XElement measurement = element.Element("measurement");
            if (measurement == null) return null;
            XElement xElement = measurement.Element("smr");
            if (xElement == null) return null;
            return measurement.Descendants("object");
        }

        public static IEnumerable<XElement> ImportMeasurementDoc(this XElement element, string keyword)
        {
            IEnumerable<XElement> measurements = element.Elements("measurement");
            XElement measurement = measurements.FirstOrDefault(x => x.Attribute("mrName").Value == keyword);
            if (measurement == null) return null;
            XElement xElement = measurement.Element("smr");
            return xElement == null ? null : measurement.Descendants("object");
        }
    }
}
