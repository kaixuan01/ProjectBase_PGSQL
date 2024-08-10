

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
        public string? Role { get; set; }

        // Non-nullable with default value of 0
        [Description("Count of failed login attempts, Default Value = 0")]
        public int iCountFailedLogin { get; set; } = 0;
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
}
