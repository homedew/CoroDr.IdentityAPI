using System;
using Microsoft.AspNetCore.Authentication;

namespace CoroDr.IdentityAPI.Service
{
	public interface ILoginInterface<T>
	{
		Task<bool> ValidateCredential(T user, string password);

		Task<T> FindByUsername(string user);

		Task SignIn(T user);

		Task SignInAsync(T user, AuthenticationProperties properties, string autheticationMethod = null);
	}
}

