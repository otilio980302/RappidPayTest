using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayTest.Application.DTOs.Security
{
    public class LoggedUser
    {
        public int Id { get; set; }
        public int InstitutionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsServerAdmin { get; set; }
        public string Ip { get; set; }
    }
}
