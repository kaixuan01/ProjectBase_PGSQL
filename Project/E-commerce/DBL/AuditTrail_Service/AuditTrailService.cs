using DAL;
using DAL.Entity;
using DAL.Repository.AuditTrailRP;
using DAL.Repository.UserRP.UserRepository;
using DAL.Tools.ListingHelper;
using Utils.Tools;

namespace DBL.AuditTrail_Service
{
    public class AuditTrailService : IAuditTrailService
    {
        private IAuditTrailRepository _auditTrailRepository;
        private IUserRepository _userRepository;
        private readonly MyDbContext _myDbContext;

        public AuditTrailService(IAuditTrailRepository auditTrailRepository, MyDbContext myDbContext, IUserRepository userRepository)
        {
            _auditTrailRepository = auditTrailRepository;
            _myDbContext = myDbContext;
            _userRepository = userRepository;
        }

        public async Task CreateAuditTrailAsync<T>(string module, string action, T originalObject, T newObject, string userId = "")
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
                    TableName = typeof(T).Name,
                    Action = action,
                    UserName = userName,
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
            }
            catch (Exception ex)
            {
                // Log
            }
        }

        public async Task<PagedResult<T_AuditTrail>> GetPagedListAsync(FilterParameters filterParameters)
        {
            var rtnValue = await _auditTrailRepository.GetPagedListAsync(filterParameters, true);

            return rtnValue;
        }
    }
}
