using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class TAudittraildetail
{
    public string Id { get; set; } = null!;

    public string AuditTrailId { get; set; } = null!;

    public string Field { get; set; } = null!;

    public string? OriginalData { get; set; }

    public string? NewData { get; set; }

    public virtual TAudittrail AuditTrail { get; set; } = null!;
}
