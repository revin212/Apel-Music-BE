﻿using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DataAccess.Admin;
using fs_12_team_1_BE.DTO.Admin;
using fs_12_team_1_BE.DTO.Admin.MsCategoryAdmin;
using fs_12_team_1_BE.DTO.Admin.MsUserAdmin;
using fs_12_team_1_BE.Email;
using fs_12_team_1_BE.Model;
using fs_12_team_1_BE.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace fs_12_team_1_BE.Controllers.Admin
{

    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MsCategoryAdminController : ControllerBase
    {
        private readonly MsCategoryAdminData _msCategoryAdminData;
        private readonly IConfiguration _configuration;
        private ImageSaverUtil _imageSaver;
        
        public MsCategoryAdminController(MsCategoryAdminData msCategoryAdminData, IConfiguration configuration, ImageSaverUtil imageSaver)
        {
            _msCategoryAdminData = msCategoryAdminData;
            _configuration = configuration;
            _imageSaver = imageSaver;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<MsCategoryAdminDTO> msCategory = _msCategoryAdminData.GetCategoryAll();
                return Ok(msCategory);
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
                MsCategoryAdminDTO msUser = _msCategoryAdminData.GetCategoryById(id);

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
        public IActionResult Update(Guid id, [FromBody] MsCategoryAdminDTO MsCategoryAdminDTO)
        {
            try
            {
                if (MsCategoryAdminDTO == null)
                    return BadRequest("Data should be inputed");

                if (MsCategoryAdminDTO.Image.Length > 50)
                {
                    MsCategoryAdminDTO.Image = _imageSaver.SaveImageToFile(MsCategoryAdminDTO.Image, id);
                }

                bool result = _msCategoryAdminData.Update(id, MsCategoryAdminDTO);
                
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

                bool result = _msCategoryAdminData.ToggleActiveStatus(id, msCategory);

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
        public IActionResult Create([FromBody] MsCategoryAdminCreateDTO MsCategoryAdminDTO)
        {
            try
            {
                if (MsCategoryAdminDTO == null)
                    return BadRequest("Data should be inputed");

                bool available = _msCategoryAdminData.CheckCategory(MsCategoryAdminDTO.Name);

                if (!available)
                {
                    return Unauthorized("Please use another category name");
                }

                MsCategoryAdminDTO.Id = Guid.NewGuid();
                MsCategoryAdminDTO.Image = _imageSaver.SaveImageToFile(MsCategoryAdminDTO.Image, MsCategoryAdminDTO.Id);
                bool result = _msCategoryAdminData.CreateCategory(MsCategoryAdminDTO);
                
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

