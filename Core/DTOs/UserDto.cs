using System;

namespace RapidPayTest.Application.DTOs
{
    public partial class UserDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SocialNumber { get; set; }
        public string Password { get; set; }

        public int? Gender { get; set; }
        public string Phone { get; set; }
        public string Estate { get; set; }
        public int RoleID { get; set; }
    }
}