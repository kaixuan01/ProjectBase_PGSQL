using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class TUserloginhistory
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public DateTime? LoginDatetime { get; set; }

    public DateTime? LogoutDatetime { get; set; }

    public string? Remark { get; set; }

    public virtual TUser User { get; set; } = null!;
}
