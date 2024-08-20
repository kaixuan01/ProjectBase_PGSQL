using DAL.Entity;
using DAL.Repository.UserRP.UserRepository.Class;
using DAL.Tools.ListingHelper;

namespace DAL.Repository.UserRP.UserRepository
{
    public interface IUserRepository
    {
        Task<IQueryable<UserL>> GetUserListing(UserListing_REQ oReq);
        Task<int> GetUserRoleByUsernameAsync(string username);
        Task<T_User> GetByIdAsync(string id);
        Task<T_User> GetByUsernameAsync(string username);
        Task<T_User> CreateAsync(T_User user);
        Task<string> UpdateAsync(T_User user);
        Task<string> DeleteAsync(T_User user);

    }
}
