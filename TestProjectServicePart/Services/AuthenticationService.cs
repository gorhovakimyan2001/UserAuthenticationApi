using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TestProjectDbPart.Data;
using TestProjectDbPart.Models;
using TestProjectServicePart.HelperMethods;
using TestProjectServicePart.ModelDtos;

namespace TestProjectServicePart.Services
{
    public class AuthenticationService
    {
        private readonly UserContext _userContext;
        private readonly IConfiguration _configuration;
        private readonly AuthenticationHelper _authenticationHelper;

        public AuthenticationService(UserContext userContext, IConfiguration configuration)
        {
            _userContext = userContext;
            _configuration = configuration;
            _authenticationHelper = new AuthenticationHelper(_configuration);
        }

        public bool Registration(AuthenticationRegisterDto authenticationRegisterDto)
        {
            var auth = _userContext.Authentications.FirstOrDefault(a => a.Email == authenticationRegisterDto.Email);

            if (auth != null )
            {
                throw new Exception("There is existing user with that Email");
            }
            else if (authenticationRegisterDto.Password != authenticationRegisterDto.PasswordConfirm)
            {
                throw new ArgumentException("Password mismatch");
            }
            else if (string.IsNullOrEmpty(authenticationRegisterDto.Password)
                        || string.IsNullOrEmpty(authenticationRegisterDto.Email))
            {
                throw new Exception("Emaill or Password can not be empty");
            }

            string passwordSalt = _authenticationHelper.GetPasswordSalt(authenticationRegisterDto.Password);
            long passwordHash = _authenticationHelper.GetPasswordHash(passwordSalt);

            AuthenticationModel dbModel = new AuthenticationModel()
            {
                Email = authenticationRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            var responce = _userContext.Authentications.Add(dbModel);
            _userContext.SaveChanges();
            return responce != null;

        }

        public Dictionary<string, string> LogIn(AuthenticationLoginDto loginDto)
        {
            var user = _userContext.Authentications.FirstOrDefault(u => u.Email == loginDto.Email);

            if (user != null)
            {
                AuthenticationLoginCheckDto check = new AuthenticationLoginCheckDto
                {
                    PasswordHash = user.PasswordHash,
                    PasswordSalt = user.PasswordSalt,
                };

                string passwordSalt = _authenticationHelper.GetPasswordSalt(loginDto.Password);
                long passwordHash = _authenticationHelper.GetPasswordHash(passwordSalt);

                if (passwordHash == check.PasswordHash && passwordSalt == check.PasswordSalt)
                {
                    var userId = _userContext.Users.FirstOrDefault(u => u.Email == loginDto.Email);
                    if (userId == null)
                    {
                        throw new Exception("There is no user with that Email");
                    }

                    return new Dictionary<string, string>{
                            { "token", _authenticationHelper.CreateToken(userId.Id) }
                    };
                }
            }

            return null;
        }

        public Dictionary<string, string> RefreshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings").GetSection("TokenKey").Value);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var userIdClaim = principal.FindFirst("userId");

                if (userIdClaim == null)
                {
                    throw new SecurityTokenException("Invalid token");
                }

                var userId = int.Parse(userIdClaim.Value);

                return new Dictionary<string, string> {
                            { "token", _authenticationHelper.CreateToken(userId) }
                };
            }
            catch (Exception)
            {
                throw new SecurityTokenException("Invalid token");
            }
        }
    }
}
