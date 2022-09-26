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
    public class ProcessAppService : IProcessDataAppService
    {

        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ITransactionRepository _transactionRepository;

        public ProcessAppService(IUserRepository userRepository, IProductRepository productRepository, IClientRepository clientRepository, ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _clientRepository = clientRepository;
            _transactionRepository = transactionRepository;
        }

        public ResponseApi GetClients()
        {
            var r = new ResponseApi();
            try
            {
                var lst = _clientRepository.GetFiltered(s => s.IdStatus == 1);
                if (lst == null)
                {
                    r.Result = false;
                    r.Message = "No existen clientes...";
                    return r;
                }

                r.Data = lst;
                r.Result = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                r.Message = exception.InnerException?.Message ?? exception.Message;
            }

            return r;
        }
        public ResponseApi GetClient(int idClient)
        {
            var r = new ResponseApi();
            try
            {
                r.Data = _clientRepository.GetFiltered(s => s.Id == idClient).FirstOrDefault();
                r.Result = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                r.Message = exception.InnerException?.Message ?? exception.Message;
            }

            return r;
        }
        public ResponseApi GetClientByIdentification(string identification)
        {
            var r = new ResponseApi();
            try
            {
                var result = _clientRepository.GetFiltered(s => s.Identification == identification).FirstOrDefault();
                if (result == null)
                {
                    r.Result = false;
                }
                else {
                    r.Data = result;
                    r.Result = true;
                }

               
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                r.Message = exception.InnerException?.Message ?? exception.Message;
            }

            return r;
        }
        public ResponseApi DeleteClient(int idClient)
        {
            var r = new ResponseApi();
            try
            {
                var data = _clientRepository.GetFiltered(s => s.Id == idClient).FirstOrDefault();
                if (data == null)
                {
                    r.Message = "El cliente a inactivar, no existe.";
                    r.Result = false;
                }
                else
                {
                    data.IdStatus = 2; //Inactivo
                    _clientRepository.Update(data);
                }

                r.Message = "Cliente inactivado correctamente.";
                r.Result = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                r.Message = exception.InnerException?.Message ?? exception.Message;
            }

            return r;
        }
        public ResponseApi AddOrUpdateClient(Client data)
        {
            var r = new ResponseApi();
            try
            {
                var consultClient = _clientRepository.Get(s => s.Id == data.Id);
                if (consultClient != null)
                {
                    //Update
                    consultClient.IdStatus = data.IdStatus;
                    consultClient.FirstName = data.FirstName;
                    consultClient.LastName = data.LastName;
                    consultClient.Identification = data.Identification;
                    consultClient.PhoneNumber = data.PhoneNumber;
                    consultClient.UpdateDate = DateTime.Now;

                    _clientRepository.Update(consultClient);
                    r.Message = "Se actualizo correctamente el cliente.";
                    r.Result = true;
                }
                else
                {
                    //Create
                    data.CreatedDate = DateTime.Now;
                    data.UpdateDate = DateTime.Now;
                    data.IdStatus = 1; //Activo

                    _clientRepository.Add(data);
                    _clientRepository.UnitOfWork.Commit();

                    r.Message = "Se guardo correctamente el cliente.";
                    r.Result = true;
                }

            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }

            return r;
        }
        public ResponseApi GetProducts()
        {
            var r = new ResponseApi();
            try
            {
                var lst = _productRepository.GetFiltered(s => s.IdStatus == 1);
                if (lst == null)
                {
                    r.Result = false;
                    r.Message = "No existen productos...";
                    return r;
                }

                r.Data = lst;
                r.Result = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                r.Message = exception.InnerException?.Message ?? exception.Message;
            }

            return r;
        }
        public ResponseApi GetProduct(int idProduct)
        {
            var r = new ResponseApi();
            try
            {
                r.Data = _productRepository.GetFiltered(s => s.Id == idProduct).FirstOrDefault();
                r.Result = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                r.Message = exception.InnerException?.Message ?? exception.Message;
            }

            return r;
        }
        public ResponseApi DeleteProduct(int idClient)
        {
            var r = new ResponseApi();
            try
            {
                var data = _productRepository.GetFiltered(s => s.Id == idClient).FirstOrDefault();
                if (data == null)
                {
                    r.Message = "El producto a inactivar, no existe.";
                    r.Result = false;
                }
                else
                {
                    data.IdStatus = 2; //Inactivo
                    _productRepository.Update(data);
                }

                r.Message = "Producto inactivado correctamente.";
                r.Result = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                r.Message = exception.InnerException?.Message ?? exception.Message;
            }

            return r;
        }
        public ResponseApi AddOrUpdateProduct(Product data)
        {
            var r = new ResponseApi();
            try
            {
                var consultProduct = _productRepository.Get(s => s.Id == data.Id);
                if (consultProduct != null)
                {
                    //Update
                    consultProduct.IdStatus = data.IdStatus;
                    consultProduct.Name = data.Name;
                    consultProduct.UnitValue = data.UnitValue;
                    consultProduct.UpdateDate = DateTime.Now;

                    _productRepository.Update(consultProduct);
                    r.Message = "Se actualizo correctamente el producto.";
                    r.Result = true;
                }
                else
                {
                    //Create
                    data.CreatedDate = DateTime.Now;
                    data.UpdateDate = DateTime.Now;
                    data.IdStatus = 1; //Activo

                    _productRepository.Add(data);
                    _productRepository.UnitOfWork.Commit();

                    r.Message = "Se guardo correctamente el producto.";
                    r.Result = true;
                }

            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }

            return r;
        }
        public ResponseApi AddTransaction(List<AddTransactionDto> data, int idUser)
        {
            var r = new ResponseApi();
            try
            {
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        var consultClient = _clientRepository.GetFiltered(s => s.Identification == item.Identification).FirstOrDefault();
                        if (consultClient != null)
                        {
                            var transaction = new Transaction()
                            {
                                IdStatus = 1, //Activo
                                IdTransactionType = 1,
                                IdClient = consultClient.Id,
                                IdProduct = item.Id,
                                Amount = (int)item.Amount,
                                UnitValue = item.UnitValue,
                                TotalValue = item.UnitValue,
                                CreatedDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                                IdUser = idUser
                            };

                            _transactionRepository.Add(transaction);
                            _transactionRepository.UnitOfWork.Commit();
                        }
                        else {
                            r.Result = false;
                            r.Message = "El cliente no existe";
                        }
                       
                    }

                    r.Message = "Se realizo correctamente la venta de los productos.";
                    r.Result = true;
                }
                else {
                    r.Message = "Ocurrio un error al realizar la transacción.";
                    r.Result = false;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }

            return r;
        }

    }
}