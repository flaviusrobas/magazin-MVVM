using MagDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagDesktopUI.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}