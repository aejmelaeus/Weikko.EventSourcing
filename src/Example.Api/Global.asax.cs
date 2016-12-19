using Autofac;
using Library;
using Example.Events;
using System.Web.Http;
using System.Reflection;
using Autofac.Integration.WebApi;
using Library.Interfaces;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;

namespace Example.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // TODO - figure out how this is fixed in a smooth manner... :)
            var projectionRepository = new SqlServerProjectionRepository();
            projectionRepository.CreateProjectionsTable();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            var bldr = new ContainerBuilder();

            // TODO: Figure out the name for this one...
            var module = new AutofacModule<EventBase>();

            bldr.RegisterModule(module);

            var config = GlobalConfiguration.Configuration;
            bldr.RegisterApiControllers(Assembly.GetExecutingAssembly());
            bldr.RegisterWebApiFilterProvider(config);
            
            bldr.RegisterInstance(GetEventSource())
                .As<IStoreEvents>();

            bldr.RegisterType<EventSource>()
                .As<IEventSource<EventBase>>();

            var container = bldr.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private IStoreEvents GetEventSource()
        {
            return Wireup
                .Init()
                .UsingSqlPersistence("EventSource")
                .WithDialect(new MsSqlDialect())
                .EnlistInAmbientTransaction()
                .InitializeStorageEngine()
                .UsingJsonSerialization()
                .Compress()
                .Build();
        }
    }
}