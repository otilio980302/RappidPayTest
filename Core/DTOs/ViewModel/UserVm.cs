using System;

namespace RapidPayTest.Application.DTOs.ViewModel
{
    public partial class UserVm
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
    }
}