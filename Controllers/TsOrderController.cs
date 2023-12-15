using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.TsOrder;
using fs_12_team_1_BE.DTO.TsOrderDetail;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fs_12_team_1_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin, User")]
    public class TsOrderController : ControllerBase
    {
        private readonly TsOrderData _tsOrderData;
        public TsOrderController(TsOrderData tsOrderData)
        {
            _tsOrderData = tsOrderData;
        }



        [HttpGet("GetMyInvoicesList")]
        [Authorize]
        public IActionResult GetMyInvoicesList(Guid userid)
        {
            try
            {
                List<TsOrderGetMyInvoiceListResDTO> myInvoiceList = _tsOrderData.GetMyInvoicesList(userid);
                return Ok(myInvoiceList);
            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpGet("GetInvoiceDetailHeader")]
        public IActionResult GetInvoiceDetailHeader(int id)
        {
            try
            {
                 TsOrderGetInvoiceDetailHeaderRes tsOrder = _tsOrderData.GetInvoiceDetailHeader(id);

                if (tsOrder == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(tsOrder); //200
            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error occured");
            }
        }
        [HttpGet("GetCartInfo")]
        [Authorize]
        public IActionResult GetCartInfo(Guid userid)
        {
            try
            {
                TsOrder tsOrder = _tsOrderData.GetCartInfo(userid);

                if (tsOrder == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(tsOrder); //200


            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpGet("GetCart")]
        [Authorize]
        public IActionResult GetCart(Guid userid)
        {
            try
            {
                List<TsOrderDetailGetCartListResDTO> tsorderdetail = _tsOrderData.GetCart(userid);

                if (tsorderdetail == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(tsorderdetail); //200


            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error occured");
            }
        }

        [HttpPost("CheckoutCart")]
        [Authorize]
        public IActionResult CheckoutCart(TsOrderDTOCheckout tsorderdtocheckout)
        {
            try
            {
                if (tsorderdtocheckout == null)
                    return BadRequest("Data should be inputed");

                bool result = _tsOrderData.CheckoutCart(tsorderdtocheckout);

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
                return StatusCode(500, "Server Error occured");
            }
        }
    }
}
