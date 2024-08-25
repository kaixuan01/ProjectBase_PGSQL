﻿using DAL.Entity;
using DAL.Repository.UserRP.UserRepository;
using DAL.Repository.UserRP.UserRepository.Class;
using DAL.Shared.Class;
using DBL.AuditTrail_Service;
using DBL.Email_Service;
using DBL.SystemConfig_Service;
using DBL.Tools;
using DBL.User_Service.UserLoginHistoryService;
using DBL.User_Service.UserService.UserActionClass;
using DBL.User_Service.UserTokensService;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Utils;
using Utils.Enums;
using Utils.Tools;

namespace DBL.User_Service.UserService
{
    public class UserService : IUserService
    {
        private readonly IAuditTrailService _auditTrailService;
        private readonly IUserRepository _userRepository;
        private readonly IUserTokensService _userTokenService;
        private readonly IUserLoginHistoryService _userLoginHistoryService;
        private readonly ISystemConfigService _systemConfigService;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IUserLoginHistoryService userLoginHistoryService, IAuditTrailService auditTrailService,
            ISystemConfigService systemConfigService, IEmailService emailService, IUserTokensService userTokensService)
        {
            _auditTrailService = auditTrailService;
            _userRepository = userRepository;
            _userLoginHistoryService = userLoginHistoryService;
            _systemConfigService = systemConfigService;
            _emailService = emailService;
            _userTokenService = userTokensService;
        }

        #region [ Get User ]

        public async Task<UserListing_RESP> GetUserListingAsync(UserListing_REQ oReq)
        {
            var rtnValue = new UserListing_RESP();

            try
            {
                var userQuery = await _userRepository.GetUserListing(oReq);

                // Apply sorting
                if (!string.IsNullOrEmpty(oReq.SortBy))
                {
                    userQuery = oReq.SortDescending == true
                        ? userQuery.OrderByDescending(e => EF.Property<object>(e, oReq.SortBy))
                        : userQuery.OrderBy(e => EF.Property<object>(e, oReq.SortBy));
                }
                else
                {
                    userQuery = userQuery.OrderBy(u => u.Username); // Default sorting
                }

                rtnValue.UserList.TotalCount = userQuery.Count();

                // Apply pagination
                var pagedUsers = await userQuery
                    .Skip((oReq.PageNumber - 1) * oReq.PageSize)
                    .Take(oReq.PageSize)
                    .ToListAsync();

                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Response User List: {JsonConvert.SerializeObject(pagedUsers)}");

                rtnValue.UserList.Items = pagedUsers;
                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = "Get User List succesful";
            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ErrorMessage.GeneralError;
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Exception Message: {ex.Message}");
            }

            return rtnValue;
        }

        public async Task<T_User> GetByIdAsync(string id)
        {
            var oUser = await _userRepository.GetByIdAsync(id);

            LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Response User: {JsonConvert.SerializeObject(oUser)}");

            return oUser;
        }

        #endregion

        #region [ Get User's Role ]

        public async Task<int> GetUserRoleByUsernameAsync(string username)
        {
            return await _userRepository.GetUserRoleByUsernameAsync(username);
        }

        #endregion

        #region [ Create User ]

        public async Task<CreateUser_RESP> CreateAsync(CreateUser_REQ oUser)
        {
            var rtnValue = new CreateUser_RESP();

            if (oUser == null)
            {
                rtnValue.Code = RespCode.RespCode_Failed;
                rtnValue.Message = ErrorMessage.MissingRequiredField;

                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"oUser is null");
                return rtnValue;
            }

            try
            {
                if (string.IsNullOrEmpty(oUser.name) || string.IsNullOrEmpty(oUser.username) || string.IsNullOrEmpty(oUser.email) || string.IsNullOrEmpty(oUser.password) || oUser.userRoleId == null)
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = ErrorMessage.MissingRequiredField;
                    return rtnValue;
                }

                if (!RegexHelper.IsEmailValid(oUser.email))
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "Invalid email format. Please enter a valid email.";
                    return rtnValue;
                }

                // Check is username exist
                bool isUsernameExist = await _userRepository.IsUsernameExistAsync(oUser.username);
                if (isUsernameExist)
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "Username already exists. Please try another username.";
                    return rtnValue;
                }
                // Check is email exist
                bool isEmailExist = await _userRepository.IsEmailExistAsync(oUser.email);
                if (isEmailExist)
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "Email already exists. Please try another email.";
                    return rtnValue;
                }

                var createUser = new T_User
                {
                    Id = IdGeneratorHelper.GenerateId(),
                    Username = oUser.username,
                    Password = oUser.password,
                    Name = oUser.name,
                    Email = oUser.email,
                    Phone = oUser.phone,
                    UserRoleId = oUser.userRoleId
                };

                // ## Create User Record
                await _userRepository.CreateAsync(createUser);

                // Insert Audit Trail Record
                await _auditTrailService.CreateAuditTrailAsync(ConstantCode.Module.User, ConstantCode.Action.Create, null, createUser);

                // ## Send Email
                await _emailService.SendConfirmEmailAsync(createUser);

                rtnValue.UserId = createUser.Id;
                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = RespCode.RespMessage_Insert_Successful;

                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"{RespCode.RespMessage_Insert_Successful}. user id: {rtnValue.UserId}");
            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ErrorMessage.GeneralError;
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Exception Message: {ex.Message}");
            }

            return rtnValue;
        }

        #endregion

        #region [ Update User ]

        public async Task<ShareResp> UpdateAsync(EditUser_REQ oReq)
        {
            var rtnValue = new ShareResp();

            if (oReq == null)
            {
                rtnValue.Code = RespCode.RespCode_Failed;
                rtnValue.Message = ErrorMessage.MissingRequiredField;
                return rtnValue;
            }

            try
            {
                if (string.IsNullOrEmpty(oReq.id) || string.IsNullOrEmpty(oReq.name) || string.IsNullOrEmpty(oReq.email) || oReq.userRoleId == null)
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = ErrorMessage.MissingRequiredField;
                    return rtnValue;
                }

                var oUser = await _userRepository.GetByIdAsync(oReq.id);

                if (oUser == null)
                {
                    LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"User not found. user id: {oReq.id}");

                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "User not found.";
                    return rtnValue;
                }

                // Check is email exist
                bool isEmailExist = await _userRepository.IsEmailExistAsync(oReq.email, oUser.Id);
                if (isEmailExist)
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "Email already exists. Please try another email.";
                    return rtnValue;
                }

                // Used to create audit trail record
                var copyUser = oUser.Clone();

                if (!string.IsNullOrEmpty(oReq.password))
                {
                    oUser.Password = oReq.password;
                }

                oUser.Name = oReq.name;
                oUser.Email = oReq.email;
                oUser.Phone = oReq.phone;
                oUser.UserRoleId = oReq.userRoleId;

                await _userRepository.UpdateAsync(oUser);

                await _auditTrailService.CreateAuditTrailAsync(ConstantCode.Module.User, ConstantCode.Action.Edit, copyUser, oUser);

                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = RespCode.RespMessage_Update_Successful;
                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"{RespCode.RespMessage_Update_Successful}. User Id: {oReq.id}");
            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ErrorMessage.GeneralError;
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Exception Message: {ex.Message}");
            }

            return rtnValue;
        }

        #endregion

        #region [ Update User Logout ]

        public async Task UpdateUserLogoutAsync(string username)
        {
            try
            {

                var oUser = await _userRepository.GetByUsernameAsync(username);

                if (oUser == null)
                {
                    LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"User not found. Username: {username}");

                    return;
                }
                // Used to create audit trail record

                await _userLoginHistoryService.UpdateUserLogoutByUserIdAsync(oUser.Id);

                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"{RespCode.RespMessage_Update_Successful}. User Name: {username}");
            }
            catch (Exception ex)
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Exception Message: {ex.Message}");
            }
        }

        #endregion

        #region [ Delete User ]

        public async Task<ShareResp> DeleteAsync(string id)
        {
            var rtnValue = new ShareResp();

            if (string.IsNullOrEmpty(id))
            {
                rtnValue.Code = RespCode.RespCode_Failed;
                rtnValue.Message = ErrorMessage.GeneralError;
                return rtnValue;
            }

            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "User not found";

                    LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"User not found, User Id: {id}");
                    return rtnValue;
                }

                await _userRepository.DeleteAsync(user);

                await _auditTrailService.CreateAuditTrailAsync(ConstantCode.Module.User, ConstantCode.Action.Delete, user, null);

                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = RespCode.RespMessage_Delete_Successful;
                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"{RespCode.RespMessage_Delete_Successful}. User Id: {id}");

            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ErrorMessage.GeneralError;

                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Exception Message: {ex.Message}");
            }

            return rtnValue;
        }

        #endregion

        #region [ Block / Unblock User ]

        /// <summary>
        /// Block or Unblock user
        /// If user status is block, will unblock it
        /// if user status is active, will block it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ShareResp> SetUserStatusAsync(string id)
        {
            var rtnValue = new ShareResp();

            if (string.IsNullOrEmpty(id))
            {
                rtnValue.Code = RespCode.RespCode_Failed;
                rtnValue.Message = ErrorMessage.GeneralError;
                return rtnValue;
            }

            try
            {
                var oUser = await _userRepository.GetByIdAsync(id);

                if (oUser == null)
                {
                    LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"User not found. user id: {id}");

                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "User not found.";
                    return rtnValue;
                }
                // Used to create audit trail record
                var copyUser = oUser.Clone();

                // Reverse the user status
                // If the user is currently blocked, set them to active
                // If the user is currently active, set them to blocked
                oUser.IsBlocked = !oUser.IsBlocked;

                await _userRepository.UpdateAsync(oUser);

                await _auditTrailService.CreateAuditTrailAsync(ConstantCode.Module.User, ConstantCode.Action.Edit, copyUser, oUser);

                var userStatus = (oUser.IsBlocked ? ConstantCode.UserStatus.Blocked : ConstantCode.UserStatus.Active);

                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = $"User's status updated to {userStatus}.";
                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"User's status updated to {userStatus}. User Id: {id}, User Name: {oUser.Name}");
            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ErrorMessage.GeneralError;

                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Exception Message: {ex.Message}");
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
                var oUser = new T_User();

                if (!string.IsNullOrEmpty(user.username) && !string.IsNullOrEmpty(user.password))
                {
                    oUser = await _userRepository.GetByUsernameAsync(user.username);
                }
                else
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = ErrorMessage.MissingRequiredField;

                    return rtnValue;
                }

                if (oUser != null)
                {
                    var oSystemConfig = await _systemConfigService.GetSystemConfigList();
                    var maxLoginFailedAttempt = oSystemConfig.FirstOrDefault(i => i.Key == ConstantCode.SystemConfig_Key.MaxLoginFailedAttempt);
                    int maxAttempts = 0;

                    if (maxLoginFailedAttempt != null && int.TryParse(maxLoginFailedAttempt.Value, out maxAttempts))
                    {
                        // Disable the check if max attemps == 0
                        if (maxAttempts != 0)
                        {
                            if (oUser.iCountFailedLogin >= maxAttempts)
                            {
                                rtnValue.Code = RespCode.RespCode_Failed;
                                rtnValue.Message = "You have exceeded the maximum number of login attempts. Please contact the admin for further assistance.";

                                return rtnValue;
                            }
                        }
                    }

                    if (!oUser.IsEmailVerified)
                    {
                        rtnValue.Code = RespCode.RespCode_Failed;
                        rtnValue.Message = "Your account has not been verified yet. Please check your email for the confirmation link. If you did not receive the email, you can request a new confirmation email.";

                        return rtnValue;
                    }

                    if (oUser.IsBlocked)
                    {
                        rtnValue.Code = RespCode.RespCode_Failed;
                        rtnValue.Message = "Your account has been blocked. Please contact the admin for assistance.";

                        return rtnValue;
                    }

                    if (string.IsNullOrEmpty(oUser.Password))
                    {
                        rtnValue.Code = RespCode.RespCode_Exception;
                        rtnValue.Message = ErrorMessage.ProcessingError;
                        LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"User Password not found. User Id: {oUser.Id}, User Name: {oUser.Name}");
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

                        if (maxAttempts != 0)
                        {
                            if (oUser.iCountFailedLogin >= maxAttempts)
                            {
                                // Block the user if failed more than the system config setting
                                oUser.IsBlocked = true;
                            }
                        }

                        rtnValue.Code = RespCode.RespCode_Failed;
                        rtnValue.Message = ErrorMessage.AuthenticationFailed;
                    }

                    // Insert Login History
                    await _userLoginHistoryService.CreateAsync(oLoginHistory);

                    // Update User's login failed Count
                    await _userRepository.UpdateAsync(oUser);

                    rtnValue.UserRoleId = oUser.UserRoleId;
                }
                else
                {
                    rtnValue.Code = RespCode.RespCode_Exception;
                    rtnValue.Message = ErrorMessage.AuthenticationFailed;

                    LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"User not found. User Name: {user.username}");
                }

            }
            catch (Exception ex)
            {
                // TODO: Add Log
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ErrorMessage.GeneralError;

                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Exception Message: {ex.Message}");

                throw;
            }

            return rtnValue;
        }

        #endregion

        #region [ User Confirm Email Process ]

        public async Task<ShareResp> UpdateUserConfirmEmailAsync(string token)
        {
            var rtnValue = new ShareResp();

            if (string.IsNullOrEmpty(token))
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, "Error occurred during the email confirmation process: No token found in the request.");

                rtnValue.Code = RespCode.RespCode_Failed;
                rtnValue.Message = ErrorMessage.ProcessingError;
                return rtnValue;
            }

            try
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Receive Request to Confirm Email. Token: {token}");
                var oUserToken = await _userTokenService.GetByTokenAsync(token);

                if (oUserToken == null)
                {
                    LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"User Token not found. Token: {token}");

                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "Invalid or expired confirm email link.";
                    return rtnValue;
                }

                var oUser = await _userRepository.GetByIdAsync(oUserToken.UserId);

                if (oUser == null)
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = ErrorMessage.ProcessingError;
                    LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"User not found. User Id: {oUserToken.UserId}");
                    return rtnValue;
                }

                if (oUser.IsEmailVerified)
                {
                    rtnValue.Code = RespCode.RespCode_Success;
                    rtnValue.Message = "Your email was confirmed previously. Please proceed to log in.";
                    LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"User retry to confirm email. User Id: {oUser.Id}");
                    return rtnValue;
                }

                if (oUserToken.IsUsed || oUserToken.ExpiresDateTime < DateTime.Now || oUserToken.TokenType != ConstantCode.UserTokenType.EmailConfirmation)
                {
                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = "Invalid or expired confirm email link.";
                    return rtnValue;
                }

                // Used to create audit trail record
                var copyUser = oUser.Clone();

                // ## Update User's Token to used
                oUserToken.IsUsed = true;
                await _userTokenService.UpdateAsync(oUserToken);

                // ## Update User Email Verified
                oUser.IsEmailVerified = true;
                await _userRepository.UpdateAsync(oUser);

                // ## Insert Audit Trail for User changes
                await _auditTrailService.CreateAuditTrailAsync(ConstantCode.Module.User, ConstantCode.Action.Edit, copyUser, oUser);

                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = "Your email address has been successfully confirmed. You can now log in to your account.";
                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"{RespCode.RespMessage_Update_Successful}. User Id: {oUser.Id}");
            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ErrorMessage.GeneralError;
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, "Error occurred during the email confirmation process.", ex);
            }

            return rtnValue;
        }

        #endregion

        #region [ Resend Confirm Email ]

        public async Task<ShareResp> ResendConfirmEmailAsync(ResendConfirmEmail_REQ oReq)
        {
            var rtnValue = new ShareResp();

            if (string.IsNullOrEmpty(oReq.Token) && string.IsNullOrEmpty(oReq.Username))
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, "Resend email request failed: Both token and username are missing.");

                rtnValue.Code = RespCode.RespCode_Failed;
                rtnValue.Message = ErrorMessage.ProcessingError;
                return rtnValue;
            }

            try
            {
                T_User oUser = null;
                T_UserTokens oUserTokens = null;

                if (!string.IsNullOrEmpty(oReq.Username))
                {
                    LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Received request to resend confirmation email. Username: {oReq.Username}");
                    oUser = await _userRepository.GetByUsernameAsync(oReq.Username);
                    oUserTokens = await _userTokenService.GetByUserIdAsync(oUser.Id);
                }
                else if (!string.IsNullOrEmpty(oReq.Token))
                {
                    LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Received request to resend confirmation email. Token: {oReq.Token}");
                    oUserTokens = await _userTokenService.GetByTokenAsync(oReq.Token);
                    oUser = await _userRepository.GetByIdAsync(oUserTokens.UserId);
                }

                if (oUserTokens == null)
                {
                    LogHelper.RaiseLogEvent(Enum_LogLevel.Error, "Resend email request failed: Token not found.");

                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = ErrorMessage.ProcessingError;
                    return rtnValue;
                }

                if (oUser == null)
                {
                    LogHelper.RaiseLogEvent(Enum_LogLevel.Error, "Resend email request failed: User not found.");

                    rtnValue.Code = RespCode.RespCode_Failed;
                    rtnValue.Message = ErrorMessage.ProcessingError;
                    return rtnValue;
                }

                if (oUser.IsEmailVerified)
                {
                    rtnValue.Code = RespCode.RespCode_Success;
                    rtnValue.Message = "Your email has already been confirmed. Please proceed to log in.";
                    LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"User attempted to resend confirmation email. User Id: {oUser.Id}");
                    return rtnValue;
                }

                // Update previous token to IsUsed.
                oUserTokens.IsUsed = true;
                await _userTokenService.UpdateAsync(oUserTokens);

                // ## Send Email
                await _emailService.SendConfirmEmailAsync(oUser);

                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = "Email sent successfully. Please check your email.";
                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Confirmation email resent successfully. User Id: {oUser.Id}");
            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ErrorMessage.GeneralError;
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, "Error occurred during the email confirmation process.", ex);
            }

            return rtnValue;
        }

        #endregion
    }
}
