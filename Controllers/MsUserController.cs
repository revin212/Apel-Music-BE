using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.MsUser;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace fs_12_team_1_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MsUserController : ControllerBase
    {
        private readonly MsUserData _msUserData;
        private readonly IConfiguration _configuration;
        public MsUserController(MsUserData msUserData, IConfiguration configuration)
        {
            _msUserData = msUserData;
            _configuration = configuration;
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
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("JwtConfig:Key").Value));

                    var claims = new Claim[] {
                    new Claim(ClaimTypes.Email, user.Email),
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

                    return Ok(new LoginResponseDTO { Token = token });
                }
            }
        }


        [HttpPost("Register")]
        public IActionResult Register([FromBody] MsUserRegisterDTO msUserDto)
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
    }
}
