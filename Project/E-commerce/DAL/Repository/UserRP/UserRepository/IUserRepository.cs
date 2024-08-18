using DAL.Entity;
using DAL.Tools.ListingHelper;

namespace DAL.Repository.UserRP.UserRepository
{
    public interface IUserRepository : IListingHelper<T_User>
    {
        Task<int> GetUserRoleByUsernameAsync(string username);
        Task<T_User> GetByIdAsync(string id);
        Task<T_User> GetByUsernameAsync(string username);
        Task<T_User> CreateAsync(T_User user);
        Task<string> UpdateAsync(T_User user);
        Task<string> DeleteAsync(T_User user);

    }
}
