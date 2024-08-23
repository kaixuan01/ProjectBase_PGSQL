using DAL.Entity;

namespace DBL.Email_Service
{
    public interface IEmailService
    {
        Task SendConfirmEmailAsync(string userId, string recipientName, string recipientEmail);
        Task UpdateEmailAsync(T_Email email);
        Task<List<T_Email>> GetSendEmailListAsync();

    }
}
