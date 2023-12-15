using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.MsCourse;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Mvc;

namespace fs_12_team_1_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MsCourseController : ControllerBase
    {
        private readonly MsCourseData _msCourseData;
        public MsCourseController(MsCourseData msCourseData)
        {
            _msCourseData = msCourseData;
        }

        [HttpGet("GetFavoriteList")]
        public IActionResult GetFavoriteList()
        {
            try
            {
                List<MsCourseGetFavoriteListResDTO> msCourse = _msCourseData.GetFavoriteList();
                return Ok(msCourse);
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpGet("GetDetail")]
        public IActionResult GetDetail(Guid id)
        {
            try
            {
                MsCourseGetDetailResDTO? msCourse = _msCourseData.GetDetail(id);

                if (msCourse == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(msCourse); //200
            }
            catch
            {

                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpGet("GetByCategoryList")]
        public IActionResult GetByCategoryList(Guid id)
        {
            try
            {
                List<MsCourseGetByCategoryListResDTO> msCourse = _msCourseData.GetByCategoryList(id);

                if (msCourse == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(msCourse); //200
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpGet("GetOtherList")]
        public IActionResult GetOtherList(Guid categoryid, Guid courseid)
        {
            try
            {
                List<MsCourseGetOtherListRes> msCourse = _msCourseData.GetOtherList(categoryid, courseid);

                if (msCourse == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(msCourse); //200
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }
    }        
}
