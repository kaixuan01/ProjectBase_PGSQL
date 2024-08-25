﻿using DAL.Entity;
using DAL.Repository.EmailRP;
using DBL.SystemConfig_Service;
using DBL.Tools;
using DBL.User_Service.UserTokensService;
using Microsoft.Extensions.Configuration;
using Utils;
using Utils.Enums;
using Utils.Tools;

namespace DBL.Email_Service
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly ISystemConfigService _systemConfigService; 
        private readonly IUserTokensService _userTokenService;
        private readonly string _reactBaseUrl;
        private readonly EncryptionHelper _encryptionHelper;

        public EmailService(IEmailRepository emailRepository, IConfiguration configuration, EncryptionHelper encryptionHelper, ISystemConfigService systemConfigService, IUserTokensService userTokensService) {
        
            _emailRepository = emailRepository;
            _systemConfigService = systemConfigService;
            _encryptionHelper = encryptionHelper;
            _userTokenService = userTokensService;

            // Get the secret key from appsettings.json
            _reactBaseUrl = configuration["ReactSettings:BaseUrl"];
        }

        #region [ Get Email List ]

        public async Task<List<T_Email>> GetSendEmailListAsync()
        {
            var oSystemConfig = await _systemConfigService.GetSystemConfigList();
            int oRetryAttempt = 3;
            if (oSystemConfig.Count > 0)
            {
                var oRetryConfig = oSystemConfig.FirstOrDefault(i => i.Key == ConstantCode.SystemConfig_Key.SendEmailTotalRetry_Background);
                if (int.TryParse(oRetryConfig?.Value, out int total))
                {
                    oRetryAttempt = total;
                }
            }

            var result = await _emailRepository.GetSendEmailListAsync(oRetryAttempt);
            return result;
        }

        #endregion

        #region [ Update Email ]

        public async Task UpdateEmailAsync(T_Email email)
        {
            try
            {
                if (email == null || string.IsNullOrEmpty(email.Id))
                {
                    LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Email not found");

                    return;
                }

                await _emailRepository.UpdateAsync(email);

                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Update Email Successful. Email Id: {email.Id}, Status: {ConstantCode.Status.StatusDictionary[email.Status]}");

            }
            catch (Exception ex)
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Update Email Failed. Email Id: {email.Id}, Exception: {ex.Message}", ex);
            }

        }

        #endregion

        #region [ Send Email ]

        public async Task SendConfirmEmailAsync(T_User oUser)
        {
            if (oUser == null)
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Send Confirm Email failed. User not found");
                throw new Exception();
            }

            LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Send Confirm Email request. User Id: {oUser.Id}, recipient name: {oUser.Name}, recipient email: {oUser.Email}");

            try
            {
                var oUserToken = await _userTokenService.CreateAsync(oUser.Id, ConstantCode.UserTokenType.EmailConfirmation);

                string confirmEmailUrl = GenerateUrlHelper.GenerateUrl(_reactBaseUrl, ConstantCode.UrlPath.ConfirmEmail, oUserToken.Token);

                var placeholders = new Dictionary<string, string>
                {
                    { ConstantCode.EmailPlaceholder.RecipientName, oUser.Name },
                    { ConstantCode.EmailPlaceholder.ConfirmEmailUrl, confirmEmailUrl },
                    { ConstantCode.EmailPlaceholder.ExpiresDateTime, oUserToken.ExpiresDateTime.ToString("MMMM dd, yyyy hh:mm tt") },
                };

                var emailContent = await PrepareEmailContentAsync(ConstantCode.Resource.EmailTemplateDesign.ConfirmEmailTemplate, placeholders);

                var email = new T_Email
                {
                    Id = IdGeneratorHelper.GenerateId(),
                    EmailSubject = "Email Confirmation",
                    EmailContent = emailContent,
                    RecipientName = oUser.Name,
                    RecipientEmail = oUser.Email,
                    Status = ConstantCode.Status.Code_Pending,
                    CreatedDateTime = DateTime.Now
                };

                await _emailRepository.CreateAsync(email);

                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Send Confirm Email Successful. Recipient name: {oUser.Name}, recipient email: {oUser.Email}");
            }
            catch (Exception ex)
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Send Confirm Email Failed. Recipient name: {oUser.Name}, recipient email: {oUser.Email}, Exception: {ex.Message}");
            }

        }

        #endregion

        #region [ Function ]

        public async Task<string> PrepareEmailContentAsync(string templateName, Dictionary<string, string> placeholders)
        {
            var templateContent = ReadResourceHelper.ReadResource(templateName);
            return EmailTemplateHelper.ReplacePlaceholders(templateContent, placeholders);
        }

        #endregion
    }
}