using DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UserService
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _myDbContext;

        public UserService(MyDbContext context)
        {
            _myDbContext = context;
        }


        public async Task<User> CreateAsync(User user)
        {
            await _myDbContext.User.AddAsync(user);
            await _myDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var user = await _myDbContext.User.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                _myDbContext.User.Remove(user);
                await _myDbContext.SaveChangesAsync();
            }
           
            return id;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _myDbContext.User.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _myDbContext.User.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> UpdateAsync(int id, User nUser)
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
    }
}
