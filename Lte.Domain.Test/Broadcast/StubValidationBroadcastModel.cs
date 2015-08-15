using System;

namespace Lte.Domain.Test.Broadcast
{
    public class StubValidationBroadcastModel
    {
        public double CalculatePathLoss(double distanceInKilometer, double baseHeight, double mobileHeight = 1.5)
        {
            Validate(distanceInKilometer, baseHeight, mobileHeight);
            return 0.0;
        }

        public void Validate(double distanceInKilometer, double baseHeight, double mobileHeight)
        {
            double eps = 1E-6;
            if ((distanceInKilometer <= eps) || (baseHeight <= eps * 100) || (mobileHeight <= eps * 100))
            {
                throw new ArgumentOutOfRangeException("计算路径损耗的输入参数不能为负数或接近0！");
            }
        }

    }
}
