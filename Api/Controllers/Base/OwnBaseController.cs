using RapidPayTest.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace RapidPayTest.Api.Controllers.Base
{
    [Authorize]
    [ApiController]
    public class OwnBaseController : ControllerBase
    {



        internal ApplicationUser GetLoggerUser()
        {
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


        internal int GetLoggerUserId()
        {
            if (User.Identity.IsAuthenticated)
            {

                return Convert.ToInt32(User.FindFirst("uid").Value);
            }


            return 0;

        }

    }
}
