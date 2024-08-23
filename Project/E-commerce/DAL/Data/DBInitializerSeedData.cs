using DAL.Entity;
using System.Linq.Dynamic.Core;
using Utils;
using Utils.Enums;
using Utils.Tools;

namespace DAL.Data
{
    public static class DBInitializerSeedData
    {
        public static void InitializeDatabase(MyDbContext myDbContext)
        {
            #region [ Seed Data ]

            var userRoles = new List<E_UserRole>
            {
                CreateUserRole(Enum_UserRole.Admin, "Admin", "Administrator"),
                CreateUserRole(Enum_UserRole.Merchant, "Merchant", "Merchant Account"),
                CreateUserRole(Enum_UserRole.NormalUser, "Normal User", "Customer Account"),
            };

            var systemConfig = new List<T_SystemConfig>
            {
                CreateSystemConfig(ConstantCode.SystemConfig_Key.MaxLoginFailedAttempt, "3", "The maximum number of failed login attempts allowed before a user account is locked. Set this value to 0 to disable the lockout feature."),
                CreateSystemConfig(ConstantCode.SystemConfig_Key.EnableSendEmail_Background, "1", "Enables or disables the background email sending function. Set this value to 0 to disable or 1 to enable the function."),
                CreateSystemConfig(ConstantCode.SystemConfig_Key.SendEmailIntervalSec_Background, "10", "The interval, in seconds, at which emails are sent in the background. For example, setting this value to 5 will cause the email sending function to execute every 5 seconds."),
                CreateSystemConfig(ConstantCode.SystemConfig_Key.SendEmailTotalRetry_Background, "3", "Specifies the maximum number of retry attempts for sending an email. If the number of failed attempts reaches this limit, further attempts to send emails to this address will be halted.")
            };

            var users = new List<T_User>
            {
                CreateUser("admin1", "admin", "Admin 1", "woonyap616@gmail.com", "0123456789", Enum_UserRole.Admin),
                CreateUser("admin2", "admin", "Admin 2", "kaixuan0131@gmail.com", "0123456789", Enum_UserRole.Admin),
                CreateUser("merchant1", "merchant", "Merchant 1", "merchant@merchant.com", "0123456789", Enum_UserRole.Merchant)
            };

            #endregion

            #region [ Process Seed Data]

            // 1. If there is no exist user role id, then add new record
            // 2. If there is exist user role id and name not equal, then update name and description
            foreach (var item in userRoles)
            {
                var oUserRole = myDbContext.E_UserRole.FirstOrDefault(i => i.Id == item.Id);
                if (oUserRole == null)
                {
                    myDbContext.E_UserRole.Add(item);
                }
                else if (oUserRole.Name != item.Name && oUserRole.Description != oUserRole.Description)
                {
                    oUserRole.Name = item.Name;
                    oUserRole.Description = item.Description;
                    myDbContext.E_UserRole.Update(oUserRole);
                }
            }

            // If there is no default user exist, add the user list
            if(!myDbContext.T_User.Any())
            {
                myDbContext.T_User.AddRange(users);
            }

            // 1. If there is no exist system config key, then add new record
            foreach (var item in systemConfig)
            {
                var oSystemConfig = myDbContext.T_SystemConfig.FirstOrDefault(i => i.Key == item.Key);
                if (oSystemConfig == null)
                {
                    myDbContext.T_SystemConfig.Add(item);
                }
            }

            #endregion

            myDbContext.SaveChanges();
        }

        // Method to create a T_User object with hashed password
        private static T_User CreateUser(string userName, string password, string name, string email, string phone, Enum_UserRole role)
        {
            return new T_User
            {
                Id = IdGeneratorHelper.GenerateId(),
                Username = userName,
                Password = PasswordHelper.HashPassword(password),
                Name = name,
                Email = email,
                Phone = phone,
                UserRoleId = (int)role,
                IsEmailVerified = true
            };
        }

        private static E_UserRole CreateUserRole(Enum_UserRole role, string name, string description)
        {
            return new E_UserRole
            {
                Id = (int)role,
                Name = name,
                Description = description
            };
        }

        private static T_SystemConfig CreateSystemConfig(string key, string value, string description)
        {
            return new T_SystemConfig
            {
                Id = IdGeneratorHelper.GenerateId(),
                Key = key,
                Value = value,
                Description = description,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
        }
    }
}
