using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.TsOrder;
using fs_12_team_1_BE.DTO.TsOrderDetail;
using fs_12_team_1_BE.Model;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fs_12_team_1_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, User")]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public IActionResult AddToCart([FromBody] TsOrderDetailDTOAddToCart tsorderdetailDto)
        {
            
            try
            {
                if (tsorderdetailDto == null)
                    return BadRequest("Data should be inputed");

                TsOrderDetail tsorderdetail = new TsOrderDetail
                {
                   
                    OrderId = null,
                    CourseId = tsorderdetailDto.CourseId,
                    Jadwal = tsorderdetailDto.Jadwal
                };

                bool available = _tsOrderDetailData.CheckJadwal(tsorderdetail);

                if (!available)
                {
                    return Unauthorized("Jadwal tidak tersedia");
                }

                TsOrder tsOrder = _tsOrderData.GetCartInfo(tsorderdetailDto.UserId); 

               
                if (tsOrder.Id != null)
                {

                    tsorderdetail.OrderId = tsOrder.Id;
                }
                else
                {
                    TsOrder tsOrderNew = new TsOrder
                    {
                        UserId = tsorderdetailDto.UserId,
                        InvoiceNo = string.Empty,
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
        [Authorize]
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
