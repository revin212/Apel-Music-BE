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


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<MsCourseResponseDTO> msCourse = _msCourseData.GetAll();
                return Ok(msCourse);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetRecommendedCourses")]
        public IActionResult GetRecommendedCourses()
        {
            try
            {
                List<MsCourseResponseDTO> msCourse = _msCourseData.GetRecommendedCourses();
                return Ok(msCourse);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[HttpGet("GetById")]
        //public IActionResult Get(Guid id)
        //{
        //    try
        //    {
        //        MsCourseResponseDTO? msCourse = _msCourseData.GetById(id);

        //        if (msCourse == null)
        //        {
        //            return NotFound("Data not found");
        //        }

        //        return Ok(msCourse); //200
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            try
            {
                MsCourseResponseDTO? msCourse = _msCourseData.GetByName(name);

                if (msCourse == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(msCourse); //200
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetByCategory")]
        public IActionResult GetByCategory(string CategoryName)
        {
            try
            {
                MsCourseResponseDTO? msCourse = _msCourseData.GetByCategory(CategoryName);

                if (msCourse == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(msCourse); //200
            }
            catch (Exception)
            {
                throw;
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
        //    catch (Exception)
        //    {

        //        throw;
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
        //    catch (Exception)
        //    {

        //        throw;
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
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

    }
}
