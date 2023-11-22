using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.MsCourse;
using fs_12_team_1_BE.DTO.MsUser;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Mvc;

namespace fs_12_team_1_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MsUserController : ControllerBase
    {
        private readonly MsUserData _msUserData;
        public MsUserController(MsUserData msUserData)
        {
            _msUserData = msUserData;
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

        [HttpPost("Register")]
        public IActionResult Register([FromBody] MsUserDTO msUserDto)
        {
            try
            {
                if (msUserDto == null)
                    return BadRequest("Data should be inputed");

                MsUserDTO msUser = new MsUserDTO
                {
                    //Id = Guid.NewGuid(),
                    Name = msUserDto.Name,
                    Email = msUserDto.Email,
                    Password = msUserDto.Password
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
        public IActionResult Put(Guid id, [FromBody] MsUserDTO msUserDto)
        {
            try
            {
                if (msUserDto == null)
                    return BadRequest("Data should be inputed");

                MsUserDTO msUser = new MsUserDTO
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
