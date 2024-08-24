using DAL.Entity;
using DAL.Repository.EmailRP;
using DBL.SystemConfig_Service;
using DBL.Tools;
using Microsoft.Extensions.Configuration;
using Utils;
using Utils.Enums;
using Utils.Tools;

namespace DBL.Email_Service
{
    public class EmailService : IEmailService
    {
        IEmailRepository _emailRepository;
        ISystemConfigService _systemConfigService;
        private readonly string _reactBaseUrl;
        private readonly EncryptionHelper _encryptionHelper;

        public EmailService(IEmailRepository emailRepository, IConfiguration configuration, EncryptionHelper encryptionHelper, ISystemConfigService systemConfigService) {
        
            _emailRepository = emailRepository;
            _systemConfigService = systemConfigService;

            // Get the secret key from appsettings.json
            _reactBaseUrl = configuration["ReactSettings:BaseUrl"];

            _encryptionHelper = encryptionHelper;
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

        public async Task SendConfirmEmailAsync(string userId, string recipientName, string recipientEmail)
        {
            LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Send Confirm Email request. user id: {userId}, recipient name: {recipientName}, recipient email: {recipientEmail}");
            try
            {
                // Encrypt the user id to attach it to confirm email url
                string encUserId = _encryptionHelper.Encrypt(userId);
                string confirmEmailUrl = GenerateUrlHelper.GenerateUrl(_reactBaseUrl, ConstantCode.UrlPath.ConfirmEmail, encUserId);

                var placeholders = new Dictionary<string, string>
                {
                    { ConstantCode.EmailPlaceholder.RecipientName, recipientName },
                    { ConstantCode.EmailPlaceholder.ConfirmEmailUrl, confirmEmailUrl }
                };

                var emailContent = await PrepareEmailContentAsync(ConstantCode.Resource.EmailTemplateDesign.ConfirmEmailTemplate, placeholders);

                var email = new T_Email
                {
                    Id = IdGeneratorHelper.GenerateId(),
                    EmailSubject = "Email Confirmation",
                    EmailContent = emailContent,
                    RecipientName = recipientName,
                    RecipientEmail = recipientEmail,
                    Status = ConstantCode.Status.Code_Pending,
                    CreatedDateTime = DateTime.Now
                };

                await _emailRepository.CreateAsync(email);

                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"Send Confirm Email Successful. user id: {userId}, recipient name: {recipientName}, recipient email: {recipientEmail}");
            }
            catch (Exception ex)
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Send Confirm Email Failed. user id: {userId}, recipient name: {recipientName}, recipient email: {recipientEmail}, Exception: {ex.Message}");
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
