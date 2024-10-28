using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class TUser
{
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    /// <summary>
    /// User role ID in e_userrole table
    /// </summary>
    public int UserRoleId { get; set; }

    /// <summary>
    /// Used to count user login failed attempts.
    /// </summary>
    public int IcountFailedLogin { get; set; }

    /// <summary>
    /// User status: False (0) - Active, True (1) - Blocked
    /// </summary>
    public bool IsBlocked { get; set; }

    public bool IsEmailVerified { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<TUserloginhistory> TUserloginhistories { get; set; } = new List<TUserloginhistory>();

    public virtual ICollection<TUsertoken> TUsertokens { get; set; } = new List<TUsertoken>();

    public virtual EUserrole UserRole { get; set; } = null!;
}
