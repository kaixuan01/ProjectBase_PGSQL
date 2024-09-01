using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.EmailRP
{
    public interface IEmailRepository
    {
        Task CreateAsync(T_Email email);
        Task UpdateAsync(T_Email oEmail);
        Task<List<T_Email>> GetSendEmailListAsync(int oRetryAttempt);
        Task<T_Email> GetSendEmailAsync(string oId);
    }
}
