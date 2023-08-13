
using System;
using Google.Apis.Auth;

namespace CoroDr.IdentityAPI.LoginProvider
{
    public interface IGoogleInterface
    {
        public Task<GoogleJsonWebSignature.Payload> VerifyAccesToken(string accessToken);
    }
}

