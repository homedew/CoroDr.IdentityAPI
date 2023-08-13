using System;
using Microsoft.AspNetCore.Identity;

namespace CoroDr.IdentityAPI.ViewModels
{
	public class RegistrationViewModel
	{
        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null! ;

        public string? ProviderUserId { get; set; }

        public string? UserRole { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? ProfileImage { get; set; }

        public string? AddressName { get; set; }
    }
}

