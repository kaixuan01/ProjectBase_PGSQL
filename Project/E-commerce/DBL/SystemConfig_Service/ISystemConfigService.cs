using DAL.Entity;

namespace DBL.SystemConfig_Service
{
    public interface ISystemConfigService
    {
        Task<UpdateSystemConfig_RESP> UpdateAsync(UpdateSystemConfig_REQ oReq);

        Task<List<T_SystemConfig>> GetSystemConfigList();
    }
}
