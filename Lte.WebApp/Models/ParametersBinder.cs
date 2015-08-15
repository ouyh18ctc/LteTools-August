using System.Web.Mvc;
using Lte.Evaluations.Dingli;
using Lte.Evaluations.Service;
using Lte.Parameters.Kpi.Service;

namespace Lte.WebApp.Models
{
    public class ParametersContainerBinder : IModelBinder
    {
        private const string sessionKey = "ParametersContainer";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            ParametersContainer container
                = (ParametersContainer)controllerContext.HttpContext.Session[sessionKey];

            if (container == null)
            {
                container = new ParametersContainer();
                controllerContext.HttpContext.Session[sessionKey] = container;
            }
            // return the cart
            return container;
        }
    }

    public class ParametersDumpInfrastructureBinder : IModelBinder
    {
        private const string sessionKey = "ParametersDumpInfrastructure";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            ParametersDumpInfrastructure infrastructure
                = (ParametersDumpInfrastructure)controllerContext.HttpContext.Session[sessionKey];

            if (infrastructure == null)
            {
                infrastructure = new ParametersDumpInfrastructure();
                controllerContext.HttpContext.Session[sessionKey] = infrastructure;
            }
            // return the cart
            return infrastructure;
        }
    }
}