﻿using DAL.Entity;
using DAL.Repository.UserRP.UserRepository;
using DAL.Tools.ListingHelper;
using DBL.AuditTrail_Service;
using DBL.Tools;
using DBL.User_Service.UserLoginHistoryService;
using DBL.User_Service.UserService.UserActionClass;
using Microsoft.AspNetCore.Components.Routing;
using Newtonsoft.Json;
using Utils;
using Utils.Enums;
using Utils.Tools;

namespace DBL.User_Service.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserLoginHistoryService _userLoginHistoryService;
        private readonly IAuditTrailService _auditTrailService;

        public UserService(IUserRepository userRepository, IUserLoginHistoryService userLoginHistoryService, IAuditTrailService auditTrailService)
        {
            _userRepository = userRepository;
            _userLoginHistoryService = userLoginHistoryService;
            _auditTrailService = auditTrailService;
        }

        #region [ Get T_User ]

        public async Task<PagedResult<dynamic>> GetPagedListAsync(FilterParameters filterParameters)
        {
            var oUserList = await _userRepository.GetPagedListDynamicAsync(filterParameters, true, "Password");

            LogHelper.RaiseLogEvent(LogLevelEnums.Information, $"Response User List: {JsonConvert.SerializeObject(oUserList)}");

            return oUserList;
        }

        public async Task<T_User> GetByIdAsync(string id)
        {
            var oUserList = await _userRepository.GetByIdAsync(id);

            LogHelper.RaiseLogEvent(LogLevelEnums.Information, $"Response User List: {JsonConvert.SerializeObject(oUserList)}");

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

                LogHelper.RaiseLogEvent(LogLevelEnums.Error, $"oUser is null");
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

                var createUser = new T_User
                {
                    Id = IdGeneratorHelper.GenerateId(),
                    UserName = oUser.username,
                    Password = oUser.password,
                    Name = oUser.name,
                    Email = oUser.email,
                    Phone = oUser.phone,
                    Address = oUser.address
                };

                await _userRepository.CreateAsync(createUser);

                await _auditTrailService.CreateAuditTrailAsync(ConstantCode.Module.User, ConstantCode.Action.Create, null, createUser);

                rtnValue.UserId = createUser.Id;
                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = RespCode.RespMessage_Insert_Successfully;

                LogHelper.RaiseLogEvent(LogLevelEnums.Information, $"{RespCode.RespMessage_Insert_Successfully}. user id: {rtnValue.UserId}");
            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ex.Message;
                LogHelper.RaiseLogEvent(LogLevelEnums.Error, $"Exception Message: {ex.Message}");
            }

            return rtnValue;
        }

        #endregion

        #region [ Update T_User ]

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
                    LogHelper.RaiseLogEvent(LogLevelEnums.Error, $"User not found. user id: {oReq.id}");

                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "User not found.";
                    return rtnValue;
                }
                var copyUser = oUser.Clone();

                if (!string.IsNullOrEmpty(oReq.password))
                {
                    oUser.Password = oReq.password;
                }

                oUser.Name = oReq.name;
                oUser.Email = oReq.email;
                oUser.Address = oReq.address;
                oUser.Phone = oReq.phone;
                oUser.UserRoleId = oReq.userRoleId;

                await _userRepository.UpdateAsync(oUser);

                await _auditTrailService.CreateAuditTrailAsync(ConstantCode.Module.User, ConstantCode.Action.Edit, copyUser, oUser);

                rtnValue.UserId = oUser.Id;
                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = RespCode.RespMessage_Update_Successfully;
                LogHelper.RaiseLogEvent(LogLevelEnums.Information, $"{RespCode.RespMessage_Update_Successfully}. User Id: {rtnValue.UserId}");
            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ex.Message;
                LogHelper.RaiseLogEvent(LogLevelEnums.Error, $"Exception Message: {ex.Message}");
            }

            return rtnValue;
        }


        #endregion

        #region [ Delete T_User ]

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

                    LogHelper.RaiseLogEvent(LogLevelEnums.Error, $"User not found, User Id: {id}");
                    return rtnValue;
                }

                await _userRepository.DeleteAsync(user);

                await _auditTrailService.CreateAuditTrailAsync(ConstantCode.Module.User, ConstantCode.Action.Delete, user, null);

                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = RespCode.RespMessage_Delete_Successfully;
                LogHelper.RaiseLogEvent(LogLevelEnums.Information, $"{RespCode.RespMessage_Delete_Successfully}. User Id: {id}");

            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ex.Message;

                LogHelper.RaiseLogEvent(LogLevelEnums.Error, $"Exception Message: {ex.Message}");
            }

            return rtnValue;
        }

        #endregion

        #region [ Verify T_User (Login) ]

        public async Task<VerifyUser_RESP> VerifyUserAsync(VerifyUser_REQ user)
        {
            var rtnValue = new VerifyUser_RESP();

            try
            {
                var oUser = new T_User();

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

                        LogHelper.RaiseLogEvent(LogLevelEnums.Error, $"User have been block. User Name: {oUser.Name}");

                        return rtnValue;
                    }

                    if (string.IsNullOrEmpty(oUser.Password))
                    {
                        rtnValue.Code = RespCode.RespCode_Exception;
                        rtnValue.Message = "Something error, Please contact admin.";
                        LogHelper.RaiseLogEvent(LogLevelEnums.Error, $"User Password not found. User Id: {oUser.Id}, User Name: {oUser.Name}");
                        return rtnValue;
                    }

                    bool success = PasswordHelper.VerifyPassword(user.password, oUser.Password);

                    var oLoginHistory = new T_UserLoginHistory()
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

                    // Insert Login History
                    await _userLoginHistoryService.CreateAsync(oLoginHistory);

                    // Update User's login failed Count
                    await _userRepository.UpdateAsync(oUser);
                }
                else
                {
                    rtnValue.Code = RespCode.RespCode_Exception;
                    rtnValue.Message = $"Username / Password incorrect. Please try again.";

                    LogHelper.RaiseLogEvent(LogLevelEnums.Information, $"User not found. User Name: {user.username}");
                }

            }
            catch (Exception ex)
            {
                // TODO: Add Log
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = $"Throw Exception when verify user. Exception: {ex}";

                LogHelper.RaiseLogEvent(LogLevelEnums.Error, $"Exception Message: {ex.Message}");

                throw;
            }

            return rtnValue;
        }

        #endregion

    }
}
