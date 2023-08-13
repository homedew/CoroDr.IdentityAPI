using System;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace CoroDr.IdentityAPI.LoginProvider
{
    public class GoogleLogin : IGoogleInterface
    {

        private IConfiguration _configuration;
        public GoogleLogin(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<GoogleJsonWebSignature.Payload> VerifyAccesToken(string accessToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>()
                    {
                        _configuration["GoogleAuthSettings:clientId"]
                    }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(accessToken, settings);
                return payload;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

