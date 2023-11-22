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
            List<TsOrderDetail> tsOrder = _tsOrderDetailData.GetAll();
            return Ok(tsOrder);
        }

        [HttpGet("GetById")]
        public IActionResult Get(Guid id)
        {
            TsOrderDetail? tsOrder = _tsOrderDetailData.GetById(id);

            if (tsOrder == null)
            {
                return NotFound("Data not found");
            }

            return Ok(tsOrder); //200
        }

        [HttpPost]
        public IActionResult Post([FromBody] TsOrderDTO tsorderDto)
        {
            if (tsorderDto == null)
                return BadRequest("Data should be inputed");

            TsOrderDetail tsorder = new TsOrderDetail
            {
                //Id = Guid.NewGuid(),
                UserId = tsorderDto.UserId,
                PaymentId = tsorderDto.PaymentId,
                InvoiceNo = tsorderDto.InvoiceNo,
                OrderDate = tsorderDto.OrderDate,
                IsPaid = tsorderDto.IsPaid
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
        public IActionResult Put(Guid id, [FromBody] TsOrderDTO tsorderDto)
        {
            if (tsorderDto == null)
                return BadRequest("Data should be inputed");

            TsOrderDetail tsorder = new TsOrderDetail
            {
                UserId = tsorderDto.UserId,
                PaymentId = tsorderDto.PaymentId,
                InvoiceNo = tsorderDto.InvoiceNo,
                OrderDate = tsorderDto.OrderDate,
                IsPaid = tsorderDto.IsPaid


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
