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
                Password = "FJOm89g6wpAfl+NGO9pcwzipujlvEtVOvT4D/NjxObcHQMkTP6MsCA8HRuReyRTg",
                Name = "Admin 1",
                Address = "admin",
                Email = "woonyap616@gmail.com",
                Phone = "0123456789",
                Role = "Admin"
            };
            myDbContext.User.Add(AdminUser);

            myDbContext.SaveChanges();
        }
    }
}
