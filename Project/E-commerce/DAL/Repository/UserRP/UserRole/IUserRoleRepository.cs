using DAL.Entity;

namespace DAL.Repository.UserRP.UserRole
{
    public interface IUserRoleRepository
    {
        Task<List<E_UserRole>> GetUserRoleListingAsync();
    }
}
