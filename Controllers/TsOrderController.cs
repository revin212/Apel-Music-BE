using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.TsOrder;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Mvc;

namespace fs_12_team_1_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TsOrderController : ControllerBase
    {
        private readonly TsOrderData _tsOrderData;
        public TsOrderController(TsOrderData tsOrderData)
        {
            _tsOrderData = tsOrderData;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            List<TsOrder> tsOrder = _tsOrderData.GetAll();
            return Ok(tsOrder);
        }

        [HttpGet("GetById")]
        public IActionResult Get(Guid id)
        {
            TsOrder? tsOrder = _tsOrderData.GetById(id);

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

            TsOrder tsorder = new TsOrder
            {
                //Id = Guid.NewGuid(),
                UserId = tsorderDto.UserId,
                PaymentId = tsorderDto.PaymentId,
                InvoiceNo = tsorderDto.InvoiceNo,
                OrderDate = tsorderDto.OrderDate,
                IsPaid = tsorderDto.IsPaid
            };

            bool result = _tsOrderData.Insert(tsorder);

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

            TsOrder tsorder = new TsOrder
            {
                UserId = tsorderDto.UserId,
                PaymentId = tsorderDto.PaymentId,
                InvoiceNo = tsorderDto.InvoiceNo,
                OrderDate = tsorderDto.OrderDate,
                IsPaid = tsorderDto.IsPaid


            };

            bool result = _tsOrderData.Update(id, tsorder);

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
            bool result = _tsOrderData.Delete(id);

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
