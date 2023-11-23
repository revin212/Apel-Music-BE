using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.TsOrder;
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
                TsOrderDetail? tsOrderDetail = _tsOrderDetailData.GetAllById(id);

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

        [HttpPost("AddToCart")]
        public IActionResult AddToCart([FromBody] TsOrderDetailDTO tsorderdetailDto)
        {
            //select * from TsOrder
            //Jika ada Order yang memiliki isPaid = false, jangan insert new Order
            //tapi ubah TsOrderDetail.OrderId masing2 dengan TsOrder.Id yang memiliki isPaid = false

            //else insert new
            //test data
            //{
            //  "userId": "a6203aed-8920-11ee-a057-5c96db8712c6",
            //  "courseId": "b3e79d1f-884a-11ee-b59a-3c5282e16d0b",
            //  "isActivated": false
            //}
            try
            {
                if (tsorderdetailDto == null)
                    return BadRequest("Data should be inputed");

                TsOrderDetail tsorderdetail = new TsOrderDetail
                {
                    //Id = Guid.NewGuid(),
                    OrderId = null,
                    CourseId = tsorderdetailDto.CourseId,
                    IsActivated = false
                };

                List<TsOrder> tsOrder = _tsOrderData.GetAllNotPaidByUserId(tsorderdetailDto.UserId); //ambil semua order dengan IsPaid = false, mudah2an dapetnya satu

                //jika tsOrder count > 0 maka ubah tsorderdetailDto.OrderId dengan tsOrder.Id
                if( tsOrder.Count > 0 )
                {
                    //TODO: add logic if Count > 1, loop delete where tsOrder[>1].Id
                    tsorderdetail.OrderId = tsOrder[0].Id;
                }
                else
                {
                    TsOrder tsOrderNew = new TsOrder
                    {
                        Id = Guid.NewGuid(),
                        UserId = tsorderdetailDto.UserId,
                        //PaymentId = null,
                        InvoiceNo = string.Empty,
                        //OrderDate = null,
                        IsPaid = false
                    };
                    _tsOrderData.Insert(tsOrderNew); //Apa boleh langsung ke DataAccess? apa harus lewat tsorderdata controller dulu?
                    tsorderdetail.OrderId = tsOrderNew.Id;
                }
                bool result = _tsOrderDetailData.Insert(tsorderdetail);

                if (result)
                {
                    return StatusCode(201, tsorderdetail.Id);
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
        [HttpPost("DeleteFromCart")]
        public IActionResult DeleteFromCart([FromBody] Guid id, Guid orderid)
        {
            
            try
            {
                bool result = _tsOrderDetailData.DeleteOneNotActivated(id, orderid);

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
        [HttpPost("ClearCart")]
        public IActionResult ClearCart([FromBody] Guid orderid)
        {

            try
            {
                bool result = _tsOrderDetailData.DeleteAllNotActivatedByOrderId(orderid);

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
                    IsActivated = tsorderdetailDto.IsActivated
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
