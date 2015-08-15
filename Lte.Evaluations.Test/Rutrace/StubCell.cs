using Lte.Domain.Geo.Abstract;
using Lte.Evaluations.Rutrace.Record;
using Lte.Evaluations.Test.Rutrace.Record;
using Moq;

namespace Lte.Evaluations.Test.Rutrace
{
    class StubCell : ICell
    {
        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public StubCell(int cellId, byte sectorId)
        {
            CellId = cellId;
            SectorId = sectorId;
        }
    }

    public static class MockOperations
    {
        public static void MockSetters(this Mock<IInterferenceRecord<FakeInterference>> mock)
        {
            mock.SetupSet(x => x.MeasuredTimes = It.IsAny<int>())
                .Callback<int>(m => mock.SetupGet(x => x.MeasuredTimes).Returns(m));
        }
    }
}
