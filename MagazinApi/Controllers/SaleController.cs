using Azure;
using Magazin.Library.DataAccess;
using Magazin.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MagazinApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        //private readonly IConfiguration _config;
        private readonly ISaleData _saleData;

        public SaleController(ISaleData saleData)
        {
            //_config = config;
            _saleData = saleData;
        }
        [Authorize(Roles = "Cashier")]
        [HttpPost]
        public void Post(SaleModel sale)
        {
            //SaleData data = new SaleData(_config);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _saleData.SaveSale(sale, userId);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
        [HttpGet]
        public List<SaleReportModel> GetSalesReport()
        {            
            //SaleData data = new SaleData(_config);
            return _saleData.GetSaleReports();
        }

        [AllowAnonymous]
        [Route("GetTaxRate")]
        [HttpGet]
        public decimal GetTaxRate()
        {
            //SaleData data = new SaleData(_config);
            return _saleData.GetTaxRate();
        }
    }
}
