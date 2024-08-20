using DAL.Entity;
using DAL.Tools.ListingHelper;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.UserRP.UserLoginHistoryRepository
{
    public class UserLoginHistoryRepository : ListingHelper<T_UserLoginHistory>, IUserLoginHistoryRepository
    {
        private readonly MyDbContext _myDbContext;

        public UserLoginHistoryRepository(MyDbContext context) : base(context)
        {
            _myDbContext = context;
        }

        public async Task CreateAsync(T_UserLoginHistory userLoginHistory)
        {
            await _myDbContext.T_UserLoginHistory.AddAsync(userLoginHistory);
            await _myDbContext.SaveChangesAsync();
        }

        public async Task<T_UserLoginHistory> GetUserLoginHistoryByUserIdAsync(string UserId)
        {
            var latestLoginHistory = await _myDbContext.T_UserLoginHistory
                .Where(x => x.UserId == UserId)
                .OrderByDescending(x => x.LoginDateTime)
                .FirstOrDefaultAsync();

            return latestLoginHistory;
        }

        public async Task UpdateAsync(T_UserLoginHistory oRec)
        {
            // Attach the user entity to the context
            _myDbContext.Attach(oRec);

            // Mark all properties as modified
            _myDbContext.Entry(oRec).State = EntityState.Modified;

            // Save changes to the database
            await _myDbContext.SaveChangesAsync();
        }

    }
}
