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
                CreateUserRole(UserRoleEnum.Admin, "Admin", "Administrator"),
                CreateUserRole(UserRoleEnum.Tester, "Tester", "Tester"),
                CreateUserRole(UserRoleEnum.Developer, "Developer", "Software Developer"),
                CreateUserRole(UserRoleEnum.NormalUser, "Normal User", "Customer Account"),
            };

            var systemConfig = new List<T_SystemConfig>
            {
                CreateSystemConfig(ConstantCode.SystemConfig_Key.MaxLoginFailedAttempt, "3", "Maximum number of login attempts before lockout")
            };

            var users = new List<T_User>
            {
                CreateUser("admin1", "admin", "Admin 1", "admin", "woonyap616@gmail.com", "0123456789", UserRoleEnum.Admin),
                CreateUser("admin2", "admin", "Admin 2", "admin 2", "kaixuan0131@gmail.com", "0123456789", UserRoleEnum.Admin)
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
        private static T_User CreateUser(string userName, string password, string name, string address, string email, string phone, UserRoleEnum role)
        {
            return new T_User
            {
                Id = IdGeneratorHelper.GenerateId(),
                UserName = userName,
                Password = PasswordHelper.HashPassword(password),
                Name = name,
                Address = address,
                Email = email,
                Phone = phone,
                UserRoleId = (int)role
            };
        }

        private static E_UserRole CreateUserRole(UserRoleEnum role, string name, string description)
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
