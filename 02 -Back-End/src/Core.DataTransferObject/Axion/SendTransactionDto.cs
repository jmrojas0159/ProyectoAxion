using Core.Entities.User;
using System;
using System.Collections.Generic;

namespace Core.DataTransferObject.Axion
{

    public class SendTransactionDto : ResponseApi
    {
        public Transaction LstTransaction { get; set; }
    }
}


