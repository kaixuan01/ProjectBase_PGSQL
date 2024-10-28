using DAL.Models;

namespace DBL.User_Service.UserRoleService
{
    public interface IUserRoleService
    {
        Task<List<EUserrole>> GetUserRoleListingAsync();
    }
}
