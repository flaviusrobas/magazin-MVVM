using Magazin.Library.Models;

namespace Magazin.Library.DataAccess
{
    public interface IUserData
    {
        void CreateUser(UserModel user);
        List<UserModel> GetUsersById(string Id);
    }
}