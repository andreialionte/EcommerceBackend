using Ecommerce2.Data;
using Ecommerce2.Dtos;
using Ecommerce2.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Ecommerce2.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly DataContextEf _ef;
        private readonly IConfiguration _config;

        public AuthController(IConfiguration configuration)
        {
            _config = configuration;
            _ef = new DataContextEf(configuration);
        }

        [HttpPost("Register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto, int userId)
        {
            if (userForRegisterDto.Password == userForRegisterDto.PasswordConfirmation)
            {
                var existingUser = _ef.userForLoginConfirmationDtos.FirstOrDefault(u => u.Email == userForRegisterDto.Email);
                if (existingUser == null) // Check if user does not exist
                {
                    byte[] passwordSalt = new byte[128 / 8]; //we need 64 for converting
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }
                    /*                    string CombineWithKeyPlusPasswordSalt = _config.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);
                    */
                    byte[]? passwordHash = GetPasswordHash(userForRegisterDto.Password, passwordSalt);

                    UserForLoginConfirmationDto userForLoginConfirmationDto = new UserForLoginConfirmationDto()
                    {
                        Email = userForRegisterDto.Email,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                    };

                    User user = new User()
                    {
                        UserId = userId,
                        Email = userForRegisterDto.Email,
                        Username = userForRegisterDto.Username,
                        RegistrationDate = DateTime.Now,

                    };

                    _ef.userForLoginConfirmationDtos.Add(userForLoginConfirmationDto);
                    _ef.users.Add(user);
                    _ef.SaveChanges();

                    return Ok(new { user, userForLoginConfirmationDto });
                }
                else
                {
                    throw new Exception("The User Already Exists!");
                }
            }
            throw new Exception("The passwords don't match");
        }

        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            UserForLoginConfirmationDto? userForLoginConfirmationDtoDB = _ef.userForLoginConfirmationDtos.FirstOrDefault(k => k.Email == userForLoginDto.Email);

            if (userForLoginConfirmationDtoDB == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            byte[] passwordSalt = userForLoginConfirmationDtoDB.PasswordSalt;
            byte[] convertedPasswordHash = GetPasswordHash(userForLoginDto.Password, passwordSalt);

            if (convertedPasswordHash.Length != userForLoginConfirmationDtoDB.PasswordHash.Length)
            {
                return Unauthorized("The Password is incorrect");
            }
            else
            {
                var user = _ef.users.FirstOrDefault(u => u.Email == userForLoginDto.Email);
                var CreateTheTokenForTheUser = CreateToken(user.UserId);
                return Ok(new { CreateTheTokenForTheUser });
            }

        }

        [HttpPost("RefreshToken")]
        //If Token Experies and the User log on TO NOT logout the user
        //try with PostMan
        public IActionResult RefreshToken()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (userIdClaim != null)
            {
                var userId = int.Parse(userIdClaim);

                var userDb = _ef.users.FirstOrDefault(k => k.UserId == userId);

                if (userDb != null)
                {
                    return Ok(CreateToken(userDb.UserId));
                }

                throw new Exception($"{userDb} dosent exists");
            }
            throw new Exception($"Invalid {userIdClaim} claim ");
        }


        private byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            string CombineWithKeyPlusPasswordSalt = _config.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(CombineWithKeyPlusPasswordSalt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                );

        }



        private string CreateToken(int userId)
        {
            Claim[] claim = new Claim[] {
        new Claim("userId", userId.ToString())
        };

            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                (_config.GetSection("AppSettings:PasswordKey").Value));

            SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claim),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(securityToken);

        }
    }



}
