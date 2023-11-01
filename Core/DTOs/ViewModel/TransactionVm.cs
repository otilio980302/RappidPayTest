using System;

namespace RapidPayTest.Application.DTOs.ViewModel
{
    public partial class TransactionVm
    {
        public int ID { get; set; }
        public DateTime CreateAt { get; set; }
        public string CardNumber { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal TotalPayment { get; set; }
    }
}