using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.TsOrderDetail;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Mvc;

namespace fs_12_team_1_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TsOrderDetailController : ControllerBase
    {
        private readonly TsOrderDetailData _tsOrderDetailData;
        public TsOrderDetailController(TsOrderDetailData tsOrderDetailData)
        {
            _tsOrderDetailData = tsOrderDetailData;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            List<TsOrderDetail> tsOrderDetail = _tsOrderDetailData.GetAll();
            return Ok(tsOrderDetail);
        }

        [HttpGet("GetById")]
        public IActionResult Get(Guid id)
        {
            TsOrderDetail? tsOrderDetail = _tsOrderDetailData.GetById(id);

            if (tsOrderDetail == null)
            {
                return NotFound("Data not found");
            }

            return Ok(tsOrderDetail); //200
        }

        [HttpPost]
        public IActionResult Post([FromBody] TsOrderDetailDTO tsorderDto)
        {
            if (tsorderDto == null)
                return BadRequest("Data should be inputed");

            TsOrderDetail tsorder = new TsOrderDetail
            {
                //Id = Guid.NewGuid(),
                OrderId = tsorderDto.OrderId,
                CourseId = tsorderDto.CourseId,
                IsActive = tsorderDto.IsActive
            };

            bool result = _tsOrderDetailData.Insert(tsorder);

            if (result)
            {
                return StatusCode(201, tsorder.Id);
            }
            else
            {
                return StatusCode(500, "Error occured");
            }
        }

        [HttpPut]
        public IActionResult Put(Guid id, [FromBody] TsOrderDetailDTO tsorderDto)
        {
            if (tsorderDto == null)
                return BadRequest("Data should be inputed");

            TsOrderDetail tsorder = new TsOrderDetail
            {
                OrderId = tsorderDto.OrderId,
                CourseId = tsorderDto.CourseId,
                IsActive = tsorderDto.IsActive
            };

            bool result = _tsOrderDetailData.Update(id, tsorder);

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
            bool result = _tsOrderDetailData.Delete(id);

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
