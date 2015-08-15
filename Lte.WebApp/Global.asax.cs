using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lte.Evaluations.Dingli;
using Lte.Evaluations.Service;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Entities;
using Lte.Parameters.Kpi.Service;
using Lte.WebApp.Controllers;
using Lte.WebApp.Models;
using Lte.Evaluations.Infrastructure;
using Lte.Evaluations.Entities;

namespace Lte.WebApp
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver();

            ModelBinders.Binders.Add(typeof(EvaluationInfrastructure), new EvaluationBinder());
            ModelBinders.Binders.Add(typeof(StatValueFieldRepository), new StatFieldBinder());
            ModelBinders.Binders.Add(typeof(StatRuFieldRepository), new StatRuBinder());
            ModelBinders.Binders.Add(typeof(StatComplexFieldRepository), new StatComplexBinder());
            ModelBinders.Binders.Add(typeof(ParametersContainer), new ParametersContainerBinder());
            ModelBinders.Binders.Add(typeof(ParametersDumpInfrastructure), new ParametersDumpInfrastructureBinder());
            ModelBinders.Binders.Add(typeof(AllCdmaDailyStatList), new CdmaDailyStatListBinder());
            ModelBinders.Binders.Add(typeof (CoverageStatChart), new CoverageStatBinder());
            ModelBinders.Binders.Add(typeof (RateStatChart), new RateStatBinder());
        }
    }
}