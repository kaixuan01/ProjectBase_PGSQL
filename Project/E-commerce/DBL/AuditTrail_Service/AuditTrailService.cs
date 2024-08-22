using DAL;
using DAL.Entity;
using DAL.Repository.AuditTrailRP;
using DAL.Tools.ListingHelper;
using DBL.Tools;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Utils.Enums;
using Utils.Tools;

namespace DBL.AuditTrail_Service
{
    public class AuditTrailService : IAuditTrailService
    {
        private IAuditTrailRepository _auditTrailRepository;
        private readonly MyDbContext _myDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string username = string.Empty;

        public AuditTrailService(IAuditTrailRepository auditTrailRepository, MyDbContext myDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _auditTrailRepository = auditTrailRepository;
            _myDbContext = myDbContext;
            _httpContextAccessor = httpContextAccessor;
            if (_httpContextAccessor.HttpContext != null)
            {
                username = _httpContextAccessor.HttpContext.Items["Username"] as string;

            }
        }

        public async Task CreateAuditTrailAsync<T>(string module, string action, T originalObject, T newObject)
        {
            try
            {
                // Get the primary key property name
                var keyName = _myDbContext.Model.FindEntityType(typeof(T))?
                    .FindPrimaryKey()?
                    .Properties
                    .Select(x => x.Name)
                    .SingleOrDefault();

                // Initialize primaryKeyValue
                object primaryKeyValue = null;

                if (!string.IsNullOrEmpty(keyName))
                {
                    var keyProperty = typeof(T).GetProperty(keyName);
                    if (keyProperty == null)
                    {
                        throw new InvalidOperationException($"Property '{keyName}' not found on {typeof(T).Name}.");
                    }

                    // Try to get the primary key value from either originalObject or newObject
                    primaryKeyValue = originalObject != null
                        ? keyProperty.GetValue(originalObject)
                        : newObject != null
                            ? keyProperty.GetValue(newObject)
                            : null;
                }

                var auditTrail = new T_AuditTrail
                {
                    Id = IdGeneratorHelper.GenerateId(),
                    Module = module,
                    TableName = typeof(T).Name,
                    Action = action,
                    Username = username,
                    Remark = $"{action} {typeof(T).Name} record, id: {primaryKeyValue}",
                    CreatedDate = DateTime.Now,
                    AuditTrailDetails = new List<T_AuditTrailDetails>()
                };

                // Compare the original and new objects if both are not null
                if (originalObject != null && newObject != null)
                {
                    foreach (var property in typeof(T).GetProperties())
                    {
                        var originalValue = property.GetValue(originalObject)?.ToString();
                        var newValue = property.GetValue(newObject)?.ToString();

                        if (string.IsNullOrEmpty(originalValue) && string.IsNullOrEmpty(newValue))
                        {
                            break;
                        }

                        if (originalValue != newValue)
                        {
                            var auditTrailDetail = new T_AuditTrailDetails
                            {
                                Id = IdGeneratorHelper.GenerateId(),
                                Field = property.Name,
                                Original_Data = originalValue,
                                New_Data = newValue
                            };

                            auditTrail.AuditTrailDetails.Add(auditTrailDetail);
                        }
                    }
                }
                // If originalObject is null, it's a Create action
                else if (originalObject == null && newObject != null)
                {
                    foreach (var property in typeof(T).GetProperties())
                    {
                        var newValue = property.GetValue(newObject)?.ToString();

                        if (string.IsNullOrEmpty(newValue))
                        {
                            break;
                        }

                        var auditTrailDetail = new T_AuditTrailDetails
                        {
                            Id = IdGeneratorHelper.GenerateId(),
                            Field = property.Name,
                            Original_Data = null,  // No original data for Create
                            New_Data = newValue
                        };

                        auditTrail.AuditTrailDetails.Add(auditTrailDetail);
                    }
                }
                // If newObject is null, it's a Delete action
                else if (newObject == null && originalObject != null)
                {
                    foreach (var property in typeof(T).GetProperties())
                    {
                        var originalValue = property.GetValue(originalObject)?.ToString();

                        if (string.IsNullOrEmpty(originalValue))
                        {
                            break;
                        }

                        var auditTrailDetail = new T_AuditTrailDetails
                        {
                            Id = IdGeneratorHelper.GenerateId(),
                            Field = property.Name,
                            Original_Data = originalValue,
                            New_Data = null  // No new data for Delete
                        };

                        auditTrail.AuditTrailDetails.Add(auditTrailDetail);
                    }
                }

                // Save the audit trail
                await _auditTrailRepository.CreateAsync(auditTrail);

                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Insert Audit Trail successful (Object). Record: {JsonConvert.SerializeObject(auditTrail)}");
            }
            catch (Exception ex)
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Error when Insert Audit Trail (Object). Exception Message: {ex.Message}");
            }
        }

        public async Task CreateAuditTrailAsync(string module, string action, string tableName, Dictionary<string, string> originalObject, Dictionary<string, string> newObject)
        {
            try
            {
                var auditTrail = new T_AuditTrail
                {
                    Id = IdGeneratorHelper.GenerateId(),
                    Module = module,
                    TableName = tableName,
                    Action = action,
                    Username = username,
                    Remark = $"{action} {module}.",
                    CreatedDate = DateTime.Now,
                    AuditTrailDetails = new List<T_AuditTrailDetails>()
                };

                // Compare the original and new objects
                foreach (var key in originalObject.Keys)
                {
                    if (newObject.ContainsKey(key))
                    {
                        var originalValue = originalObject[key];
                        var newValue = newObject[key];

                        if (originalValue != newValue)
                        {
                            var auditTrailDetail = new T_AuditTrailDetails
                            {
                                Id = IdGeneratorHelper.GenerateId(),
                                Field = key,
                                Original_Data = originalValue,
                                New_Data = newValue
                            };
                            auditTrail.AuditTrailDetails.Add(auditTrailDetail);
                        }
                    }
                }

                // Save the audit trail
                await _auditTrailRepository.CreateAsync(auditTrail);

                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Insert Audit Trail successful (Dictionary). Record: {JsonConvert.SerializeObject(auditTrail)}");
            }
            catch (Exception ex)
            {
                LogHelper.RaiseLogEvent(Enum_LogLevel.Error, $"Error when Insert Audit Trail (Dictionary). Exception Message: {ex.Message}");
            }
        }

        public async Task<PagedResult<T_AuditTrail>> GetPagedListAsync(FilterParameters filterParameters)
        {
            var rtnValue = await _auditTrailRepository.GetPagedListAsync(filterParameters, true);

            return rtnValue;
        }
    
    }
}
