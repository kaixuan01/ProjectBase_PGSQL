using DAL.Entity;

namespace DAL.Repository.UserRP.UserLoginHistoryRepository
{
    public interface IUserLoginHistoryRepository
    {
        Task<List<UserLoginHistory>> GetAllAsync();
        Task<UserLoginHistory> CreateAsync(UserLoginHistory user);
    }
}
