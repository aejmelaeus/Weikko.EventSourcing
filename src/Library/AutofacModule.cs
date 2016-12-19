using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Library.Interfaces;
using Module = Autofac.Module;

namespace Library
{
    public class AutofacModule<TEventBase> : Module where TEventBase : class, new()
    {
        protected override void Load(ContainerBuilder bldr)
        {
            bldr.RegisterType<SqlServerProjectionRepository>()
                .As<IProjectionRepository>();

            bldr.RegisterType<Projections<TEventBase>>()
                .As<IProjections<TEventBase>>();

            bldr.RegisterType<Aggregates<TEventBase>>()
                .As<IAggregates<TEventBase>>();

            var asseblies = AppDomain.CurrentDomain.GetAssemblies();

            bldr.RegisterAssemblyTypes(asseblies)
                .Where(a => a.GetInterfaces().Any(i => i.IsAssignableFrom(typeof (IProjectionBuilder<TEventBase>))))
                .AsImplementedInterfaces()
                .PropertiesAutowired();
        }
    }
}
