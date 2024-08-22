using DAL.Entity;

namespace DAL.Repository.EmailRP
{
    public interface IEmailRepository
    {
        Task CreateAsync(T_Email email);
    }
}
