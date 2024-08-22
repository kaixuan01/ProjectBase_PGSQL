namespace DBL.Email_Service
{
    public interface IEmailService
    {
        Task SendConfirmEmailAsync(string userId, string recipientName, string recipientEmail);
    }
}
