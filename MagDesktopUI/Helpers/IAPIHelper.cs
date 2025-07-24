using MagDesktopUI.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        //HttpClient ApiClient { get; set; }
        // nu dorim sa avem acces la HttpClient din afara acestei interfete
        // avem nevoie doar de rezultate finale si de clientul API
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}