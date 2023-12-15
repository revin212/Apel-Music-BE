using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.MsCategory;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Mvc;

namespace fs_12_team_1_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MsCategoryController : ControllerBase
    {
        private readonly MsCategoryData _msCategoryData;
        public MsCategoryController(MsCategoryData msCategoryData)
        {
            _msCategoryData = msCategoryData;
        }


        [HttpGet("GetShortList")]
        public IActionResult GetShortList()
        {
            try
            {
                List<MsCategoryShortListResDTO> msCategory = _msCategoryData.GetShortList();
                return Ok(msCategory);
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpGet("GetCategoryDetail")]
        public IActionResult GetCategoryDetail(Guid id)
        {
            try
            {
                MsCategoryDetailResDTO? msCategory = _msCategoryData.GetCategoryDetail(id);

                if (msCategory == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(msCategory); //200
            }
            catch
            {

                return StatusCode(500, "Server Error occured");
            }
        }

     
    }
}
