using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Utils;

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
        public string? Status { get; set; } = ConstantCode.Status.Code_Pending;
        public string? Remark { get; set; }
        public int ICntFailedSend { get; set; } = 0;
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? SentDateTime { get; set; }
    }
}
