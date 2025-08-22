using Magazin.Library.Internal.DataAccess;
using Magazin.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProductsById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { Id = Id };
            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new {}, "MagData");
            return output;
        }
    }
}
