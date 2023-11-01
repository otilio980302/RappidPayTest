
using RapidPayTest.Domain.Entities.Base;
using System.Collections.Generic;

namespace RapidPayTest.Domain.Entities
{
    public partial class CardManagement : BaseEntity
    {
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
        public int IDUser { get; set; }
    }
}