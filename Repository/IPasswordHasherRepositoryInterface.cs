using System;
namespace CoroDr.IdentityAPI.Repository
{
	public interface IPasswordHasherRepositoryInterface
    {
		public string HashingPassword(string password);
	}
}

