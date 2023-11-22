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
        private readonly TsOrderData _tsOrderData;
        public TsOrderDetailController(TsOrderDetailData tsOrderDetailData, TsOrderData tsOrderData)
        {
            _tsOrderData = tsOrderData;
            _tsOrderDetailData = tsOrderDetailData;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<TsOrderDetail> tsOrderDetail = _tsOrderDetailData.GetAll();
                return Ok(tsOrderDetail);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("GetById")]
        public IActionResult Get(Guid id)
        {
            try
            {
                TsOrderDetail? tsOrderDetail = _tsOrderDetailData.GetById(id);

                if (tsOrderDetail == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(tsOrderDetail); //200
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] TsOrderDetailDTO tsorderdetailDto)
        {
            //select * from TsOrder
            //Jika ada Order yang memiliki isPaid = false, jangan insert new Order
            //tapi ubah TsOrderDetail.OrderId masing2 dengan TsOrder.Id yang memiliki isPaid = false

            //else insert new

            try
            {
                if (tsorderdetailDto == null)
                    return BadRequest("Data should be inputed");

                TsOrderDetail tsorder = new TsOrderDetail
                {
                    //Id = Guid.NewGuid(),
                    OrderId = tsorderdetailDto.OrderId,
                    CourseId = tsorderdetailDto.CourseId,
                    IsActive = tsorderdetailDto.IsActive
                };

                List<TsOrder> tsOrder = _tsOrderData.GetAllIsPaidfalse(tsorderdetailDto.UserId); //ambil semua order dengan IsPaid = false, mudah2an dapetnya satu

                //TODO: jika tsOrder count > 0 maka ubah tsorderdetailDto.OrderId dengan tsOrder.Id

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
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut]
        public IActionResult Put(Guid id, [FromBody] TsOrderDetailDTO tsorderdetailDto)
        {
            try
            {
                if (tsorderdetailDto == null)
                    return BadRequest("Data should be inputed");

                TsOrderDetail tsorder = new TsOrderDetail
                {
                    OrderId = tsorderdetailDto.OrderId,
                    CourseId = tsorderdetailDto.CourseId,
                    IsActive = tsorderdetailDto.IsActive
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
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            try
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
            catch (Exception)
            {

                throw;
            }
        }
    }
}
