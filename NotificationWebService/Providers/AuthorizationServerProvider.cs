using DevOne.Security.Cryptography.BCrypt;
using Microsoft.Owin.Security.OAuth;
using NotificationWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace NotificationWebService.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                using (var ctx = new DatabaseContext())
                {
                    var username = context.UserName;
                    var user = ctx.Users.SingleOrDefault(w => w.UserName == username);
                    if (user != null && BCryptHelper.CheckPassword(context.Password, user.Password))
                    {
                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.Guid));
                        identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("Invalid_grant", "wrong credentials");
                    }

                }
            }
            catch (Exception ex)
            {
                context.SetError("error_occured", ex.Message);
            }
        }
    }
}