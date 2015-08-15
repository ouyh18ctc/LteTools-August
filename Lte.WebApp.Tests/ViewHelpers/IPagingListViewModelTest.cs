using System;
using Lte.Evaluations.ViewHelpers;
using Moq;
using Lte.Domain.Regular;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace Lte.WebApp.Tests.ViewHelpers
{
    internal class IPagingListViewModelTestHelper
    { 
        private readonly Mock<IPagingListViewModel<int>> mockPagingListViewModel
            = new Mock<IPagingListViewModel<int>>();

        public IPagingListViewModelTestHelper()
        { 
            mockPagingListViewModel.BindGetAndSetAttributes(x => x.Items, (x, v) => x.Items = v);
            mockPagingListViewModel.BindGetAndSetAttributes(x => x.PagingInfo, (x, v) => x.PagingInfo = v);
            mockPagingListViewModel.SetupGet(x => x.QueryItems).Returns(Enumerable.Range(1, 100));
        }

        public void AssertTest(int page, int pageSize)
        { 
            mockPagingListViewModel.Object.SetItems(page, pageSize);
            Assert.AreEqual(mockPagingListViewModel.Object.PagingInfo.CurrentPage, page);
            Assert.AreEqual(mockPagingListViewModel.Object.PagingInfo.ItemsPerPage, pageSize);
            Assert.AreEqual(mockPagingListViewModel.Object.PagingInfo.TotalItems, 100);
            Assert.AreEqual(mockPagingListViewModel.Object.PagingInfo.TotalPages,
                (int)Math.Ceiling((double)100 / pageSize));
            Assert.AreEqual(mockPagingListViewModel.Object.Items.Count(),
                (page < mockPagingListViewModel.Object.PagingInfo.TotalPages) ? pageSize :
                ((page == mockPagingListViewModel.Object.PagingInfo.TotalPages) ?
                (100 % pageSize == 0 ? pageSize : 100 % pageSize) : 0));
            if (mockPagingListViewModel.Object.Items.Any())
            {
                Assert.AreEqual(mockPagingListViewModel.Object.Items.ElementAt(0),
                    pageSize * (page - 1) + 1);
            }
        }
    }

    [TestFixture]
    public class IPagingListViewModelTest
    {
        private IPagingListViewModelTestHelper helper = new IPagingListViewModelTestHelper();

        [Test]
        public void TestIPagingListViewModel_page1_pageSize20()
        {
            helper.AssertTest(1, 20);
        }

        public void TestIPagingListViewModel_page5_pageSize20()
        {
            helper.AssertTest(5, 20);
        }

        [Test]
        public void TestIPagingListViewModel_page1_pageSize60()
        {
            helper.AssertTest(1, 60);
        }

        [Test]
        public void TestIPagingListViewModel_page2_pageSize60()
        {
            helper.AssertTest(2, 60);
        }

        [Test]
        public void TestIPagingListViewModel_page3_pageSize60()
        {
            helper.AssertTest(3, 60);
        }

        [Test]
        public void TestIPagingListViewModel_page1_pageSize101()
        {
            helper.AssertTest(1, 101);
        }
    }
}
