using DBL.SystemConfig_Service;
using Serilog;
using System.Net.Mail;
using Utils;


namespace Background_WorkerService.Worker
{
    public class SendEmailWorker : BackgroundService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public SendEmailWorker(IServiceProvider serviceProvider)
        {
            _logger = new LoggerConfiguration()
            .WriteTo.File("Logs/SendEmailWorker/.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var systemConfigService = scope.ServiceProvider.GetRequiredService<ISystemConfigService>();
                var oSystemConfig = await systemConfigService.GetSystemConfigList();
                var enableSendEmail = oSystemConfig.FirstOrDefault(i => i.Key == ConstantCode.SystemConfig_Key.EnableSendEmail_Background);
                var intervalSendEmail = oSystemConfig.FirstOrDefault(i => i.Key == ConstantCode.SystemConfig_Key.SendEmailIntervalSec_Background);
                // Use systemConfigService to perform background tasks

                while (enableSendEmail.Value == "1")
                {
                    _logger.Information($"EmailWorker running at: {DateTimeOffset.Now}, Interval (Secs): {intervalSendEmail}");

                    // Email sending logic here
                    await SendEmailAsync("recipient@example.com", "Subject", "Email Body");

                    // Wait for a specified time before next execution (e.g., 10 Secs)
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }
        }

        private async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient("smtp.example.com"))
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("no-reply@example.com"),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(recipientEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.Information("Email sent to {recipient}", recipientEmail);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to send email");
            }
        }
    }
}
