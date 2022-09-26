using Core.Entities.User;
using System;
using System.Collections.Generic;

namespace Core.DataTransferObject.Axion
{

    public class AddTransactionDto : ResponseApi
    {
        public int Id { get; set; }
        public int IdStatus { get; set; }
        public string Name { get; set; }
        public decimal UnitValue { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Identification { get; set; }
    }
}


