using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DataAccess.Admin;
using fs_12_team_1_BE.DTO.Admin.MsCategoryAdmin;
using fs_12_team_1_BE.DTO.Admin;
using fs_12_team_1_BE.DTO.Admin.MsCourseAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using fs_12_team_1_BE.Utilities;
namespace fs_12_team_1_BE.Controllers.Admin
{
    [Authorize(Roles ="Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class MsCourseAdminController : ControllerBase
    {
        private readonly MsCourseAdminData _msCourseAdminData;
        private ImageSaverUtil _imageSaver;
        public MsCourseAdminController(MsCourseAdminData msCourseAdminData, ImageSaverUtil imageSaver)
        {
            _msCourseAdminData = msCourseAdminData;
            _imageSaver = imageSaver;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<MsCourseAdminDTO> msCourse = _msCourseAdminData.GetAll();
                return Ok(msCourse);
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }
        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            try
            {
                List<MsCategoryAdminDTO> msCourse = _msCourseAdminData.GetCategories();
                return Ok(msCourse);
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
                MsCourseAdminDTO msUser = _msCourseAdminData.GetById(id);

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
        public IActionResult Update(Guid id, [FromBody] MsCourseAdminDTO MsCourseAdminDTO)
        {
            try
            {
                if (MsCourseAdminDTO == null)
                    return BadRequest("Data should be inputed");

                if (MsCourseAdminDTO.Image.Length > 50)
                {
                    MsCourseAdminDTO.Image = _imageSaver.SaveImageToFile(MsCourseAdminDTO.Image, id);
                }

                bool result = _msCourseAdminData.Update(id, MsCourseAdminDTO);

                if (result)
                {
                    return StatusCode(201, "Edit category success");
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
        public IActionResult ToggleActiveStatus(Guid id, [FromBody] ToggleActiveStatusDTO msCategory)
        {
            try
            {
                if (msCategory == null)
                    return BadRequest("Data should be inputed");

                bool result = _msCourseAdminData.ToggleActiveStatus(id, msCategory);

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
        public IActionResult Create([FromBody] MsCourseAdminDTO MsCourseAdminDTO)
        {
            try
            {
                if (MsCourseAdminDTO == null)
                    return BadRequest("Data should be inputed");

                MsCourseAdminDTO.Id = Guid.NewGuid();
                MsCourseAdminDTO.Image = _imageSaver.SaveImageToFile(MsCourseAdminDTO.Image, MsCourseAdminDTO.Id);
                bool result = _msCourseAdminData.Create(MsCourseAdminDTO);

                if (result)
                {
                    return StatusCode(201, "Create Category Success");
                }
                else
                {
                    return StatusCode(500, "Error Occured");
                }
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }
    }
}
