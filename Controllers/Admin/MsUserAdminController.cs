using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DataAccess.Admin;
using fs_12_team_1_BE.DTO.Admin;
using fs_12_team_1_BE.DTO.Admin.MsUserAdmin;
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
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class MsUserAdminController : ControllerBase
    {
        private readonly MsUserAdminData _msUserData;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        public MsUserAdminController(MsUserAdminData msUserData, IConfiguration configuration, EmailService emailService)
        {
            _msUserData = msUserData;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpGet("GetUserClass")]
        public IActionResult GetUserClass(Guid userid) {
            try
            {
                List<MsUserAdminGetUserClassListResDTO> myclass = _msUserData.GetUserClass(userid);
                return Ok(myclass);
            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error occured");
                
            }
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<MsUserAdminDTO> msUser = _msUserData.GetAll();
                return Ok(msUser);
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }
        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            try
            {
                List<MsRoleAdminDTO> msUser = _msUserData.GetRoles();
                return Ok(msUser);
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpGet("GetById")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                MsUserAdminDTO msUser = _msUserData.GetById(id);

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

       

        [HttpPatch("Update")]
        public IActionResult Update(Guid id, [FromBody] MsUserAdminCreateDTO msUserDto)
        {
            try
            {
                if (msUserDto == null)
                    return BadRequest("Data should be inputed");
                if (msUserDto.Password != msUserDto.ConfirmPassword)
                    return BadRequest("Password does not match");

                if (msUserDto.Password != "")
                    msUserDto.Password = BCrypt.Net.BCrypt.HashPassword(msUserDto.Password);

                bool result = _msUserData.Update(id, msUserDto);

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

        [HttpPatch("ToggleActiveStatus")]
        public IActionResult ToggleActiveStatus(Guid id, [FromBody] ToggleActiveStatusDTO msUser)
        {
            try
            {
                if (msUser == null)
                    return BadRequest("Data should be inputed");

                bool result = _msUserData.ToggleActiveStatus(id, msUser);

                if (result)
                {
                    return StatusCode(201, "Toggle active status success");
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
       

        [HttpPost("Create")]
        public IActionResult Create([FromBody] MsUserAdminCreateDTO msUserDto)
        {
            try
            {
                if (msUserDto == null)
                    return BadRequest("Data should be inputed");

                if (!_emailService.IsValidEmail(msUserDto.Email))
                    return BadRequest("Invalid Email Address");

                Guid user = _msUserData.CheckUser(msUserDto.Email);
                   
                if (user != Guid.Empty)
                    return BadRequest("This email address is already used by another account");

                if (msUserDto.Password != msUserDto.ConfirmPassword)
                    return BadRequest("Password does not match");

                msUserDto.Password = BCrypt.Net.BCrypt.HashPassword(msUserDto.Password);

                bool result = _msUserData.Create(msUserDto);

                if (result)
                {
                    return StatusCode(201, "Create Berhasil");
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
