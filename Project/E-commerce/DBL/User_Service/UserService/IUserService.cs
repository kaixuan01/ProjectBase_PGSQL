using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using DAL.Tools.ListingHelper;
using DBL.User_Service.UserService.VerifyUser;

namespace DBL.User_Service.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUserAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
        Task<int> UpdateAsync(int id, User user);
        Task<int> DeleteAsync(int id);
        Task<bool> VerifyUserAsync(VerifyUser_REQ user);
        Task<PagedResult<User>> GetPagedListAsync(FilterParameters filterParameters);
    }
}
