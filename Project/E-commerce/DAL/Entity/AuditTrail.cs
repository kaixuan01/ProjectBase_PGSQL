using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DAL.Entity
{
    public class T_AuditTrail
    {
        [MaxLength(20)]
        public string Id { get; set; }
        public string Module { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public string? Username { get; set; }
        public string? Remark { get; set; }
        public DateTime? CreatedDate { get; set; }

        // Navigation property for the related Audit_Trail_Details
        public ICollection<T_AuditTrailDetails> AuditTrailDetails { get; set; }
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
