using Magazin.Library.DataAccess;
using Magazin.Library.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Magazin.Controllers
{
    [Authorize]
    
    public class UserController : ApiController
    {
        [HttpGet]
        public UserModel GetById()
        {
           
                string userId = RequestContext.Principal.Identity.GetUserId();

                UserData data = new UserData();

                return data.GetUsersById(userId).First();
            
        }
    }
}
