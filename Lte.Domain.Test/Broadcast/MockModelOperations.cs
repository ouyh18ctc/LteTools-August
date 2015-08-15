using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.Measure;
using Moq;
using Lte.Domain.TypeDefs;

namespace Lte.Domain.Test.Broadcast
{
    public static class MockModelOperations
    {
        public static void MockUrbanTypeAndKValues(this Mock<IBroadcastModel> model, UrbanType utype)
        {
            model.SetupGet(x => x.UrbanType).Returns(utype);
            switch (utype)
            {
                case UrbanType.Middle:
                case UrbanType.Large:
                    model.SetupGet(x => x.K1).Returns(69.55);
                    model.SetupGet(x => x.K4).Returns(44.9);
                    break;
                default:
                    model.SetupGet(x => x.K1).Returns(85.83);
                    model.SetupGet(x => x.K4).Returns(60);
                    break;
            }
        }

        public static void MockFrequencyType(this Mock<IBroadcastModel> model, FrequencyBandType type)
        {
            switch (type)
            {
                case FrequencyBandType.Downlink2100:
                    model.SetupGet(x => x.Frequency).Returns(2120);
                    break;
                case FrequencyBandType.Uplink2100:
                    model.SetupGet(x => x.Frequency).Returns(1930);
                    break;
                case FrequencyBandType.Downlink1800:
                    model.SetupGet(x => x.Frequency).Returns(1860);
                    break;
                case FrequencyBandType.Uplink1800:
                    model.SetupGet(x => x.Frequency).Returns(1765);
                    break;
                case FrequencyBandType.Tdd2600:
                    model.SetupGet(x => x.Frequency).Returns(2645);
                    break;
                default:
                    model.SetupGet(x => x.Frequency).Returns(2120);
                    break;
            }
        }
    }
}
