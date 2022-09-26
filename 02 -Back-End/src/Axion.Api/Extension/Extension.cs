using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Core.Entities.User;
using Axion.Api.Common;

namespace Axion.Api.Extension
{
    public static class Extension
    {
        public static User ToUser(this IPrincipal principal)
        {
            var princi = (ClaimsPrincipal)principal;
            User user = null;

            if (principal != null)
            {
                user = new User();
                user.Id = (int)long.Parse(princi.Claims.First(s => s.Type == "Id").Value);
                user.Email = (princi.Claims.First(s => s.Type == "Email").Value);
                user.Password = princi.Claims.First(s => s.Type == "Password").Value;
                user.UserName = princi.Claims.First(s => s.Type == "UserName").Value;
                user.FullName = princi.Claims.First(s => s.Type == "FullName").Value;
                user.PhoneNumber = princi.Claims.First(s => s.Type == "PhoneNumber").Value;
                user.Identification = princi.Claims.First(s => s.Type == "Identification").Value;
            }
            return user;
        }

        public static IEnumerable<Claim> ToClaims(this User user)
        {
            var lclaims = new List<Claim>();
            lclaims.Add(new Claim("Id", user.Id.ToString()));
            lclaims.Add(new Claim("Email", user.Email));
            lclaims.Add(new Claim("Password", user.Password));
            lclaims.Add(new Claim("UserName", user.UserName));
            lclaims.Add(new Claim("Password", user.Password));
            lclaims.Add(new Claim("FullName", user.FullName));
            lclaims.Add(new Claim("PhoneNumber", !string.IsNullOrEmpty(user.PhoneNumber) ? user.PhoneNumber : string.Empty));
            lclaims.Add(new Claim("Identification", user.Identification));

            return lclaims;
        }
    }
}