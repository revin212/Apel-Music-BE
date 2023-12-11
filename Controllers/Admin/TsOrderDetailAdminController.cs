﻿using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.TsOrder;
using fs_12_team_1_BE.DTO.TsOrderDetail;
using fs_12_team_1_BE.Model;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fs_12_team_1_BE.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TsOrderDetailAdminController : ControllerBase
    {
        private readonly TsOrderDetailAdminData _tsOrderDetailData;
        public TsOrderDetailAdminController(TsOrderDetailAdminData tsOrderDetailData)
        {
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
        [HttpGet("GetAllInvoicesDetailList")]
        public IActionResult GetAllInvoicesDetailList(int orderid)
        {
            try
            {
                List<TsOrderDetailAdminGetAllInvoiceDetailListResDTO> tsOrderDetail = _tsOrderDetailData.GetInvoiceDetailList(orderid);
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
        //[HttpPost("UpdateSelectedCartItem")]
        //[Authorize]
        //public IActionResult UpdateSelectedCartItem([FromBody] TsOrderDetailUpdateSelectedCartItemDTO selectedCartItemDTO)
        //{
        //    TsOrder result = new TsOrder();
        //    try 
        //    {

        //        result = _tsOrderDetailData.UpdateSelectedCartItem(selectedCartItemDTO.CartItemId, selectedCartItemDTO.IsSelected, selectedCartItemDTO.UserId);
                
        //        return Ok(result);
                
        //    }
        //    catch(Exception)
        //    {
        //        return StatusCode(500, "Error occured");
        //    }
        //}
        
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
