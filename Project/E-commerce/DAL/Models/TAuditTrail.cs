using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class TAudittrail
{
    public string Id { get; set; } = null!;

    public string Module { get; set; } = null!;

    public string Tablename { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? Remark { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<TAudittraildetail> TAudittraildetails { get; set; } = new List<TAudittraildetail>();
}
