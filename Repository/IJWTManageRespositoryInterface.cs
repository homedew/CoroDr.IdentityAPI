using System;
using CoroDr.IdentityAPI.Models;
using CoroDr.IdentityAPI.ViewModels;

namespace CoroDr.IdentityAPI.Repository
{
	public interface IJWTManageRespositoryInterface
	{
		public string GenerateToken(string userName);
	}
}

