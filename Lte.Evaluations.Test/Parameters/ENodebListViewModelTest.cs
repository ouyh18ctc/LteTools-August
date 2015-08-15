using System.Linq;
using Lte.Evaluations.ViewHelpers;
using Lte.Parameters.Abstract;
using Moq;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Parameters
{
    internal class ENodebListViewModelTestHelper
    {
        public void AssertTest(IENodebRepository repository, int townId, int page, int pageSize, int expectedSize)
        {
            Mock<ITownRepository> townRepository = new Mock<ITownRepository>();
            ENodebListViewModel viewModel 
                = new ENodebListViewModel(repository, townRepository.Object, townId, page, pageSize);
            Assert.AreEqual(viewModel.TownId, townId);
            Assert.AreEqual(viewModel.PagingInfo.CurrentPage, page);
            Assert.AreEqual(viewModel.PagingInfo.ItemsPerPage, pageSize);
            Assert.AreEqual(viewModel.PagingInfo.TotalItems, expectedSize);
            Assert.AreEqual(viewModel.Items.Count(),
                page == (expectedSize / pageSize + 1) ? (expectedSize % pageSize) : pageSize);
            Assert.AreEqual(viewModel.QueryItems.Count(), expectedSize);
        }
    }

    [TestFixture]
    public class ENodebListViewModelTest : ParametersConfig
    {
        private readonly Mock<IENodebRepository> eNodebRepository = new Mock<IENodebRepository>();
        private readonly ENodebListViewModelTestHelper helper = new ENodebListViewModelTestHelper();

        [SetUp]
        public void TestInitialize()
        {
            eNodebRepository.Setup(x => x.GetAll()).Returns(lotsOfENodebs.AsQueryable());
        }

        [Test]
        public void TestENodebListViewModel_TownId1_Page1_PageSize2_Expected4()
        {
            helper.AssertTest(eNodebRepository.Object, 1, 1, 2, 4);
        }

        [Test]
        public void TestENodebListViewModel_TownId2_Page1_PageSize2_Expected2()
        {
            helper.AssertTest(eNodebRepository.Object, 2, 1, 2, 2);
        }

        [Test]
        public void TestENodebListViewModel_TownId1_Page2_PageSize2_Expected4()
        {
            helper.AssertTest(eNodebRepository.Object, 1, 2, 2, 4);
        }

        [Test]
        public void TestENodebListViewModel_TownId5_Page1_PageSize2_Expected6()
        {
            helper.AssertTest(eNodebRepository.Object, 5, 1, 2, 6);
        }

        [Test]
        public void TestENodebListViewModel_TownId7_Page1_PageSize2_Expected3()
        {
            helper.AssertTest(eNodebRepository.Object, 7, 1, 2, 3);
        }

        [Test]
        public void TestENodebListViewModel_TownId7_Page2_PageSize2_Expected3()
        {
            helper.AssertTest(eNodebRepository.Object, 7, 2, 2, 3);
        }

        [Test]
        public void TestENodebListViewModel_TownId5_Page2_PageSize4_Expected6()
        {
            helper.AssertTest(eNodebRepository.Object, 5, 2, 4, 6);
        }
    }
}
