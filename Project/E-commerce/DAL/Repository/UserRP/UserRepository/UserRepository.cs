using DAL.Entity;
using DAL.Tools.ListingHelper;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.UserRP.UserRepository
{
    public class UserRepository : ListingHelper<T_User>, IUserRepository
    {
        private readonly MyDbContext _myDbContext;

        public UserRepository(MyDbContext context) : base(context)
        {
            _myDbContext = context;
        }

        #region [ Get T_User ]

        public async Task<T_User> GetByIdAsync(string id)
        {
            return await _myDbContext.T_User.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T_User> GetByUsernameAsync(string username)
        {
            return await _myDbContext.T_User.FirstOrDefaultAsync(x => x.UserName == username);
        }

        #endregion

        public async Task<T_User> CreateAsync(T_User user)
        {
            await _myDbContext.T_User.AddAsync(user);
            await _myDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<string> UpdateAsync(T_User oUser)
        {
            // Attach the user entity to the context
            _myDbContext.Attach(oUser);

            // Mark all properties as modified
            _myDbContext.Entry(oUser).State = EntityState.Modified;

            // Save changes to the database
            await _myDbContext.SaveChangesAsync();

            return oUser.Id;
        }

        public async Task<string> DeleteAsync(T_User user)
        {
            if (user != null)
            {
                _myDbContext.T_User.Remove(user);
                await _myDbContext.SaveChangesAsync();
            }

            return user.Id;
        }
    }
}
