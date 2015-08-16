using System.Web.Http;
using System.Configuration;
using CacheCow.Server;
using CacheCow.Server.EntityTagStore.SqlServer;

namespace Lte.WebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "QueryPreciseStatApi2",
                routeTemplate: "api/{controller}/{cellId}/{sectorId}/{begin}/{end}",
                defaults: null,
                constraints: new { controller = "QueryPreciseStat" }
                );

            config.Routes.MapHttpRoute(
                name: "MroQueryRsrpTaApi",
                routeTemplate: "api/{controller}/{cellId}/{sectorId}/{date}",
                defaults: null,
                constraints: new { controller = "MroQueryRsrpTa" }
                );

            config.Routes.MapHttpRoute(
                name: "MrsQueryTaApi",
                routeTemplate: "api/{controller}/{cellId}/{sectorId}/{date}",
                defaults: null,
                constraints: new { controller = "MrsQueryTa" }
                );

            config.Routes.MapHttpRoute(
               name: "QueryPreciseStatApi",
               routeTemplate: "api/{controller}/{cellId}/{sectorId}/{date}",
               defaults: null,
               constraints: new { controller = "QueryPreciseStat" }
               );

            config.Routes.MapHttpRoute(
                name: "ByKeywordApi",
                routeTemplate: "api/{controller}/{district}/{keyword}/{type}",
                defaults: new {type = RouteParameter.Optional},
                constraints: new {controller = "DtFileInfos"}
                );

            config.Routes.MapHttpRoute(
                name: "TownTypeApi",
                routeTemplate: "api/{controller}/{town}/{type}",
                defaults: null,
                constraints: new {controller = "FileNames"}
                );

            config.Routes.MapHttpRoute(
                name: "CityTownListApi",
                routeTemplate: "api/{controller}/{cityName}/{districtName}",
                defaults: null,
                constraints: new {controller = "TownList"}
                );

            config.Routes.MapHttpRoute(
                name: "RegionNameApi",
                routeTemplate: "api/{controller}/{cityName}/{districtName}",
                defaults: null,
                constraints: new {controller = "RegionName"}
                );

            config.Routes.MapHttpRoute(
                name: "QueryLteTownMrsApi",
                routeTemplate: "api/{controller}/{district}/{town}",
                defaults: null,
                constraints: new { controller = "QueryLteMrs" }
                );

            config.Routes.MapHttpRoute(
                name: "QueryTownPreciseRatesApi",
                routeTemplate: "api/{controller}/{district}/{town}",
                defaults: null,
                constraints: new { controller = "QueryPreciseRates" }
                );

            config.Routes.MapHttpRoute(
                name: "QueryLteStatApi",
                routeTemplate: "api/{controller}/{begin}/{end}",
                defaults: null,
                constraints: new {controller = "QueryLteStat"}
                );

            config.Routes.MapHttpRoute(
                name: "DtPointsApi",
                routeTemplate: "api/{controller}/{low}/{high}"
                );

            config.Routes.MapHttpRoute(
                name: "DistrictListApi",
                routeTemplate: "api/{controller}/{cityName}",
                defaults: null,
                constraints: new {controller = "DistrictList"}
                );

            config.Routes.MapHttpRoute(
                name: "DistrictInfosApi",
                routeTemplate: "api/{controller}/{district}",
                defaults: null,
                constraints: new {controller = "DtFileInfos"}
                );

            config.Routes.MapHttpRoute(
                name: "QueryLteDistrictDatesApi",
                routeTemplate: "api/{controller}/{district}",
                defaults: null,
                constraints: new { controller = "QueryLteDistrictDates" }
                );

            config.Routes.MapHttpRoute(
                name: "QueryLteDistrictMrsApi",
                routeTemplate: "api/{controller}/{district}",
                defaults: null,
                constraints: new { controller = "QueryLteMrs" }
                );

            config.Routes.MapHttpRoute(
                name: "QueryDistrictPreciseRatesApi",
                routeTemplate: "api/{controller}/{district}",
                defaults: null,
                constraints: new { controller = "QueryPreciseRates" }
                );

            config.Routes.MapHttpRoute(
                name: "QueryLteDistrictMrTableApi",
                routeTemplate: "api/{controller}/{district}",
                defaults: null,
                constraints: new { controller = "QueryLteMrTable" }
                );

            config.Routes.MapHttpRoute(
                name: "QueryDistrictPreciseRateTableApi",
                routeTemplate: "api/{controller}/{district}",
                defaults: null,
                constraints: new { controller = "QueryPreciseRateTable" }
                );

            config.Routes.MapHttpRoute(
                name: "QueryLteKpiTownListApi",
                routeTemplate: "api/{controller}/{district}",
                defaults: null,
                constraints: new { controller = "QueryLteKpiTownList" }
                );

            config.Routes.MapHttpRoute(
                name: "MrsQueryCoverageApi",
                routeTemplate: "api/{controller}/{topNum}",
                defaults: null,
                constraints: new {controller = "MrsQueryCoverage"}
                );

            config.Routes.MapHttpRoute(
                name: "DefaultIdApi",
                routeTemplate: "api/{controller}/{id}"
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}"
                );

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // 取消注释下面的代码行可对具有 IQueryable 或 IQueryable<T> 返回类型的操作启用查询支持。
            // 若要避免处理意外查询或恶意查询，请使用 QueryableAttribute 上的验证设置来验证传入查询。
            // 有关详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=279712。
            //config.EnableQuerySupport();

            //string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //SqlServerEntityTagStore eTagStore = new SqlServerEntityTagStore(connString);
            //CachingHandler handler = new CachingHandler(config, eTagStore) {AddLastModifiedHeader = false};
            //config.MessageHandlers.Add(handler);
        }
    }
}