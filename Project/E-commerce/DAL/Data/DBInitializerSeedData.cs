using DAL.Entity;
using Utils.Enums;
using Utils.Tools;

namespace DAL.Data
{
    public static class DBInitializerSeedData
    {
        public static void InitializeDatabase(MyDbContext myDbContext)
        {
            if(myDbContext.T_User.Any())
            {
                return;
            }

            var users = new List<T_User>
            {
                CreateUser("admin1", "admin", "Admin 1", "admin", "woonyap616@gmail.com", "0123456789", UserRoleEnum.Admin),
                CreateUser("admin2", "admin", "Admin 2", "admin 2", "kaixuan0131@gmail.com", "0123456789", UserRoleEnum.Admin),
                CreateUser("testUser", "test", "test User 3", "test 3", "test@test.com", "0123456789", UserRoleEnum.Tester)
            };

            var userRoles = new List<E_UserRole>
            {
                CreateUserRole(UserRoleEnum.Admin, "Admin", "Administrator"),
                CreateUserRole(UserRoleEnum.Tester, "Tester", "Tester"),
                CreateUserRole(UserRoleEnum.Developer, "Developer", "Software Developer"),
                CreateUserRole(UserRoleEnum.NormalUser, "Normal User", "Customer Account"),
            };

            myDbContext.E_UserRole.AddRange(userRoles);
            myDbContext.T_User.AddRange(users);

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
    }
}
