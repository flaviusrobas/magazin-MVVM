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
    public class InventoryData : IInventoryData
    {
        private readonly IConfiguration _config;
        private readonly ISqlDataAccess _sql;

        public InventoryData(IConfiguration config, ISqlDataAccess sql)
        {
            _config = config;
            _sql = sql;
        }
        public List<InventoryModel> GetInventory()
        {
            //SqlDataAccess sql = new SqlDataAccess(_config);

            var output = _sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "MagData");

            return output;

        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            //SqlDataAccess sql = new SqlDataAccess(_config);

            _sql.SaveData<InventoryModel, InventoryModel>("dbo.spInventory_Insert", item, "MagData");

        }

    }
}
