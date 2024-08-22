using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class T_Email
    {
        [MaxLength(20)]
        public string Id { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailContent { get; set; }
        public string? RecipientName { get; set; }
        public string? RecipientEmail { get; set; }
        public bool IsSent { get; set; } = false;
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? SentDateTime { get; set; }
    }
}
