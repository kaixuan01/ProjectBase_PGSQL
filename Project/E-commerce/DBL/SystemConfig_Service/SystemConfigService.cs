using DAL.Entity;
using DAL.Repository.SystemConfigRP;
using DAL.Tools.ListingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;
using Utils;
using Microsoft.AspNetCore.Components.Routing;
using DBL.Tools;
using DAL.Repository.AuditTrailRP;
using DBL.AuditTrail_Service;
using DAL;
using Utils.Tools;
using DAL.Repository.UserRP.UserRepository;

namespace DBL.SystemConfig_Service
{
    public class SystemConfigService : ISystemConfigService
    {
        private readonly ISystemConfigRepository _systemConfigRepository;
        private readonly IAuditTrailRepository _auditTrailRepository;
        private readonly IUserRepository _userRepository;

        public SystemConfigService(ISystemConfigRepository systemConfigRepository, IAuditTrailRepository auditTrailRepository, IUserRepository userRepository) {
            _systemConfigRepository = systemConfigRepository;
            _auditTrailRepository = auditTrailRepository;
            _userRepository = userRepository;
        }
        public async Task<List<T_SystemConfig>> GetSystemConfigList()
        {
            var result = await _systemConfigRepository.GetAllAsync();

            return result;
         }

        public async Task<UpdateSystemConfig_RESP> UpdateAsync(UpdateSystemConfig_REQ oReq)
        {
            var rtnValue = new UpdateSystemConfig_RESP();

            try
            {
                var systemConfig = await GetSystemConfigList();
                var copySystemConfig = systemConfig.Clone();

                foreach (var item in oReq.sysConfigList)
                {
                    var oSystemConfig = await _systemConfigRepository.GetByKeyAsync(item.key);

                    if (oSystemConfig == null)
                    {
                        LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"System Config not found. Key: {item.key}");

                        rtnValue.Code = RespCode.RespCode_Failed;
                        rtnValue.Message = "System Config not found.";
                        return rtnValue;
                    }

                    oSystemConfig.Value = item.value;
                    oSystemConfig.UpdatedDate = DateTime.Now;

                    await _systemConfigRepository.UpdateAsync(oSystemConfig);
                }

                await CreateAuditTrailAsync(ConstantCode.Module.SystemConfig, ConstantCode.Action.Edit, copySystemConfig, systemConfig, oReq.userId);

                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = RespCode.RespMessage_Update_Successful;
                LogHelper.RaiseLogEvent(Enum_LogLevel.Information, $"{RespCode.RespMessage_Update_Successful}.");
            }
            catch (Exception ex)
            {
                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ex.Message;
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Exception Message: {ex.Message}");
            }
            
            return rtnValue;
        }

        private async Task CreateAuditTrailAsync(string module, string action, List<T_SystemConfig> originalObject, List<T_SystemConfig> newObject, string userId = "")
        {
            try
            {
                string userName = string.Empty;
                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user.UserName != null)
                    {
                        userName = user.UserName;
                    }
                }

                var auditTrail = new T_AuditTrail
                {
                    Id = IdGeneratorHelper.GenerateId(),
                    Module = module,
                    TableName = typeof(T_SystemConfig).Name,
                    Action = action,
                    UserName = userName,
                    Remark = $"{action} System Config.",
                    CreatedDate = DateTime.Now,
                    AuditTrailDetails = new List<T_AuditTrailDetails>()
                };

                // Compare the original and new objects if both are not null
                if (originalObject != null && newObject != null)
                {
                    foreach (var item in originalObject)
                    {
                        foreach (var item1 in newObject)
                        {
                            if (item.Key == item1.Key)
                            {
                                if (item.Value != item1.Value)
                                {
                                    var auditTrailDetail = new T_AuditTrailDetails
                                    {
                                        Id = IdGeneratorHelper.GenerateId(),
                                        Field = item.Key,
                                        Original_Data = item.Value,
                                        New_Data = item1.Value
                                    };
                                    auditTrail.AuditTrailDetails.Add(auditTrailDetail);
                                }
                            }
                        }
                    }
                }

                // Save the audit trail
                await _auditTrailRepository.CreateAsync(auditTrail);
            }
            catch (Exception ex)
            {
                // Log
            }
        }
    }
}
