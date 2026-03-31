using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PositivityCoupon.Api.Dtos;
using PositivityCoupon.Api.Services;

namespace PositivityCoupon.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AdminViewModelService _service;

        public DashboardController(AdminViewModelService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardSummaryDto>> Get()
        {
            var summary = await _service.GetDashboardAsync();
            return Ok(summary);
        }
    }
}

