using Lte.Parameters.Abstract;
using Moq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Coverage
{
    internal class InterferenceDbTestHelper
    {
        private readonly Mock<IInterferenceDb> src = new Mock<IInterferenceDb>();
        private readonly Mock<IInterferenceDb> dst = new Mock<IInterferenceDb>();

        public InterferenceDbTestHelper()
        {
            dst.SetupSet(x => x.VictimCells = It.IsAny<int>()).Callback<int>(
                v => dst.SetupGet(x => x.VictimCells).Returns(v));
            dst.SetupSet(x => x.InterferenceCells = It.IsAny<int>()).Callback<int>(
                i => dst.SetupGet(x => x.InterferenceCells).Returns(i));
        }

        public void SetSrcParameters(int victimCells, int interferenceCells)
        {
            src.SetupGet(x => x.VictimCells).Returns(victimCells);
            src.SetupGet(x => x.InterferenceCells).Returns(interferenceCells);
        }

        public void SetDstParameters(int victimCells, int interferenceCells)
        {
            dst.SetupGet(x => x.VictimCells).Returns(victimCells);
            dst.SetupGet(x => x.InterferenceCells).Returns(interferenceCells);
        }

        public void Execute()
        {
            src.Object.UpdateInterferenceInfo(dst.Object);
        }

        public void AssertValues(int victimCells, int interferenceCells)
        {
            Assert.AreEqual(dst.Object.VictimCells, victimCells);
            Assert.AreEqual(dst.Object.InterferenceCells, interferenceCells);
        }
    }

    [TestFixture]
    public class InterferenceDbTest
    {
        private readonly InterferenceDbTestHelper helper = new InterferenceDbTestHelper();

        [Test]
        public void Test_AllZeros()
        {
            helper.SetSrcParameters(0, 0);
            helper.SetDstParameters(0, 0);
            helper.Execute();
            helper.AssertValues(0, 0);
        }

        [Test]
        public void Test_UpdateOneElement()
        {
            helper.SetSrcParameters(1, 0);
            helper.SetDstParameters(0, 0);
            helper.Execute();
            helper.AssertValues(1, 0);
        }

        [Test]
        public void Test_UpdateTwoElements()
        {
            helper.SetSrcParameters(1, 2);
            helper.SetDstParameters(0, 0);
            helper.Execute();
            helper.AssertValues(1, 2);
        }

        [Test]
        public void Test_UpdateNoElements()
        {
            helper.SetSrcParameters(1, 1);
            helper.SetDstParameters(2, 3);
            helper.Execute();
            helper.AssertValues(2, 3);
        }
    }
}
