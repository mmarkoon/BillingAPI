using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace BillingAPI.Web.Handlers
{
    public class Log4NetExceptionLogger : ExceptionLogger
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void Log(ExceptionLoggerContext context)
        {
            Logger.Error("Unexpected WebApi error", context.ExceptionContext.Exception);
        }
    }
}