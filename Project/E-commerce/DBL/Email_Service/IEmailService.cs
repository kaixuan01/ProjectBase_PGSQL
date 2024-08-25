using DAL.Entity;

namespace DBL.Email_Service
{
    public interface IEmailService
    {
        Task SendConfirmEmailAsync(T_User oUser);
        Task UpdateEmailAsync(T_Email email);
        Task<List<T_Email>> GetSendEmailListAsync();

    }
}
