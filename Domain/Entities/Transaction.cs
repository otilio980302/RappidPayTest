
using RapidPayTest.Domain.Entities.Base;
using System.Collections.Generic;

namespace RapidPayTest.Domain.Entities
{
    public partial class Transaction : BaseEntity
    {
        public string CardNumber { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal TotalPayment { get; set; }
    }
}