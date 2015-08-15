using System.Collections.Generic;
using Lte.Domain.Geo.Entities;
using Lte.Evaluations.Entities;
using Lte.Domain.Measure;
using Lte.Domain.Geo;

namespace Lte.Evaluations.Test.Kml
{
    public static class KmlTestInfrastructure
    {
        private static readonly StatValueField statValueField = new StatValueField
        {
            IntervalList = new List<StatValueInterval>
            {
                new StatValueInterval
                {
                    IntervalLowLevel = 0,
                    IntervalUpLevel = 1,
                    Color = new Color
                    {
                        ColorA = 128,
                        ColorB = 10,
                        ColorG = 12,
                        ColorR = 128
                    }
                },
                new StatValueInterval
                {
                    IntervalLowLevel = 1,
                    IntervalUpLevel = 2,
                    Color = new Color
                    {
                        ColorA = 128,
                        ColorB = 103,
                        ColorG = 12,
                        ColorR = 12
                    }
                },
                new StatValueInterval
                {
                    IntervalLowLevel = 2,
                    IntervalUpLevel = 3,
                    Color = new Color
                    {
                        ColorA = 128,
                        ColorB = 10,
                        ColorG = 123,
                        ColorR = 12
                    }
                }
            },
            FieldName = "同模干扰电平"
        };

        public static StatValueField StatValueField
        {
            get { return statValueField; }
        }

        private static List<MeasurePoint> measurePointList = new List<MeasurePoint>{
            new MeasurePoint(new GeoPoint(112.1, 23.2))
            {
                Result = new SfMeasurePointResult
                {
                    SameModInterferenceLevel = 0.5,
                    DifferentModInterferenceLevel = 1.5,
                    TotalInterferencePower = 2.5,
                    NominalSinr = 3.5
                }
            },
            new MeasurePoint(new GeoPoint(112.2, 23.3))
            {
                Result = new SfMeasurePointResult
                {
                    SameModInterferenceLevel = 1.5,
                    DifferentModInterferenceLevel = 1.5,
                    TotalInterferencePower = 0.5,
                    NominalSinr = 3.5
                }
            },
            new MeasurePoint(new GeoPoint(112.3, 23.4))
            {
                Result = new SfMeasurePointResult
                {
                    SameModInterferenceLevel = 3.5,
                    DifferentModInterferenceLevel = 1.5,
                    TotalInterferencePower = 2.5,
                    NominalSinr = 0.5
                }
            }
        };

        public static List<MeasurePoint> MeasurePointList
        {
            get { return measurePointList; }
        }
    }
}
