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


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            List<MsCategory> msCategory = _msCategoryData.GetAll();
            return Ok(msCategory);
        }

        [HttpGet("GetById")]
        public IActionResult Get(Guid id)
        {
            MsCategory? msCategory = _msCategoryData.GetById(id);

            if (msCategory == null)
            {
                return NotFound("Data not found");
            }

            return Ok(msCategory); //200
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string Name)
        {
            MsCategory? msCategory = _msCategoryData.GetByName(Name);

            if (msCategory == null)
            {
                return NotFound("Data not found");
            }

            return Ok(msCategory); //200
        }

        [HttpPost]
        public IActionResult Post([FromBody] MsCategoryDTO msCategoryDto)
        {
            if (msCategoryDto == null)
                return BadRequest("Data should be inputed");

            MsCategory msCategory = new MsCategory
            {
                //Id = Guid.NewGuid(),
                Name = msCategoryDto.Name,
                Title = msCategoryDto.Title,
                Description = msCategoryDto.Description,
                Image = msCategoryDto.Image,
                HeaderImage = msCategoryDto.HeaderImage,
            };

            bool result = _msCategoryData.Insert(msCategory);

            if (result)
            {
                return StatusCode(201, "Category created");
            }
            else
            {
                return StatusCode(500, "Error occured");
            }
        }

        [HttpPut]
        public IActionResult Put(Guid id, [FromBody] MsCategoryDTO msCategoryDto)
        {
            if (msCategoryDto == null)
                return BadRequest("Data should be inputed");

            MsCategory msCategory = new MsCategory
            {
                Name = msCategoryDto.Name,
                Title = msCategoryDto.Title,
                Description = msCategoryDto.Description,
                Image = msCategoryDto.Image,
                HeaderImage = msCategoryDto.HeaderImage,
            };

            bool result = _msCategoryData.Update(id, msCategory);

            if (result)
            {
                return NoContent();//204
            }
            else
            {
                return StatusCode(500, "Error occured");
            }
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            bool result = _msCategoryData.Delete(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "Error occured");
            }
        }

    }
}
