using MagDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace MagDesktopUI.Library.Api
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}