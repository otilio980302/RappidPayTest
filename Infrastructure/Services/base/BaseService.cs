using RappidPayTest.Identity.Models;
using Microsoft.AspNetCore.Http;

namespace RappidPayTest.Infrastructure.Services
{
    public class BaseService
    {
        private readonly IHttpContextAccessor _context;

        public BaseService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public ApplicationUser GetLoggerUser()
        {
            var User = _context.HttpContext.User;

            if (User.Identity.IsAuthenticated)
            {

                var UserData = new ApplicationUser()
                {
                    Id = User.FindFirst("uid").Value,
                    Name = User.FindFirst("Name").Value,
                    LastName = User.FindFirst("LastName").Value,
                    Email = User.FindFirst("Email").Value,
                };

                return UserData;
            }
            return null;
        }


        public string GetLoggerUserId()
        {
            var User = _context.HttpContext.User;
            if (User.Identity.IsAuthenticated)
            {

                return User.FindFirst("uid").Value;
            }


            return null;

        }


    }
}
