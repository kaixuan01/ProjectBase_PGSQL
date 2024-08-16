using DAL.Entity;
using DAL.Repository.UserRP.UserRepository;
using DAL.Tools.ListingHelper;
using DBL.User_Service.UserLoginHistoryService;
using DBL.User_Service.UserService.UserActionClass;
using Utils;
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

        #region [ Get User ]

        public async Task<PagedResult<dynamic>> GetPagedListAsync(FilterParameters filterParameters)
        {
            var oUserList = await _userRepository.GetPagedListDynamicAsync(filterParameters, false, "Password");

            return oUserList;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var oUserList = await _userRepository.GetByIdAsync(id);

            return oUserList;
        }

        #endregion

        #region [ Create User ]

        public async Task<CreateUser_RESP> CreateAsync(CreateUser_REQ oUser)
        {
            var rtnValue = new CreateUser_RESP();

            if (oUser == null)
            {
                rtnValue.Code = RespCode.RespCode_Failed;
                rtnValue.Message = "Please enter user's details";
                return rtnValue;
            }

            try
            {
                if (string.IsNullOrEmpty(oUser.name) || string.IsNullOrEmpty(oUser.username) || string.IsNullOrEmpty(oUser.email) || string.IsNullOrEmpty(oUser.password) || oUser.userRoleId == null)
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "Please complete the user's details";
                    return rtnValue;
                }

                var createUser = new User
                {
                    Id = IdGeneratorHelper.GenerateId(),
                    UserName = oUser.username,
                    Password = PasswordHelper.HashPassword(oUser.password),
                    Name = oUser.name,
                    Email = oUser.email,
                    Phone = oUser.phone,
                    Address = oUser.address
                };

                await _userRepository.CreateAsync(createUser);

                rtnValue.UserId = createUser.Id;
                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = RespCode.RespMessage_Insert_Successfully;
            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ex.Message;
            }

            return rtnValue;
        }

        #endregion

        #region [ Update User ]

        public async Task<EditUser_RESP> UpdateAsync(EditUser_REQ oReq)
        {
            var rtnValue = new EditUser_RESP();

            if (oReq == null)
            {
                rtnValue.Code = RespCode.RespCode_Failed;
                rtnValue.Message = "Please enter user's details";
                return rtnValue;
            }

            try
            {
                if (string.IsNullOrEmpty(oReq.id) || string.IsNullOrEmpty(oReq.name) || string.IsNullOrEmpty(oReq.email) || oReq.userRoleId == null)
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "Please complete the user's details";
                    return rtnValue;
                }

                var oUser = await _userRepository.GetByIdAsync(oReq.id);

                if (oUser == null)
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "User not found.";
                    return rtnValue;
                }

                if (!string.IsNullOrEmpty(oReq.password))
                {
                    oUser.Password = PasswordHelper.HashPassword(oReq.password);
                }

                oUser.Name = oReq.name;
                oUser.Email = oReq.email;
                oUser.Address = oReq.address;
                oUser.Phone = oReq.phone;
                oUser.UserRoleId = oReq.userRoleId;

                await _userRepository.UpdateAsync(oUser);

                rtnValue.UserId = oUser.Id;
                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = RespCode.RespMessage_Update_Successfully;
            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ex.Message;
            }

            return rtnValue;
        }


        #endregion

        #region [ Delete User ]

        public async Task<DeleteUser_RESP> DeleteAsync(string id)
        {
            var rtnValue = new DeleteUser_RESP();

            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "User not found";
                    return rtnValue;
                }

                await _userRepository.DeleteAsync(user);

                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = RespCode.RespMessage_Delete_Successfully;
            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ex.Message;
            }

            return rtnValue;
        }

        #endregion

        #region [ Verify User (Login) ]

        public async Task<VerifyUser_RESP> VerifyUserAsync(VerifyUser_REQ user)
        {
            var rtnValue = new VerifyUser_RESP();

            try
            {
                var oUser = new User();

                if (!string.IsNullOrEmpty(user.username))
                {
                    oUser = await _userRepository.GetByUsernameAsync(user.username);
                }

                if (oUser != null)
                {
                    if (oUser.iCountFailedLogin > 2)
                    {
                        rtnValue.Code = RespCode.RespCode_Failed;
                        rtnValue.Message = "You have exceeded the maximum number of login attempts. Please contact admin to proceed.";
                        return rtnValue;
                    }

                    if (string.IsNullOrEmpty(oUser.Password))
                    {
                        rtnValue.Code = RespCode.RespCode_Exception;
                        rtnValue.Message = "Something error, Please contact admin.";
                        return rtnValue;
                    }

                    bool success = PasswordHelper.VerifyPassword(user.password, oUser.Password);

                    var oLoginHistory = new UserLoginHistory()
                    {
                        Id = IdGeneratorHelper.GenerateId(),
                        UserId = oUser.Id,
                        LoginDateTime = DateTime.Now
                    };

                    if (success)
                    {
                        oLoginHistory.Remark = "Login Successfully";
                        oUser.iCountFailedLogin = 0;

                        rtnValue.Code = RespCode.RespCode_Success;
                        rtnValue.Message = $"Login Successfully";
                    }
                    else
                    {
                        oLoginHistory.Remark = $"Login Failed, Wrong Password.";
                        oUser.iCountFailedLogin++;

                        rtnValue.Code = RespCode.RespCode_Failed;
                        rtnValue.Message = $"Username / Password incorrect. Please try again.";
                    }

                    try
                    {
                        // Insert Login History
                        await _userLoginHistoryService.CreateAsync(oLoginHistory);

                        // Update User Count
                        await _userRepository.UpdateAsync(oUser);
                    }
                    catch (Exception ex)
                    {
                        rtnValue.Code = RespCode.RespCode_Exception;
                        rtnValue.Message = $"Throw Exception when Insert / Update Record. Exception: {ex}";
                        throw;
                    }
                }
                else
                {
                    rtnValue.Code = RespCode.RespCode_Exception;
                    rtnValue.Message = $"Username / Password incorrect. Please try again.";
                }

            }
            catch (Exception ex)
            {
                // TODO: Add Log
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = $"Throw Exception when verify user. Exception: {ex}";
                throw;
            }

            return rtnValue;
        }

        #endregion

    }
}
