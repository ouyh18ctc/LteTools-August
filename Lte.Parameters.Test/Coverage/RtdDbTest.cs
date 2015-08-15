using Lte.Parameters.Abstract;
using Moq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Coverage
{
    internal class RtdDbTestHelper
    {
        private readonly Mock<IRtdDb> src = new Mock<IRtdDb>();
        private readonly Mock<IRtdDb> dst = new Mock<IRtdDb>();

        public RtdDbTestHelper()
        {
            dst.SetupSet(x => x.MinRtd = It.IsAny<double>()).Callback<double>(
                m => dst.SetupGet(x => x.MinRtd).Returns(m));
            dst.SetupSet(x => x.SumRtds = It.IsAny<double>()).Callback<double>(
                s => dst.SetupGet(x => x.SumRtds).Returns(s));
            dst.SetupSet(x => x.TotalRtds = It.IsAny<int>()).Callback<int>(
                t => dst.SetupGet(x => x.TotalRtds).Returns(t));
        }

        public void SetupSrcParameters(double minRtd, double sumRtds, int totalRtds)
        {
            src.SetupGet(x => x.MinRtd).Returns(minRtd);
            src.SetupGet(x => x.SumRtds).Returns(sumRtds);
            src.SetupGet(x => x.TotalRtds).Returns(totalRtds);
        }

        public void SetupDstParameters(double minRtd, double sumRtds, int totalRtds)
        {
            dst.SetupGet(x => x.MinRtd).Returns(minRtd);
            dst.SetupGet(x => x.SumRtds).Returns(sumRtds);
            dst.SetupGet(x => x.TotalRtds).Returns(totalRtds);
        }

        public void Execute()
        {
            src.Object.UpdateRtdInfo(dst.Object);
        }

        public void AssertValues(double minRtd, double sumRtds, int totalRtds)
        {
            Assert.AreEqual(dst.Object.MinRtd, minRtd);
            Assert.AreEqual(dst.Object.SumRtds, sumRtds);
            Assert.AreEqual(dst.Object.TotalRtds, totalRtds);
        }
    }

    [TestFixture]
    public class RtdDbTest
    {
        private readonly RtdDbTestHelper helper = new RtdDbTestHelper();

        [Test]
        public void Test_AllZeros()
        {
            helper.SetupSrcParameters(0, 0, 0);
            helper.SetupDstParameters(0, 0, 0);
            helper.Execute();
            helper.AssertValues(0, 0, 0);
        }

        [Test]
        public void Test_DstAllZeros()
        {
            helper.SetupSrcParameters(1, 2, 1);
            helper.SetupDstParameters(0, 0, 0);
            helper.Execute();
            helper.AssertValues(1, 2, 1);
        }

        [Test]
        public void Test_SrcTotal_MoreThanTenDst()
        {
            helper.SetupSrcParameters(3, 4, 11);
            helper.SetupDstParameters(1, 2, 1);
            helper.Execute();
            helper.AssertValues(3, 4, 11);
        }

        [Test]
        public void Test_SrcTotal_TenDst()
        {
            helper.SetupSrcParameters(3, 4, 10);
            helper.SetupDstParameters(1, 2, 1);
            helper.Execute();
            helper.AssertValues(1, 6, 11);
        }

        [Test]
        public void Test_SrcTotal_FiveDst()
        {
            helper.SetupSrcParameters(3, 4, 5);
            helper.SetupDstParameters(1, 2, 1);
            helper.Execute();
            helper.AssertValues(1, 6, 6);
        }

        [Test]
        public void Test_DstTotal_TwoDst()
        {
            helper.SetupDstParameters(1, 2, 2);
            helper.SetupSrcParameters(3, 4, 1);
            helper.Execute();
            helper.AssertValues(1, 6, 3);
        }

        [Test]
        public void Test_DstTotal_TenDst()
        {
            helper.SetupSrcParameters(3, 4, 1);
            helper.SetupDstParameters(1, 2, 10);
            helper.Execute();
            helper.AssertValues(1, 6, 11);
        }

        [Test]
        public void Test_DstTotal_MoreThanTenDst()
        {
            helper.SetupSrcParameters(3, 4, 1);
            helper.SetupDstParameters(1, 2, 11);
            helper.Execute();
            helper.AssertValues(1, 2, 11);
        }
    }
}
