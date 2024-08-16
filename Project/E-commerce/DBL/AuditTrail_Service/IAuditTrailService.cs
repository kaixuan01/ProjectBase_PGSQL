using DAL.Entity;
using DAL.Tools.ListingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.AuditTrail_Service
{
    public interface IAuditTrailService
    {
        Task CreateAuditTrailAsync<T>(string module, string action, T originalObject, T newObject, string userId = "");
        Task<PagedResult<T_AuditTrail>> GetPagedListAsync(FilterParameters filterParameters);
    }
}
