
using RappidPayTest.Domain.Entities.Base;
using System.Collections.Generic;

namespace RappidPayTest.Domain.Entities
{
    public partial class CardManagement : BaseEntity
    {
        public int CardNumber { get; set; }
        public decimal Balance { get; set; }
        public int IDUser { get; set; }
    }
}