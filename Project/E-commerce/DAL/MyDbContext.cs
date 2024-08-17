using DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<T_User> T_User { get; set; }
        public DbSet<T_UserLoginHistory> T_UserLoginHistory { get; set; }
        public DbSet<E_UserRole> E_UserRole { get; set; }
        public DbSet<T_AuditTrail> T_AuditTrail { get; set; }
        public DbSet<T_AuditTrailDetails> T_AuditTrailDetails { get; set; }
        public DbSet<T_SystemConfig> T_SystemConfig { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Disabled to identity the primary key
            modelBuilder.Entity<E_UserRole>()
                .Property(ur => ur.Id)
                .ValueGeneratedNever();  // Disable auto-increment

            // Configure the relationship between Audit_Trail and Audit_Trail_Details
            modelBuilder.Entity<T_AuditTrail>()
           .HasMany(a => a.AuditTrailDetails)
           .WithOne(d => d.AuditTrail)
           .HasForeignKey(d => d.Audit_Trail_Id);

            base.OnModelCreating(modelBuilder);
        }
    }



}
