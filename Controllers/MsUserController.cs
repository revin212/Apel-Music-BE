using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.MsUser;
using fs_12_team_1_BE.Email;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
            catch
            {
                return StatusCode(500, "Server Error occured");
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

                return Ok(msUser);
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] MsUserLoginDTO credential)
        {
            try
            {
                if (credential is null)
                    return BadRequest("Invalid client request");

                if (string.IsNullOrEmpty(credential.Email) || string.IsNullOrEmpty(credential.Password))
                    return BadRequest("Invalid client request");

                MsUser? user = _msUserData.CheckUser(credential.Email);

                if (user == null)
                    return Unauthorized("User doesn't exist");
                else
                {
                    if (user.IsDeleted)
                        return BadRequest("Account deleted");

                    bool isVerified = BCrypt.Net.BCrypt.Verify(credential.Password, user.Password);

                    if (!isVerified)
                    {
                        return BadRequest("Incorrect Password");
                    }

                    if (!user.IsActivated)
                        return BadRequest("Please Activate your account first");

                    string token = GenerateToken(user.Email);
                    DateTime TokenExpires = DateTime.UtcNow.AddMinutes(15);
                    MsUserRefreshToken refreshToken = GenerateRefreshToken(credential.Email);
                    SetRefreshTokenCookies(refreshToken, user.Id.ToString() ?? string.Empty);
                    _msUserData.UpdateRefreshToken(refreshToken);

                    return Ok(new LoginResponseDTO { Token = token, TokenExpires = TokenExpires });
                }
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpPost("RefreshToken")]
        public ActionResult RefreshToken()
        {
            try
            {
                string refreshToken = Request.Cookies["refreshToken"] ?? String.Empty;
                string Email = Request.Cookies["email"] ?? String.Empty;
                string Id = Request.Cookies["userId"] ?? String.Empty;
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
                SetRefreshTokenCookies(newRefreshToken, Id);
                _msUserData.UpdateRefreshToken(newRefreshToken);

                return Ok(new LoginResponseDTO { Token = newToken, TokenExpires = newTokenExpires });
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] MsUserRegisterDTO msUserDto)
        {
            try
            {
                if (msUserDto == null)
                    return BadRequest("Data should be inputed");

                if (!_emailService.IsValidEmail(msUserDto.Email))
                    return BadRequest("Invalid Email Address");

                MsUser? user = _msUserData.CheckUser(msUserDto.Email);

                if (user != null)
                    return BadRequest("This email address is already used by another account");

                if(msUserDto.Password != msUserDto.ConfirmPassword)
                    return BadRequest("Password do not match");


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
            catch
            {
                return StatusCode(500, "Server Error occured");
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

        private void SetRefreshTokenCookies(MsUserRefreshToken newRefreshToken, string Id)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = newRefreshToken.ExpiredAt
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.RefreshToken, cookieOptions);
            Response.Cookies.Append("email", newRefreshToken.UserEmail, cookieOptions);
            Response.Cookies.Append("userId", Id, cookieOptions);
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
                    return StatusCode(201, "User Activated");
                else
                    return StatusCode(500, "Activation Failed");
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
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
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                    return BadRequest("Email is empty");

                MsUser? user = _msUserData.CheckUser(Email);

                if (user == null)
                    return NotFound("User doesn't exist");

                bool sendMail = await SendEmailForgetPassword(user.Id.ToString() ?? string.Empty, Email);

                if (sendMail)
                {
                    return StatusCode(201, "Please check your email to get the link for reset password");
                }
                else
                {
                    return StatusCode(500, "Server Error, Please try again");
                }
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
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

                bool reset = _msUserData.ResetPassword(resetPassword.Id, BCrypt.Net.BCrypt.HashPassword(resetPassword.Password));

                if (reset)
                {
                    return StatusCode(201, "Reset password success");
                }
                else
                {
                    return StatusCode(500, "Server Error occured");
                }
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }


        private async Task<bool> SendEmailForgetPassword(string Id, string Email)
        {
            // send email
            List<string> to = new List<string>();
            to.Add(Email);

            string subject = "Forget Password";

            var param = new Dictionary<string, string?>
                    {
                        { "Id", Id },
                    };

            string callbackUrl = QueryHelpers.AddQueryString("https://localhost:3000/formReset", param);

            string body = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";

            EmailModel mailModel = new EmailModel(to, subject, body);

            bool mailResult = await _emailService.SendAsync(mailModel, new CancellationToken());

            return mailResult;
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
                    return StatusCode(201, "Edit user success");
                }
                else
                {
                    return StatusCode(500, "Error occured");
                }
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
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
                    return StatusCode(201, "Account soft deleted");
                }
                else
                {
                    return StatusCode(500, "Error occured");
                }
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpDelete("HardDelete")]
        public IActionResult HardDelete(Guid id)
        {
            try
            {
                bool result = _msUserData.SoftDelete(id);

                if (result)
                {
                    return StatusCode(201, "Account deleted");
                }
                else
                {
                    return StatusCode(500, "Error occured");
                }
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }
    }
}
