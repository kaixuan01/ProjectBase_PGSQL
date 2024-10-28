using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class TUsertoken
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    /// <summary>
    /// Stores Base64 encoded token
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// Token type: 1. EmailConfirmation - For confirming a newly created user email address, 2. ResetPassword - For password reset requests.
    /// </summary>
    public string TokenType { get; set; } = null!;

    public DateTime CreatedDatetime { get; set; }

    public DateTime ExpiresDatetime { get; set; }

    public bool IsUsed { get; set; }

    public virtual TUser User { get; set; } = null!;
}
