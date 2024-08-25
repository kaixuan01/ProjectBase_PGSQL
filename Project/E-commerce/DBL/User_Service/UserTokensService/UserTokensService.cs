﻿using DAL.Entity;
using DAL.Repository.UserRP.UserRepository;
using DBL.AuditTrail_Service;
using DBL.Email_Service;
using DBL.SystemConfig_Service;
using DBL.User_Service.UserLoginHistoryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;
using Utils;
using Utils.Tools;
using DBL.Tools;
using DAL.Repository.UserRP.UserTokens;
using System.Linq.Dynamic.Core.Tokenizer;

namespace DBL.User_Service.UserTokensService
{
    public class UserTokensService : IUserTokensService
    {
        private readonly IAuditTrailService _auditTrailService;
        private readonly IUserRepository _userRepository;
        private readonly IUserTokensRepository _userTokensRepository;

        private readonly ISystemConfigService _systemConfigService;

        public UserTokensService(IUserRepository userRepository, IUserTokensRepository userTokensRepository, IAuditTrailService auditTrailService, ISystemConfigService systemConfigService)
        {
            _auditTrailService = auditTrailService;
            _userRepository = userRepository;
            _userTokensRepository = userTokensRepository;
            _systemConfigService = systemConfigService;
        }

        public async Task<T_UserTokens> CreateAsync(string UserId, string TokenType)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(TokenType))
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Warning, $"Invalid parameters: UserId or TokenType is null or empty.");
                throw new ArgumentException("UserId and TokenType cannot be null or empty.");
            }

            try
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Receive Request to Generate Token. User Id: {UserId}, Token Type: {TokenType}");
                var oUserTokenExpiration = await _systemConfigService.GetSystemConfigByKeyAsync(ConstantCode.SystemConfig_Key.UserTokenExpiration);

                var newToken = new T_UserTokens
                {
                    Id = IdGeneratorHelper.GenerateId(),
                    UserId = UserId,
                    Token = AuthToken.GenerateToken(),
                    TokenType = TokenType,
                    CreatedDateTime = DateTime.Now,
                    ExpiresDateTime = DateTime.Now.AddHours(1), // By Default set it 1 hour
                    IsUsed = false
                };

                if (oUserTokenExpiration != null)
                {
                    // Assign the expiresDateTime base on System Config
                    if (int.TryParse(oUserTokenExpiration.Value, out int expirationHours))
                    {
                        if (expirationHours > 0) // Expiration hours cant be 0
                        {
                            newToken.ExpiresDateTime = DateTime.Now.AddHours(expirationHours);
                        }
                    }
                }

                // ## Create User Record
                await _userTokensRepository.CreateAsync(newToken);

                // Insert Audit Trail Record
                await _auditTrailService.CreateAuditTrailAsync(ConstantCode.Module.UserTokens, ConstantCode.Action.Create, null, newToken);

                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"{RespCode.RespMessage_Insert_Successful}. User Id: {UserId}");

                return newToken;
            }
            catch (Exception ex)
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Exception when insert user's token. User Id: {UserId}, Token Type: {TokenType}", ex);
                throw;
            }
        }

        public async Task<T_UserTokens> GetByTokenAsync(string token)
        {
            return await _userTokensRepository.GetByTokenAsync(token);
        }

        public async Task UpdateAsync(T_UserTokens userTokens)
        {
            await _userTokensRepository.UpdateAsync(userTokens);
        }
    }
}