using DAL.Entity;
using DAL.Tools.ListingHelper;

namespace DAL.Repository.UserRP.UserRepository
{
    public interface IUserRepository : IListingHelper<User>
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
        Task<string> UpdateAsync(User user);
        Task<string> DeleteAsync(User user);

    }
}
