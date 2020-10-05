using BillingAPI.Data.Operations;
using BillingAPI.Models.Models;
using BillingAPI.Web.Security;
using log4net;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace BillingAPI.Web.Controllers
{
    [BasicAuthentication]
    public class OrderController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IServiceLocator serviceLocator;

        public OrderController(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        [HttpPost]
        [Route("order/process")]
        public HttpResponseMessage ProcessOrder([FromBody] ProcessOrderRequestData orderData)
        {
            try
            {
                var service = serviceLocator.GetInstance<IProcessOrderOperation>();

                var model = service.ProcessOrder(orderData);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception e)
            {
                Logger.Error($"Method: ProcessOrderOperation, Exception: {e.Message}");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}