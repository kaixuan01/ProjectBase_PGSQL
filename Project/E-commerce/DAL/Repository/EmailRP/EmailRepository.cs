using DAL.Entity;

namespace DAL.Repository.EmailRP
{
    public class EmailRepository : IEmailRepository
    {
        private readonly MyDbContext _myDbContext;

        public EmailRepository(MyDbContext context)
        {
            _myDbContext = context;
        }

        public async Task CreateAsync(T_Email email)
        {
            await _myDbContext.T_Email.AddAsync(email);
            await _myDbContext.SaveChangesAsync();
        }
    }
}
