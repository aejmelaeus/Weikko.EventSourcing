using Autofac;
using Library.Interfaces;

namespace Library
{
    public class AutofacModule<TEventBase> : Module where TEventBase : class 
    {
        protected override void Load(ContainerBuilder bldr)
        {
            bldr.RegisterType<SqlServerProjectionRepository>()
                .As<IProjectionRepository>();

            bldr.RegisterGeneric(typeof(ProjectionBuilderBase<,>))
                .As(typeof(IProjectionBuilder<TEventBase>));

            bldr.RegisterType<Projections<TEventBase>>()
                .As<IProjections<TEventBase>>();
        }
    }
}
