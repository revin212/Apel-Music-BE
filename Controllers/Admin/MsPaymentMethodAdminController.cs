using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DataAccess.Admin;
using fs_12_team_1_BE.DTO.Admin.MsCategoryAdmin;
using fs_12_team_1_BE.DTO.Admin.MsPaymentMethod;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Authorization;
using fs_12_team_1_BE.Utilities;
using Microsoft.AspNetCore.Mvc;
using fs_12_team_1_BE.DTO.Admin;
using fs_12_team_1_BE.DTO.Admin.MsCourseAdmin;

namespace fs_12_team_1_BE.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MsPaymentMethodAdminController : ControllerBase
    {
        private readonly MsPaymentMethodAdminData _msPaymentMethodAdminData;
        private ImageSaverUtil _imageSaver;

        public MsPaymentMethodAdminController(MsPaymentMethodAdminData msPaymentMethodAdminData, ImageSaverUtil imageSaver)
        {
            _msPaymentMethodAdminData = msPaymentMethodAdminData;
            _imageSaver = imageSaver;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<MsPaymentMethodAdminDTO> msPaymentMethod = _msPaymentMethodAdminData.GetAll();
                return Ok(msPaymentMethod);
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpGet("GetById")]
        public IActionResult Get(Guid id)
        {
            try
            {
                MsPaymentMethodAdminDTO? msPaymentMethod = _msPaymentMethodAdminData.GetById(id);

                if (msPaymentMethod == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(msPaymentMethod);
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] MsPaymentMethodAdminDTO MsPaymentMethodAdminCreate)
        {
            try
            {
                if (MsPaymentMethodAdminCreate == null)
                    return BadRequest("Data should be inputed");

                bool available = _msPaymentMethodAdminData.CheckPaymentMethod(MsPaymentMethodAdminCreate.Name);

                if (!available)
                {
                    return Unauthorized("Please use another payment method name");
                }

                MsPaymentMethodAdminCreate.Id = Guid.NewGuid();
                MsPaymentMethodAdminCreate.Image = _imageSaver.SaveImageToFile(MsPaymentMethodAdminCreate.Image, MsPaymentMethodAdminCreate.Id);
                bool result = _msPaymentMethodAdminData.CreatePaymentMethod(MsPaymentMethodAdminCreate);

                if (result)
                {
                    return StatusCode(201, "Create payment method success");
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

        [HttpPatch("Update")]
        public IActionResult Update(Guid id, [FromBody] MsPaymentMethodAdminDTO MsPaymentMethodAdminUpdate)
        {
            try
            {
                if (MsPaymentMethodAdminUpdate == null)
                    return BadRequest("Data should be inputed");

                if (MsPaymentMethodAdminUpdate.Image.Length > 50)
                {
                    MsPaymentMethodAdminUpdate.Image = _imageSaver.SaveImageToFile(MsPaymentMethodAdminUpdate.Image, id);
                }

                bool result = _msPaymentMethodAdminData.Update(id, MsPaymentMethodAdminUpdate);

                if (result)
                {
                    return StatusCode(201, "Edit payment method success");
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
        public IActionResult ToggleActiveStatus(Guid id, [FromBody] ToggleActiveStatusDTO msPaymentMethod)
        {
            try
            {
                if (msPaymentMethod == null)
                    return BadRequest("Data should be inputed");

                bool result = _msPaymentMethodAdminData.ToggleActiveStatus(id, msPaymentMethod);

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
    }
}
