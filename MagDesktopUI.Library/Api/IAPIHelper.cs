using MagDesktopUI.Library.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagDesktopUI.Library.Api
{
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }
        // nu dorim sa avem acces la HttpClient din afara acestei interfete
        // avem nevoie doar de rezultate finale si de clientul API
        Task<AuthenticatedUser> Authenticate(string username, string password);

        Task GetLoggedInUserInfo(string token);
    }
}