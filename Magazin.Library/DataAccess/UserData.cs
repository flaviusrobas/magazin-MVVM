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
    public class UserData : IUserData
    {
        //private readonly IConfiguration _config;
        private readonly ISqlDataAccess _sql;
        public UserData(ISqlDataAccess sql)
        {
            //_config = config;
            _sql = sql;
        }
        public List<UserModel> GetUsersById(string Id)
        {
            //SqlDataAccess sql = new SqlDataAccess(_config);

            //var p = new { Id = Id };

            var output = _sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", new { Id }, "MagData");

            return output;
        }

        public void CreateUser(UserModel user)
        {
            _sql.SaveData<UserModel, dynamic>("dbo.spUser_Insert", new { user.ID, user.FirstName, user.LastName, user.EmailAddress }, "MagData");
        }
    }
}
