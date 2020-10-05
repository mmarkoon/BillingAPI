using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Integration.WebApi;
using BillingAPI.Data.Autofac;
using FluentValidation;
using FluentValidation.WebApi;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;
using System.Web.Http.Validation;

namespace BillingAPI.Web.DependencyInjection
{
    public class AutofacContainerBuilder
    {
        private ContainerBuilder builder;

        public IContainer BuildContainer()
        {
            builder = new ContainerBuilder();
            RegisterWebApiInternal();
            builder.RegisterModule(new BillingServiceModule());
            var container = builder.Build();
            RegisterCommonServiceLocator(container);
            return container;
        }

        private static void RegisterCommonServiceLocator(IContainer container)
        {
            // Set the service locator to an AutofacServiceLocator.
            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);

            var cslBuilder = new ContainerBuilder();
            cslBuilder.RegisterInstance(csl).As<IServiceLocator>();
            cslBuilder.Update(container);
        }

        private void RegisterWebApiInternal()
        {
            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Fluent validation support
            builder.RegisterType<FluentValidationModelValidatorProvider>().As<ModelValidatorProvider>();
            builder.RegisterType<AutofacValidatorFactory>().As<IValidatorFactory>().SingleInstance();
        }
    }
}