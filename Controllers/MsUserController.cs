using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.MsUser;
using fs_12_team_1_BE.Email;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using Org.BouncyCastle.Crypto.Generators;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace fs_12_team_1_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MsUserController : ControllerBase
    {
        private readonly MsUserData _msUserData;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        public MsUserController(MsUserData msUserData, IConfiguration configuration, EmailService emailService)
        {
            _msUserData = msUserData;
            _configuration = configuration;
            _emailService = emailService;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<MsUserDTO> msUser = _msUserData.GetAll();
                return Ok(msUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetById")]
        [Authorize]
        public IActionResult Get(Guid id)
        {
            try
            {
                MsUserDTO? msUser = _msUserData.GetById(id);

                if (msUser == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(msUser); //200
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] MsUserLoginDTO credential)
        {
            if (credential is null)
                return BadRequest("Invalid client request");

            if (string.IsNullOrEmpty(credential.Email) || string.IsNullOrEmpty(credential.Password))
                return BadRequest("Invalid client request");

            MsUser? user = _msUserData.CheckUser(credential.Email);

            if (user == null)
                return Unauthorized("You do not authorized");
            else
            {
                //bool isVerified = user?.Password == credential.Password;
                bool isVerified = BCrypt.Net.BCrypt.Verify(credential.Password, user.Password);

                if (!isVerified)
                {
                    return BadRequest("Incorrect Password! Please check your password!");
                }
                else
                {
                    string token = GenerateToken(user.Email);
                    DateTime TokenExpires = DateTime.UtcNow.AddMinutes(15);
                    MsUserRefreshToken refreshToken = GenerateRefreshToken(credential.Email);
                    _msUserData.UpdateRefreshToken(refreshToken);

                    return Ok(new LoginResponseDTO { Email = credential.Email, Token = token, RefreshToken = refreshToken.RefreshToken, TokenExpires = TokenExpires });
                }
            }
        }

        [HttpPost("refresh-token")]
        public ActionResult RefreshToken(string refreshToken, string Email)
        {
            MsUserRefreshToken dbRefreshToken = _msUserData.GetRefreshToken(Email);

            if (!dbRefreshToken.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (dbRefreshToken.ExpiredAt < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string newToken = GenerateToken(Email);
            DateTime newTokenExpires = DateTime.UtcNow.AddMinutes(15);
            MsUserRefreshToken newRefreshToken = GenerateRefreshToken(Email);
            _msUserData.UpdateRefreshToken(newRefreshToken);

            return Ok(new LoginResponseDTO { Email = Email, Token = newToken, RefreshToken = newRefreshToken.RefreshToken, TokenExpires = newTokenExpires });
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] MsUserRegisterDTO msUserDto)
        {
            try
            {
                if (msUserDto == null)
                    return BadRequest("Data should be inputed");

                MsUserRegisterDTO msUser = new MsUserRegisterDTO
                {
                    Name = msUserDto.Name,
                    Email = msUserDto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(msUserDto.Password),
                };

                bool result = _msUserData.Register(msUser);

                if (result)
                {
                    bool mailResult = await SendEmailActivation(msUser);
                    return StatusCode(201, "Register Berhasil");
                }
                else
                {
                    return StatusCode(500, "Error occured");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string GenerateToken(string Email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("JwtConfig:Key").Value));

            var claims = new Claim[] {
                    new Claim(ClaimTypes.Email, Email),
                    };

            var signingCredential = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = signingCredential
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            string token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        private MsUserRefreshToken GenerateRefreshToken(string Email)
        {
            var refreshToken = new MsUserRefreshToken
            {
                UserEmail = Email,
                RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                CreatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddDays(7)
            };
            return refreshToken;
        }

        [HttpPut]
        public IActionResult Put(Guid id, [FromBody] MsUserRegisterDTO msUserDto)
        {
            try
            {
                if (msUserDto == null)
                    return BadRequest("Data should be inputed");

                MsUserRegisterDTO msUser = new MsUserRegisterDTO
                {
                    Name = msUserDto.Name,
                    Email = msUserDto.Email,
                    Password = msUserDto.Password
                };

                bool result = _msUserData.Update(id, msUser);

                if (result)
                {
                    return NoContent();//204
                }
                else
                {
                    return StatusCode(500, "Error occured");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("SoftDelete")]
        public IActionResult SoftDelete(Guid id)
        {
            try
            {
                bool result = _msUserData.SoftDelete(id);

                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(500, "Error occured");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("ActivateUser")]
        public IActionResult ActivateUser(string email)
        {
            try
            {
                MsUser? user = _msUserData.CheckUser(email);

                if (user == null)
                    return BadRequest("Activation Failed");

                if (user.IsActivated == true)
                    return BadRequest("User has been activated");

                bool result = _msUserData.ActivateUser(email);

                if (result)
                    return Ok("User activated");
                else
                    return StatusCode(500, "Activation Failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private async Task<bool> SendEmailActivation(MsUserRegisterDTO user)
        {
            if (user == null)
                return false;

            if (string.IsNullOrEmpty(user.Email))
                return false;

            // send email
            List<string> to = new List<string>();
            to.Add(user.Email);

            string subject = "Account Activation";
            var param = new Dictionary<string, string?>
                    {
                        {"Email", user.Email }
                    };

            string callbackUrl = QueryHelpers.AddQueryString("https://localhost:7201/api/MsUser/ActivateUser", param);

            //string body = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
            string body = _emailService.GetEmailTemplate(new EmailActivation
            {
                Email = user.Email,
                Link = callbackUrl
            });


            EmailModel mailModel = new EmailModel(to, subject, body);
            bool mailResult = await _emailService.SendAsync(mailModel, new CancellationToken());
            return mailResult;
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest("Email is empty");

                bool sendMail = await SendEmailForgetPassword(email);

                if (sendMail)
                {
                    return Ok("Mail sent");
                }
                else
                {
                    return StatusCode(500, "Error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDTO resetPassword)
        {
            try
            {
                if (resetPassword == null)
                    return BadRequest("No Data");

                if (resetPassword.Password != resetPassword.ConfirmPassword)
                {
                    return BadRequest("Password doesn't match");
                }

                bool reset = _msUserData.ResetPassword(resetPassword.Email, BCrypt.Net.BCrypt.HashPassword(resetPassword.Password));

                if (reset)
                {
                    return Ok("Reset password OK");
                }
                else
                {
                    return StatusCode(500, "Error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        private async Task<bool> SendEmailForgetPassword(string email)
        {
            // send email
            List<string> to = new List<string>();
            to.Add(email);

            string subject = "Forget Password";

            var param = new Dictionary<string, string?>
                    {
                        {"email", email }
                    };

            string callbackUrl = QueryHelpers.AddQueryString("https://localhost:3000/formReset", param);

            string body = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";

            EmailModel mailModel = new EmailModel(to, subject, body);

            bool mailResult = await _emailService.SendAsync(mailModel, new CancellationToken());

            return mailResult;
        }
    }
}
