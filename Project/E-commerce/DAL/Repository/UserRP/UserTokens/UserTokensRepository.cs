using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.UserRP.UserTokens
{
    public class UserTokensRepository : IUserTokensRepository
    {
        private readonly MyDbContext _myDbContext;

        public UserTokensRepository(MyDbContext context)
        {
            _myDbContext = context;
        }

        public async Task CreateAsync(T_UserTokens userTokens)
        {
            await _myDbContext.T_UserTokens.AddAsync(userTokens);
            await _myDbContext.SaveChangesAsync();
        }

        public async Task<T_UserTokens> GetByTokenAsync(string token)
        {
            var oUserToken = await _myDbContext.T_UserTokens
                .Where(x => x.Token == token)
                .OrderByDescending(x => x.CreatedDateTime)
                .FirstOrDefaultAsync();

            return oUserToken;
        }

        public async Task<T_UserTokens> GetByUserIdAsync(string UserId)
        {
            var oUserToken = await _myDbContext.T_UserTokens
                           .Where(x => x.UserId == UserId)
                           .OrderByDescending(x => x.CreatedDateTime)
                           .FirstOrDefaultAsync();

            return oUserToken;
        }

        public async Task UpdateAsync(T_UserTokens userTokens)
        {
            // Attach the user entity to the context
            _myDbContext.Attach(userTokens);

            // Mark all properties as modified
            _myDbContext.Entry(userTokens).State = EntityState.Modified;

            // Save changes to the database
            await _myDbContext.SaveChangesAsync();
        }
    }
}
