using System;
using Microsoft.AspNetCore.Identity;

namespace CoroDr.IdentityAPI.ViewModels
{
	public class LoginViewModel
	{
		public required string Username { get; set; }
        public string? Password { get; set; }
        public required string ProviderId { get; set; }
    }

}

