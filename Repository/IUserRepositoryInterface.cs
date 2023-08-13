using System;
using CoroDr.IdentityAPI.Models;

namespace CoroDr.IdentityAPI.Repository
{
	public interface IUserRepositoryInterface
	{
		public User FindUserByUserName(string username);
		public bool SaveChanges(User user);

	}
}

