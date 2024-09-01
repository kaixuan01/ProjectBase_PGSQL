using DAL.Entity;

namespace DBL.Email_Service
{
    public interface IEmailService
    {
        Task SendConfirmEmailAsync(T_User oUser);
        Task SendResetPasswordEmailAsync(T_User oUser);

        Task UpdateEmailAsync(string oId, string oStatus, string oRemark);
        Task<List<T_Email>> GetSendEmailListAsync();

    }
}
