using DAL.Entity;
using DAL.Repository.UserRP.UserRepository.Class;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Utils;

namespace DAL.Repository.UserRP.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _myDbContext;

        public UserRepository(MyDbContext context)
        {
            _myDbContext = context;
        }

        public async Task<IQueryable<UserL>> GetUserListing(UserListing_REQ oReq)
        {
            var query = _myDbContext.T_User
                .Include(u => u.UserRole) // Include UserRole for role name mapping
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(oReq.SearchTerm))
            {
                query = query.Where(u =>
                    u.Username.Contains(oReq.SearchTerm) ||
                    u.Name.Contains(oReq.SearchTerm) ||
                    u.Email.Contains(oReq.SearchTerm) ||
                    u.Phone.Contains(oReq.SearchTerm) ||
                    u.UserRole.Name.Contains(oReq.SearchTerm)
                );
            }
            else
            {
                if (!string.IsNullOrEmpty(oReq.Username))
                {
                    query = query.Where(u => u.Username.Contains(oReq.Username));
                }

                if (!string.IsNullOrEmpty(oReq.Name))
                {
                    query = query.Where(u => u.Name.Contains(oReq.Name));
                }

                if (!string.IsNullOrEmpty(oReq.Email))
                {
                    query = query.Where(u => u.Email.Contains(oReq.Email));
                }

                if (!string.IsNullOrEmpty(oReq.Phone))
                {
                    query = query.Where(u => u.Phone.Contains(oReq.Phone));
                }

                if (!string.IsNullOrEmpty(oReq.Role))
                {
                    var roleIds = oReq.Role.Split(',').Select(int.Parse).ToList();
                    query = query.Where(u => roleIds.Contains(u.UserRoleId));
                }

                if (oReq.Status.HasValue)
                {
                    // Apply status filter
                    query = query.Where(u => u.IsBlocked == oReq.Status.Value);
                }
            }

            var result = query
                .Select(u => new UserL
                {
                    Id = u.Id,
                    Username = u.Username,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    Role = u.UserRole.Name, // Map UserRole.Name to role
                    Status = u.IsBlocked ? "Blocked" : "Active" // Map status
                });

            return result;
        }

        #region [ Get User ]

        public async Task<T_User> GetByIdAsync(string id)
        {
            return await _myDbContext.T_User.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T_User> GetByUsernameAsync(string username)
        {
            return await _myDbContext.T_User.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<T_User> GetByEmailAsync(string email)
        {
            return await _myDbContext.T_User.FirstOrDefaultAsync(x => x.Email == email);
        }

        #endregion

        #region [ Get User's Role ]

        public async Task<int> GetUserRoleByUsernameAsync(string username)
        {
            return await _myDbContext.T_User
                .Where(x => x.Username == username)
                .Select(x => x.UserRoleId)
                .FirstOrDefaultAsync();
        }

        #endregion

        public async Task CreateAsync(T_User user)
        {
            await _myDbContext.T_User.AddAsync(user);
            await _myDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T_User oUser)
        {
            // Attach the user entity to the context
            _myDbContext.Attach(oUser);

            // Mark all properties as modified
            _myDbContext.Entry(oUser).State = EntityState.Modified;

            // Save changes to the database
            await _myDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T_User user)
        {
            _myDbContext.T_User.Remove(user);
            await _myDbContext.SaveChangesAsync();
        }

        public async Task<bool> IsUsernameExistAsync(string username)
        {
            return await _myDbContext.T_User.AnyAsync(user => user.Username == username);
        }

        public async Task<bool> IsEmailExistAsync(string email, string? userId = null)
        {
            return await _myDbContext.T_User
                .AnyAsync(user => user.Email == email && (userId == null ? true : user.Id != userId));
        }
    }
}
