using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Lte.Evaluations.ViewHelpers
{
    public interface IPagingListViewModel<TItem>
    {
        IEnumerable<TItem> Items { get; set; }

        PagingInfo PagingInfo { get; set; }

        IEnumerable<TItem> QueryItems { get; }
    }

    public static class IRegionListViewModelOperations
    {
        public static void SetItems<T>(this IPagingListViewModel<T> viewModel,
            int page, int pageSize)
        {
            viewModel.Items = viewModel.QueryItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            viewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = viewModel.QueryItems.Count()
            };
        }
    }

    public class PagingInfo
    {
        public int TotalItems { get; set; }

        public int ItemsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }

    public static class PagingHelpers
    {

        public static MvcHtmlString PageLinks(this HtmlHelper html,
                                              PagingInfo pagingInfo,
                                              Func<int, string> pageUrl)
        {

            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                if (i > 1 && i < pagingInfo.CurrentPage - 5)
                {
                    if (i == pagingInfo.CurrentPage - 6)
                    { result.Append("..."); }
                    continue;
                }
                if (i > pagingInfo.CurrentPage + 5 && i < pagingInfo.TotalPages)
                {
                    if (i == pagingInfo.CurrentPage + 6)
                    { result.Append("..."); }
                    continue;
                }
                TagBuilder tag = new TagBuilder("a"); // Construct an <a> tag
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString(CultureInfo.InvariantCulture);
                if (i == pagingInfo.CurrentPage)
                    tag.AddCssClass("selected");
                result.Append(tag);
                if (i < pagingInfo.TotalPages)
                {
                    result.Append("&nbsp;|&nbsp;");
                }
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}
