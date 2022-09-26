using Application.Main.Definition;
using Core.DataTransferObject.Axion;
using Crosscutting.DependencyInjectionFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Axion.Api.Extension;
using Axion.Api.Filters;
using Axion.Api.Models.Token;
using Axion.Api.Models.User;
using AutoMapper;
using Core.Entities.User;

namespace Axion.Api.Controllers
{
    /// <summary>
    /// Controlador de Tokens
    /// </summary>
    public class UserController : ApiController
    {
        private readonly IUserDataAppService _userDataAppService;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserController()
        {
            _userDataAppService = Factory.Resolve<IUserDataAppService>();
        }

        /// <summary>
        /// Crear el token del usuario
        /// </summary>
        /// <remarks>
        /// Recibe las credenciales del usuario
        /// </remarks>
        /// <param name="data"></param>
        /// <returns>Retorna el token de sesión</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("User/Login")]
        [EnableCors("*", "*", "*")]
        [ResponseType(typeof(ResponseApi))]
        public IHttpActionResult Login(LoginModel data)
        {
            var response = new ResponseApi();
            HttpStatusCode resulthttp;
            try
            {
                if (!ModelState.IsValid)
                {
                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    resulthttp = HttpStatusCode.BadRequest;
                    response.Message = string.Join(" ,", allErrors.Select(s => s.ErrorMessage).ToArray());
                }
                else
                {
                    var result = _userDataAppService.CheckUser(data.UserName, data.Password);
                    if (result.Result)
                    {

                        var user = result.User;
                        if (user != null)
                        {
                            var token = JwtManager.GenerateToken(user.ToClaims(), 90);

                            response.Result = true;
                            response.Data = new
                            {
                                Token = token,
                                User = user
                            };
                            resulthttp = HttpStatusCode.OK;

                        }
                        else
                        {
                            response.Result = false;
                            response.Message = "User not found.";
                            resulthttp = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Result = false;
                        response.Message = result.Mesage;
                        resulthttp = HttpStatusCode.OK;
                    }
                }

                return Content(resulthttp, response);
            }
            catch (System.Exception e)
            {
                resulthttp = HttpStatusCode.InternalServerError;

                response.Message = $"Service error {e.Message}";
                return Content(resulthttp, response);
            }
        }

    }
}
