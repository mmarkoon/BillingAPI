using BillingAPI.Web.Handlers;
using BillingAPI.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BillingAPI.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.MessageHandlers.Add(new LogRequestAndResponseHandler());
            config.Filters.Add(new ValidateModelStateFilterAttribute());
            config.Filters.Add(new BasicAuthenticationAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
