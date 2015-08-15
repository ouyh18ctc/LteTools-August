using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using System.Web.Routing;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Concrete;
using Lte.Parameters.Kpi.Entities;
using Ninject;
using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;

namespace Lte.WebApp.Controllers
{
    public static class KernelBindingOpertions
    {
        public static void AddBindings(this IKernel ninjectKernel)
        {
            ninjectKernel.Bind<ITownRepository>().To<EFTownRepository>();
            ninjectKernel.Bind<IRegionRepository>().To<EFRegionRepository>();
            ninjectKernel.Bind<IENodebRepository>().To<EFENodebRepository>();
            ninjectKernel.Bind<ICellRepository>().To<EFCellRepository>();
            ninjectKernel.Bind<IBtsRepository>().To<EFBtsRepository>();
            ninjectKernel.Bind<ICdmaCellRepository>().To<EFCdmaCellRepository>();
            ninjectKernel.Bind<ITopCellRepository<CdmaRegionStat>>().To<EFCdmaRegionStatRepository>();
            ninjectKernel.Bind<ITopCellRepository<TopDrop2GCell>>().To<EFTopDrop2GCellRepository>();
            ninjectKernel.Bind<ITopCellRepository<TopDrop2GCellDaily>>().To<EFTopDrop2GcellDailyRepository>();
            ninjectKernel.Bind<ITopCellRepository<TopConnection3GCell>>().To<EFTopConnection3GRepository>();
            ninjectKernel.Bind<ITopCellRepository<PreciseCoverage4G>>().To<EFPreciseCoverage4GRepository>();
            ninjectKernel.Bind<ITopCellRepository<TownPreciseCoverage4GStat>>()
                .To<EFTownPreciseCoverage4GStatRepository>();
            ninjectKernel.Bind<IInterferenceStatRepository>().To<EFInterferenceStatRepository>();
            ninjectKernel.Bind<IMrsCellRepository>().To<EFMrsCellRepository>();
            ninjectKernel.Bind<IMroCellRepository>().To<EFMroCellRepository>();
            ninjectKernel.Bind<IENodebPhotoRepository>().To<EFENodebPhotoRepository>();
            ninjectKernel.Bind<ILteNeighborCellRepository>().To<EFLteNeighborCellRepository>();
            ninjectKernel.Bind<ICollegeRepository>().To<EFCollegeRepository>();
            ninjectKernel.Bind<IInfrastructureRepository>().To<EFInfrastructureRepository>();
            ninjectKernel.Bind<IIndoorDistributioinRepository>().To<EFIndoorDistributionRepository>();
        }
    }

    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            ninjectKernel.AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext
            requestContext, Type controllerType)
        {

            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);
        }

    }

    public class NinjectDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IKernel ninjectKernel;
        private readonly List<IDisposable> disposableService=new List<IDisposable>(); 

        public NinjectDependencyResolver()
        {
            ninjectKernel = new StandardKernel();
            NinjectKernel.AddBindings();
        }

        public NinjectDependencyResolver(NinjectDependencyResolver parent)
        {
            ninjectKernel = parent.NinjectKernel;
        }

        public IKernel NinjectKernel
        {
            get { return ninjectKernel; }
        }

        public object GetService(Type serviceType)
        {
            return NinjectKernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            foreach (object service in NinjectKernel.GetAll(serviceType))
            {
                AddDisposableService(service);
                yield return service;
            }
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyResolver(this);
        }

        private void AddDisposableService(object service)
        {
            IDisposable disposable = service as IDisposable;
            if (disposable != null && !disposableService.Contains(disposable))
            {
                disposableService.Add(disposable);
            }
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in disposableService)
            {
                disposable.Dispose();
            }
        }
    }
}