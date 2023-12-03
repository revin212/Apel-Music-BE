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


        //[HttpGet("GetAll")]
        //public IActionResult GetAll()
        //{
        //    try
        //    {
        //        List<MsCourseResponseDTO> msCourse = _msCourseData.GetAll();
        //        return Ok(msCourse);
        //    }
        //    catch
        //    {
        //        return StatusCode(500, "Server Error occured");
        //    }
        //}

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

        //[HttpGet("GetByName")]
        //public IActionResult GetByName(string name)
        //{
        //    try
        //    {
        //        MsCourseResponseDTO? msCourse = _msCourseData.GetByName(name);

        //        if (msCourse == null)
        //        {
        //            return NotFound("Data not found");
        //        }

        //        return Ok(msCourse); //200
        //    }
        //    catch
        //    {
        //        return StatusCode(500, "Server Error occured");
        //    }
        //}

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
        //[HttpPost]
        //public IActionResult Post([FromBody] MsCourseDTO mscourseDto)
        //{
        //    try
        //    {
        //        if (mscourseDto == null)
        //            return BadRequest("Data should be inputed");

        //        MsCourse mscourse = new MsCourse
        //        {
        //            //Id = Guid.NewGuid(),
        //            Name = mscourseDto.Name,
        //            Description = mscourseDto.Description,
        //            Image = mscourseDto.Image,
        //            Price = mscourseDto.Price,
        //            CategoryId = mscourseDto.CategoryId
        //        };

        //        bool result = _msCourseData.Insert(mscourse);

        //        if (result)
        //        {
        //            return StatusCode(201, mscourse.Id);
        //        }
        //        else
        //        {
        //            return StatusCode(500, "Error occured");
        //        }
        //    }
        //    catch
        //    {

        //        return StatusCode(500, "Server Error occured");
        //    }
        //}

        //[HttpPut]
        //public IActionResult Put(Guid id, [FromBody] MsCourseDTO mscourseDto)
        //{
        //    try
        //    {
        //        if (mscourseDto == null)
        //            return BadRequest("Data should be inputed");

        //        MsCourse mscourse = new MsCourse
        //        {
        //            Name = mscourseDto.Name,
        //            Description = mscourseDto.Description,
        //            Image = mscourseDto.Image,
        //            Price = mscourseDto.Price,
        //            CategoryId = mscourseDto.CategoryId


        //        };

        //        bool result = _msCourseData.Update(id, mscourse);

        //        if (result)
        //        {
        //            return NoContent();//204
        //        }
        //        else
        //        {
        //            return StatusCode(500, "Error occured");
        //        }
        //    }
        //    catch
        //    {

        //        return StatusCode(500, "Server Error occured");
        //    }
        //}

        //[HttpDelete]
        //public IActionResult Delete(Guid id)
        //{
        //    try
        //    {
        //        bool result = _msCourseData.Delete(id);

        //        if (result)
        //        {
        //            return NoContent();
        //        }
        //        else
        //        {
        //            return StatusCode(500, "Error occured");
        //        }
        //    }
        //    catch
        //    {

        //        return StatusCode(500, "Server Error occured");
        //    }
        //}

    }
}
