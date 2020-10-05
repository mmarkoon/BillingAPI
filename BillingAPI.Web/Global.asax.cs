using Autofac.Integration.WebApi;
using BillingAPI.Web.DependencyInjection;
using BillingAPI.Web.Handlers;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace BillingAPI.Web
{
    public class WebApiApplication : HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            LogConfiguration();

            JsonConfiguration();

            AutofacConfiguration();
        }

        private void AutofacConfiguration()
        {
            var config = GlobalConfiguration.Configuration;

            var builder = new AutofacContainerBuilder();
            var container = builder.BuildContainer();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private void JsonConfiguration()
        {
            var config = GlobalConfiguration.Configuration;

            var serializerSettings = config
                .Formatters
                .JsonFormatter
                .SerializerSettings;

            serializerSettings
                .Converters
                .Add(new StringEnumConverter());

            serializerSettings
                .NullValueHandling = NullValueHandling.Ignore;
        }

        private void LogConfiguration()
        {
            XmlConfigurator.Configure();

            var config = GlobalConfiguration.Configuration;

            config
                .Services
                .Add(typeof(IExceptionLogger), new Log4NetExceptionLogger());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Log.Error("Unhandled BillingService error", Server.GetLastError());
        }
    }
}
