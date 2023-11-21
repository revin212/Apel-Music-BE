using fs_12_team_1_BE.DataAccess;
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

        //[HttpPost]
        //public IActionResult Post([FromBody] BookDTO bookDto)
        //{
        //    if (bookDto == null)
        //        return BadRequest("Data should be inputed");

        //    Book book = new Book
        //    {
        //        Id = Guid.NewGuid(),
        //        Title = bookDto.Title,
        //        Description = bookDto.Description,
        //        Author = bookDto.Author,
        //        Stock = bookDto.Stock,
        //        Created = DateTime.Now,
        //        Updated = DateTime.Now,
        //    };

        //    bool result = _bookData.Insert(book);

        //    if (result)
        //    {
        //        return StatusCode(201, book.Id);
        //    }
        //    else
        //    {
        //        return StatusCode(500, "Error occur");
        //    }
        //}

        //[HttpPut]
        //public IActionResult Put(Guid id, [FromBody] BookDTO bookDto)
        //{
        //    if (bookDto == null)
        //        return BadRequest("Data should be inputed");

        //    Book book = new Book
        //    {
        //        Title = bookDto.Title,
        //        Description = bookDto.Description,
        //        Author = bookDto.Author,
        //        Stock = bookDto.Stock,
        //        Updated = DateTime.Now
        //    };

        //    bool result = _bookData.Update(id, book);

        //    if (result)
        //    {
        //        return NoContent();//204
        //    }
        //    else
        //    {
        //        return StatusCode(500, "Error occur");
        //    }
        //}

        //[HttpDelete]
        //public IActionResult Delete(Guid id)
        //{
        //    bool result = _bookData.Delete(id);

        //    if (result)
        //    {
        //        return NoContent();
        //    }
        //    else
        //    {
        //        return StatusCode(500, "Error occur");
        //    }
        //}

    }
}
