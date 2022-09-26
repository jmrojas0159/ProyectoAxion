using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Axion.Api.Controllers
{
    public class JwtManager
    {
        public const string Secret = "856FECBA3B06519C8DDDBC80BB080553"; // your symetric

        public static ResultValidateToken ValidateToken(string token)
        {
            var r = new ResultValidateToken();

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                r.Result = true;
                r.Principal = principal;
                return r;
            }

            catch (Exception exception)
            {
                r.Result = false;
                r.Message = exception.InnerException?.Message ?? exception.Message;
                return r;
            }
        }

        public static string GenerateToken(IEnumerable<Claim> claims, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(symmetricKey),
                        SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}