using Autofac;

namespace TAC.Business
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<VehicleManager>().As<IVehicleManager>().InstancePerLifetimeScope();
        }
    }
}
