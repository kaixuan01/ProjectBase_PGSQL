using DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Utils;

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

        public async Task UpdateAsync(T_Email oEmail)
        {
            // Attach the user entity to the context
            _myDbContext.Attach(oEmail);

            // Mark all properties as modified
            _myDbContext.Entry(oEmail).State = EntityState.Modified;

            // Save changes to the database
            await _myDbContext.SaveChangesAsync();
        }

        public async Task<List<T_Email>> GetSendEmailListAsync(int oRetryAttempt)
        {
            if (oRetryAttempt <= 0)
            {
                throw new ArgumentException("Retry attempts must be greater than zero.", nameof(oRetryAttempt));
            }

            return await _myDbContext.T_Email
                        .Where(e => e.Status == ConstantCode.Status.Code_Pending ||
                                    (e.Status == ConstantCode.Status.Failed && e.ICntFailedSend < oRetryAttempt))  // Filter for pending or failed emails within retry limit
                        .OrderBy(e => e.CreatedDateTime)  // Order by creation date to process oldest emails first
                        .ToListAsync();  // Execute the query and return the results as a list
        }
    }
}
