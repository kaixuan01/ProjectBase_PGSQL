using System.ComponentModel.DataAnnotations;

namespace DAL.Entity
{
    public class T_SystemConfig
    {
        [MaxLength(20)]
        public string Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
