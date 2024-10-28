using DAL.Models;
using DAL.Tools.ListingHelper;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.UserRP.UserLoginHistoryRepository
{
    public class UserLoginHistoryRepository : ListingHelper<TUserloginhistory>, IUserLoginHistoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserLoginHistoryRepository(AppDbContext context) : base(context)
        {
            _appDbContext = context;
        }

        public async Task CreateAsync(TUserloginhistory userLoginHistory)
        {
            await _appDbContext.TUserloginhistories.AddAsync(userLoginHistory);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<TUserloginhistory> GetUserLoginHistoryByUserIdAsync(string UserId)
        {
            var latestLoginHistory = await _appDbContext.TUserloginhistories
                .Where(x => x.UserId == UserId)
                .OrderByDescending(x => x.LoginDatetime)
                .FirstOrDefaultAsync();

            return latestLoginHistory;
        }

        public async Task UpdateAsync(TUserloginhistory oRec)
        {
            // Attach the user entity to the context
            _appDbContext.Attach(oRec);

            // Mark all properties as modified
            _appDbContext.Entry(oRec).State = EntityState.Modified;

            // Save changes to the database
            await _appDbContext.SaveChangesAsync();
        }

    }
}
