using Lte.Domain.Measure;
using Moq;

namespace Lte.Domain.Test.Measure
{
    public static class MockOperations
    {
        public static void SetupComparableCellProperties(this ComparableCell mockCC,
            double distance, double azimuthAngle)
        {
            mockCC.Distance = distance;
            mockCC.AzimuthAngle = azimuthAngle;
        }

        public static void MockMeasurePointProperties(this MeasurePoint point,
            double nominalSinr, double strongestCellRsrp, double strongestInterferenceRsrp,
            double totalInterferencePower)
        {
            Mock<IMeasurePointResult> mockResult = new Mock<IMeasurePointResult>();
            mockResult.Setup(x => x.NominalSinr).Returns(nominalSinr);
            MeasurableCell signal = new MeasurableCell();
            signal.ReceivedRsrp = strongestCellRsrp;
            mockResult.Setup(x => x.StrongestCell).Returns(signal);
            MeasurableCell interference = new MeasurableCell();
            interference.ReceivedRsrp = strongestInterferenceRsrp;
            mockResult.Setup(x => x.StrongestInterference).Returns(interference);
            mockResult.Setup(x => x.TotalInterferencePower).Returns(totalInterferencePower);
            point.Result = mockResult.Object;
        }
    }
}
