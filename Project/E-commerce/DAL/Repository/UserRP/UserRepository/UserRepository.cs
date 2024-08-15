using DAL.Entity;
using DAL.Tools.ListingHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.UserRP.UserRepository
{
    public class UserRepository : ListingHelper<User>, IUserRepository
    {
        private readonly MyDbContext _myDbContext;

        public UserRepository(MyDbContext context) : base(context)
        {
            _myDbContext = context;
        }

        #region [ Get User ]

        public async Task<List<User>> GetAllAsync()
        {
            return await _myDbContext.User.ToListAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _myDbContext.User.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _myDbContext.User.FirstOrDefaultAsync(x => x.UserName == username);
        }

        #endregion

        public async Task<User> CreateAsync(User user)
        {
            await _myDbContext.User.AddAsync(user);
            await _myDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<string> UpdateAsync(string id, User nUser)
        {
            var oUser = await _myDbContext.User.FirstOrDefaultAsync(x => x.Id == id);
            if (oUser != null)
            {
                oUser.Name = nUser.Name;
                oUser.Email = nUser.Email;
                oUser.Phone = nUser.Phone;
                oUser.Address = nUser.Address;

                await _myDbContext.SaveChangesAsync();
            }

            return id;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var user = await _myDbContext.User.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                _myDbContext.User.Remove(user);
                await _myDbContext.SaveChangesAsync();
            }

            return id;
        }
    }
}
