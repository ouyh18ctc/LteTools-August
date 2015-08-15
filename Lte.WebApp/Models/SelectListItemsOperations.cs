using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Lte.WebApp.Models
{
    public static class SelectListItemsOperations
    {
        private static SelectListItem GetSelectedItem(this IEnumerable<SelectListItem> itemList)
        {
            return itemList.FirstOrDefault(x => x.Selected);
        }

        public static string GetSelectedItemText(this List<SelectListItem> itemList)
        {
            return (itemList.GetSelectedItem() == null) ? null : itemList.GetSelectedItem().Text;
        }

        public static string GetSelectedItemValue(this List<SelectListItem> itemList)
        {
            return (itemList.GetSelectedItem() == null) ? null : itemList.GetSelectedItem().Value;
        }
    }
}
