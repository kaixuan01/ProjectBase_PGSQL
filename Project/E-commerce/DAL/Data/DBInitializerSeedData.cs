using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;
using Utils.Tools;
using static System.Net.Mime.MediaTypeNames;

namespace DAL.Data
{
    public static class DBInitializerSeedData
    {
        public static void InitializeDatabase(MyDbContext myDbContext)
        {
            if(myDbContext.User.Any())
            {
                return;
            }

            var users = new List<User>
            {
                CreateUser("admin1", "admin", "Admin 1", "admin", "woonyap616@gmail.com", "0123456789", UserRoleEnum.Admin),
                CreateUser("admin2", "admin", "Admin 2", "admin 2", "kaixuan0131@gmail.com", "0123456789", UserRoleEnum.Admin),
                CreateUser("testUser", "test", "test User 3", "test 3", "test@test.com", "0123456789", UserRoleEnum.Tester)
            };

            var userRoles = new List<UserRole>
            {
                CreateUserRole(UserRoleEnum.Admin, "Admin", "Administrator"),
                CreateUserRole(UserRoleEnum.Tester, "Tester", "Tester"),
                CreateUserRole(UserRoleEnum.Developer, "Developer", "Software Developer"),
                CreateUserRole(UserRoleEnum.NormalUser, "Normal User", "Customer Account"),
            };

            myDbContext.UserRole.AddRange(userRoles);
            myDbContext.User.AddRange(users);

            myDbContext.SaveChanges();
        }

        // Method to create a User object with hashed password
        private static User CreateUser(string userName, string password, string name, string address, string email, string phone, UserRoleEnum role)
        {
            return new User
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

        private static UserRole CreateUserRole(UserRoleEnum role, string name, string description)
        {
            return new UserRole
            {
                Id = (int)role,
                Name = name,
                Description = description
            };
        }
    }
}
