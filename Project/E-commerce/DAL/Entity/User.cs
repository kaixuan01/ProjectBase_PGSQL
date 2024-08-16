

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entity
{
    public class User
    {
        [MaxLength(20)]
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? UserRoleId { get; set; }  // Foreign Key for UserRole

        // Non-nullable with default value of 0
        [Description("Count of failed login attempts, Default Value = 0")]
        public int iCountFailedLogin { get; set; } = 0;

        [ForeignKey("UserRoleId")] // Assign Foreign Key
        public UserRole UserRole { get; set; } // Navigation property
    }

    public class UserLoginHistory
    {
        [MaxLength(20)]
        public string Id { get; set; }
        [MaxLength(20)]
        public string UserId { get; set; } // Foreign Key
        public DateTime? LoginDateTime { get; set; }
        public DateTime? LogoutDateTime { get; set; }
        public string? Remark { get; set; }

        [ForeignKey("UserId")] // Assign Foreign Key
        public User User { get; set; } // Navigation property
    }

    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
