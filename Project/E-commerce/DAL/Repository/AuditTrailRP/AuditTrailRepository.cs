using DAL.Entity;
using DAL.Tools.ListingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.AuditTrailRP
{
    public class AuditTrailRepository : ListingHelper<T_AuditTrail>, IAuditTrailRepository
    {
        private readonly MyDbContext _myDbContext;

        public AuditTrailRepository(MyDbContext context) : base(context)
        {
            _myDbContext = context;
        }

        public async Task CreateAsync(T_AuditTrail auditTrail)
        {
            await _myDbContext.T_AuditTrail.AddAsync(auditTrail);

            // Save changes to the database
            await _myDbContext.SaveChangesAsync();
        }
    }
}
