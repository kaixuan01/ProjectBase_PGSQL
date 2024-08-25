using DAL.Entity;
using DAL.Repository.UserRP.UserRepository.Class;
using DAL.Tools.ListingHelper;

namespace DAL.Repository.UserRP.UserRepository
{
    public interface IUserRepository
    {
        Task<IQueryable<UserL>> GetUserListing(UserListing_REQ oReq);
        Task<T_User> GetByIdAsync(string id);
        Task<T_User> GetByUsernameAsync(string username);
        Task<int> GetUserRoleByUsernameAsync(string username);
        Task CreateAsync(T_User user);
        Task UpdateAsync(T_User user);
        Task DeleteAsync(T_User user);
        Task<bool> IsUsernameExistAsync(string username);
        Task<bool> IsEmailExistAsync(string email, string? userId = null);

    }
}
