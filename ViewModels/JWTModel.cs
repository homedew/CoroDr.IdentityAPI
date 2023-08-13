using System;
namespace CoroDr.IdentityAPI.ViewModels
{
	public class JWTModel
	{
        public string? Issuer { get; set; }
		public string? Audience { get; set; }
		public string? Key { get; set; }
        public string? ExpiredTime { get; set; }
		public string? RefreshToken { get; set; }
		public string? Token { get; set; }

    }
}

