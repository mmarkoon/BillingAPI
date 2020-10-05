using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingAPI.Data.Autofac
{
    public class BillingServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterAssemblyTypes(builder);
        }

        private void RegisterAssemblyTypes(ContainerBuilder builder)
        {
            var billingServiceData = typeof(BillingServiceModule).Assembly;

            builder
                .RegisterAssemblyTypes(billingServiceData)
                .AsImplementedInterfaces();
        }
    }
}
