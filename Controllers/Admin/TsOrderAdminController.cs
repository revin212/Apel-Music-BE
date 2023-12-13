using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.DTO.TsOrder;
using fs_12_team_1_BE.DTO.TsOrderDetail;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fs_12_team_1_BE.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class TsOrderAdminController : ControllerBase
    {
        private readonly TsOrderAdminData _tsOrderAdminData;
        public TsOrderAdminController(TsOrderAdminData tsOrderAdminData)
        {
            _tsOrderAdminData = tsOrderAdminData;
        }

        [HttpGet("GetAllInvoicesList")]
        public IActionResult GetAllInvoicesList()
        {
            try
            {
                List<TsOrderAdminGetAllInvoiceListResDTO> myInvoiceList = _tsOrderAdminData.GetAllInvoicesList();
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
                 TsOrderAdminGetInvoiceDetailHeaderRes tsOrder = _tsOrderAdminData.GetInvoiceDetailHeader(id);

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
       
    }
}
