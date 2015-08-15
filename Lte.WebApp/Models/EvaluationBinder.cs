using System.Web.Mvc;
using Lte.Evaluations.Dingli;
using Lte.Evaluations.Entities;
using Lte.Evaluations.Infrastructure;
using Lte.Parameters.Entities;

namespace Lte.WebApp.Models
{
    public class EvaluationBinder : IModelBinder
    {
        private const string sessionKey = "Evaluation";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            EvaluationInfrastructure evaluation
                = (EvaluationInfrastructure)controllerContext.HttpContext.Session[sessionKey];

            if (evaluation == null)
            {
                evaluation = new EvaluationInfrastructure();
                controllerContext.HttpContext.Session[sessionKey] = evaluation;
            }
            // return the cart
            return evaluation;
        }
    }

    public class StatFieldBinder : IModelBinder
    {
        private const string sessionKey = "StatValue";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            StatValueFieldRepository repository
                = (StatValueFieldRepository)controllerContext.HttpContext.Session[sessionKey];

            if (repository == null)
            {
                repository = new StatValueFieldRepository();
                controllerContext.HttpContext.Session[sessionKey] = repository;
            }
            // return the cart
            return repository;
        }
    }

    public class StatRuBinder : IModelBinder
    {
        private const string sessionKey = "StatRu";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            StatRuFieldRepository repository
                = (StatRuFieldRepository)controllerContext.HttpContext.Session[sessionKey];

            if (repository == null)
            {
                repository = new StatRuFieldRepository();
                controllerContext.HttpContext.Session[sessionKey] = repository;
            }
            // return the cart
            return repository;
        }
    }

    public class StatComplexBinder : IModelBinder
    {
        private const string sessionKey = "StatComplex";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            StatComplexFieldRepository repository
                = (StatComplexFieldRepository)controllerContext.HttpContext.Session[sessionKey];

            if (repository == null)
            {
                repository = new StatComplexFieldRepository();
                controllerContext.HttpContext.Session[sessionKey] = repository;
            }
            // return the cart
            return repository;
        }
    }

    public class CoverageStatBinder : IModelBinder
    {
        private const string sessionKey = "CoverageStat";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            CoverageStatChart repository
                = (CoverageStatChart)controllerContext.HttpContext.Session[sessionKey];

            if (repository == null)
            {
                repository = new CoverageStatChart();
                controllerContext.HttpContext.Session[sessionKey] = repository;
            }
            // return the cart
            return repository;
        }
    }

    public class RateStatBinder : IModelBinder
    {
        private const string sessionKey = "RateStat";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            RateStatChart repository
                = (RateStatChart)controllerContext.HttpContext.Session[sessionKey];

            if (repository == null)
            {
                repository = new RateStatChart();
                controllerContext.HttpContext.Session[sessionKey] = repository;
            }
            // return the cart
            return repository;
        }
    }
}