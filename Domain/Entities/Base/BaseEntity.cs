using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayTest.Domain.Entities.Base
{
    public class BaseEntity
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public int UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
