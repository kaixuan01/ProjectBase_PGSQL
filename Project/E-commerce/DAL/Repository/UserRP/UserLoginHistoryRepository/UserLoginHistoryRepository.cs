using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.UserRP.UserLoginHistoryRepository
{
    public class UserLoginHistoryRepository : IUserLoginHistoryRepository
    {
        private readonly MyDbContext _myDbContext;

        public UserLoginHistoryRepository(MyDbContext context)
        {
            _myDbContext = context;
        }

        #region [ Get User ]

        public async Task<List<UserLoginHistory>> GetAllAsync()
        {
            return await _myDbContext.UserLoginHistory.ToListAsync();
        }

        #endregion

        public async Task<UserLoginHistory> CreateAsync(UserLoginHistory userLoginHistory)
        {
            await _myDbContext.UserLoginHistory.AddAsync(userLoginHistory);
            await _myDbContext.SaveChangesAsync();
            return userLoginHistory;
        }
    }
}
