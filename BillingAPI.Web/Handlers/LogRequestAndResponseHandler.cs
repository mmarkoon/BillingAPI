using log4net;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BillingAPI.Web.Handlers
{
    public class LogRequestAndResponseHandler : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // log request body
            var requestBody = await request.Content.ReadAsStringAsync();
            Log.Info($"Request RequestUri: {request.RequestUri}, Body: {requestBody}");

            // let other handlers process the request
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                string responseBody = null;
                if (response.Content != null)
                {
                    // once response body is ready, log it
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                Log.Info($"Response StatusCode: {response.StatusCode}, Body: {responseBody}");
                return response;
            }
            catch (Exception ex)
            {
                Log.Info($"Response Exception: {ex.Message}. For more details, see error log.");
                throw;
            }
        }
    }
}