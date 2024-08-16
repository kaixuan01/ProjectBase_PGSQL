using DAL.Entity;
using DAL.Tools.ListingHelper;

namespace DAL.Repository.AuditTrailRP
{
    public interface IAuditTrailRepository : IListingHelper<T_AuditTrail>
    {
        Task CreateAsync(T_AuditTrail auditTrail);
    }
}
