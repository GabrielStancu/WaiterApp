using Core.Context;
using Infrastructure.Business.Database;
using Infrastructure.Business.DeviceInfo;
using Infrastructure.Business.Filter;
using Infrastructure.Business.Parameters;
using Infrastructure.Business.Specification;
using Infrastructure.Business.TablesDrawing;
using Infrastructure.Business.Wifi;
using Infrastructure.Repositories;
using Infrastructure.ViewModels;
using Nancy.TinyIoc;
using System.Linq;
using System.Reflection;
using WaiterApp.Pages;

namespace WaiterApp.Utils
{
    public static class TinyIocBuilder
    {
        public static void RegisterApp(this TinyIoCContainer container)
        {
            RegisterSelfClasses(container, typeof(MainPage).Assembly.FullName);
            RegisterInterfaceClassPairs(container, typeof(IBaseViewModel).Assembly.FullName);
            RegisterInterfaceClassPairs(container, typeof(IGenericRepository<>).Assembly.FullName);
            RegisterInterfaceClassPairs(container, typeof(IConnectionStringBuilder).Assembly.FullName);
            RegisterInterfaceClassPairs(container, typeof(IDeviceInfoCollector).Assembly.FullName);
            RegisterInterfaceClassPairs(container, typeof(IProductsFilter).Assembly.FullName);
            RegisterInterfaceClassPairs(container, typeof(IDepartmentLoader).Assembly.FullName);
            RegisterInterfaceClassPairs(container, typeof(ISpecification<>).Assembly.FullName);
            RegisterInterfaceClassPairs(container, typeof(ITableDrawer).Assembly.FullName);
            RegisterInterfaceClassPairs(container, typeof(IWifiConnectionChecker).Assembly.FullName);
            RegisterSelfClasses(container, typeof(RestaurantContext).Assembly.FullName);
        }

        private static void RegisterSelfClasses(TinyIoCContainer container, string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);
            var classTypes = assembly.GetTypes().Where(t => !(t.IsInterface || t.IsAbstract || t.IsSealed));

            foreach (var classType in classTypes)
            {
                container.Register(classType);
            }
        }

        private static void RegisterInterfaceClassPairs(TinyIoCContainer container, string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);
            var classTypes = assembly.GetTypes().Where(t => !(t.IsInterface || t.IsAbstract || t.IsSealed));

            foreach (var classType in classTypes)
            {
                var interfaceTypes = classType.GetInterfaces();

                if(interfaceTypes.Length > 0)
                {
                    foreach (var interfaceType in interfaceTypes)
                    {
                        container.Register(interfaceType, classType);
                    }
                }
            }
        }
    }
}
