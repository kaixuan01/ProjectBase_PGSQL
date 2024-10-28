using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.UserRP.UserTokens
{
    public class UserTokensRepository : IUserTokensRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserTokensRepository(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task CreateAsync(TUsertoken userTokens)
        {
            await _appDbContext.TUsertokens.AddAsync(userTokens);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<TUsertoken> GetByTokenAsync(string token)
        {
            var oUserToken = await _appDbContext.TUsertokens
                .Where(x => x.Token == token)
                .OrderByDescending(x => x.CreatedDatetime)
                .FirstOrDefaultAsync();

            return oUserToken;
        }

        public async Task<TUsertoken> GetByUserIdAsync(string UserId)
        {
            var oUserToken = await _appDbContext.TUsertokens
                           .Where(x => x.UserId == UserId)
                           .OrderByDescending(x => x.CreatedDatetime)
                           .FirstOrDefaultAsync();

            return oUserToken;
        }

        public async Task UpdateAsync(TUsertoken userTokens)
        {
            // Attach the user entity to the context
            _appDbContext.Attach(userTokens);

            // Mark all properties as modified
            _appDbContext.Entry(userTokens).State = EntityState.Modified;

            // Save changes to the database
            await _appDbContext.SaveChangesAsync();
        }
    }
}
