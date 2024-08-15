using DAL;
using DAL.Entity;
using DAL.Repository.UserRP.UserRepository;
using DAL.Tools.ListingHelper;
using DBL.User_Service.UserLoginHistoryService;
using DBL.User_Service.UserService.VerifyUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Tools;

namespace DBL.User_Service.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserLoginHistoryService _userLoginHistoryService;

        public UserService(IUserRepository userRepository, IUserLoginHistoryService userLoginHistoryService)
        {
            _userRepository = userRepository;
            _userLoginHistoryService = userLoginHistoryService;
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            var oUserList = await _userRepository.GetAllAsync();

            return oUserList;
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<User> CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(int id, User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        #region [ Verify User ]

        public async Task<bool> VerifyUserAsync(VerifyUser_REQ user)
        {
            var rtnValue = false;

            try
            {
                var oUser = new User();

                if (!string.IsNullOrEmpty(user.username))
                {
                    oUser = await _userRepository.GetByUsernameAsync(user.username);
                }

                if (oUser != null)
                {
                    if (!string.IsNullOrEmpty(user.username) && !string.IsNullOrEmpty(oUser.Password))
                    {
                        rtnValue = PasswordHelper.VerifyPassword(user.password, oUser.Password);

                        var oLoginHistory = new UserLoginHistory()
                        {
                            Id = IdGeneratorHelper.GenerateId(),
                            UserId = oUser.Id,
                            LoginDateTime = DateTime.Now
                        };

                        if (rtnValue)
                        {
                            oLoginHistory.Remark = "Login Successfully";
                        }
                        else
                        {
                            oLoginHistory.Remark = $"Login Failed, Wrong Password.";
                        }

                        // Insert Login History
                        try
                        {
                            await _userLoginHistoryService.CreateAsync(oLoginHistory);
                        }
                        catch (Exception ex)
                        {
                            // Log
                            throw;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // TODO: Add Log
                throw;
            }

            return rtnValue;
        }

        #endregion

        public async Task<PagedResult<User>> GetPagedListAsync(FilterParameters filterParameters)
        {
            var oUserList = await _userRepository.GetPagedListAsync(filterParameters);

            return oUserList;
        }
    }
}
