using DAL.Models;
using DAL.Tools.ListingHelper;

namespace DAL.Repository.AuditTrailRP
{
    public class AuditTrailRepository : ListingHelper<TAudittrail>, IAuditTrailRepository
    {
        private readonly AppDbContext _appDbContext;

        public AuditTrailRepository(AppDbContext context) : base(context)
        {
            _appDbContext = context;
        }

        public async Task CreateAsync(TAudittrail auditTrail)
        {
            await _appDbContext.TAudittrails.AddAsync(auditTrail);

            // Save changes to the database
            await _appDbContext.SaveChangesAsync();
        }
    }
}
