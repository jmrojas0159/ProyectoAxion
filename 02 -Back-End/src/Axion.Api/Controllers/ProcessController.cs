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
using Core.Entities.User;

namespace Axion.Api.Controllers
{
    /// <summary>
    /// Controlador de los procesos
    /// </summary>
    public class ProcessController : ApiController
    {
        private readonly IProcessDataAppService _processDataAppService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessController()
        {
            _processDataAppService = Factory.Resolve<IProcessDataAppService>();
        }

        /// <summary>
        /// Obtener listado de clientes
        /// </summary>
        /// <returns></returns>
        /// 
        [JwtAuthentication]
        [Route("Process/GetClients")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        [ResponseType(typeof(ResponseApi))]
        public IHttpActionResult GetClients()
        {
            var response = _processDataAppService.GetClients();
            return Json(response);
        }

        /// <summary>
        /// Obtener un cliente en especifico.
        /// </summary>
        /// <returns></returns>
        /// 
        [JwtAuthentication]
        [Route("Process/GetClient/{idClient?}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        [ResponseType(typeof(ResponseApi))]
        public IHttpActionResult GetClient(int idClient)
        {
            var response = _processDataAppService.GetClient(idClient);
            return Json(response);
        }

        /// <summary>
        /// Obtener un cliente en especifico por identificación.
        /// </summary>
        /// <returns></returns>
        /// 
        [JwtAuthentication]
        [Route("Process/GetClientByIdentification/{identification?}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        [ResponseType(typeof(ResponseApi))]
        public IHttpActionResult GetClientByIdentification(string identification)
        {
            var response = _processDataAppService.GetClientByIdentification(identification);
            return Json(response);
        }

        /// <summary>
        /// Eliminar/Inactivar Cliente
        /// </summary>
        /// <returns></returns>
        /// 
        [JwtAuthentication]
        [Route("Process/DeleteClient/{idClient?}")]
        [HttpDelete]
        [EnableCors("*", "*", "*")]
        [ResponseType(typeof(ResponseApi))]
        public IHttpActionResult DeleteClient(int idClient)
        {
            var response = _processDataAppService.DeleteClient(idClient);
            return Json(response);
        }

        /// <summary>
        /// Guardar/Actualizar cliente.
        /// </summary>
        /// <returns></returns>
        [JwtAuthentication]
        [Route("Process/AddOrUpdateClient")]
        [HttpPost]
        [EnableCors("*", "*", "*")]
        [ResponseType(typeof(ResponseApi))]
        public IHttpActionResult AddOrUpdateClient(Client data)
        {
            var r = new ResponseApi();

            r = _processDataAppService.AddOrUpdateClient(data);
            StatusCode(HttpStatusCode.OK);

            return Json(r);
        }

        /// <summary>
        /// Obtener listado de productos
        /// </summary>
        /// <returns></returns>
        /// 
        [JwtAuthentication]
        [Route("Process/GetProducts")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        [ResponseType(typeof(ResponseApi))]
        public IHttpActionResult GetProducts()
        {
            var response = _processDataAppService.GetProducts();
            return Json(response);
        }

        /// <summary>
        /// Obtener un producto en especifico.
        /// </summary>
        /// <returns></returns>
        /// 
        [JwtAuthentication]
        [Route("Process/GetProduct/{idProduct?}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        [ResponseType(typeof(ResponseApi))]
        public IHttpActionResult GetProduct(int idProduct)
        {
            var response = _processDataAppService.GetProduct(idProduct);
            return Json(response);
        }

        /// <summary>
        /// Eliminar/Inactivar Producto
        /// </summary>
        /// <returns></returns>
        /// 
        [JwtAuthentication]
        [Route("Process/DeleteProduct/{idProduct?}")]
        [HttpDelete]
        [EnableCors("*", "*", "*")]
        [ResponseType(typeof(ResponseApi))]
        public IHttpActionResult DeleteProduct(int idProduct)
        {
            var response = _processDataAppService.DeleteProduct(idProduct);
            return Json(response);
        }

        /// <summary>
        /// Guardar/Actualizar producto.
        /// </summary>
        /// <returns></returns>
        [JwtAuthentication]
        [Route("Process/AddOrUpdateProduct")]
        [HttpPost]
        [EnableCors("*", "*", "*")]
        [ResponseType(typeof(ResponseApi))]
        public IHttpActionResult AddOrUpdateProduct(Product data)
        {
            var r = new ResponseApi();

            r = _processDataAppService.AddOrUpdateProduct(data);
            StatusCode(HttpStatusCode.OK);

            return Json(r);
        }

        /// <summary>
        /// Guardar/Actualizar Transacción.
        /// </summary>
        /// <returns></returns>
        [JwtAuthentication]
        [Route("Process/AddTransaction")]
        [HttpPost]
        [EnableCors("*", "*", "*")]
        [ResponseType(typeof(ResponseApi))]
        public IHttpActionResult AddTransaction(List<AddTransactionDto> data)
        {
            var r = new ResponseApi();

            var u = RequestContext.Principal.ToUser();
            r = _processDataAppService.AddTransaction(data, u.Id);
            StatusCode(HttpStatusCode.OK);

            return Json(r);
        }
    }
}
