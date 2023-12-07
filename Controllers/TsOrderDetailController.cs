using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.TsOrder;
using fs_12_team_1_BE.DTO.TsOrderDetail;
using fs_12_team_1_BE.Model;
using Humanizer;
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


        //[HttpGet("GetAll")]
        //public IActionResult GetAll()
        //{
        //    try
        //    {
        //        List<TsOrderDetail> tsOrderDetail = _tsOrderDetailData.GetAll();
        //        return Ok(tsOrderDetail);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //[HttpGet("GetById")]
        //public IActionResult Get(int id)
        //{
        //    try
        //    {
        //        TsOrderDetail? tsOrderDetail = _tsOrderDetailData.GetById(id);

        //        if (tsOrderDetail == null)
        //        {
        //            return NotFound("Data not found");
        //        }

        //        return Ok(tsOrderDetail); //200
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        [HttpGet("GetMyInvoicesDetailList")]
        public IActionResult GetMyInvoicesDetailList(int orderid)
        {
            try
            {
                List<TsOrderDetailGetMyInvoiceDetailListResDTO> tsOrderDetail = _tsOrderDetailData.GetMyInvoiceDetailList(orderid);
                return Ok(tsOrderDetail);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //[HttpGet("GetCart")]
        //public IActionResult GetCart(Guid orderid, Guid userid)
        //{
        //    try
        //    {
        //        List<TsOrderDetail?> tsOrderDetail = _tsOrderDetailData.GetCart(orderid, userid);

        //        if (tsOrderDetail == null)
        //        {
        //            return NotFound("Data not found");
        //        }

        //        return Ok(tsOrderDetail); //200


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        [HttpPost("UpdateSelectedCartItem")]
        public IActionResult UpdateSelectedCartItem([FromBody] TsOrderDetailUpdateSelectedCartItemDTO selectedCartItemDTO)
        {
            TsOrder result = new TsOrder();
            try 
            {

                result = _tsOrderDetailData.UpdateSelectedCartItem(selectedCartItemDTO.CartItemId, selectedCartItemDTO.IsSelected, selectedCartItemDTO.UserId);
                
                return Ok(result);
                
            }
            catch(Exception)
            {
                return StatusCode(500, "Error occured");
            }
        }
        
        [HttpPost("AddToCart")]
        public IActionResult AddToCart([FromBody] TsOrderDetailDTOAddToCart tsorderdetailDto) //not complete
        {
            //select * from TsOrder
            //Jika ada Order yang memiliki isPaid = false, jangan insert new Order
            //tapi ubah TsOrderDetail.OrderId masing2 dengan TsOrder.Id yang memiliki isPaid = false

            //else insert new
            //test data
            //{
            //    "userId": "a6203aed-8920-11ee-a057-5c96db8712c6",
            //  "courseId": "b3e79d1f-884a-11ee-b59a-3c5282e16d0b"

            //}
            //{
            //    "userId": "a6203aed-8920-11ee-a057-5c96db8712c6",
            //  "courseId": "b3e79c28-884a-11ee-b59a-3c5282e16d0b"
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
                    Jadwal = tsorderdetailDto.Jadwal
                };

                TsOrder tsOrder = _tsOrderData.GetCartInfo(tsorderdetailDto.UserId); //ambil semua order dengan IsPaid = false

                //jika tsOrder.Id not null maka ubah tsorderdetail.OrderId dengan tsOrder.Id
                if (tsOrder != null)
                {

                    tsorderdetail.OrderId = tsOrder.Id;
                }
                else
                {
                    TsOrder tsOrderNew = new TsOrder
                    {
                        //Id = Guid.NewGuid(),
                        UserId = tsorderdetailDto.UserId,
                        //PaymentId = null,
                        InvoiceNo = string.Empty,
                        //OrderDate = null,
                        IsPaid = false
                    };
                    int new_cartid = _tsOrderData.NewCart(tsOrderNew);
                    tsorderdetail.OrderId = new_cartid;
                }
                bool result = _tsOrderDetailData.AddToCart(tsorderdetail);

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
        public IActionResult DeleteFromCart([FromBody] TsOrderDetailDTODeleteFromCart deletedata)
        {

            try
            {
                bool result = _tsOrderDetailData.DeleteFromCart(deletedata.Id);

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
                bool result = _tsOrderDetailData.ClearCart(orderid);

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
        //[HttpPut]
        //public IActionResult Put(Guid id, [FromBody] TsOrderDetailDTOAddToCart tsorderdetailDto)
        //{
        //    try
        //    {
        //        if (tsorderdetailDto == null)
        //            return BadRequest("Data should be inputed");

        //        TsOrderDetail tsorder = new TsOrderDetail
        //        {
        //            OrderId = tsorderdetailDto.OrderId,
        //            CourseId = tsorderdetailDto.CourseId,
        //            IsActivated = tsorderdetailDto.IsActivated
        //        };

        //        bool result = _tsOrderDetailData.Update(id, tsorder);

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
        //        bool result = _tsOrderDetailData.Delete(id);

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
