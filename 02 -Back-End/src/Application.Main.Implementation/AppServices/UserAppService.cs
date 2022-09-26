#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Application.Main.Definition;
using Core.DataTransferObject.Axion;
using Core.Entities.User;
using Core.GlobalRepository.SQL.User;

#endregion

namespace Application.Main.Implementation.AppServices
{
    public class UserAppService : IUserDataAppService
    {
        private readonly IUserRepository _userRepository;

        public UserAppService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ValidateUserLogin CheckUser(string username, string password)
        {
            var r = new ValidateUserLogin { Result = false };
            var pass = EncryptPassword(password);

            var user = _userRepository.Get(s => s.UserName == username) ??
                       _userRepository.Get(s => s.Email == username);

            if (user == null)
            {
                r.Result = false;
                r.Mesage = "Combinación incorrecta de usuario y contraseña.";
                return r;
            }

            if (user != null)
            {
                r.Result = user.Password == pass;
                r.Mesage = (user.Password == pass) ? "" : "Combinación incorrecta de usuario y contraseña...";
                r.User = user;
            }

            return r;
        }

        private static string EncryptPassword(string password)
        {
            var md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(Encoding.ASCII.GetBytes(password));

            //get hash result after compute it
            var result = md5.Hash;

            var strBuilder = new StringBuilder();
            foreach (var t in result)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(t.ToString("x2"));
            }

            return strBuilder.ToString();
        }

    }
}