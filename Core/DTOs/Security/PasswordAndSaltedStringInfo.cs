using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayTest.Application.DTOs.Security
{
    public class PasswordAndSaltedStringInfo
    {
        public string HashedPassword { get; set; }
        public string SecurityKey { get; set; }

    }
}
