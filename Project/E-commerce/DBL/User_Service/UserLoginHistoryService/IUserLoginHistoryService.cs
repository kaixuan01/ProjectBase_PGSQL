﻿using DAL.Models;
using DAL.Shared.Class;
using DAL.Tools.ListingHelper;

namespace DBL.User_Service.UserLoginHistoryService
{
    public interface IUserLoginHistoryService
    {
        Task<ShareResp> CreateAsync(TUserloginhistory userLoginHistory);

        Task<PagedResult<TUserloginhistory>> GetLoginHistoryList(FilterParameters filterParameters, bool includeForeignRelationship = false);

        Task UpdateUserLogoutByUserIdAsync(string UserId);

    }
}
