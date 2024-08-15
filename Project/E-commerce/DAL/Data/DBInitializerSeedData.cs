using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var AdminUser = new User
            {
                Id = IdGeneratorHelper.GenerateId(),
                UserName = "admin1",
                Password = "admin",
                Name = "Admin 1",
                Address = "admin",
                Email = "woonyap616@gmail.com",
                Phone = "0123456789",
                Role = "Admin"
            };

            var AdminUser2 = new User
            {
                Id = IdGeneratorHelper.GenerateId(),
                UserName = "admin2",
                Password = "admin",
                Name = "Admin 2",
                Address = "admin 2",
                Email = "kaixuan0131@gmail.com",
                Phone = "0123456789",
                Role = "Admin"
            };

            var testUser = new User
            {
                Id = IdGeneratorHelper.GenerateId(),
                UserName = "testUser",
                Password = "test",
                Name = "test User 3",
                Address = "test 3",
                Email = "test@test.com",
                Phone = "0123456789",
                Role = "test"
            };

            AdminUser.Password = PasswordHelper.HashPassword(AdminUser.Password);
            AdminUser2.Password = PasswordHelper.HashPassword(AdminUser2.Password);
            testUser.Password = PasswordHelper.HashPassword(testUser.Password);

            myDbContext.User.Add(AdminUser);
            myDbContext.User.Add(AdminUser2);
            myDbContext.User.Add(testUser);

            myDbContext.SaveChanges();
        }
    }
}
