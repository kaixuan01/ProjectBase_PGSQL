using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.UserRP.UserRole
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly MyDbContext _myDbContext;

        public UserRoleRepository(MyDbContext context)
        {
            _myDbContext = context;
        }

        public async Task<List<E_UserRole>> GetUserRoleListingAsync()
        {
            return await _myDbContext.E_UserRole.ToListAsync();
        }
    }
}
