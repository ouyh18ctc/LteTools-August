using System.Web.Mvc;

namespace Lte.WebApp.Controllers.Home
{
    public class HomeController : Controller
    {
        //基础数据
        public ActionResult Infrastructure()
        {
            return View();
        }

        public ActionResult Topic()
        {
            return View();
        }

        public ActionResult Manage()
        {
            return View();
        }
    }
}
