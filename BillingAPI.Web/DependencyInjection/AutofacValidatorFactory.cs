using Autofac;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingAPI.Web.DependencyInjection
{
    public class AutofacValidatorFactory : ValidatorFactoryBase
    {
        private readonly IComponentContext context;

        public AutofacValidatorFactory(IComponentContext context)
        {
            this.context = context;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            if (context.TryResolve(validatorType, out object instance))
            {
                var validator = instance as IValidator;
                return validator;
            }

            return null;
        }
    }
}