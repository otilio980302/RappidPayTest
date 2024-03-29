﻿using Microsoft.AspNetCore.Identity;

namespace RapidPayTest.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string IdentificationNumber { get; set; }
    }
}
