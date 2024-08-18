using DAL.Entity;
using DBL.Shared.Class;

namespace DBL.SystemConfig_Service
{
    public interface ISystemConfigService
    {
        Task<ShareResp> UpdateAsync(UpdateSystemConfig_REQ oReq);

        Task<List<T_SystemConfig>> GetSystemConfigList();
    }
}
