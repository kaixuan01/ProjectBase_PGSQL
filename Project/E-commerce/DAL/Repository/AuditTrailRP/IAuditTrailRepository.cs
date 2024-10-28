using DAL.Models;
using DAL.Tools.ListingHelper;

namespace DAL.Repository.AuditTrailRP
{
    public interface IAuditTrailRepository : IListingHelper<TAudittrail>
    {
        Task CreateAsync(TAudittrail auditTrail);
    }
}
