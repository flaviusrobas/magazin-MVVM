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
    public class ProductData
    {
        private readonly IConfiguration _config;
        public ProductData(IConfiguration config)
        {
            _config = config;
        }
        public List<ProductModel> GetProductsById()
        {
            SqlDataAccess sql = new SqlDataAccess(_config);
            
            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "MagData");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess(_config);

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "MagData").FirstOrDefault();

            return output;
        }
    }
}
