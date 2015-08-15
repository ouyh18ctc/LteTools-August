using System.Web.Mvc;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Kpi.Entities;

namespace Lte.WebApp.Models
{
    public class CdmaDailyStatListBinder : IModelBinder
    {
        private const string sessionKey = "CdmaDailyStatList";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            AllCdmaDailyStatList list
                = (AllCdmaDailyStatList)controllerContext.HttpContext.Session[sessionKey];

            if (list == null)
            {
                IRegionRepository regionRepository = new EFRegionRepository();
                list = new AllCdmaDailyStatList(regionRepository.GetAllList());
                controllerContext.HttpContext.Session[sessionKey] = list;
            }
            // return the cart
            return list;
        }
    }
}