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

        #region [ Get T_User ]

        public async Task<List<T_UserLoginHistory>> GetAllAsync()
        {
            return await _myDbContext.T_UserLoginHistory.ToListAsync();
        }

        #endregion

        public async Task CreateAsync(T_UserLoginHistory userLoginHistory)
        {
            await _myDbContext.T_UserLoginHistory.AddAsync(userLoginHistory);
            await _myDbContext.SaveChangesAsync();
        }
    }
}
