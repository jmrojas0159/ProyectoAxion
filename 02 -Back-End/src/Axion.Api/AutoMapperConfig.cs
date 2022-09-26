using AutoMapper;
using Core.DataTransferObject.Axion;
using Core.Entities.User;
using Axion.Api.Models;
using Axion.Api.Models.User;

namespace Axion.Api
{
    /// <summary>
    /// AutoMapperConfig
    /// </summary>
    public class AutoMapperConfig
    {
        /// <summary>
        /// Initializer
        /// </summary>
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                //User
                cfg.CreateMap<UserRegisterModel, User>();
                cfg.CreateMap<User, UserRegisterModel>();
            });
        }
    }
}