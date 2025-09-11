using Magazin.Library.Internal.DataAccess;
using Magazin.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Library.DataAccess
{
    public class UserData
    {
        private readonly IConfiguration _config;
        public UserData(IConfiguration config)
        {
            _config = config;
        }
        public List<UserModel> GetUsersById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess(_config);

            var p = new { Id = Id };

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "MagData");

            return output;
        }
    }
}
