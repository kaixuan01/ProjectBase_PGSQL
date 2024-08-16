using DAL.Entity;
using DAL.Tools.ListingHelper;
using DBL.User_Service.UserService.UserActionClass;

namespace DBL.User_Service.UserService
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(string id);
        Task<CreateUser_RESP> CreateAsync(CreateUser_REQ user);
        Task<EditUser_RESP> UpdateAsync(EditUser_REQ user);
        Task<DeleteUser_RESP> DeleteAsync(string id);
        Task<VerifyUser_RESP> VerifyUserAsync(VerifyUser_REQ user);
        Task<PagedResult<dynamic>> GetPagedListAsync(FilterParameters filterParameters);
    }
}
