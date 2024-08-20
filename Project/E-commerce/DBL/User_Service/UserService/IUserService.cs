using DAL.Entity;
using DAL.Repository.UserRP.UserRepository.Class;
using DAL.Shared.Class;
using DBL.User_Service.UserService.UserActionClass;

namespace DBL.User_Service.UserService
{
    public interface IUserService
    {
        // ## Get User
        Task<UserListing_RESP> GetUserListingAsync(UserListing_REQ oReq);
        Task<T_User> GetByIdAsync(string id);

        // ## Get User Role
        Task<int> GetUserRoleByUsernameAsync(string username);

        // ## Create, Edit, Delete
        Task<CreateUser_RESP> CreateAsync(CreateUser_REQ user);
        Task<ShareResp> UpdateAsync(EditUser_REQ user);
        Task<ShareResp> DeleteAsync(string id);

        // ## Login Verify
        Task<VerifyUser_RESP> VerifyUserAsync(VerifyUser_REQ user);
        // ## Update Logout
        Task UpdateUserLogoutAsync(string username);


        // ## Block / Unblock user
        Task<ShareResp> SetUserStatusAsync(string id);
    }
}
