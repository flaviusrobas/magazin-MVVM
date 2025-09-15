using Magazin.Library.DataAccess;
using Magazin.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagazinApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Cashier")]
    public class ProductController : ControllerBase
    {
        //private readonly IConfiguration _config;

        //public ProductController(IConfiguration config)
        //{
        //    _config = config;
        //}

        private readonly IProductData _productData;

        public ProductController(IProductData productData)
        {
            _productData = productData;
        }


        [HttpGet]
        public List<ProductModel> Get()
        {
            //ProductData data = new ProductData(_config);

            return _productData.GetProductsById();

        }
    }
}
