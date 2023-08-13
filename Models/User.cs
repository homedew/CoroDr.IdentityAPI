using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CoroDr.IdentityAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string? ProviderUserId { get; set; }

    public string? UserRole { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ProfileImage { get; set; }

    public string? AddressName { get; set; }
}
