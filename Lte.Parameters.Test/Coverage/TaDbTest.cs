using Lte.Parameters.Abstract;
using Moq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Coverage
{
    internal class TaDbTestHelper
    {
        private readonly Mock<ITaDb> src = new Mock<ITaDb>();
        private readonly Mock<ITaDb> dst = new Mock<ITaDb>();

        public TaDbTestHelper()
        {
            dst.SetupSet(x => x.TaInnerIntervalExcessNum = It.IsAny<int>()).Callback<int>(
                ie => dst.SetupGet(x => x.TaInnerIntervalExcessNum).Returns(ie));
            dst.SetupSet(x => x.TaInnerIntervalNum = It.IsAny<int>()).Callback<int>(
                inum => dst.SetupGet(x => x.TaInnerIntervalNum).Returns(inum));
            dst.SetupSet(x => x.TaMax = It.IsAny<double>()).Callback<double>(
                max => dst.SetupGet(x => x.TaMax).Returns(max));
            dst.SetupSet(x => x.TaOuterIntervalExcessNum = It.IsAny<int>()).Callback<int>(
                oe => dst.SetupGet(x => x.TaOuterIntervalExcessNum).Returns(oe));
            dst.SetupSet(x => x.TaOuterIntervalNum = It.IsAny<int>()).Callback<int>(
                onum => dst.SetupGet(x => x.TaOuterIntervalNum).Returns(onum));
            dst.SetupSet(x => x.TaSum = It.IsAny<double>()).Callback<double>(
                sum => dst.SetupGet(x => x.TaSum).Returns(sum));
        }

        public void SetupSrcParameters(int innerExcess, int innerNum, double taMax,
            int outerExcess, int outerNum, double taSum)
        {
            src.SetupGet(x => x.TaInnerIntervalExcessNum).Returns(innerExcess);
            src.SetupGet(x => x.TaInnerIntervalNum).Returns(innerNum);
            src.SetupGet(x => x.TaMax).Returns(taMax);
            src.SetupGet(x => x.TaOuterIntervalExcessNum).Returns(outerExcess);
            src.SetupGet(x => x.TaOuterIntervalNum).Returns(outerNum);
            src.SetupGet(x => x.TaSum).Returns(taSum);
        }

        public void SetupDstParameters(int innerExcess, int innerNum, double taMax,
            int outerExcess, int outerNum, double taSum)
        {
            dst.SetupGet(x => x.TaInnerIntervalExcessNum).Returns(innerExcess);
            dst.SetupGet(x => x.TaInnerIntervalNum).Returns(innerNum);
            dst.SetupGet(x => x.TaMax).Returns(taMax);
            dst.SetupGet(x => x.TaOuterIntervalExcessNum).Returns(outerExcess);
            dst.SetupGet(x => x.TaOuterIntervalNum).Returns(outerNum);
            dst.SetupGet(x => x.TaSum).Returns(taSum);
        }

        public void Execute()
        {
            src.Object.UpdateTaInfo(dst.Object);
        }

        public void AssertValues(int innerExcess, int innerNum, double taMax,
            int outerExcess, int outerNum, double taSum)
        {
            Assert.AreEqual(dst.Object.TaInnerIntervalExcessNum, innerExcess);
            Assert.AreEqual(dst.Object.TaInnerIntervalNum, innerNum);
            Assert.AreEqual(dst.Object.TaMax, taMax);
            Assert.AreEqual(dst.Object.TaOuterIntervalExcessNum, outerExcess);
            Assert.AreEqual(dst.Object.TaOuterIntervalNum, outerNum);
            Assert.AreEqual(dst.Object.TaSum, taSum);
        }
    }

    [TestFixture]
    public class TaDbTest
    {
        private readonly TaDbTestHelper helper = new TaDbTestHelper();

        [Test]
        public void Test_AllZeros()
        {
            helper.SetupSrcParameters(0, 0, 0, 0, 0, 0);
            helper.SetupDstParameters(0, 0, 0, 0, 0, 0);
            helper.Execute();
            helper.AssertValues(0, 0, 0, 0, 0, 0);
        }

        [Test]
        public void Test_DstAllZeros()
        {
            helper.SetupSrcParameters(1, 2, 3, 4, 5, 6);
            helper.SetupDstParameters(0, 0, 0, 0, 0, 0);
            helper.Execute();
            helper.AssertValues(1, 2, 3, 4, 5, 6);
        }

        [Test]
        public void Test_SrcSum_MoreThanTenDst()
        {
            helper.SetupSrcParameters(1, 2, 3, 4, 5, 11);
            helper.SetupDstParameters(1, 1, 1, 1, 1, 1);
            helper.Execute();
            helper.AssertValues(1, 2, 3, 4, 5, 11);
        }

        [Test]
        public void Test_SrcTotal_TenDst()
        {
            helper.SetupSrcParameters(1, 2, 3, 4, 5, 10);
            helper.SetupDstParameters(1, 1, 1, 1, 1, 1);
            helper.Execute();
            helper.AssertValues(2, 3, 3, 5, 6, 11);
        }

        [Test]
        public void Test_SrcTotal_FiveDst()
        {
            helper.SetupSrcParameters(1, 2, 3, 4, 5, 5);
            helper.SetupDstParameters(1, 1, 1, 1, 1, 1);
            helper.Execute();
            helper.AssertValues(2, 3, 3, 5, 6, 6);
        }

        [Test]
        public void Test_DstTotal_TwoDst()
        {
            helper.SetupSrcParameters(1, 2, 3, 4, 5, 1);
            helper.SetupDstParameters(1, 1, 1, 1, 1, 2);
            helper.Execute();
            helper.AssertValues(2, 3, 3, 5, 6, 3);
        }

        [Test]
        public void Test_DstTotal_TenDst()
        {
            helper.SetupSrcParameters(1, 2, 3, 4, 5, 1);
            helper.SetupDstParameters(1, 1, 1, 1, 1, 10);
            helper.Execute();
            helper.AssertValues(2, 3, 3, 5, 6, 11);
        }

        [Test]
        public void Test_DstTotal_MoreThanTenDst()
        {
            helper.SetupSrcParameters(1, 2, 3, 4, 5, 1);
            helper.SetupDstParameters(1, 1, 1, 1, 1, 11);
            helper.Execute();
            helper.AssertValues(1, 1, 1, 1, 1, 11);
        }
    }
}
