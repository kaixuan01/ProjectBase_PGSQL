using DAL.Entity;
using DAL.Repository.EmailRP;
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
        private readonly string _reactBaseUrl;
        private readonly EncryptionHelper _encryptionHelper;

        public EmailService(IEmailRepository emailRepository, IConfiguration configuration, EncryptionHelper encryptionHelper) {
        
            _emailRepository = emailRepository;

            // Get the secret key from appsettings.json
            _reactBaseUrl = configuration["ReactSettings:BaseUrl"];

            _encryptionHelper = encryptionHelper;
        }

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
                    IsSent = false,
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

        #region [ Function ]

        public async Task<string> PrepareEmailContentAsync(string templateName, Dictionary<string, string> placeholders)
        {
            var templateContent = ReadResourceHelper.ReadResource(templateName);
            return EmailTemplateHelper.ReplacePlaceholders(templateContent, placeholders);
        }

        #endregion
    }
}
