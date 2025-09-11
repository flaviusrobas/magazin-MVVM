using Magazin.Library.DataAccess;
using Magazin.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagazinApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class InventoryController : ControllerBase
    {
        private readonly IConfiguration _config;

        public InventoryController(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration Config { get; }

        [Authorize(Roles = "Admin,Manager")]
        public List<InventoryModel> Get()
        {
            InventoryData data = new InventoryData(_config);
            return data.GetInventory();
        }

        [Authorize(Roles = "WarehouseWorker")]
        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel item)
        {
            InventoryData data = new InventoryData(_config);
            data.SaveInventoryRecord(item);
        }
    }
}
