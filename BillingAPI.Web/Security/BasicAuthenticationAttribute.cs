using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BillingAPI.Web.Security
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private string ValidUserName => ConfigurationManager.AppSettings["ApiLoginUsername"];
        private string ValidPassWord => ConfigurationManager.AppSettings["ApiLoginPassword"];

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                var userName = usernamePasswordArray[0];
                var passWord = usernamePasswordArray[1];

                if (!Login(userName, passWord))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }

        private bool Login(string userName, string passWord)
        {
            if (userName == ValidUserName && passWord == ValidPassWord)
                return true;
            return false;
        }
    }
}