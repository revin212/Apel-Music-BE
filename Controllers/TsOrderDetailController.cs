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
        [HttpGet("GetMyInvoicesDetailList")]
        public IActionResult GetMyInvoicesDetailList(Guid orderid)
        {
            try
            {
                List<TsOrderDetail> tsOrderDetail = _tsOrderDetailData.GetMyInvoiceDetailList(orderid);
                return Ok(tsOrderDetail);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("GetCart")]
        public IActionResult GetCart(Guid orderid, Guid userid)
        {
            try
            {
                List<TsOrderDetail?> tsOrderDetail = _tsOrderDetailData.GetCart(orderid, userid);

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
        
        [HttpPost("CheckoutCart")]
        public IActionResult CheckoutCart(TsOrderDetailDTOCheckout tsorderdetaildtocheckout)
        {
            //{
            //    "id": "c12e14c7-41e7-4aa1-a16a-4504e57e5e88",
            //  "userId": "a6203aed-8920-11ee-a057-5c96db8712c6",
            //  "paymentId": "3e544b7c-884a-11ee-b59a-3c5282e16d0b",
            //  "orderDate": "2023-11-26T03:50:42.941Z"
            //}

            //usethis
            //{
            //    "cartInfo": {
            //      "id": "faa4129e-655c-4492-a435-4bfe1e8959cf",
            //      "userId": "a6203aed-8920-11ee-a057-5c96db8712c6",
            //      "paymentId": "3e544b7c-884a-11ee-b59a-3c5282e16d0b",
            //      "orderDate": "2023-11-27T02:04:31",
            //                    },
            //"cartItem": [
            //          {
            //    "id": "a4197ebe-8c8e-11ee-a7fb-71f57a8118f6",
            //            "orderId": "faa4129e-655c-4492-a435-4bfe1e8959cf",
            //            "courseId": "b3e79d1f-884a-11ee-b59a-3c5282e16d0b",
            //            "isChecked": true
            //          },
            //          {
            //    "id": "c8994757-8c8e-11ee-a7fb-71f57a8118f6",
            //            "orderId": "faa4129e-655c-4492-a435-4bfe1e8959cf",
            //            "courseId": "b3e79c28-884a-11ee-b59a-3c5282e16d0b",
            //            "isChecked": false
            //          }
            //        ]
            // }
            try
            {
                if (tsorderdetaildtocheckout == null)
                    return BadRequest("Data should be inputed");

                DateTime now = DateTime.Now;
                TsOrderDTOCheckout order = tsorderdetaildtocheckout.CartInfo;
                string inv = $"APM{order?.Id.ToString("D5")}";

                TsOrder tsorder = new TsOrder
                {
                    Id = order?.Id,
                    UserId = order?.UserId ?? Guid.Empty,
                    PaymentId = order?.PaymentId,
                    InvoiceNo = inv,
                    OrderDate = DateTime.UtcNow,
                    IsPaid = true
                };
                List<TsOrderDetail> tsOrderDetailListChecked = new List<TsOrderDetail>();
                List<TsOrderDetail> tsOrderDetailListUnchecked = new List<TsOrderDetail>();

                List<TsOrderDetailDTOCheckoutCart> cartitem = tsorderdetaildtocheckout.CartItem;

                foreach (var item in cartitem)
                {
                    if (item.IsChecked)
                    {
                        TsOrderDetail orderdetailschecked = new TsOrderDetail
                        {
                            Id = item.Id,
                            OrderId = item.OrderId,
                            CourseId = item.CourseId,
                            Jadwal = item.Jadwal
                        };
                        tsOrderDetailListChecked.Add(orderdetailschecked);
                    }
                    else
                    {
                        TsOrderDetail orderdetailsunchecked = new TsOrderDetail
                        {
                            Id = item.Id,
                            OrderId = item.OrderId,
                            CourseId = item.CourseId,
                            Jadwal = item.Jadwal
                        };
                        tsOrderDetailListUnchecked.Add(orderdetailsunchecked);
                    }
                    
                }

               
                bool result = _tsOrderDetailData.CheckoutCart(tsOrderDetailListChecked,tsOrderDetailListUnchecked, tsorder);

                if (result)
                {
                    return StatusCode(201, result);
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
                   
                };

                TsOrder? tsOrder = _tsOrderData.GetCartInfo(tsorderdetailDto.UserId); //ambil semua order dengan IsPaid = false

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
                bool result = _tsOrderDetailData.DeleteOneNotActivated(deletedata.Id);

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
        [HttpPut]
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
