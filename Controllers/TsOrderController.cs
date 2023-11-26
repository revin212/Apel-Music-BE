﻿using fs_12_team_1_BE.DataAccess;
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
            try
            {
                List<TsOrder> tsOrder = _tsOrderData.GetAll();
                return Ok(tsOrder);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("OrderRecords")]
        public IActionResult GetOrderRecords(Guid userid)
        {
            try
            {
                List<TsOrder> tsOrder = _tsOrderData.GetAllPaidByUserId(userid);
                return Ok(tsOrder);
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
                TsOrder? tsOrder = _tsOrderData.GetById(id);

                if (tsOrder == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(tsOrder); //200
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("GetCartInfo")]
        public IActionResult GetCartInfo(Guid userid)
        {
            try
            {
                TsOrder? tsOrder = _tsOrderData.GetCartInfo(userid);

                if (tsOrder == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(tsOrder); //200


            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("GetCart")]
        public IActionResult GetCart(Guid userid)
        {
            try
            {
                List<TsOrderDetail?> tsorderdetail = _tsOrderData.GetCart(userid);

                if (tsorderdetail == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(tsorderdetail); //200


            }
            catch (Exception)
            {

                throw;
            }
        }

        //[HttpPost("Checkout")]
        //public IActionResult Checkout([FromBody] TsOrderDTOCheckout tsorderDtoCheckout)
        //{
        //    //{
        //    //    "id": "c12e14c7-41e7-4aa1-a16a-4504e57e5e88",
        //    //  "userId": "a6203aed-8920-11ee-a057-5c96db8712c6",
        //    //  "paymentId": "3e544b7c-884a-11ee-b59a-3c5282e16d0b",
        //    //  "orderDate": "2023-11-26T03:50:42.941Z"
        //    //}
        //    try 
        //    {
        //        if (tsorderDtoCheckout == null)
        //            return BadRequest("Data should be inputed");

        //        DateTime now = DateTime.Now;
        //        string inv = $"INV/{now.ToString("yyyyMMdd")}/{tsorderDtoCheckout.Id.ToString("N")}";

        //        TsOrder tsorder = new TsOrder
        //        {
        //            Id = tsorderDtoCheckout.Id,
        //            UserId = tsorderDtoCheckout.UserId,
        //            PaymentId = tsorderDtoCheckout.PaymentId,
        //            InvoiceNo = inv,
        //            OrderDate = DateTime.UtcNow,
        //            IsPaid = true
        //        };

        //        bool result = _tsOrderData.Checkout(tsorder);

        //        if (result)
        //        {
        //            return StatusCode(201, result);
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
        [HttpPost]
        //public IActionResult Post([FromBody] TsOrderDTO tsorderDto)
        //{
        //    try
        //    {
        //        if (tsorderDto == null)
        //            return BadRequest("Data should be inputed");

        //        TsOrder tsorder = new TsOrder
        //        {
        //            //Id = Guid.NewGuid(),
        //            UserId = tsorderDto.UserId,
        //            PaymentId = tsorderDto.PaymentId,
        //            InvoiceNo = tsorderDto.InvoiceNo,
        //            OrderDate = tsorderDto.OrderDate,
        //            IsPaid = tsorderDto.IsPaid
        //        };

        //        bool result = _tsOrderData.Insert(tsorder);

        //        if (result)
        //        {
        //            return StatusCode(201, tsorder.Id);
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

        [HttpPut]
        public IActionResult Put(Guid id, [FromBody] TsOrderDTO tsorderDto)
        {
            try
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

                bool result = _tsOrderData.Update(tsorder);

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
            catch (Exception)
            {

                throw;
            }
        }
    }
}
