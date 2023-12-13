using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.MsUser;
using fs_12_team_1_BE.Email;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("GetMyClass")]
        [Authorize]
        public IActionResult GetMyClass(Guid userid) {
            try
            {
                List<MsUserGetMyClassListResDTO> myclass = _msUserData.GetMyClass(userid);
                return Ok(myclass);
            }
            catch (Exception)
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

                    bool isVerified = BCrypt.Net.BCrypt.Verify(credential.Password, user.Password);

                    if (!isVerified)
                    {
                        return BadRequest("Incorrect Password");
                    }

                    if (!user.IsActivated)
                        return BadRequest("Please Activate your account first");

                    string token = GenerateToken(user.Email, user.RoleName);
                    DateTime TokenExpires = DateTime.UtcNow.AddMinutes(15);
                    RefreshTokenDTO refreshToken = GenerateRefreshToken(credential.Email, user.RoleName);
                    //SetRefreshTokenCookies(refreshToken);
                    _msUserData.UpdateRefreshToken(refreshToken);

                    return Ok(new LoginResponseDTO { Token = token, TokenExpires = TokenExpires, UserId =  user.Id.ToString() ?? string.Empty, RoleName = RoleNameEncoder(user.RoleName), RefreshToken = refreshToken });
                }
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpPost("RefreshToken")]
        public ActionResult RefreshToken([FromBody] RefreshTokenDTO refreshToken)
        {
            try
            {
                //string refreshToken = Request.Cookies["refreshToken"] ?? string.Empty;
                //string Email = Request.Cookies["email"] ?? string.Empty;

                RefreshTokenDTO? dbRefreshToken = _msUserData.GetRefreshToken(refreshToken.Email);

                if (dbRefreshToken == null)
                {
                    return Unauthorized("User does not exist");
                }

                if (!dbRefreshToken.RefreshToken.Equals(refreshToken.RefreshToken))
                {
                    return Unauthorized("Invalid Refresh Token.");
                }
                else if (dbRefreshToken.RefreshTokenExpires < DateTime.Now)
                {
                    return Unauthorized("Token expired.");
                }

                string newToken = GenerateToken(refreshToken.Email, dbRefreshToken.RoleName);
                DateTime newTokenExpires = DateTime.UtcNow.AddMinutes(15);
                RefreshTokenDTO newRefreshToken = GenerateRefreshToken(refreshToken.Email, dbRefreshToken.RoleName);
                //SetRefreshTokenCookies(newRefreshToken);
                _msUserData.UpdateRefreshToken(newRefreshToken);

                return Ok(new LoginResponseDTO { Token = newToken, TokenExpires = newTokenExpires, UserId = dbRefreshToken.UserId, RoleName = RoleNameEncoder(dbRefreshToken.RoleName), RefreshToken = newRefreshToken });
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout([FromBody] MsUserLogoutDTO LogoutDTO)
        {
            try
            {
                bool isLoggedOut = false;
                
                isLoggedOut = _msUserData.Logout(LogoutDTO.Email);

                if(isLoggedOut)
                    return StatusCode(204);
                else
                {
                    return BadRequest("Already logged out");
                }
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

        private string GenerateToken(string Email, string Rolename)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("JwtConfig:Key").Value));

            var claims = new Claim[] {
                    new Claim(ClaimTypes.Email, Email),
                    new Claim(ClaimTypes.Role, Rolename),
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

        private RefreshTokenDTO GenerateRefreshToken(string Email, string RoleName)
        {
            var refreshToken = new RefreshTokenDTO
            {
                Email = Email,
                RoleName = RoleName,
                RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                RefreshTokenExpires = DateTime.Now.AddDays(7)
            };
            return refreshToken;
        }

        //private void SetRefreshTokenCookies(RefreshTokenDTO newRefreshToken)
        //{
        //    var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true,
        //        SameSite = SameSiteMode.Unspecified,
        //        Secure = false,
        //        Expires = newRefreshToken.RefreshTokenExpires
        //    };
        //    Response.Cookies.Append("refreshToken", newRefreshToken.RefreshToken, cookieOptions);
        //    Response.Cookies.Append("email", newRefreshToken.Email, cookieOptions);
        //}

        private string RoleNameEncoder(string RoleName)
        {
            if (RoleName == "User")
            {
                return _configuration.GetSection("RoleNameEncode:User").Value;
            }
            else if (RoleName == "Admin")
            {
                return _configuration.GetSection("RoleNameEncode:Admin").Value;
            }
            else return "";
        }
        private string RoleNameDecoder(string RoleNameEncoded)
        {
            if (RoleNameEncoded == _configuration.GetSection("RoleNameEncode:Admin").Value)
            {
                return "Admin";
            }
            else if (RoleNameEncoded == _configuration.GetSection("RoleNameEncode:User").Value)
            {
                return "User";
            }
            else return "User";
        }

        [HttpPost("ActivateUser")]
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

           
            List<string> to = new List<string>();
            to.Add(user.Email);

            string subject = "Account Activation";
            var param = new Dictionary<string, string?>
                    {
                        {"email", user.Email }
                    };
            string frontendUrl = _configuration["AllowedUrls:FrontEnd1"];
            string callbackUrl = QueryHelpers.AddQueryString(frontendUrl+"/email-confirm", param);

            
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
                if (!_emailService.IsValidEmail(Email))
                    return BadRequest("Invalid Email Address");

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
            
            List<string> to = new List<string>();
            to.Add(Email);

            string subject = "Forget Password";

            var param = new Dictionary<string, string?>
                    {
                        { "Id", Id },
                    };

            string callbackUrl = QueryHelpers.AddQueryString("http://localhost:5173/new-password", param);

            string body = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";

            EmailModel mailModel = new EmailModel(to, subject, body);

            bool mailResult = await _emailService.SendAsync(mailModel, new CancellationToken());

            return mailResult;
        }

    }
}
