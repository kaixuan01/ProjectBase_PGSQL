﻿using DAL.Entity;
using DAL.Repository.UserRP.UserRole;
using DBL.Tools;
using Newtonsoft.Json;
using Utils.Enums;

namespace DBL.User_Service.UserRoleService
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<List<E_UserRole>> GetUserRoleListingAsync()
        {
            var result = await _userRoleRepository.GetUserRoleListingAsync();

            LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Response User Role List: {JsonConvert.SerializeObject(result)}");

            return result;
        }
    }
}