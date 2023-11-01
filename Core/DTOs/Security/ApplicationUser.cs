using Microsoft.AspNetCore.Identity;
using System;

namespace RapidPayTest.Application.DTOs.Security
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string IdentificationNumber { get; set; }


    }
}
