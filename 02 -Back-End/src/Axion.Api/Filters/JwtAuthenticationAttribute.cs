
#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Axion.Api.Controllers;

#endregion

namespace Axion.Api.Filters
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
                return;

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwtToken(token);

            if (principal == null)
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);

            else
                context.Principal = principal;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private static bool ValidateToken(string token, out IEnumerable<Claim> claims)
        {
            var resultValidateToken = JwtManager.ValidateToken(token);
            claims = new List<Claim>();
            if (resultValidateToken.Result)
            {
                var identity = resultValidateToken.Principal.Identity as ClaimsIdentity;
                claims = new List<Claim>();
                if (identity == null)
                {
                    resultValidateToken.Result = false;
                }
                else
                {
                    if (!identity.IsAuthenticated)
                    {
                        resultValidateToken.Result = false;
                    }
                    else
                    {
                        claims = identity.Claims;
                        if (claims == null || (!claims.Any()))
                        {
                            resultValidateToken.Result = false;
                        }
                    }
                }

                resultValidateToken.Result = true;
            }

            return resultValidateToken.Result;
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            IEnumerable<Claim> claims;
            if (ValidateToken(token, out claims))
            {
                // based on username to get more information from database in order to build local identity
                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parameter);
        }
    }
}