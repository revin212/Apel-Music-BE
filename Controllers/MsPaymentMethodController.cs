using fs_12_team_1_BE.DataAccess;
using fs_12_team_1_BE.Model;
using Microsoft.AspNetCore.Mvc;

namespace fs_12_team_1_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MsPaymentMethodController : ControllerBase
    {
        private readonly MsPaymentMethodData _msPaymentMethodData;
        public MsPaymentMethodController(MsPaymentMethodData msPaymentMethodData)
        {
            _msPaymentMethodData = msPaymentMethodData;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<MsPaymentMethod> msPaymentMethod = _msPaymentMethodData.GetAll();
                return Ok(msPaymentMethod);
            }
            catch
            {
                return StatusCode(500, "Server Error occured");
            }
        }
    }
}
