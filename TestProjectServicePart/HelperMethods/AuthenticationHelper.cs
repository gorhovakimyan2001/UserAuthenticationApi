using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestProjectDbPart.Data;
using TestProjectServicePart.ModelDtos;

namespace TestProjectServicePart.HelperMethods
{
    public class AuthenticationHelper
    {
        private readonly IConfiguration _configuration;

        public AuthenticationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetPasswordSalt(string password)
        {
            return password + _configuration.GetSection("AppSettings").GetSection("PasswordKey").Value;
        }

        public long GetPasswordHash(string passwordSalt)
        {
            long hash = 0;

            foreach (char item in passwordSalt)
            {
                hash += item;
            }

            return hash;
        }

        public string CreateToken(int userId)
        {
            Claim[] claims = new Claim[]
            {new Claim("userId", userId.ToString()) };

            string tonkenKeyString = _configuration.GetSection("AppSettings").GetSection("TokenKey").Value;
            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                tonkenKeyString != null ? tonkenKeyString : "")
                );

            SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(10),
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(securityToken);
        }
    }
}
