using DAL.Models;
using DAL.Tools.ListingHelper;

namespace DAL.Repository.UserRP.UserLoginHistoryRepository
{
    public interface IUserLoginHistoryRepository : IListingHelper<TUserloginhistory>
    {
        Task CreateAsync(TUserloginhistory user);
        Task UpdateAsync(TUserloginhistory oRec);
        Task<TUserloginhistory> GetUserLoginHistoryByUserIdAsync(string UserId);

    }
}
