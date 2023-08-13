using System;
using Microsoft.AspNetCore.Identity;

namespace CoroDr.IdentityAPI.ViewModels
{
	public class ApplicationUser: IdentityUser
	{
        public string? AddressName { get; set; }
		public string? Username { get; set; }
	}
}

