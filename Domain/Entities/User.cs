﻿using RapidPayTest.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayTest.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SocialNumber { get; set; }
        public string Password { get; set; }
        public string PasswordKey { get; set; }
        public bool? ContractValidation { get; set; }
        public DateTime? LastAccess { get; set; }
        public int? Gender { get; set; }
        public string Phone { get; set; }
        public string Estate { get; set; }
        public int RoleID { get; set; }
        public virtual Role Role { get; set; }
    }
}