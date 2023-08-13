using System;
using System.Security.Cryptography;
using System.Text;

namespace CoroDr.IdentityAPI.Repository
{
	public class PasswordHashing: IPasswordHasherRepositoryInterface
    {
        const int keySize = 64;
        const int interations = 350000;
        private readonly IConfiguration _configuration;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;  
		public PasswordHashing(IConfiguration configuration)
		{
            _configuration = configuration;
		}

        public string HashingPassword(string password)
        {
            var saltKey = Encoding.UTF8.GetBytes(_configuration["Hashing:saltKey"]);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltKey,
                interations,
                hashAlgorithm,
                keySize
                );

            return Convert.ToHexString(hash);
        }
    }
}

