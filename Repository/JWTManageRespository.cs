using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoroDr.IdentityAPI.Models;
using CoroDr.IdentityAPI.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace CoroDr.IdentityAPI.Repository
{
    public class JWTManageRespository : IJWTManageRespositoryInterface
    {
        private readonly IConfiguration _configuration;
        public JWTManageRespository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var  token = tokenHandler.CreateToken(tokenDescriptor);

            return new string(tokenHandler.WriteToken(token));
        }

    }
}

