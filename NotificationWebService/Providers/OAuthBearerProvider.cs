using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NotificationWebService.Providers
{
    public class OAuthBearerProvider : OAuthBearerAuthenticationProvider
    {
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            string cookieToken = null;
            string queryStringToken = null;
            string headerToken = null;

            try
            {
                cookieToken = context.OwinContext.Request.Cookies["bearer"];
            }
            catch (NullReferenceException)
            {
                System.Diagnostics.Debug.WriteLine("The cookie does not contain the bearer token");
            }

            try
            {
                queryStringToken = context.OwinContext.Request.Query["bearer"].ToString();
            }
            catch (NullReferenceException)
            {
                System.Diagnostics.Debug.WriteLine("The query string does not contain the bearer token");
            }

            try
            {
                headerToken = context.OwinContext.Request.Headers["bearer"];
            }
            catch (NullReferenceException)
            {
                System.Diagnostics.Debug.WriteLine("The connection header does not contain the bearer token");
            }

            if (!String.IsNullOrEmpty(cookieToken))
                context.Token = cookieToken;

            else if (!String.IsNullOrEmpty(queryStringToken))
                context.Token = queryStringToken;

            else if (!String.IsNullOrEmpty(headerToken))
                context.Token = headerToken;

            return Task.FromResult<object>(null);
        }
    }
}