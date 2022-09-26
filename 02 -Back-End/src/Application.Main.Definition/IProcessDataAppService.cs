using Core.DataTransferObject.Axion;
using Core.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Main.Definition
{
    public interface IProcessDataAppService
    {
        ResponseApi GetClients();
        ResponseApi GetClient(int idClient);
        ResponseApi GetClientByIdentification(string identification);
        ResponseApi DeleteClient(int idClient);
        ResponseApi AddOrUpdateClient(Client data);
        ResponseApi GetProducts();
        ResponseApi GetProduct(int idProduct);
        ResponseApi DeleteProduct(int idClient);
        ResponseApi AddOrUpdateProduct(Product data);
        ResponseApi AddTransaction(List<AddTransactionDto> data, int idUser);

    }
}
