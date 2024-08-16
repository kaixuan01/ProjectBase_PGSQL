using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class T_AuditTrail
    {
        [MaxLength(20)]
        public string Id { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string? UserId { get; set; }
        public string? Remark { get; set; }

        // Navigation property for the related Audit_Trail_Details
        public ICollection<T_AuditTrailDetails> AuditTrailDetails { get; set; }

        [ForeignKey("UserId")] // Assign Foreign Key
        public T_User User { get; set; } // Navigation property
    }

    public class T_AuditTrailDetails
    {
        public string Id { get; set; }
        public string Audit_Trail_Id { get; set; }  // Foreign key
        public string Field { get; set; }  // Foreign key
        public string? Original_Data { get; set; }
        public string? New_Data { get; set; }

        // Navigation property to the related Audit_Trail
        [JsonIgnore]
        public T_AuditTrail AuditTrail { get; set; }
    }
}
