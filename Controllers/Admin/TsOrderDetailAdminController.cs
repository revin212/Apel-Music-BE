using fs_12_team_1_BE.DataAccess;
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
                return StatusCode(500, "Server Error occured");
            }
        }

      
    }
}
