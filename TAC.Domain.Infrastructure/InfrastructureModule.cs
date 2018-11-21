using Autofac;
using TAC.Domain.Infrastructure.Repositories;

namespace TAC.Domain.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<VehicleRepository>().As<IVehicleRepository>().InstancePerLifetimeScope();
        }
    }
}
