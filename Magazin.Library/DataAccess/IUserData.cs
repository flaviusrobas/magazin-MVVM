using Magazin.Library.Models;

namespace Magazin.Library.DataAccess
{
    public interface IUserData
    {
        List<UserModel> GetUsersById(string Id);
    }
}